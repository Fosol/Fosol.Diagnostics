using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    [ConfigurationCollection(typeof(FilterElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    internal class FilterElementCollection
        : Fosol.Common.Configuration.ConfigurationElementCollection<FilterElement>
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Checks all the filters to confirm whether this TraceEvent should be sent to the listeners.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>'True' if the TraceEvent should be sent to the listeners.</returns>
        public bool ShouldTrace(TraceEvent traceEvent)
        {
            foreach (var filter in this)
            {
                if (!filter.GetFilter().ShouldTrace(traceEvent))
                    return false;
            }

            return true;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
