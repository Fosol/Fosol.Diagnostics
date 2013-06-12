using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    public class TraceManager
        : IDisposable
    {
        #region Variables
        private static TraceManager _Manager;
        private Fosol.Common.Configuration.ConfigurationSectionWatcher<Configuration.DiagnosticsSection> _ConfigWatcher;
        #endregion

        #region Properties
        internal static TraceManager Manager
        {
            get { return _Manager; }
        }

        internal Configuration.DiagnosticsSection Config
        {
            get { return _ConfigWatcher != null ? _ConfigWatcher.ConfigSection : null; }
        }

        internal Configuration.ListenerElementCollection SharedListeners
        {
            get { return this.Config != null ? this.Config.SharedListeners : null; }
        }

        internal Configuration.SourceElementCollection Sources
        {
            get { return this.Config != null ? this.Config.Sources : null; }
        }

        internal Configuration.TraceElement Trace
        {
            get { return this.Config != null ? this.Config.Trace : CreateDefaultTrace(); }
        }
        #endregion

        #region Constructors
        static TraceManager()
        {
            _Manager = new TraceManager();
        }

        internal TraceManager()
        {
            _ConfigWatcher = new Common.Configuration.ConfigurationSectionWatcher<Configuration.DiagnosticsSection>(Configuration.DiagnosticsSection.SectionName);
            _ConfigWatcher.Start();
        }
        #endregion

        #region Methods
        private Configuration.TraceElement CreateDefaultTrace()
        {
            return new Configuration.TraceElement();
        }

        public void Dispose()
        {
            _ConfigWatcher.Dispose();
        }

        public static TraceWriter GetWriter()
        {
            return new TraceWriter();
        }

        public static TraceWriter GetWriter(string source)
        {
            return new TraceWriter(source);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
