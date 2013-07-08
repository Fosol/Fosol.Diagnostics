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
    /// A FilterElement provides a way to dynamically configure which TraceEvent message are sent to the listeners.
    /// </summary>
    internal class FilterElement
        : ConfigurationElement
    {
        #region Variables
        private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();
        private const string _NameKey = "name";
        private const string _TypeNameKey = "type";
        private const string _InitializeKey = "initialize";
        private const string _SettingsKey = "settings";
        private Lazy<Filters.TraceFilter> _Filter;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A unique name to identify the TraceFilter.
        /// </summary>
        [ConfigurationProperty(_NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[_NameKey]; }
            set { base[_NameKey] = value; }
        }

        /// <summary>
        /// get/set - The Type of TraceFilter this configuration will create.
        /// </summary>
        [ConfigurationProperty(_TypeNameKey)]
        public string TypeName
        {
            get { return (string)base[_TypeNameKey]; }
            set { base[_TypeNameKey] = value; }
        }

        /// <summary>
        /// get/set - Initialization configuration items provide a way to construct the TraceFilter.
        /// </summary>
        [ConfigurationProperty(_InitializeKey)]
        public ArgumentElementCollection Initialize
        {
            get { return (ArgumentElementCollection)base[_InitializeKey]; }
            set { base[_InitializeKey] = value; }
        }

        /// <summary>
        /// get/set - Setting configuration items provide a way to intialize TraceFilter properties.
        /// </summary>
        [ConfigurationProperty(_SettingsKey)]
        public ArgumentElementCollection Settings
        {
            get { return (ArgumentElementCollection)base[_SettingsKey]; }
            set { base[_SettingsKey] = value; }
        }

        public Filters.TraceFilter Filter
        {
            get
            {
                return _Filter.Value;
            }
        }
        #endregion

        #region Constructors
        public FilterElement()
        {
            _Filter = new Lazy<Filters.TraceFilter>(() => GetFilter(), true);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the TraceFilter represented by this configuration.
        /// If it hasn't already been created it will create it.
        /// </summary>
        /// <returns>TraceFilter object.</returns>
        internal Filters.TraceFilter GetFilter()
        {
            var name = this.Name;
            var type_name = this.TypeName;

            // Annoyingly an empty TraceFilter is created even if one isn't configured.
            // So check for this and exit.
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type_name))
                type_name = "Fosol.Diagnostics.Filters.DefaultFilter, Fosol.Diagnostics";

            // This is a reference to a SharedFilter, so confirm that it exists.
            if (string.IsNullOrEmpty(type_name))
            {
                // When refrencing a SharedFilter you cannot include other configuration options.
                if (this.Initialize.Count != 0 || this.Settings.Count != 0)
                    throw new Exceptions.ConfigurationFilterException(string.Format(Resources.Strings.Configuration_Exception_Filter_Reference_Invalid_Properties, name));

                // The reference must exist in the shared listeners.
                if (TraceManager.Manager.SharedFilters == null)
                    throw new Exceptions.ConfigurationFilterException(string.Format(Resources.Strings.Configuration_Exception_Filter_Reference_Must_Exist, name));

                var shared = TraceManager.Manager.SharedFilters[name];
                if (shared == null)
                    throw new Exceptions.ConfigurationFilterException(string.Format(Resources.Strings.Configuration_Exception_Filter_Reference_Must_Exist, name));

                // Reference the shared listener.
                return shared.GetFilter();
            }
            // Try to create the listener as it has been defined in the configuration.
            else
            {
                // Initialize the listener with the arguments.
                return CreateFilter();
            }
        }

        /// <summary>
        /// Creates a new TraceFilter object based on this configuration.
        /// </summary>
        /// <returns>A new TraceFilter object.</returns>
        internal Filters.TraceFilter CreateFilter()
        {
            var name = this.Name;
            var type_name = this.TypeName;
            var initialize = (this.Initialize.Count == 0) ? null : this.Initialize;
            var settings = (this.Settings.Count == 0) ? null : this.Settings;

            var type = Type.GetType(type_name);
            Filters.TraceFilter filter = null;

            if (type == null)
                throw new Exceptions.ConfigurationFilterException(string.Format(Resources.Strings.Configuration_Exception_Filter_Type_Invalid, name, type_name));

            // Use the default constructor to create a new TraceFilter.
            if (initialize == null)
            {
                var ctor = type.GetConstructor(new Type[0]);

                if (ctor != null)
                    filter = ctor.Invoke(new object[0]) as Filters.TraceFilter;
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
                    filter = ctor.Invoke(init) as Filters.TraceFilter;
            }

            if (filter == null)
                throw new Exceptions.ConfigurationFilterException(string.Format(Resources.Strings.Configuration_Exception_Filter_Failed, this.Name));

            var pinfos = (
                from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                where p.GetCustomAttribute(typeof(TraceSettingAttribute), true) != null
                select p);

            foreach (var prop in pinfos)
            {
                var attr = prop.GetCustomAttribute(typeof(TraceSettingAttribute), true) as TraceSettingAttribute;

                // If property values were included in the configuration update the TraceFilter.
                if (settings != null)
                {
                    var config = settings.FirstOrDefault(p => p.Name.Equals(attr.Name, StringComparison.InvariantCulture));

                    // The configuration provides a default value so apply it to the property.
                    if (config != null)
                    {
                        if (attr.Converter != null)
                        {
                            prop.SetValue2(filter, config.Value, attr.Converter);
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            prop.SetValue(filter, config.Value);
                        }
                        else
                        {
                            // Convert the configuration value to the property type.
                            prop.SetValue2(filter, config.Value);
                        }
                    }
                    else if (attr.IsRequired)
                        throw new Exceptions.ConfigurationFilterException(string.Format(Resources.Strings.Configuration_Exception_Filter_Setting_Required, this.Name, attr.Name));
                    else
                        prop.ApplyDefaultValue(filter, attr.Converter);
                }
                else if (attr.IsRequired)
                    throw new Exceptions.ConfigurationFilterException(string.Format(Resources.Strings.Configuration_Exception_Filter_Setting_Required, this.Name, attr.Name));
                else
                    prop.ApplyDefaultValue(filter, attr.Converter);
            }

            // Call the Initialize method.
            filter.Initialize();

            return filter;
        }

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
