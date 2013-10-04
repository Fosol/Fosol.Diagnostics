using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    internal class TraceElement
        : System.Configuration.ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Controls whether every write will automatically flush afterwards.
        /// </summary>
        [ConfigurationProperty("autoFlush", DefaultValue = false)]
        public bool AutoFlush
        {
            get { return (bool)this["autoFlush"]; }
            set { this["autoFlush"] = value; }
        }

        /// <summary>
        /// get/set - Controls whether it will attempt to automatically flush when the program exits.
        /// </summary>
        [ConfigurationProperty("flushOnExit", DefaultValue = false)]
        public bool FlushOnExit
        {
            get { return (bool)this["flushOnExit"]; }
            set { this["flushOnExit"] = value; }
        }

        /// <summary>
        /// get/set - Collection of TraceListener objects that listen for trace messages.
        /// </summary>
        [ConfigurationProperty("listeners")]
        public ListenerElementCollection Listeners
        {
            get { return (ListenerElementCollection)this["listeners"]; }
            set { this["listeners"] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods

        /// <summary>
        /// Add the DefaultListener to the collection.
        /// </summary>
        protected override void InitializeDefault()
        {
            base.InitializeDefault();
            InternalInitializeDefault();
        }

        /// <summary>
        /// Add the DebugListener to the collection if in DEBUG mode.
        /// </summary>
        internal void InternalInitializeDefault()
        {
#if DEBUG
            // Only add the DebugListener in debug mode.
            var default_listener = new ListenerElement()
            {
                Name = "Debug",
                ListenerTypeName = typeof(Fosol.Diagnostics.Listeners.DebugListener).FullName
            };
            this.Listeners.Add(default_listener);
#endif
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
