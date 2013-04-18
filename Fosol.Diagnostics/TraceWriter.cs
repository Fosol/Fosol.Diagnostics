using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    public class TraceWriter
    {
        #region Variables
        private Configuration.ListenerElementCollection _Listeners;
        private Configuration.SourceElement _Source;
        private string _SourceName;
        #endregion

        #region Properties
        internal Configuration.ListenerElementCollection Listeners
        {
            get { return _Listeners; }
            set { _Listeners = value; }
        }

        public string Source
        {
            get { return _SourceName; }
        }
        #endregion

        #region Constructors

        internal TraceWriter()
        {
            _Listeners = TraceManager.Manager.Trace.Listeners;
        }

        internal TraceWriter(string source)
        {
            _SourceName = source;
            _Source = TraceManager.Manager.Sources[source];

            if (_Source != null)
            {
                _Listeners = _Source.Listeners;
            }
        }
        #endregion

        #region Methods
        public void Write(string message)
        {
            if (this.Listeners != null)
            {
                var trace_event = new TraceEvent();

                foreach (var listener in this.Listeners)
                {
                    if (listener.Filter.GetFilter()
                }
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
