using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Filters
{
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
