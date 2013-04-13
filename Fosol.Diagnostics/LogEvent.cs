using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// A LogEvent captures all the information sent as a Trace.
    /// </summary>
    public sealed class LogEvent
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Provides trace event data.
        /// </summary>
        public TraceEventCache EventCache { get; set; }

        /// <summary>
        /// get/set - The source of the trace (normally the application name).
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// get/set - Identifies the type of trace event.
        /// </summary>
        public TraceEventType EventType { get; set; }

        /// <summary>
        /// get/set - A numeric identity of the trace event.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - The message included in the trace event.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// get/set - An array of data included in the trace event.
        /// </summary>
        public object[] Data { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LogEvent object.
        /// </summary>
        /// <param name="eventCache">Provides trace event data.</param>
        /// <param name="source">The source of the trace (normally the application name).</param>
        /// <param name="eventType">Identifies the type of trace event.</param>
        /// <param name="id">A numeric identity of the trace event.</param>
        public LogEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            this.EventCache = eventCache;
            this.Source = source;
            this.EventType = eventType;
            this.Id = id;
        }

        /// <summary>
        /// Creates a new instance of a LogEvent object.
        /// </summary>
        /// <param name="eventCache">Provides trace event data.</param>
        /// <param name="source">The source of the trace (normally the application name).</param>
        /// <param name="eventType">Identifies the type of trace event.</param>
        /// <param name="id">A numeric identity of the trace event.</param>
        /// <param name="message">The message included in the trace event.</param>
        public LogEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
            : this(eventCache, source, eventType, id)
        {
            this.Message = message;
        }

        /// <summary>
        /// Creates a new instance of a LogEvent object.
        /// </summary>
        /// <param name="eventCache">Provides trace event data.</param>
        /// <param name="source">The source of the trace (normally the application name).</param>
        /// <param name="eventType">Identifies the type of trace event.</param>
        /// <param name="id">A numeric identity of the trace event.</param>
        /// <param name="messageOrFormat">The message included in the trace event, or the format string.</param>
        /// <param name="args">Additional arguments for a format.</param>
        public LogEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string messageOrFormat, params object[] args)
            : this(eventCache, source, eventType, id)
        {
            if (args != null && args.Length > 0)
                this.Message = string.Format(messageOrFormat, args);
            else
                this.Message = messageOrFormat;
        }

        /// <summary>
        /// Creates a new instance of a LogEvent object.
        /// </summary>
        /// <param name="eventCache">Provides trace event data.</param>
        /// <param name="source">The source of the trace (normally the application name).</param>
        /// <param name="eventType">Identifies the type of trace event.</param>
        /// <param name="id">A numeric identity of the trace event.</param>
        /// <param name="data">Data to include with the trace event.</param>
        public LogEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
            : this(eventCache, source, eventType, id, new object[1] { data })
        {
        }

        /// <summary>
        /// Creates a new instance of a LogEvent object.
        /// </summary>
        /// <param name="eventCache">Provides trace event data.</param>
        /// <param name="source">The source of the trace (normally the application name).</param>
        /// <param name="eventType">Identifies the type of trace event.</param>
        /// <param name="id">A numeric identity of the trace event.</param>
        /// <param name="data">An array of data to include with the trace event.</param>
        public LogEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
            : this(eventCache, source, eventType, id)
        {
            this.Data = data;
        }
        #endregion

        #region Methods
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
