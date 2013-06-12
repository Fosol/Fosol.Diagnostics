using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    internal class SourceElement
        : ConfigurationElement
    {
        #region Variables
        private const string _NameKey = "name";
        private const string _ListenersKey = "listeners";
        #endregion

        #region Properties
        [ConfigurationProperty(_NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[_NameKey]; }
            set { base[_NameKey] = value; }
        }

        [ConfigurationProperty(_ListenersKey)]
        public ListenerElementCollection Listeners
        {
            get { return (ListenerElementCollection)base[_ListenersKey]; }
            set { base[_ListenersKey] = value; }
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
