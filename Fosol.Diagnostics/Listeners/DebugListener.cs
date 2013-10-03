using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// The DebugListener writes TraceEvents to the local machine debug listener.
    /// </summary>
    public sealed class DebugListener
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
        /// Write message to the local machine debug listener.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        protected override void OnWrite(TraceEvent trace)
        {
            var message = this.Render(trace);

            if (System.Diagnostics.Debugger.IsLogging())
            {
                System.Diagnostics.Debugger.Log(0, null, message);
            }

            Fosol.Common.Helpers.SafeNativeMethods.OutputDebugString(message);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
