using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    internal class ListenerElement
        : ConfigurationElement
    {
        #region Variables
        private const string NameKey = "name";
        private const string TypeNameKey = "type";
        private const string InitializeKey = "initialize";
        private const string SettingsKey = "settings";
        private const string FilterKey = "filter";
        private TraceListener _Listener;
        #endregion

        #region Properties
        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[NameKey]; }
            set { base[NameKey] = value; }
        }

        [ConfigurationProperty(TypeNameKey)]
        public string TypeName
        {
            get { return (string)base[TypeNameKey]; }
            set { base[TypeNameKey] = value; }
        }

        [ConfigurationProperty(InitializeKey)]
        public ArgumentElementCollection Initialize
        {
            get { return (ArgumentElementCollection)base[InitializeKey]; }
            set { base[InitializeKey] = value; }
        }

        [ConfigurationProperty(SettingsKey)]
        public ArgumentElementCollection Settings
        {
            get { return (ArgumentElementCollection)base[SettingsKey]; }
            set { base[SettingsKey] = value; }
        }

        [ConfigurationProperty(FilterKey, DefaultValue = null)]
        public FilterElement Filter
        {
            get { return (FilterElement)base[FilterKey]; }
            set { base[FilterKey] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        internal TraceListener GetListener()
        {
            if (_Listener != null)
                return _Listener;

            var name = this.Name;
            var type_name = this.TypeName;
            var filter = (string.IsNullOrEmpty(this.Filter.Name) && string.IsNullOrEmpty(this.Filter.TypeName)) ? null : this.Filter;
            var initialize = (this.Initialize.Count == 0) ? null : this.Initialize;
            var properties = (this.Properties.Count == 0) ? null : this.Properties;

            // Annoyingly an empty TraceListener is created even if one isn't configured.
            // So check for this and exit.
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type_name))
                return null;

            // This is a reference to a SharedListener, so confirm that it exists.
            if (string.IsNullOrEmpty(type_name))
            {
                // When refrencing a SharedListener you cannot include other configuration options.
                if (filter != null || initialize != null || properties != null)
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Invalid_Properties, name));

                // The reference must exist in the shared listeners.
                if (TraceManager.Manager.SharedListeners == null)
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Must_Exist, name));

                var listener = TraceManager.Manager.SharedListeners[name];
                if (listener == null)
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Must_Exist, name));

                // Reference the shared listener.
                _Listener = listener.GetListener();
                return _Listener;
            }
            // Try to create the listener as it has been defined in the configuration.
            else
            {
                try
                {
                    if (initialize != null && initialize.Count > 0)
                    {
                        // Initialize the listener with the arguments.
                        _Listener = CreateListener();
                        return _Listener;
                    }
                    else
                    {
                        // Initialize the listener without any arguments.
                        _Listener = Fosol.Common.Helpers.ReflectionHelper.ConstructObject<TraceListener>(type_name);
                        return _Listener;
                    }
                }
                catch
                {
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Listener_Arguments_Invalid, this.Name));
                }
            }
        }

        private TraceListener CreateListener()
        {
            var name = this.Name;
            var type_name = this.TypeName;
            var filter = (string.IsNullOrEmpty(this.Filter.Name) && string.IsNullOrEmpty(this.Filter.TypeName)) ? null : this.Filter;
            var initialize = (this.Initialize.Count == 0) ? null : this.Initialize;
            var settings = (this.Settings.Count == 0) ? null : this.Settings;

            var type = Type.GetType(type_name);

            if (type == null)
                throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Listener_Type_Invalid, name, type_name));

            // Use the default constructor to create a new TraceListener.
            if (initialize == null)
            {
                var ctor = type.GetConstructor(new Type[0]);

                if (ctor != null)
                    _Listener = ctor.Invoke(new object[0]) as TraceListener;
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
                    _Listener = ctor.Invoke(init) as TraceListener;
            }

            if (_Listener == null)
                throw new ConfigurationErrorsException();

            // If property values were included in the configuration update the TraceListener.
            if (settings != null)
            {
                var pinfos = (
                    from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    where p.GetCustomAttribute(typeof(TracePropertyAttribute), true) != null
                    select p);

                foreach (var prop in pinfos)
                {
                    var attr = prop.GetCustomAttribute(typeof(TracePropertyAttribute), true) as TracePropertyAttribute;
                    var config = settings.FirstOrDefault(p => p.Name.Equals(attr.Name, StringComparison.InvariantCulture));

                    // The configuration provides a default value so apply it to the property.
                    if (config != null)
                    {
                        if (attr.Converter != null)
                        {
                            Common.Helpers.ReflectionHelper.SetValue(prop, _Listener, config.Value, attr.Converter);
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            prop.SetValue(_Listener, config.Value);
                        }
                        else
                        {
                            // Convert the configuration value to the property type.
                            Common.Helpers.ReflectionHelper.SetValue(prop, _Listener, config.Value);
                        }
                    }
                    else
                    {
                        var attr_default = prop.GetCustomAttribute(typeof(DefaultValueAttribute), true) as DefaultValueAttribute;
                        if (attr_default != null)
                        {
                            if (attr.Converter != null)
                                Common.Helpers.ReflectionHelper.SetValue(prop, _Listener, attr_default.Value, attr.Converter);
                            else
                                prop.SetValue(_Listener, attr_default.Value);
                        }
                        else if (attr.IsRequired)
                            throw new ConfigurationErrorsException();
                    }
                }
            }

            // Call the Initialize method.
            _Listener.Initialize();

            if (_Listener != null)
                return _Listener;

            throw new ConfigurationErrorsException();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
