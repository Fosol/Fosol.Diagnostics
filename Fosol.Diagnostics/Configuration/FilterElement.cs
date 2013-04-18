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
        private const string NameKey = "name";
        private const string TypeNameKey = "type";
        private const string ArgsKey = "args";
        private TraceFilter _Filter;
        #endregion

        #region Properties
        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[NameKey]; }
            set { base[NameKey] = value; }
        }

        [ConfigurationProperty(TypeNameKey, IsRequired = true, IsKey = true)]
        public string TypeName
        {
            get { return (string)base[TypeNameKey]; }
            set { base[TypeNameKey] = value; }
        }

        [ConfigurationProperty(ArgsKey)]
        public ArgumentElementCollection Args
        {
            get { return (ArgumentElementCollection)base[ArgsKey]; }
            set { base[ArgsKey] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        internal TraceFilter GetFilter()
        {
            if (_Filter != null)
                return _Filter;

            var type_name = this.TypeName;
            var args = this.Args;

            try
            {
                if (args != null && args.Count > 0)
                {
                    // Initialize the filter with the arguments.
                    var largs = args.GetArguments(this.Name).Select(a => a.Value).ToArray();
                    _Filter = Fosol.Common.Helpers.ReflectionHelper.ConstructObject<TraceFilter>(type_name, largs);
                    return _Filter;
                }
                else
                {
                    // Initialize the filter without any arguments.
                    _Filter = Fosol.Common.Helpers.ReflectionHelper.ConstructObject<TraceFilter>(type_name);
                    return _Filter;
                }
            }
            catch
            {
                throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Filter_Arguments_Invalid, this.Name));
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
