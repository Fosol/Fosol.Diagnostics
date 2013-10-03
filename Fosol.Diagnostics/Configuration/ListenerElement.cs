using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    /// <summary>
    /// ListenerElement provides a way to configure a TraceListener object.
    /// </summary>
    internal class ListenerElement
        : System.Configuration.ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A unique name to identify TraceListener.
        /// </summary>
        [System.Configuration.ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// get/set - The Type of TraceListener.
        /// </summary>
        [System.Configuration.ConfigurationProperty("type")]
        public string ListenerTypeName
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        /// <summary>
        /// get/set - Collection of FilterElement objects.
        /// </summary>
        [System.Configuration.ConfigurationProperty("filters")]
        public FilterElementCollection Filters
        {
            get { return (FilterElementCollection)this["filters"]; }
            set { this["filters"] = value; }
        }

        /// <summary>
        /// get/set - SettingElementCollection for constructing a new TraceListener.
        /// </summary>
        [System.Configuration.ConfigurationProperty("constructor")]
        public SettingElementCollection Constructor
        {
            get { return (SettingElementCollection)this["constructor"]; }
            set { this["constructor"] = value; }
        }

        /// <summary>
        /// get/set - Collection of SettingElement objects.
        /// </summary>
        [System.Configuration.ConfigurationProperty("", IsDefaultCollection = true)]
        public SettingElementCollection Settings
        {
            get { return (SettingElementCollection)this[""]; }
            set { this[""] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Merge the sharedFilter with this ListenerElement.
        /// Settings within this ListenerElement will override the sharedFilter settings.
        /// </summary>
        /// <param name="sharedListener">Shared ListenerElement configuration.</param>
        public void Merge(ListenerElement sharedListener)
        {
            this.ListenerTypeName = sharedListener.ListenerTypeName;

            // Add additional settings from the shared listener.
            foreach (var setting in sharedListener.Settings)
            {
                // Only add the settings from the sharedListener if it doesn't already exist here.
                if (this.Settings.FirstOrDefault(s => s.Name.Equals(setting.Name)) == null)
                    this.Settings.Add(setting);
            }

            // Add additional filters from the shared listener.
            foreach (var filter in sharedListener.Filters)
            {
                var local_filter = this.Filters.FirstOrDefault(f => f.Name.Equals(filter.Name));

                // If a local filter exists with the same name as the shared filter merge it, otherwise add it.
                if (local_filter != null)
                {
                    local_filter.Merge(filter);
                }
                else
                    this.Filters.Add(filter);
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
