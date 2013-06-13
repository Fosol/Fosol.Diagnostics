using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// The TraceManager provides a single point of control for diagnostics.
    /// </summary>
    public sealed class TraceManager
        : IDisposable
    {
        #region Variables
        private const string _DefaultWriterKey = "fosol.diagnostics.default.writer";
        private static TraceManager _Manager;
        private Fosol.Common.Configuration.ConfigurationSectionWatcher<Configuration.DiagnosticsSection> _ConfigWatcher;
        private readonly Fosol.Common.Caching.SimpleCache<TraceWriter> _TraceWriterCache = new Common.Caching.SimpleCache<TraceWriter>();
        #endregion

        #region Properties
        /// <summary>
        /// get - Global TraceManager.
        /// </summary>
        internal static TraceManager Manager
        {
            get { return _Manager; }
        }

        /// <summary>
        /// get - Configuration settings.
        /// </summary>
        internal Configuration.DiagnosticsSection Configuration
        {
            get { return _ConfigWatcher != null ? _ConfigWatcher.Section : null; }
        }

        /// <summary>
        /// get - Shared listeners within the configuration.
        /// </summary>
        internal Configuration.ListenerElementCollection SharedListeners
        {
            get { return this.Configuration != null ? this.Configuration.SharedListeners : null; }
        }

        /// <summary>
        /// get - Shared filters within the configuration.
        /// </summary>
        internal Configuration.FilterElementCollection SharedFilters
        {
            get { return this.Configuration != null ? this.Configuration.SharedFilters : null; }
        }

        /// <summary>
        /// get - Source listeners within the configuration.
        /// </summary>
        internal Configuration.SourceElementCollection Sources
        {
            get { return this.Configuration != null ? this.Configuration.Sources : null; }
        }

        /// <summary>
        /// get - Generic trace within the configuration.
        /// </summary>
        internal Configuration.TraceElement Trace
        {
            get { return this.Configuration != null ? this.Configuration.Trace : CreateDefaultTrace(); }
        }

        /// <summary>
        /// get - Controls whether every write will flush.
        /// </summary>
        public bool AutoFlush
        {
            get { return this.Configuration != null ? this.Configuration.Trace.AutoFlush : false; }
        }

        /// <summary>
        /// get - Controls whether the TraceManager will flush the listeners if the application exits.
        /// </summary>
        public bool FlushOnExit
        {
            get { return this.Configuration != null ? this.Configuration.Trace.FlushOnExit : false; }
        }

        /// <summary>
        /// get - Cached TraceWriter collection.
        /// </summary>
        internal Fosol.Common.Caching.SimpleCache<TraceWriter> Writers
        {
            get { return _TraceWriterCache; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the global TraceManager.
        /// </summary>
        static TraceManager()
        {
            _Manager = new TraceManager();
        }

        /// <summary>
        /// Creates a new instance of a TraceManager.
        /// Initializes the configuration.
        /// </summary>
        internal TraceManager()
        {
            _ConfigWatcher = new Common.Configuration.ConfigurationSectionWatcher<Configuration.DiagnosticsSection>(Fosol.Diagnostics.Configuration.DiagnosticsSection.SectionName);
            _ConfigWatcher.ConfigurationError += Watcher_ConfigurationError;
            _ConfigWatcher.Start();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a default trace configuration.
        /// </summary>
        /// <returns></returns>
        private Configuration.TraceElement CreateDefaultTrace()
        {
            return new Configuration.TraceElement();
        }

        /// <summary>
        /// Get a generic TraceWriter.
        /// </summary>
        /// <returns>A TraceWriter object which can be used to send messages to TraceListeners.</returns>
        public static TraceWriter GetWriter()
        {
            return TraceManager.Manager.GetWriterFromCache(_DefaultWriterKey);
        }

        /// <summary>
        /// Get a generic TraceWriter.
        /// </summary>
        /// <param name="type">A type to identify the source of the messages.</param>
        /// <returns>A TraceWriter object which can be used to send messages to TraceListeners.</returns>
        public static TraceWriter GetWriter(Type type)
        {
            return TraceManager.Manager.GetWriterFromCache(type);
        }

        /// <summary>
        /// Get a TraceWriter for the specific source.
        /// </summary>
        /// <param name="source">The source provides a way to filter messages to the appropiate listeners.</param>
        /// <returns>A TraceWriter obejct which can be used to send message to source TraceListeners.</returns>
        public static TraceWriter GetWriter(string source)
        {
            return TraceManager.Manager.GetWriterFromCache(source);
        }

        /// <summary>
        /// Get the TraceWriter from cache, or create a new one and add it to the cache.
        /// </summary>
        /// <param name="key">Unique key to identify the TraceWriter.</param>
        /// <returns>TraceWriter object.</returns>
        private TraceWriter GetWriterFromCache(string key)
        {
            var writer = this.Writers[key];
            if (writer != null)
                return writer;

            writer = new TraceWriter();
            this.Writers.Add(key, writer);
            return writer;
        }

        /// <summary>
        /// Get the TraceWriter from cache, or create a new one and add it to the cache.
        /// </summary>
        /// <param name="type">The source Type of the TraceWriter.</param>
        /// <returns>TraceWriter object.</returns>
        private TraceWriter GetWriterFromCache(Type type)
        {
            var writer = this.Writers[type.FullName];
            if (writer != null)
                return writer;

            writer = new TraceWriter(type);
            this.Writers.Add(type.FullName, writer);
            return writer;
        }

        /// <summary>
        /// Dispose the configuration watcher.
        /// </summary>
        public void Dispose()
        {
            _ConfigWatcher.Dispose();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        /// <summary>
        /// When the application exits check if it should flush the writers.
        /// </summary>
        /// <param name="sender">Object which called this event.</param>
        /// <param name="e">EventArgs object.</param>
        public void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (this.FlushOnExit)
            {
                foreach (var key in _TraceWriterCache.Keys)
                {
                    var writer = _TraceWriterCache[key];
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// When an exception occurs during loading or initializing the configuration it will be passed to this event.
        /// </summary>
        /// <param name="sender">Object the event originated from.</param>
        /// <param name="e">ConfigurationSectionErrorEventArgs object.</param>
        void Watcher_ConfigurationError(object sender, Common.Configuration.Events.ConfigurationSectionErrorEventArgs e)
        {

        }
        #endregion
    }
}
