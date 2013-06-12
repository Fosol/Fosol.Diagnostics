using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// A TraceEvent contains all the information being sent to the listeners.
    /// </summary>
    public sealed class TraceEvent
    {
        #region Variables
        private string _Source;
        private string _Message;
        private System.DateTime _DateTime;
        private TraceEventType _EventType;
        private int _Id;
        private object _Data;
        private TraceEventThread _Thread;
        private TraceEventProcess _Process;
        private string _StackTrace;
        private Stack _ActivityStackTrace;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The source of the TraceEvent.
        /// </summary>
        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        /// <summary>
        /// get - When the TraceEvent was created.
        /// </summary>
        public DateTime DateTime
        {
            get { return _DateTime; }
        }

        /// <summary>
        /// get - The TraceEventType value.
        /// </summary>
        public TraceEventType EventType
        {
            get { return _EventType; }
        }

        /// <summary>
        /// get/set - The identity of the TraceEvent.
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        /// <summary>
        /// get/set - A message containing information about the TraceEvent.
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        /// <summary>
        /// get/set - Raw data to be included in the TraceEvent.
        /// </summary>
        public object Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        /// <summary>
        /// get - The thread that TraceEvent occured.
        /// </summary>
        public TraceEventThread Thread
        {
            get { return _Thread; }
        }

        /// <summary>
        /// get - The process the TraceEvent was created by.
        /// </summary>
        public TraceEventProcess Process
        {
            get { return _Process; }
        }

        /// <summary>
        /// get - A StackTrace containing more information about the reason for the TraceEvent.
        /// </summary>
        public string StackTrace
        {
            get { return _StackTrace; }
        }

        /// <summary>
        /// get - An activity StackTrace containing more information about the reason for the TraceEvent.
        /// </summary>
        public Stack ActivityStackTrace
        {
            get { return _ActivityStackTrace; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceEvent.
        /// </summary>
        /// <param name="eventType">TraceEventType value to identify serverity of the TraceEvent.</param>
        /// <param name="message">Information about the reason for the TraceEvent.</param>
        public TraceEvent(TraceEventType eventType, string message)
        {
            _DateTime = Fosol.Common.Optimization.FastDateTime.UtcNow;
            _EventType = eventType;
            _Message = message;
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
