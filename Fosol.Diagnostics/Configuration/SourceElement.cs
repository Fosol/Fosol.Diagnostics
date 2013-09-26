using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    /// <summary>
    /// SourceElement provides a way to configure TraceListener objects for specific sources.
    /// </summary>
    internal class SourceElement
        : System.Configuration.ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A unique name to identify this source.
        /// </summary>
        [System.Configuration.ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// get/set - Collection of TraceListener objects that listen for trace messages.
        /// </summary>
        [System.Configuration.ConfigurationProperty("listeners")]
        public ListenerElementCollection Listeners
        {
            get { return (ListenerElementCollection)this["listeners"]; }
            set { this["listeners"] = value; }
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
