using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    internal class TraceElement
        : ConfigurationElement
    {
        #region Variables
        private const string ListenersKey = "listeners";
        private const string AutoFlushKey = "autoFlush";
        private const string AutoRefreshKey = "autoRefresh";
        #endregion

        #region Properties

        [ConfigurationProperty(ListenersKey)]
        public ListenerElementCollection Listeners
        {
            get { return (ListenerElementCollection)base[ListenersKey]; }
            set { base[ListenersKey] = value; }
        }

        [ConfigurationProperty(AutoFlushKey, DefaultValue = false)]
        public bool AutoFlush
        {
            get { return (bool)base[AutoFlushKey]; }
            set { base[AutoFlushKey] = value; }
        }

        [ConfigurationProperty(AutoRefreshKey, DefaultValue = false)]
        public bool AutoRefresh
        {
            get { return (bool)base[AutoRefreshKey]; }
            set { base[AutoRefreshKey] = value; }
        }
        #endregion

        #region Constructors
        internal TraceElement()
        {
            this.AutoFlush = false;
            this.AutoRefresh = false;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
