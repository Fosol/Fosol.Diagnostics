using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// A LogEvent captures all the information sent as a Trace.
    /// </summary>
    public sealed class TraceEvent
    {
        #region Variables
        private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();
        private readonly TraceEventType _EventType;
        private readonly DateTime _DateTime;
        private readonly string _Source;
        private int _Id;
        private string _Message;
        private object _Data;
        private TraceEventThread _Thread;
        private TraceEventProcess _Process;
        private string _StackTrace;
        private Stack _ActivityStackTrace;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The source of the trace (normally the application name).
        /// </summary>
        public string Source 
        {
            get
            {
                return _Source;
            }
        }

        /// <summary>
        /// get/set - A numeric identity of the trace event.
        /// </summary>
        public int Id
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _Id;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            set
            {
                _Lock.EnterWriteLock();
                try
                {
                    _Id = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get - Identifies the type of trace event.
        /// </summary>
        public TraceEventType EventType
        {
            get
            {
                return _EventType;
            }
        }

        /// <summary>
        /// get - When the trace event was created.
        /// </summary>
        public DateTime DateTime
        {
            get { return _DateTime; }
        }

        /// <summary>
        /// get/set - The message included in the trace event.
        /// </summary>
        public string Message
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _Message;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            set
            {
                _Lock.EnterWriteLock();
                try
                {
                    _Message = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get/set - Data included in the trace event.
        /// </summary>
        public object Data
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _Data;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            set
            {
                _Lock.EnterWriteLock();
                try
                {
                    _Data = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get - The thread information for the trace event.
        /// </summary>
        public TraceEventThread Thread
        {
            get { return _Thread; }
        }

        /// <summary>
        /// get - The process information for the trace event.
        /// </summary>
        public TraceEventProcess Process
        {
            get { return _Process; }
        }

        /// <summary>
        /// get - Provides stack trace event data.
        /// </summary>
        public string StackTrace
        {
            get { return _StackTrace; }
        }

        /// <summary>
        /// get - Provides the activity stack trace event data.
        /// </summary>
        public Stack ActivityStackTrace
        {
            get { return _ActivityStackTrace; }
        }
        #endregion

        #region Constructors
        public TraceEvent(TraceEventType eventType)
            : this(eventType, null, 0, null, null)
        {
        }

        public TraceEvent(TraceEventType eventType, int id)
            : this(eventType, null, id, null, null)
        {
        }

        public TraceEvent(TraceEventType eventType, int id, string message)
            : this(eventType, null, id, message, null)
        {
        }

        public TraceEvent(TraceEventType eventType, int id, string message, object data)
            : this(eventType, null, id, message, data)
        {
        }

        public TraceEvent(TraceEventType eventType, string message)
            : this(eventType, null, 0, message, null)
        {
        }

        public TraceEvent(TraceEventType eventType, string message, object data)
            : this(eventType, null, 0, message, data)
        {
        }

        public TraceEvent(TraceEventType eventType, object data)
            : this(eventType, null, 0, null, data)
        {
        }

        public TraceEvent(TraceEventType eventType, string source, int id, string message, object data)
        {
            _DateTime = Fosol.Common.Optimization.FastDateTime.UtcNow;
            _EventType = eventType;
            _Source = source;

            this.Id = id;
            this.Message = message;
            this.Data = data;
        }
        #endregion

        #region Methods
        internal void Initialize()
        {
            if (TraceManager.Factory.IncludeThreadInfo)
                _Thread = new TraceEventThread(System.Threading.Thread.CurrentThread);

            if (TraceManager.Factory.IncludeProcessInfo)
                _Process = new TraceEventProcess(System.Diagnostics.Process.GetCurrentProcess());

            if (TraceManager.Factory.IncludeStackTrace)
                _StackTrace = GetCallingStackTrace();
        }

        public static string GetCallingStackTrace()
        {
            return Environment.StackTrace;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
