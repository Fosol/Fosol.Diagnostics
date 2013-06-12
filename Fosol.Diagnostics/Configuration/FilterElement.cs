using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    internal class FilterElement
        : ConfigurationElement
    {
        #region Variables
        private const string _NameKey = "name";
        private const string _TypeNameKey = "type";
        private const string _InitializeKey = "initialize";
        private const string _SettingsKey = "settings";
        private Filters.TraceFilter _Filter;
        #endregion

        #region Properties
        [ConfigurationProperty(_NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[_NameKey]; }
            set { base[_NameKey] = value; }
        }

        [ConfigurationProperty(_TypeNameKey, IsRequired = true, IsKey = true)]
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
        #endregion

        #region Constructors
        #endregion

        #region Methods
        internal Filters.TraceFilter GetFilter()
        {
            if (_Filter != null)
                return _Filter;

            var name = this.Name;
            var type_name = this.TypeName;
            var initialize = (this.Initialize.Count == 0) ? null : this.Initialize;
            var settings = (this.Settings.Count == 0) ? null : this.Settings;

            // Use the default filter.
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type_name))
                type_name = "Fosol.Diagnostics.Filters.DefaultFilter, Fosol.Diagnostics";

            var type = Type.GetType(type_name);

            if (type == null)
                throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Filter_Type_Invalid, name, type_name));

            try
            {
                if (initialize != null)
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
                    {
                        _Filter = ctor.Invoke(init) as Filters.TraceFilter;
                        return _Filter;
                    }
                    else
                        throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Filter_Initialize_Invalid, this.Name));
                }
                else
                {
                    // Initialize the filter without any arguments.
                    _Filter = Fosol.Common.Helpers.ReflectionHelper.ConstructObject<Filters.TraceFilter>(type_name);
                    return _Filter;
                }
            }
            catch
            {
                throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Filter_Initialize_Invalid, this.Name));
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
