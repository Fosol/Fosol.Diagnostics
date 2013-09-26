using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// A TraceWriter provides a way to send messages to TraceListeners.
    /// </summary>
    public sealed class TraceWriter
    {
        #region Variables
        internal readonly System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        private readonly Type _SourceType;
        private readonly TraceData _Data;
        private readonly InstanceProcess _Process;
        private readonly InstanceThread _Thread;
        private readonly TraceManager _Manager;
        #endregion

        #region Properties
        /// <summary>
        /// get - The Type of the source which created the TraceWriter.
        /// </summary>
        public Type SourceType { get { return _SourceType; } }

        /// <summary>
        /// get - Additional information included with this TraceWriter.
        /// </summary>
        public TraceData Data { get { return _Data; } }

        /// <summary>
        /// get - The Process this TraceEvent was created with.
        /// </summary>
        public InstanceProcess Process { get { return _Process; } }

        /// <summary>
        /// get - The Thread this TraceEvent was created with.
        /// </summary>
        public InstanceThread Thread { get { return _Thread; } }

        /// <summary>
        /// get - Reference to the TraceManager this writer belongs to.
        /// </summary>
        internal TraceManager Manager
        {
            get { return _Manager; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceWriter object.
        /// </summary>
        /// <param name="manager">Reference to the TraceManager this writer belongs to.</param>
        /// <param name="source">Source object Type creating the TraceWriter.</param>
        /// <param name="data"></param>
        internal TraceWriter(TraceManager manager, Type source, TraceData data = null)
        {
            Fosol.Common.Validation.Assert.IsNotNull(manager, "manager");
            Fosol.Common.Validation.Assert.IsNotNull(source, "source");

            _Manager = manager;
            _SourceType = source;

            // From this point on the TraceData is readonly.
            if (data != null)
                data.IsReadonly = true;

            _Data = data;
            _Process = new InstanceProcess();
            _Thread = new InstanceThread();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the cache key for this TraceWriter.
        /// </summary>
        /// <returns>Unique cache key value.</returns>
        public string GetCacheKey()
        {
            return TraceWriter.GenerateCacheKey(this.SourceType, this.Data);
        }

        /// <summary>
        /// Generated a cache key for a TraceWriter.
        /// </summary>
        /// <param name="source">Source object Type creating the TraceWriter.</param>
        /// <returns>Unique cache key value.</returns>
        public static string GenerateCacheKey(Type source, TraceData data = null)
        {
            var sb = new StringBuilder();
            if (data != null)
            {
                foreach (var key in data.Keys)
                {
                    sb.Append(string.Format("|{0}={1}", key, data[key].ToString()));
                }
            }

            return string.Format("{0}{1}", source.FullName, sb.ToString());
        }

        /// <summary>
        /// Write TraceLevel.Information message to TraceListeners.
        /// </summary>
        /// <param name="message">Message to write to TraceListeners.</param>
        public void Write(string message)
        {
            Write(TraceLevel.Information, message);
        }

        /// <summary>
        /// Write message to TraceListeners.
        /// </summary>
        /// <param name="level">TraceLevel of message.</param>
        /// <param name="message">Message to write to TraceListeners.</param>
        public void Write(TraceLevel level, string message)
        {
            Write(new TraceEvent(this, level, message));
        }

        /// <summary>
        /// Send TraceEvent to the TraceListeners.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        public void Write(TraceEvent trace)
        {
            this.Manager.Write(trace);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
