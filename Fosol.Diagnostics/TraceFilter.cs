using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Base abstract class for all TraceFilter objects.
    /// A TraceFilter provides a way to control what TraceEvents are sent to the TraceListeners.
    /// </summary>
    public abstract class TraceFilter
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A condition provides a way to control behavior with multiple TraceFilters.
        /// </summary>
        public FilterCondition Condition { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the TraceFilter object.
        /// </summary>
        protected internal virtual void Initialize()
        {

        }

        /// <summary>
        /// Validate whether the TraceEvent should be sent to the TraceListeners.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        /// <returns>'True' if the TraceEvent passes the filter.</returns>
        public bool Validate(TraceEvent trace)
        {
            return OnValidate(trace);
        }

        /// <summary>
        /// Validate whether the TraceEvent should be sent to the TraceListeners.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        /// <returns>'True' if the TraceEvent passes the filter.</returns>
        protected abstract bool OnValidate(TraceEvent trace);
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
