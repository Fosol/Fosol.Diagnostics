using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    /// <summary>
    /// SettingElement provides a way to include initialization property values.
    /// </summary>
    internal class SettingElement
        : System.Configuration.ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A unique key name to identify the setting property.
        /// </summary>
        [System.Configuration.ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// get/set - The value of the setting property.
        /// </summary>
        [System.Configuration.ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
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
