using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    internal class ArgumentElement
        : ConfigurationElement
    {
        #region Variables
        private const string NameKey = "name";
        private const string ValueKey = "value";
        private const string TypeNameKey = "type";
        private const string ConverterTypeNameKey = "converter";
        #endregion

        #region Properties
        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[NameKey]; }
            set { base[NameKey] = value; }
        }

        [ConfigurationProperty(ValueKey, IsRequired = true, IsKey = true)]
        public string Value
        {
            get { return (string)base[ValueKey]; }
            set { base[ValueKey] = value; }
        }

        [ConfigurationProperty(TypeNameKey)]
        public string TypeName
        {
            get { return (string)base[TypeNameKey]; }
            set { base[TypeNameKey] = value; }
        }

        [ConfigurationProperty(ConverterTypeNameKey)]
        public string ConverterName
        {
            get { return (string)base[ConverterTypeNameKey]; }
            set { base[ConverterTypeNameKey] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
