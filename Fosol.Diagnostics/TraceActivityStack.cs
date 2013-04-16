using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public sealed class TraceActivityStack
    {
        #region Variables
        private readonly System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        private Guid _CurrentActivityId;
        private Stack _ActivityStack;
        #endregion

        #region Properties
        public Guid CurrentActivityId
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    if (_CurrentActivityId == null)
                        return Guid.Empty;

                    return _CurrentActivityId;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            private set
            {
                _Lock.EnterWriteLock();
                try
                {
                    _CurrentActivityId = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public Stack ActivityStack
        {
            get
            {
                _Lock.EnterUpgradeableReadLock();
                try
                {
                    if (_ActivityStack == null)
                    {
                        _Lock.EnterWriteLock();
                        try
                        {
                            _ActivityStack = new Stack();
                        }
                        finally
                        {
                            _Lock.ExitWriteLock();
                        }
                    }

                    return _ActivityStack;
                }
                finally
                {
                    _Lock.ExitUpgradeableReadLock();
                }
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void Start()
        {
            Start(Guid.NewGuid());
        }

        public void Start(object activityId)
        {
            Common.Validation.Assert.IsNotNull(activityId, "activityId");

            if (activityId is Guid)
            {
                var id = (Guid)activityId;
                this.CurrentActivityId = id;
                this.ActivityStack.Push(new TraceActivity(id));
            }
            else if (activityId is TraceActivity)
            {
                var activity = (TraceActivity)activityId;
                this.CurrentActivityId = activity.Id;
                this.ActivityStack.Push(activity);
            }
            else
            {
                var activity = new TraceActivity(Guid.NewGuid(), activityId);
                this.CurrentActivityId = activity.Id;
                this.ActivityStack.Push(activity);
            }
        }

        public void Stop()
        {
            this.ActivityStack.Pop();

            _Lock.EnterWriteLock();
            try
            {
                if (_ActivityStack.Count > 0)
                {
                    var activity = (TraceActivity)this.ActivityStack.Peek();
                    _CurrentActivityId = activity.Id;
                }
                else
                    _CurrentActivityId = Guid.Empty;
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
