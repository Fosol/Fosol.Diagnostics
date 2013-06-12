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
        private const string _NameKey = "name";
        private const string _ValueKey = "value";
        #endregion

        #region Properties
        [ConfigurationProperty(_NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[_NameKey]; }
            set { base[_NameKey] = value; }
        }

        [ConfigurationProperty(_ValueKey, IsRequired = true)]
        public string Value
        {
            get { return (string)base[_ValueKey]; }
            set { base[_ValueKey] = value; }
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
