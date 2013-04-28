using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
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
        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        public DateTime DateTime
        {
            get { return _DateTime; }
        }

        public TraceEventType EventType
        {
            get { return _EventType; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public object Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        public TraceEventThread Thread
        {
            get { return _Thread; }
        }

        public TraceEventProcess Process
        {
            get { return _Process; }
        }

        public string StackTrace
        {
            get { return _StackTrace; }
        }

        public Stack ActivityStackTrace
        {
            get { return _ActivityStackTrace; }
        }
        #endregion

        #region Constructors
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
