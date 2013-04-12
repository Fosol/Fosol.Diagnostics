using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    static class NativeMethods
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
