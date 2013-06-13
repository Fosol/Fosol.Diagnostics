using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Filters
{
    /// <summary>
    /// A DefaultFilter is used when a TraceFilter is not configured.
    /// The ShouldTrace function will always return 'true'.
    /// </summary>
    public sealed class DefaultFilter
        : TraceFilter
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Returns 'true' always.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>'True' always.</returns>
        public override bool ShouldTrace(TraceEvent traceEvent)
        {
            return true;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
