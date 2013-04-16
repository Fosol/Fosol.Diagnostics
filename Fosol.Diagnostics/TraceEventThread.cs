using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public struct TraceEventThread
    {
        #region Variables
        private readonly int _Id;
        private readonly string _Name;
        private readonly ThreadPriority _Priority;
        #endregion

        #region Properties
        public int Id
        {
            get { return _Id; }
        }

        public string Name
        {
            get { return _Name; }
        }

        public ThreadPriority Priority
        {
            get { return _Priority; }
        }
        #endregion

        #region Constructors
        public TraceEventThread(Thread thread)
        {
            _Id = thread.ManagedThreadId;
            _Name = thread.Name;
            _Priority = thread.Priority;
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
