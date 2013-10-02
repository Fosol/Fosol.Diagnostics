using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    /// <summary>
    /// Configuration section for Fosol.Diagnostics.
    /// </summary>
    internal class DiagnosticSection
        : ConfigurationSection
    {
        #region Variables
        public const string Name = "fosol.diagnostics";
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Namespace for xml configuration.
        /// </summary>
        [ConfigurationProperty("xmlns")]
        public string Xmlns
        {
            get { return (string)this["xmlns"]; }
            set { this["xmlns"] = value; }
        }

        /// <summary>
        /// get/set - Whether errors will be thrown as exceptions.
        /// </summary>
        [ConfigurationProperty("throwOnError")]
        public bool ThrowOnError
        {
            get { return (Boolean)this["throwOnError"]; }
            set { this["throwOnError"] = value; }
        }

        /// <summary>
        /// get/set - Configuration for generic trace messages.
        /// </summary>
        [ConfigurationProperty("trace")]
        public TraceElement Trace
        {
            get { return (TraceElement)this["trace"]; }
            set { this["trace"] = value; }
        }

        /// <summary>
        /// get/set - Shared filters.
        /// </summary>
        [ConfigurationProperty("sharedFilters")]
        public FilterElementCollection SharedFilters
        {
            get { return (FilterElementCollection)this["sharedFilters"]; }
            set { this["sharedFilters"] = value; }
        }

        /// <summary>
        /// get/set - Shared listeners.
        /// </summary>
        [ConfigurationProperty("sharedListeners")]
        public ListenerElementCollection SharedListeners
        {
            get { return (ListenerElementCollection)this["sharedListeners"]; }
            set { this["sharedListeners"] = value; }
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
