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
        private const string _ListenersKey = "listeners";
        private const string _AutoFlushKey = "autoFlush";
        private const string _FlushOnExitKey = "flushOnExit";
        #endregion

        #region Properties

        [ConfigurationProperty(_ListenersKey)]
        public ListenerElementCollection Listeners
        {
            get { return (ListenerElementCollection)base[_ListenersKey]; }
            set { base[_ListenersKey] = value; }
        }

        [ConfigurationProperty(_AutoFlushKey, DefaultValue = false)]
        public bool AutoFlush
        {
            get { return (bool)base[_AutoFlushKey]; }
            set { base[_AutoFlushKey] = value; }
        }

        [ConfigurationProperty(_FlushOnExitKey, DefaultValue = false)]
        public bool FlushOnExit
        {
            get { return (bool)base[_FlushOnExitKey]; }
            set { base[_FlushOnExitKey] = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of TraceElement.
        /// </summary>
        internal TraceElement()
        {
            this.AutoFlush = false;
            this.FlushOnExit = false;
        }
        #endregion

        #region Methods
        internal void InitializeDefaultInternal()
        {
            this.InitializeDefault();
        }

        protected override void InitializeDefault()
        {
            this.Listeners.InitializeDefaultInternal();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
