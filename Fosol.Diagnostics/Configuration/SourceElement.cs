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
        private const string NameKey = "name";
        private const string ListenersKey = "listeners";
        #endregion

        #region Properties
        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[NameKey]; }
            set { base[NameKey] = value; }
        }

        [ConfigurationProperty(ListenersKey)]
        public ListenerElementCollection Listeners
        {
            get { return (ListenerElementCollection)base[ListenersKey]; }
            set { base[ListenersKey] = value; }
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
