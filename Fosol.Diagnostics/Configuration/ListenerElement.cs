using Fosol.Common.Extensions.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Fosol.Diagnostics.Configuration
{
    /// <summary>
    /// A ListenerElement provides a way to dynamically configure a TraceListener.
    /// </summary>
    internal class ListenerElement
        : ConfigurationElement
    {
        #region Variables
        private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();
        private const string _NameKey = "name";
        private const string _TypeNameKey = "type";
        private const string _InitializeKey = "initialize";
        private const string _SettingsKey = "settings";
        private const string _FiltersKey = "filters";
        private Lazy<Listeners.TraceListener> _Listener;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Unique name to identify this ListenerElement.
        /// </summary>
        [ConfigurationProperty(_NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[_NameKey]; }
            set { base[_NameKey] = value; }
        }

        /// <summary>
        /// get/set - The Type of TraceListener this will create.
        /// </summary>
        [ConfigurationProperty(_TypeNameKey)]
        public string TypeName
        {
            get { return (string)base[_TypeNameKey]; }
            set { base[_TypeNameKey] = value; }
        }

        /// <summary>
        /// get/set - Constructor initialization parameters.  Used when creating the TraceListener.
        /// </summary>
        [ConfigurationProperty(_InitializeKey)]
        public ArgumentElementCollection Initialize
        {
            get { return (ArgumentElementCollection)base[_InitializeKey]; }
            set { base[_InitializeKey] = value; }
        }

        /// <summary>
        /// get/set - Property intialization parameters.  Used when creating the TraceListener.
        /// </summary>
        [ConfigurationProperty(_SettingsKey)]
        public ArgumentElementCollection Settings
        {
            get { return (ArgumentElementCollection)base[_SettingsKey]; }
            set { base[_SettingsKey] = value; }
        }

        /// <summary>
        /// get/set - Collection of FilterElement objects used to filter what TraceEvents will be sent to the listeners.
        /// </summary>
        [ConfigurationProperty(_FiltersKey)]
        public FilterElementCollection Filters
        {
            get { return (FilterElementCollection)base[_FiltersKey]; }
            set { base[_FiltersKey] = value; }
        }

        /// <summary>
        /// get - The initialized listener based on the configuration settings.
        /// </summary>
        public Listeners.TraceListener Listener
        {
            get
            {
                return _Listener.Value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ListenerElement.
        /// Initializes the Lazy listener.
        /// </summary>
        public ListenerElement()
        {
            _Listener = new Lazy<Listeners.TraceListener>(() => GetListener(), true);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the TraceListener for this configuration.
        /// If the listener has already been created it will return it.
        /// If the listener has not been created it will create it.
        /// </summary>
        /// <returns>The TraceListener for this configuration.</returns>
        private Listeners.TraceListener GetListener()
        {
            var name = this.Name;
            var type_name = this.TypeName;

            // Annoyingly an empty TraceListener is created even if one isn't configured.
            // So check for this and exit.
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type_name))
                return null;

            // This is a reference to a SharedListener, so confirm that it exists.
            if (string.IsNullOrEmpty(type_name))
            {
                // When refrencing a SharedListener you cannot include other configuration options.
                if (this.Initialize.Count != 0 || this.Settings.Count != 0)
                    throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Invalid_Properties, name));

                // The reference must exist in the shared listeners.
                if (TraceManager.Manager.SharedListeners == null)
                    throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Must_Exist, name));

                var shared = TraceManager.Manager.SharedListeners[name];
                if (shared == null)
                    throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Must_Exist, name));

                // Reference the shared listener.
                var listener = shared.GetListener();

                // Apply the shared listener's filters to this instance.
                if (this.Filters.Count == 0
                    && shared.Filters.Count != 0)
                    this.Filters = shared.Filters;

                return listener;
            }
            // Try to create the listener as it has been defined in the configuration.
            else
            {
                // Initialize the listener with the arguments.
                return CreateListener();
            }
        }

        private Listeners.TraceListener CreateListener()
        {
            var name = this.Name;
            var type_name = this.TypeName;
            var filters = (this.Filters.Count == 0) ? null : this.Filters;
            var initialize = (this.Initialize.Count == 0) ? null : this.Initialize;
            var settings = (this.Settings.Count == 0) ? null : this.Settings;

            var type = Type.GetType(type_name);
            Listeners.TraceListener listener = null;

            if (type == null)
                throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Type_Invalid, name, type_name));

            // Use the default constructor to create a new TraceListener.
            if (initialize == null)
            {
                var ctor = type.GetConstructor(new Type[0]);

                if (ctor != null)
                    listener = ctor.Invoke(new object[0]) as Listeners.TraceListener;
            }
            else
            {
                object[] init = new object[initialize.Count];
                int i = 0;
                // Get the initialize attributes.
                foreach (TraceInitializeAttribute attr in type.GetCustomAttributes(typeof(TraceInitializeAttribute), true))
                {
                    var config = initialize.FirstOrDefault(a => a.Name.Equals(attr.Name, StringComparison.InvariantCulture));

                    // Found an initialize attribute that matches the configured argument.
                    if (config != null)
                    {
                        init[i] = attr.Convert(config.Value);
                        i++;
                    }
                }

                // Get the constructor that matches the supplied initialize args (order is important).
                var ctor = type.GetConstructor(init.Select(a => a.GetType()).ToArray());

                if (ctor != null)
                    listener = ctor.Invoke(init) as Listeners.TraceListener;
            }

            if (listener == null)
                throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Failed, this.Name));

            var pinfos = (
                from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                where p.GetCustomAttribute(typeof(TraceSettingAttribute), true) != null
                select p);

            foreach (var prop in pinfos)
            {
                var attr = prop.GetCustomAttribute(typeof(TraceSettingAttribute), true) as TraceSettingAttribute;

                // If property values were included in the configuration update the TraceListener.
                if (settings != null)
                {
                    var config = settings.FirstOrDefault(p => p.Name.Equals(attr.Name, StringComparison.InvariantCulture));

                    // The configuration provides a default value so apply it to the property.
                    if (config != null)
                    {
                        if (attr.Converter != null)
                        {
                            prop.SetValue2(listener, config.Value, attr.Converter);
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            prop.SetValue(listener, config.Value);
                        }
                        else
                        {
                            // Convert the configuration value to the property type.
                            prop.SetValue2(listener, config.Value);
                        }
                    }
                    else if (attr.IsRequired)
                        throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Setting_Required, this.Name, attr.Name));
                    else
                        prop.ApplyDefaultValue(listener, attr.Converter);
                }
                else if (attr.IsRequired)
                    throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Setting_Required, this.Name, attr.Name));
                else
                    prop.ApplyDefaultValue(listener, attr.Converter);
            }

            listener.Config = this;
            // Call the Initialize method.
            listener.Initialize();

            return listener;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
