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
        private const string _NameKey = "name";
        private const string _TypeNameKey = "type";
        private const string _InitializeKey = "initialize";
        private const string _SettingsKey = "settings";
        private const string _FilterKey = "filter";
        private Listeners.TraceListener _Listener;
        #endregion

        #region Properties
        [ConfigurationProperty(_NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[_NameKey]; }
            set { base[_NameKey] = value; }
        }

        [ConfigurationProperty(_TypeNameKey)]
        public string TypeName
        {
            get { return (string)base[_TypeNameKey]; }
            set { base[_TypeNameKey] = value; }
        }

        [ConfigurationProperty(_InitializeKey)]
        public ArgumentElementCollection Initialize
        {
            get { return (ArgumentElementCollection)base[_InitializeKey]; }
            set { base[_InitializeKey] = value; }
        }

        [ConfigurationProperty(_SettingsKey)]
        public ArgumentElementCollection Settings
        {
            get { return (ArgumentElementCollection)base[_SettingsKey]; }
            set { base[_SettingsKey] = value; }
        }

        [ConfigurationProperty(_FilterKey, DefaultValue = null)]
        public FilterElement Filter
        {
            get { return (FilterElement)base[_FilterKey]; }
            set { base[_FilterKey] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        internal Listeners.TraceListener GetListener()
        {
            if (_Listener != null)
                return _Listener;

            var name = this.Name;
            var type_name = this.TypeName;
            var filter = (string.IsNullOrEmpty(this.Filter.Name) && string.IsNullOrEmpty(this.Filter.TypeName)) ? null : this.Filter;
            var initialize = (this.Initialize.Count == 0) ? null : this.Initialize;
            var settings = (this.Settings.Count == 0) ? null : this.Settings;

            // Annoyingly an empty TraceListener is created even if one isn't configured.
            // So check for this and exit.
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type_name))
                return null;

            // This is a reference to a SharedListener, so confirm that it exists.
            if (string.IsNullOrEmpty(type_name))
            {
                // When refrencing a SharedListener you cannot include other configuration options.
                if (filter != null || initialize != null || settings != null)
                    throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Invalid_Properties, name));

                // The reference must exist in the shared listeners.
                if (TraceManager.Manager.SharedListeners == null)
                    throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Must_Exist, name));

                var listener = TraceManager.Manager.SharedListeners[name];
                if (listener == null)
                    throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Must_Exist, name));

                // Reference the shared listener.
                _Listener = listener.GetListener();
                return _Listener;
            }
            // Try to create the listener as it has been defined in the configuration.
            else
            {
                // Initialize the listener with the arguments.
                _Listener = CreateListener();
                return _Listener;
            }
        }

        private Listeners.TraceListener CreateListener()
        {
            var name = this.Name;
            var type_name = this.TypeName;
            var filter = (string.IsNullOrEmpty(this.Filter.Name) && string.IsNullOrEmpty(this.Filter.TypeName)) ? null : this.Filter;
            var initialize = (this.Initialize.Count == 0) ? null : this.Initialize;
            var settings = (this.Settings.Count == 0) ? null : this.Settings;

            var type = Type.GetType(type_name);

            if (type == null)
                throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Type_Invalid, name, type_name));

            // Use the default constructor to create a new TraceListener.
            if (initialize == null)
            {
                var ctor = type.GetConstructor(new Type[0]);

                if (ctor != null)
                    _Listener = ctor.Invoke(new object[0]) as Listeners.TraceListener;
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
                    _Listener = ctor.Invoke(init) as Listeners.TraceListener;
            }

            if (_Listener == null)
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
                        ApplyDefaults(attr, prop);
                }
                else
                    ApplyDefaults(attr, prop);
            }

            // Call the Initialize method.
            _Listener.Initialize();

            return _Listener;
        }

        /// <summary>
        /// Apply the default attribute values to the property.
        /// </summary>
        /// <param name="attr">TraceSettingAttribute object.</param>
        /// <param name="prop">PropertyInfo object.</param>
        private void ApplyDefaults(TraceSettingAttribute attr, PropertyInfo prop)
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
                throw new Exceptions.ConfigurationListenerException(string.Format(Resources.Strings.Configuration_Exception_Listener_Setting_Required, this.Name, attr.Name));
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
