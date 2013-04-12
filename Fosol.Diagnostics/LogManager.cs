using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public sealed class LogManager
    {
        #region Variables
        private readonly static TraceSource _DefaultSource;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        static LogManager()
        {
            _DefaultSource = new TraceSource(GetApplicationName());
        }
        #endregion

        #region Methods
        public static LogWriter GetWriter()
        {
            return new LogWriter(_DefaultSource);
        }

        private static string GetApplicationName()
        {
            var assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
            {
                var module_file_name = new StringBuilder(260);
                var size = NativeMethods.GetModuleFileName(NativeMethods.NullHandleRef, module_file_name, module_file_name.Capacity);
                if (size > 0)
                {
                    return Path.GetFileNameWithoutExtension(module_file_name.ToString());
                }
            }

            return Path.GetFileNameWithoutExtension(assembly.EscapedCodeBase);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
