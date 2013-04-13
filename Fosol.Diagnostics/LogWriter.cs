using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Sends trace event messages to a specified TraceSource.
    /// </summary>
    public sealed class LogWriter
        : IDisposable
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The TraceSource this LogWriter sends trace events to.
        /// </summary>
        public TraceSource Source { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LogWriter object.
        /// </summary>
        /// <param name="source">TraceSource object to use in this LogWriter.</param>
        public LogWriter(TraceSource source)
        {
            this.Source = source;
        }

        /// <summary>
        /// Creates a new instance of a LogWriter object.
        /// </summary>
        /// <param name="name">The name of the source.</param>
        public LogWriter(string name)
        {
            this.Source = new TraceSource(name);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write the message to the TraceSource.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void Write(System.Diagnostics.TraceEventType level, int id, string message)
        {
            this.Source.TraceEvent(level, id, message);
        }

        /// <summary>
        /// Write a verbose message to the TraceSource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void Verbose(int id, string message)
        {
            Write(TraceEventType.Verbose, id, message);
        }

        /// <summary>
        /// Write an information message to the TraceSource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void Info(int id, string message)
        {
            Write(TraceEventType.Information, id, message);
        }

        /// <summary>
        /// Write a warning message to the TraceSource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void Warn(int id, string message)
        {
            Write(TraceEventType.Warning, id, message);
        }

        /// <summary>
        /// Write an error message to the TraceSource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void Error(int id, string message)
        {
            Write(TraceEventType.Error, id, message);
        }

        /// <summary>
        /// Write a critical message to the TraceSource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void Critical(int id, string message)
        {
            Write(TraceEventType.Critical, id, message);
        }

        /// <summary>
        /// Close the TraceSource.
        /// </summary>
        public void Close()
        {
            this.Source.Close();
        }

        /// <summary>
        /// Flush the TraceSource.
        /// </summary>
        public void Flush()
        {
            this.Source.Flush();
        }
        
        /// <summary>
        /// Dispose the LogWriter.
        /// </summary>
        public void Dispose()
        {
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
