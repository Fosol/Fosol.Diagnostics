using Fosol.Common.Extensions.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Monitors the configuration file based on the entry assembly.
    /// </summary>
    public sealed class TraceConfigurationMonitor
    {
        #region Variables
        private FileSystemWatcher _Watcher;
        public EventHandler<FileSystemEventArgs> ConfigCreated;
        public EventHandler<FileSystemEventArgs> ConfigChanged;
        public EventHandler<FileSystemEventArgs> ConfigDeleted;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether changes should be monitored.
        /// </summary>
        public bool IsEnabled
        {
            get { return _Watcher.EnableRaisingEvents; }
            set { _Watcher.EnableRaisingEvents = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceConfigurationMonitor object.
        /// </summary>
        public TraceConfigurationMonitor()
            : this(Assembly.GetEntryAssembly().Location + ".config")
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceConfigurationMonitor object.
        /// </summary>
        /// <param name="configFilePath">Path to the configuration file.</param>
        public TraceConfigurationMonitor(string configFilePath)
            : this(configFilePath, true)
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceConfigurationMonitor object.
        /// </summary>
        /// <param name="configFilePath">Path to the configuration file.</param>
        /// <param name="isEnabled">Whether to enable the watcher.</param>
        public TraceConfigurationMonitor(string configFilePath, bool isEnabled)
        {
            Common.Validation.Assert.IsNotNullOrEmpty(configFilePath, "configFilePath");

            var path = Path.GetDirectoryName(configFilePath);
            var fileName = Path.GetFileName(configFilePath);

            _Watcher = new FileSystemWatcher(path, fileName);
            _Watcher.Changed += OnConfigChanged;
            _Watcher.Created += OnConfigCreated;
            _Watcher.Deleted += OnConfigDeleted;
            _Watcher.EnableRaisingEvents = isEnabled;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start monitoring changes.
        /// </summary>
        public void Start()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Stop monitoring changes.
        /// </summary>
        public void Stop()
        {
            IsEnabled = false;
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public void Dispose()
        {
            if (_Watcher != null)
            {
                _Watcher.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public static void RefreshAll()
        {
            var source = typeof(TraceSource);

            var method = source.GetMethod("RefreshAll", BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, null);
        }
        #endregion

        #region Operators
        #endregion

        #region Events

        /// <summary>
        /// The configuration file has been changed, refresh the trace settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfigChanged(object sender, FileSystemEventArgs e)
        {
            int retry_count = 0;
            while (retry_count < 3)
            {
                try
                {
                    Trace.Refresh();
                    RefreshAll();
                    break;
                }
                catch (Exception ex)
                {
                    // This is at the top of user code -- need to catch unhandled
                    // exceptions otherwise the application will crash.
                    // Can't rely on tracing (it's the bit that failed),
                    // so write to the error stream.
                    Console.Error.WriteLine(ex.ToString());

                    // TODO: Consider logging direct to Windows Event Log, if possible.
                    retry_count++;
                    Thread.Sleep(1);
                }
            }
            this.ConfigChanged.Raise(sender, e);
        }

        private void OnConfigCreated(object sender, FileSystemEventArgs e)
        {
            this.ConfigCreated.Raise(sender, e);
        }

        private void OnConfigDeleted(object sender, FileSystemEventArgs e)
        {
            this.ConfigDeleted.Raise(sender, e);
        }
        #endregion
    }
}
