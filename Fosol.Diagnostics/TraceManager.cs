using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Provides a central class to initialize LogWriter objects.
    /// </summary>
    public sealed class TraceManager
        : IDisposable
    {
        #region Variables
        private static readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();
        private static readonly TraceFactory _Factory = new TraceFactory();
        #endregion

        #region Properties
        internal static TraceFactory Factory
        {
            get { return _Factory; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Get the default LogWriter for the application.
        /// </summary>
        /// <returns>LogWriter object.</returns>
        public static TraceWriter GetWriter()
        {
            return _Factory.GetWriter();
        }

        /// <summary>
        /// Get a specific LogWriter (or create one).
        /// </summary>
        /// <param name="name">The unique name to identify the LogWriter source.</param>
        /// <returns>LogWriter object.</returns>
        public static TraceWriter GetWriter(string name)
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
