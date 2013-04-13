using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Provides a central class to initialize LogWriter objects.
    /// </summary>
    public sealed class LogManager
        : IDisposable
    {
        #region Variables
        private static readonly LogFactory _Factory = new LogFactory();
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Get the default LogWriter for the application.
        /// </summary>
        /// <returns>LogWriter object.</returns>
        public static LogWriter GetWriter()
        {
            return _Factory.GetWriter();
        }

        /// <summary>
        /// Get a specific LogWriter (or create one).
        /// </summary>
        /// <param name="name">The unique name to identify the LogWriter source.</param>
        /// <returns>LogWriter object.</returns>
        public static LogWriter GetWriter(string name)
        {
            return _Factory.GetWriter(name);
        }

        /// <summary>
        /// Close all LogWriter objects.
        /// </summary>
        public void Close()
        {
            _Factory.Close();
        }

        /// <summary>
        /// Flush all LogWriter objects.
        /// </summary>
        public void Flush()
        {
            _Factory.Flush();
        }

        /// <summary>
        /// Dispose all LogWriter objects.
        /// </summary>
        public void Dispose()
        {
            _Factory.Dispose();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
