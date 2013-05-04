using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
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
        public override void Write(string message)
        {
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
