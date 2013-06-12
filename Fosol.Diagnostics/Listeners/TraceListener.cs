using Fosol.Common.Extensions.Bytes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Base abstract class TraceListener provides a way to implement specialized listeners.
    /// Inherit from TraceListener and override the Write(string message) method to implement custom listeners.
    /// </summary>
    public abstract class TraceListener
        : IDisposable
    {
        #region Variables
        private const string _DefaultFormat = "{type}: [{id}] {source}: {datetime}: {message}{newline}";
        private TraceFormatter _Format;
        private TraceFormatter _DebugFormat;
        private TraceFormatter _InformationFormat;
        private TraceFormatter _WarningFormat;
        private TraceFormatter _ErrorFormat;
        private TraceFormatter _CriticalFormat;
        private TraceFormatter _StartFormat;
        private TraceFormatter _StopFormat;
        private TraceFormatter _SuspendFormat;
        private TraceFormatter _ResumeFormat;
        private bool _UseEventTypeFormat;
        private Encoding _Encoding;

        public delegate void WriteEventHandler(object sender, Events.WriteEventArgs e);
        public event WriteEventHandler BeforeWrite;
        public event WriteEventHandler AfterWrite;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - General output format for all messages.  This can be overridden by the specific format if it is set.
        /// </summary>
        [DefaultValue(_DefaultFormat)]
        [TraceSetting("Format", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter Format
        {
            get { return _Format; }
            set { _Format = value; }
        }

        /// <summary>
        /// get/set - Output format for messages of EventType Debug.
        /// </summary>
        [TraceSetting("DebugFormat", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter DebugFormat
        {
            get { return _DebugFormat; }
            set 
            { 
                _DebugFormat = value;
                _UseEventTypeFormat = UseEventTypeFormat();
            }
        }

        /// <summary>
        /// get/set - Output format for messages of EventType Information.
        /// </summary>
        [TraceSetting("InformationFormat", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter InformationFormat
        {
            get { return _InformationFormat; }
            set
            {
                _InformationFormat = value;
                _UseEventTypeFormat = UseEventTypeFormat();
            }
        }

        /// <summary>
        /// get/set - Output format for messages of EventType Warning.
        /// </summary>
        [TraceSetting("WarningFormat", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter WarningFormat
        {
            get { return _WarningFormat; }
            set
            {
                _WarningFormat = value;
                _UseEventTypeFormat = UseEventTypeFormat();
            }
        }

        /// <summary>
        /// get/set - Output format for messages of EventType Error.
        /// </summary>
        [TraceSetting("ErrorFormat", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter ErrorFormat
        {
            get { return _ErrorFormat; }
            set
            {
                _ErrorFormat = value;
                _UseEventTypeFormat = UseEventTypeFormat();
            }
        }

        /// <summary>
        /// get/set - Output format for messages of EventType Critical.
        /// </summary>
        [TraceSetting("CriticalFormat", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter CriticalFormat
        {
            get { return _CriticalFormat; }
            set
            {
                _CriticalFormat = value;
                _UseEventTypeFormat = UseEventTypeFormat();
            }
        }

        /// <summary>
        /// get/set - Output format for messages of EventType Start.
        /// </summary>
        [TraceSetting("StartFormat", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter StartFormat
        {
            get { return _StartFormat; }
            set
            {
                _StartFormat = value;
                _UseEventTypeFormat = UseEventTypeFormat();
            }
        }

        /// <summary>
        /// get/set - Output format for messages of EventType Stop.
        /// </summary>
        [TraceSetting("StopFormat", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter StopFormat
        {
            get { return _StopFormat; }
            set
            {
                _StopFormat = value;
                _UseEventTypeFormat = UseEventTypeFormat();
            }
        }

        /// <summary>
        /// get/set - Output format for messages of EventType Suspend.
        /// </summary>
        [TraceSetting("SuspendFormat", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter SuspendFormat
        {
            get { return _SuspendFormat; }
            set
            {
                _SuspendFormat = value;
                _UseEventTypeFormat = UseEventTypeFormat();
            }
        }

        /// <summary>
        /// get/set - Output format for messages of EventType Resume.
        /// </summary>
        [TraceSetting("ResumeFormat", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter ResumeFormat
        {
            get { return _ResumeFormat; }
            set
            {
                _ResumeFormat = value;
                _UseEventTypeFormat = UseEventTypeFormat();
            }
        }

        /// <summary>
        /// get/set - The encoding messages should use.
        /// </summary>
        [DefaultValue("default")]
        [TraceSetting("Encoding", typeof(Common.Converters.EncodingConverter))]
        public Encoding Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceListener.
        /// </summary>
        public TraceListener()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize is automatically called after the listener has been contructed.
        /// Override this method if the listener requires initialization before it is used.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Executes before the OnWrite method.
        /// If you update the WriteEventArgs.Cancel property to 'true' it will cancel the OnWrite and OnAfterWrite methods.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected virtual bool OnBeforeWrite(TraceEvent traceEvent)
        {
            var args = new Events.WriteEventArgs(traceEvent);
            if (BeforeWrite != null)
                BeforeWrite(this, args);

            return !args.Cancel;
        }

        /// <summary>
        /// The actual writing method that must be overridden by listeners.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected abstract void OnWrite(TraceEvent traceEvent);

        /// <summary>
        /// Executes after the OnWrite method.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected virtual void OnAfterWrite(TraceEvent traceEvent)
        {
            if (AfterWrite != null)
                AfterWrite(this, new Events.WriteEventArgs(traceEvent));
        }

        /// <summary>
        /// Applies the appropriate Format to the message and sends it to the Write(string message) method.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        public void Write(TraceEvent traceEvent)
        {
            if (OnBeforeWrite(traceEvent))
            {
                OnWrite(traceEvent);
                OnAfterWrite(traceEvent);
            }
        }

        /// <summary>
        /// Apply the correct format to the TraceEvent message.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        /// <returns>Dynamically generated message.</returns>
        protected string Render(TraceEvent traceEvent)
        {
            if (!_UseEventTypeFormat)
                return this.Format.Render(traceEvent);
            else if (traceEvent.EventType == TraceEventType.Critical && this.CriticalFormat != null)
                return this.CriticalFormat.Render(traceEvent);
            else if (traceEvent.EventType == TraceEventType.Error && this.ErrorFormat != null)
                return this.ErrorFormat.Render(traceEvent);
            else if (traceEvent.EventType == TraceEventType.Warning && this.WarningFormat != null)
                return this.WarningFormat.Render(traceEvent);
            else if (traceEvent.EventType == TraceEventType.Information && this.InformationFormat != null)
                return this.InformationFormat.Render(traceEvent);
            else if (traceEvent.EventType == TraceEventType.Debug && this.DebugFormat != null)
                return this.DebugFormat.Render(traceEvent);
            else if (traceEvent.EventType == TraceEventType.Start && this.StartFormat != null)
                return this.StartFormat.Render(traceEvent);
            else if (traceEvent.EventType == TraceEventType.Stop && this.StopFormat != null)
                return this.StopFormat.Render(traceEvent);
            else if (traceEvent.EventType == TraceEventType.Suspend && this.SuspendFormat != null)
                return this.SuspendFormat.Render(traceEvent);
            else if (traceEvent.EventType == TraceEventType.Resume && this.ResumeFormat != null)
                return this.ResumeFormat.Render(traceEvent);
            else
                return this.Format.Render(traceEvent);
        }

        /// <summary>
        /// Checks if any of the special formats have been set.
        /// </summary>
        /// <returns>'True' if any of the special formats have been set.</returns>
        private bool UseEventTypeFormat()
        {
            if (this.DebugFormat != null
                || this.InformationFormat != null
                || this.WarningFormat != null
                || this.ErrorFormat != null
                || this.CriticalFormat != null
                || this.StartFormat != null
                || this.StopFormat != null
                || this.SuspendFormat != null
                || this.ResumeFormat != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Override this method if the listener requires closing after use.
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// Override this method if the listener requires flushing.
        /// </summary>
        public virtual void Flush()
        {
        }

        /// <summary>
        /// Override this method if the listener has objects that need to be disposed.
        /// </summary>
        public virtual void Dispose()
        {
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
