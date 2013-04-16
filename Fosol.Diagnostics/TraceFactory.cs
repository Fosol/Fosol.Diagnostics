using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Internal class that maintains all the LogWriters.
    /// </summary>
    sealed class TraceFactory
        : IDisposable
    {
        #region Variables
        private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();
        private readonly TraceWriterCache _Writers = new TraceWriterCache();
        private readonly string _ApplicationName;
        private readonly static TraceConfigurationMonitor _Monitor = new TraceConfigurationMonitor();
        private readonly TraceListenerCollection _Listeners;
        private volatile bool _AutoFlush;
        private volatile bool _IncludeStackTrace;
        private volatile bool _IncludeThreadInfo;
        private volatile bool _IncludeProcessInfo;
        #endregion

        #region Properties
        public TraceWriterCache Writers
        {
            get
            {
                return _Writers;
            }
        }

        public bool AutoFlush
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _AutoFlush;
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
                    _AutoFlush = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public bool IncludeStackTrace
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _IncludeStackTrace;
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
                    _IncludeStackTrace = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public bool IncludeThreadInfo
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _IncludeThreadInfo;
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
                    _IncludeThreadInfo = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public bool IncludeProcessInfo
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _IncludeProcessInfo;
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
                    _IncludeProcessInfo = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public TraceListenerCollection Listeners
        {
            get { return _Listeners; }
        }
        #endregion

        #region Constructors
        public TraceFactory()
        {
            _ApplicationName = GetApplicationName();
            _Monitor.Start();
        }
        #endregion

        #region Methods
        public TraceWriter GetWriter()
        {
            return GetWriter(_ApplicationName);
        }

        public TraceWriter GetWriter(string name)
        {
            Common.Validation.Assert.IsNotNullOrEmpty(name, "name");

            if (this.Writers.ContainsKey(name))
            {
                var cache = this.Writers.Get(name);
                if (cache != null)
                    return cache;
            }

            // Create a new LogWriter and add it to cache.
            var writer = new TraceWriter(this, name);
            this.Writers.Add(name, writer);
            return writer;
        }

        private static string GetApplicationName()
        {
            var assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
            {
                var module_file_name = new StringBuilder(260);
                var size = NativeMethods.GetModuleFileName(NativeMethods.NullHandleRef, module_file_name, module_file_name.Capacity);
                if (size > 0)
                {
                    return Path.GetFileNameWithoutExtension(module_file_name.ToString());
                }
            }

            return Path.GetFileNameWithoutExtension(assembly.EscapedCodeBase);
        }

        public void Write(TraceEvent trace)
        {
            if (this.Listeners != null && this.Listeners.Count > 0)
            {
                int i = 0;
                while (i < this.Listeners.Count)
                {
                    var listener = this.Listeners[i];

                    if (!listener.IsThreadSafe)
                    {
                        lock (listener)
                        {
                            listener.Write(trace);

                            if (this.AutoFlush)
                                listener.Flush();
                        }
                    }
                    else
                    {
                        listener.Write(trace);

                        if (this.AutoFlush)
                            listener.Flush();
                    }
                }
            }
        }

        public void Close()
        {
            foreach (var key in this.Writers.Keys)
            {
                this.Writers[key].Close();
            }
        }

        public void Flush()
        {
            foreach (var key in this.Writers.Keys)
            {
                this.Writers[key].Flush();
            }
        }

        public void Dispose()
        {
            foreach (var key in this.Writers.Keys)
            {
                this.Writers[key].Dispose();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
