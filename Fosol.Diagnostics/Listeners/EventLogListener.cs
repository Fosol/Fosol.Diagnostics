using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Provides a way to write TraceEvents to an EventLog.
    /// </summary>
    public sealed class EventLogListener
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
        /// Creates a new instance of a EventLogListener object.
        /// </summary>
        public EventLogListener()
        {

        }

        /// <summary>
        /// Creates a new instance of a EventLogListener object.
        /// </summary>
        /// <param name="eventLog">EventLog object to write TraceEvents to.</param>
        public EventLogListener(System.Diagnostics.EventLog eventLog)
        {
            _EventLog = eventLog;
        }

        /// <summary>
        /// Creates a new instance of a EventLogListener object.
        /// </summary>
        /// <param name="source">A source name to identify the EventLog.</param>
        public EventLogListener(string source)
        {
            _EventLog = new System.Diagnostics.EventLog();
            _EventLog.Source = source;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write a message to the EventLog.
        /// </summary>
        /// <param name="trace"></param>
        protected override void OnWrite(TraceEvent trace)
        {
            if (this.EventLog != null)
            {
                var id = trace.Id > 65535 ? 65535 : trace.Id < 0 ? 0 : trace.Id;
                var message = this.Render(trace);
                this.EventLog.WriteEntry(message, ChooseEventLogEntryType(trace.Level), id);
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
            base.Close();
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
        /// Choose an appropiate EventLogEntryType based on the TraceEvent.Level value.
        /// </summary>
        /// <param name="level">TraceLevel value.</param>
        /// <returns>System.Diagnostitics.EventLogEntryType value.</returns>
        private System.Diagnostics.EventLogEntryType ChooseEventLogEntryType(TraceLevel level)
        {
            switch (level)
            {
                case (TraceLevel.Critical):
                case (TraceLevel.Error):
                    return System.Diagnostics.EventLogEntryType.Error;
                case (TraceLevel.Warning):
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
