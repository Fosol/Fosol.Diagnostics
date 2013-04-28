using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    internal interface IDynamicKeyword
        : ITraceKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        string Render(TraceEvent traceEvent);
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
