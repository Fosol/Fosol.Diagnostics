using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// The TraceManager provides a single point of control for diagnostics.
    /// </summary>
    public class TraceManager
        : IDisposable
    {
        #region Variables
        private static TraceManager _Manager;
        private Fosol.Common.Configuration.ConfigurationSectionWatcher<Configuration.DiagnosticsSection> _ConfigWatcher;
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
        internal Configuration.DiagnosticsSection Config
        {
            get { return _ConfigWatcher != null ? _ConfigWatcher.ConfigSection : null; }
        }

        /// <summary>
        /// get - Shared listeners within the configuration.
        /// </summary>
        internal Configuration.ListenerElementCollection SharedListeners
        {
            get { return this.Config != null ? this.Config.SharedListeners : null; }
        }

        /// <summary>
        /// get - Source listeners within the configuration.
        /// </summary>
        internal Configuration.SourceElementCollection Sources
        {
            get { return this.Config != null ? this.Config.Sources : null; }
        }

        /// <summary>
        /// get - Generic trace within the configuration.
        /// </summary>
        internal Configuration.TraceElement Trace
        {
            get { return this.Config != null ? this.Config.Trace : CreateDefaultTrace(); }
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
            _ConfigWatcher = new Common.Configuration.ConfigurationSectionWatcher<Configuration.DiagnosticsSection>(Configuration.DiagnosticsSection.SectionName);
            _ConfigWatcher.Start();
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
            return new TraceWriter();
        }

        /// <summary>
        /// Get a generic TraceWriter.
        /// </summary>
        /// <param name="type">A type to identify the source of the messages.</param>
        /// <returns>A TraceWriter object which can be used to send messages to TraceListeners.</returns>
        public static TraceWriter GetWriter(Type type)
        {
            return new TraceWriter(type);
        }

        /// <summary>
        /// Get a TraceWriter for the specific source.
        /// </summary>
        /// <param name="source">The source provides a way to filter messages to the appropiate listeners.</param>
        /// <returns>A TraceWriter obejct which can be used to send message to source TraceListeners.</returns>
        public static TraceWriter GetWriter(string source)
        {
            return new TraceWriter(source);
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
        #endregion
    }
}
