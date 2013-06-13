using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Filters
{
    /// <summary>
    /// A TraceFilter is an abstract base class for all filters.
    /// </summary>
    public abstract class TraceFilter
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Returns whether the TraceEvent should be sent to the listeners.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>'True' if the TraceEvent should be sent to the listeners.</returns>
        public abstract bool ShouldTrace(TraceEvent traceEvent);

        /// <summary>
        /// Perform initialization after the TraceFilter has been created.
        /// </summary>
        public virtual void Initialize()
        {
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
