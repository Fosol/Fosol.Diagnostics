using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    public sealed class LogWriter
    {
        #region Variables
        #endregion

        #region Properties
        public TraceSource Source { get; private set; }
        #endregion

        #region Constructors
        public LogWriter(TraceSource source)
        {
            this.Source = source;
        }
        #endregion

        #region Methods
        public void Write(System.Diagnostics.TraceEventType level, int id, string message)
        {
            this.Source.TraceEvent(level, id, message);
        }

        public void Verbose(int id, string message)
        {
            Write(TraceEventType.Verbose, id, message);
        }

        public void Info(int id, string message)
        {
            Write(TraceEventType.Information, id, message);
        }

        public void Warn(int id, string message)
        {
            Write(TraceEventType.Warning, id, message);
        }

        public void Error(int id, string message)
        {
            Write(TraceEventType.Error, id, message);
        }

        public void Critical(int id, string message)
        {
            Write(TraceEventType.Critical, id, message);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
