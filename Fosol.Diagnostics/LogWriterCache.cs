using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Cache for LogWriter objects.
    /// The purpose is so that new instances of LogWriter are not created when they don't need to be.
    /// </summary>
    internal class LogWriterCache
        : IDisposable
    {
        #region Variables
        private static readonly Dictionary<string, WeakReference> _Cache = new Dictionary<string, WeakReference>();
        private ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();
        #endregion

        #region Properties
        /// <summary>
        /// get - All the keys within the cache.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return _Cache.Keys; }
        }

        /// <summary>
        /// get - The LogWriter with the specified cache key.
        /// </summary>
        /// <param name="key">Cache key name.</param>
        /// <returns>LogWriter if it exists.</returns>
        public LogWriter this[string key]
        {
            get
            {
                return Get(key);
            }
        }

        /// <summary>
        /// get - Number of LogWriters in cache.
        /// </summary>
        public int Count
        {
            get { return _Cache.Count; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Add a LogWriter to cache so that it doens't need to be recreated each time.
        /// </summary>
        /// <param name="key">Cache key value to identify LogWriter.</param>
        /// <param name="value">LogWriter object to add to cache.</param>
        public void Add(string key, LogWriter value)
        {
            _LockSlim.EnterWriteLock();
            try
            {
                _Cache.Add(key, new WeakReference(value));
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Get the LogWriter.
        /// </summary>
        /// <param name="key">Cache key value to identify LogWriter.</param>
        /// <returns>LogWriter object if found.</returns>
        public LogWriter Get(string key)
        {
            _LockSlim.EnterReadLock();
            try
            {
                return _Cache[key].Target as LogWriter;
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Determines whether the cache contains the key.
        /// </summary>
        /// <param name="key">Cache key to search for.</param>
        /// <returns>True if the key exists in cache.</returns>
        public bool ContainsKey(string key)
        {
            _LockSlim.EnterReadLock();
            try
            {
                return _Cache.ContainsKey(key);
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Removes the LogWriter with the specified cache key.
        /// </summary>
        /// <param name="key">Cache key value to identify LogWriter.</param>
        /// <returns>True if LogWriter existed and was removed.</returns>
        public bool Remove(string key)
        {
            _LockSlim.EnterWriteLock();
            try
            {
                return _Cache.Remove(key);
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Dispose this object.
        /// Clear cache object.
        /// </summary>
        public void Dispose()
        {
            Clear();
        }

        /// <summary>
        /// Attempt to get the LogWriter with the specified key name.
        /// </summary>
        /// <param name="key">Cache key name to identify the LogWriter.</param>
        /// <param name="value">LogWriter variable to propulate.</param>
        /// <returns>The LogWriter if it exists, or null.</returns>
        public bool TryGetValue(string key, out LogWriter value)
        {
            if (ContainsKey(key))
            {
                value = Get(key);
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Add a new LogWriter to the cache.
        /// </summary>
        /// <param name="item">KeyValuePair object with a LogWriter.</param>
        public void Add(KeyValuePair<string, LogWriter> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public void Clear()
        {
            _LockSlim.EnterWriteLock();
            try
            {
                _Cache.Clear();
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Check if the cache contains the specified LogWriter.
        /// </summary>
        /// <param name="item">KeyValuePair with a LogWriter.</param>
        /// <returns>True if the cache contains the LogWriter.</returns>
        public bool Contains(KeyValuePair<string, LogWriter> item)
        {
            return _Cache.Contains(new KeyValuePair<string, WeakReference>(item.Key, new WeakReference(item.Value)));
        }

        /// <summary>
        /// Remove the LogWriter form cache.
        /// </summary>
        /// <param name="item">KeyValuePair with LogWriter.</param>
        /// <returns>True if the LogWriter was removed.</returns>
        public bool Remove(KeyValuePair<string, LogWriter> item)
        {
            return Remove(item.Key);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
