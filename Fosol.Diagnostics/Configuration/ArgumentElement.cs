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
        #endregion

        #region Properties
        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[NameKey]; }
            set { base[NameKey] = value; }
        }

        [ConfigurationProperty(ValueKey, IsRequired = true)]
        public string Value
        {
            get { return (string)base[ValueKey]; }
            set { base[ValueKey] = value; }
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
