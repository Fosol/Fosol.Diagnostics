using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    /// <summary>
    /// FilterElement provides a way to configure TraceFilter objects.
    /// </summary>
    internal class FilterElement
        : System.Configuration.ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A unique name to identify TraceFilter.
        /// </summary>
        [System.Configuration.ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// get/set - The Type name of TraceFilter.
        /// </summary>
        [System.Configuration.ConfigurationProperty("type")]
        public string FilterTypeName
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        /// <summary>
        /// get/set - Control how this filter will work with other filters.
        /// </summary>
        [System.Configuration.ConfigurationProperty("condition", IsRequired = false, DefaultValue = FilterCondition.None)]
        [TypeConverter(typeof(Fosol.Common.Converters.EnumConverter<FilterCondition>))]
        public FilterCondition Condition
        {
            get { return (FilterCondition)this["condition"]; }
            set { this["condition"] = value; }
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
        /// Merge the sharedFilter with this FilterElement.
        /// Settings within this FilterElement will override the sharedFilter settings.
        /// </summary>
        /// <param name="sharedFilter">Shared FilterElement configuration.</param>
        public void Merge(FilterElement sharedFilter)
        {
            this.FilterTypeName = sharedFilter.FilterTypeName;

            // Use the shared filter condition configuration.
            if (this.Condition == FilterCondition.None)
            {
                // Change the condition None to And.
                if (sharedFilter.Condition == FilterCondition.None)
                    sharedFilter.Condition = FilterCondition.And;

                this.Condition = sharedFilter.Condition;
            }

            // Add additional settings from the shared filter.
            foreach (var setting in sharedFilter.Settings)
            {
                // Only add the settings from the sharedFilter if it doesn't already exist here.
                if (this.Settings.FirstOrDefault(s => s.Name.Equals(setting.Name)) == null)
                    this.Settings.Add(setting);
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
