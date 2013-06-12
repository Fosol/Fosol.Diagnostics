using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// A TraceEventThread provides a way to capture thread information.
    /// </summary>
    public sealed class TraceEventThread
    {
        #region Variables
        private int _Id;
        private string _Name;
        private ThreadPriority _Priority;
        private ThreadState _State;
        #endregion

        #region Properties
        public int Id
        {
            get { return _Id; }
            private set { _Id = value; }
        }

        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

        public ThreadPriority Priority
        {
            get { return _Priority; }
            private set { _Priority = value; }
        }

        public ThreadState State
        {
            get { return _State; }
            private set { _State = value; }
        }
        #endregion

        #region Constructors
        public TraceEventThread()
        {
            var thread = Thread.CurrentThread;
            this.Id = thread.ManagedThreadId;
            this.Name = thread.Name;
            this.Priority = thread.Priority;
            this.State = thread.ThreadState;
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
