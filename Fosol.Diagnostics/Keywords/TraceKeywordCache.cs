using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// Collection of Keyword objects related to a specific formatted string.
    /// </summary>
    internal class TraceKeywordCache
        : IDisposable
    {
        #region Variables
        private static readonly Dictionary<string, TraceKeywordBase> _Cache = new Dictionary<string, TraceKeywordBase>();
        private ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Add a TraceKeywordBase to cache so that it doens't need to be recreated each time.
        /// </summary>
        /// <param name="cacheKey">Cache key value to identify TraceKeywordBase.</param>
        /// <param name="keyword">TraceKeywordBase object to add to cache.</param>
        /// <returns>Number of items in Cache.</returns>
        public int Add(string cacheKey, TraceKeywordBase keyword)
        {
            _LockSlim.EnterWriteLock();
            try
            {
                _Cache.Add(cacheKey, keyword);
                return _Cache.Count;
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Get the TraceKeywordBase.
        /// If cacheKey does not exist it will return null.
        /// </summary>
        /// <param name="cacheKey">Cache key value to identify TraceKeywordBase.</param>
        /// <returns>TraceKeywordBase object if found, or null if not.</returns>
        public TraceKeywordBase Get(string cacheKey)
        {
            _LockSlim.EnterReadLock();
            try
            {
                return _Cache[cacheKey];
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Determines if the cache contains the key.
        /// </summary>
        /// <param name="cacheKey">Cache key to search for.</param>
        /// <returns>True if the cache key exists.</returns>
        public bool ContainsKey(string cacheKey)
        {
            _LockSlim.EnterReadLock();
            try
            {
                return _Cache.ContainsKey(cacheKey);
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Removes the TraceKeywordBase with the specified cache key.
        /// </summary>
        /// <param name="cacheKey">Cache key value to identify TraceKeywordBase.</param>
        /// <returns>True if TraceKeywordBase existed and was removed.</returns>
        public bool Remove(string cacheKey)
        {
            _LockSlim.EnterWriteLock();
            try
            {
                return _Cache.Remove(cacheKey);
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Dispose this object.
        /// Clear the cache of objects.
        /// </summary>
        public void Dispose()
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
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
