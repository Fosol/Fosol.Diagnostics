using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// The DefaultListener sends messages to the Operating System OuputDebugString endpoint.
    /// </summary>
    public sealed class DefaultListener
        : TraceListener
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Write message to Operating System OutputDebugString endpoint.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected override void OnWrite(TraceEvent traceEvent)
        {
            var message = this.Render(traceEvent);
            if (Debugger.IsLogging())
                Debugger.Log(0, null, message);
            else if (message == null)
                Common.Helpers.SafeNativeMethods.OutputDebugString(string.Empty);
            else
                Common.Helpers.SafeNativeMethods.OutputDebugString(message);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
