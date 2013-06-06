using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Filters
{
    public abstract class TraceFilter
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public abstract bool ShouldTrace(TraceEvent traceEvent);
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
