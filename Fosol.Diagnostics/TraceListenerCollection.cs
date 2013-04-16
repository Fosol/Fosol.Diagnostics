using Fosol.Diagnostics.Listeners;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public sealed class TraceListenerCollection
        : IList<TraceListener>, ICollection<TraceListener>, IEnumerable<TraceListener>
    {
        #region Variables
        private readonly System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        private readonly List<TraceListener> _Items;
        #endregion

        #region Properties

        public TraceListener this[int index]
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _Items[index];
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
                    _Items[index] = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public int Count
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _Items.Count;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        public TraceListenerCollection()
        {
            _Items = new List<TraceListener>();
        }
        #endregion

        #region Methods

        public int IndexOf(TraceListener item)
        {
            _Lock.EnterReadLock();
            try
            {
                return _Items.IndexOf(item);
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        public void Insert(int index, TraceListener item)
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.Insert(index, item);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public void RemoveAt(int index)
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.RemoveAt(index);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public void Add(TraceListener item)
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.Add(item);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public void Clear()
        {
            _Lock.EnterWriteLock();
            try
            {
                _Items.Clear();
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public bool Contains(TraceListener item)
        {
            _Lock.EnterReadLock();
            try
            {
                return _Items.Contains(item);
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        public void CopyTo(TraceListener[] array, int arrayIndex)
        {
            _Lock.EnterReadLock();
            try
            {
                _Items.CopyTo(array, arrayIndex);
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }

        public bool Remove(TraceListener item)
        {
            _Lock.EnterWriteLock();
            try
            {
                return _Items.Remove(item);
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        public IEnumerator<TraceListener> GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Items.GetEnumerator();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
