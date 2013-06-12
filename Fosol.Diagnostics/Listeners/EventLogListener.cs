using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Provides a basic way to write to the EventLog.
    /// </summary>
    [TraceInitialize("Source", typeof(string))]
    public class EventLogListener
        : TraceListener
    {
        #region Variables
        private System.Diagnostics.EventLog _EventLog;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The EventLog that will be written to.
        /// </summary>
        public System.Diagnostics.EventLog EventLog
        {
            get { return _EventLog; }
            set { _EventLog = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an EventLogListener.
        /// </summary>
        public EventLogListener()
        {
        }

        /// <summary>
        /// Creates a new instance of an EventLogListener.
        /// </summary>
        /// <param name="eventLog">EventLog object to write to.</param>
        public EventLogListener(System.Diagnostics.EventLog eventLog)
        {
            _EventLog = eventLog;
        }

        /// <summary>
        /// Creates a new instance of an EventLogListener.
        /// </summary>
        /// <param name="source">Source name of the EventLog.</param>
        public EventLogListener(string source)
        {
            _EventLog = new System.Diagnostics.EventLog();
            _EventLog.Source = source;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write the TraceEvent to the EventLog.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected override void OnWrite(TraceEvent traceEvent)
        {
            if (this.EventLog != null)
            {
                var id = traceEvent.Id > 65535 ? 65535 : traceEvent.Id < 0 ? 0 : traceEvent.Id;
                var message = this.Render(traceEvent);
                this.EventLog.WriteEntry(message, ChooseEventLogEntryType(traceEvent.EventType), id);
            }
        }

        /// <summary>
        /// Close the EventLog.
        /// </summary>
        public override void Close()
        {
            if (this.EventLog != null)
            {
                this.EventLog.Close();
            }
        }

        /// <summary>
        /// Dispose the EventLog.
        /// </summary>
        public override void Dispose()
        {
            try
            {
                this.Close();
                this.EventLog = null;
            }
            finally
            {
                base.Dispose();
            }
        }

        /// <summary>
        /// Choose an appropiate EventLogEntryType based on the TraceEvent.EventType value.
        /// </summary>
        /// <param name="type">TraceEventType value.</param>
        /// <returns>System.Diagnostitics.EventLogEntryType value.</returns>
        private System.Diagnostics.EventLogEntryType ChooseEventLogEntryType(TraceEventType type)
        {
            switch (type)
            {
                case (TraceEventType.Critical):
                case (TraceEventType.Error):
                    return System.Diagnostics.EventLogEntryType.Error;
                case (TraceEventType.Warning):
                    return System.Diagnostics.EventLogEntryType.Warning;
                default:
                    return System.Diagnostics.EventLogEntryType.Information;
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
