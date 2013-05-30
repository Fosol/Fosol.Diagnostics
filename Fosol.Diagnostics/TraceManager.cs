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
        private static Fosol.Common.Configuration.ConfigurationSectionWatcher<Configuration.DiagnosticsSection> _ConfigWatcher;
        private Configuration.ListenerElementCollection _SharedListeners;
        private Configuration.SourceElementCollection _Sources;
        private Configuration.TraceElement _Trace;
        #endregion

        #region Properties
        internal static TraceManager Manager
        {
            get { return _Manager; }
        }

        internal static Configuration.DiagnosticsSection Config
        {
            get { return _ConfigWatcher != null ? _ConfigWatcher.ConfigSection : null; }
        }

        internal Configuration.ListenerElementCollection SharedListeners
        {
            get { return _SharedListeners; }
        }

        internal Configuration.SourceElementCollection Sources
        {
            get { return _Sources; }
        }

        internal Configuration.TraceElement Trace
        {
            get 
            {
                if (_Trace == null)
                    InitializeTrace();
                return _Trace; 
            }
        }
        #endregion

        #region Constructors
        static TraceManager()
        {
            _ConfigWatcher = new Common.Configuration.ConfigurationSectionWatcher<Configuration.DiagnosticsSection>(Configuration.DiagnosticsSection.SectionName);
            _ConfigWatcher.Start();
            _Manager = new TraceManager();
        }

        internal TraceManager()
        {
            if (TraceManager.Config != null)
            {
                // Reference the configuration collection.
                _SharedListeners = TraceManager.Config.SharedListeners;
                _Sources = TraceManager.Config.Sources;
                _Trace = TraceManager.Config.Trace;
            }
        }
        #endregion

        #region Methods
        private void InitializeTrace()
        {
            _Trace = new Configuration.TraceElement();
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
