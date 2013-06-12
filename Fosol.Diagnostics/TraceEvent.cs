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
        private TraceEventType _EventType;
        private System.DateTime _DateTime;
        private int _Id;
        private string _Source;
        private Type _SourceType;
        private string _Message;
        private object _Data;
        private TraceEventThread _Thread;
        private TraceEventProcess _Process;
        private string _StackTrace;
        private Stack _ActivityStackTrace;
        #endregion

        #region Properties
        /// <summary>
        /// get - The TraceEventType value.
        /// </summary>
        public TraceEventType EventType
        {
            get { return _EventType; }
            private set { _EventType = value; }
        }

        /// <summary>
        /// get - When the TraceEvent was created.
        /// </summary>
        public DateTime DateTime
        {
            get { return _DateTime; }
            private set { _DateTime = value; }
        }

        /// <summary>
        /// get - The identity of the TraceEvent.
        /// </summary>
        public int Id
        {
            get { return _Id; }
            private set { _Id = value; }
        }

        /// <summary>
        /// get - The source of the TraceEvent.
        /// </summary>
        public string Source
        {
            get { return _Source; }
            private set { _Source = value; }
        }

        /// <summary>
        /// get - The originating source type.
        /// </summary>
        public Type SourceType
        {
            get { return _SourceType; }
            private set { _SourceType = value; }
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
            private set { _Thread = value; }
        }

        /// <summary>
        /// get - The process the TraceEvent was created by.
        /// </summary>
        public TraceEventProcess Process
        {
            get { return _Process; }
            private set { _Process = value; }
        }

        /// <summary>
        /// get - A StackTrace containing more information about the reason for the TraceEvent.
        /// </summary>
        public string StackTrace
        {
            get { return _StackTrace; }
            private set { _StackTrace = value; }
        }

        /// <summary>
        /// get - An activity StackTrace containing more information about the reason for the TraceEvent.
        /// </summary>
        public Stack ActivityStackTrace
        {
            get { return _ActivityStackTrace; }
            private set { _ActivityStackTrace = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceEvent.
        /// </summary>
        /// <param name="eventType">TraceEventType value to identify serverity of the TraceEvent.</param>
        /// <param name="message">Information about the reason for the TraceEvent.</param>
        public TraceEvent(TraceEventType eventType, string message)
            : this(eventType, 0, null, null, message)
        {
        }

        /// <summary>
        /// Creates a new instance of TraceEvent.
        /// </summary>
        /// <param name="eventType">TraceEventType value to identify serverity of the TraceEvent.</param>
        /// <param name="id">Unique id to identify this type of message.</param>
        /// <param name="message">Information about the reason for the TraceEvent.</param>
        public TraceEvent(TraceEventType eventType, int id, string message)
            : this(eventType, id, null, null, message)
        {
        }

        /// <summary>
        /// Creates a new instance of TraceEvent.
        /// </summary>
        /// <param name="eventType">TraceEventType value to identify serverity of the TraceEvent.</param>
        /// <param name="source">Source of the message and filter for listeners.</param>
        /// <param name="message">Information about the reason for the TraceEvent.</param>
        public TraceEvent(TraceEventType eventType, string source, string message)
            : this(eventType, 0, source, null, message)
        {
        }

        /// <summary>
        /// Creates a new instance of TraceEvent.
        /// </summary>
        /// <param name="eventType">TraceEventType value to identify serverity of the TraceEvent.</param>
        /// <param name="id">Unique id to identify this type of message.</param>
        /// <param name="source">Source of the message and filter for listeners.</param>
        /// <param name="message">Information about the reason for the TraceEvent.</param>
        public TraceEvent(TraceEventType eventType, int id, string source, string message)
            : this(eventType, id, source, null, message)
        {
        }

        /// <summary>
        /// Creates a new instance of TraceEvent.
        /// </summary>
        /// <param name="eventType">TraceEventType value to identify serverity of the TraceEvent.</param>
        /// <param name="id">Unique id to identify this type of message.</param>
        /// <param name="source">Source of the message and filter for listeners.</param>
        /// <param name="sourceType">The source Type that is sending this message.</param>
        /// <param name="message">Information about the reason for the TraceEvent.</param>
        public TraceEvent(TraceEventType eventType, int id, string source, Type sourceType, string message)
        {
            this.DateTime = Fosol.Common.Optimization.FastDateTime.UtcNow;
            this.EventType = eventType;
            this.Id = id;
            this.Source = source;
            this.SourceType = sourceType;
            this.Message = message;
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
