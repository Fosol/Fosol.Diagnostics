using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Collection of TraceFilter objects.
    /// Provides a way to validate all filters.
    /// </summary>
    public sealed class TraceFilterCollection
        : List<TraceFilter>
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Execute each TraceFilter.Validate() method to check if the TraceEvent should be sent to the TraceListeners.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        /// <returns>'True' if the TraceEvent should be sent to the TraceListeners.</returns>
        public bool Validate(TraceEvent trace)
        {
            var valid = true;
            foreach (var filter in this)
            {
                var validate = filter.Validate(trace);
                if (filter.Condition == FilterCondition.None && !validate)
                    valid = false;
                else if (filter.Condition == FilterCondition.And && !validate)
                    valid = false;
                else if (filter.Condition == FilterCondition.Xor && valid && validate)
                    valid = false;
                else if (filter.Condition == FilterCondition.Xor && !valid && validate)
                    valid = true;
                else if (filter.Condition == FilterCondition.Or && validate)
                    return true;
            }
            return valid;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
