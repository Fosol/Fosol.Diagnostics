using Fosol.Common.Parsers;
using Fosol.Common.Parsers.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// This is the base abstract class for all TraceListeners.
    /// A TraceListener listens for TraceEvents and outputs them to their intended targets.
    /// </summary>
    public abstract class TraceListener
        : IDisposable
    {
        #region Variables
        private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();
        private const string _DefaultFormat = "{level}: [{id}] {source}: {datetime}: {message}{newline}";
        private const string _DefaultHeaderFormat = "{newline}";
        private const string _DefaultFooterFormat = "{newline}";
        private Format _Format;
        private Format _DebugFormat;
        private Format _InformationFormat;
        private Format _WarningFormat;
        private Format _ErrorFormat;
        private Format _CriticalFormat;
        private Format _StartFormat;
        private Format _StopFormat;
        private Format _SuspendFormat;
        private Format _ResumeFormat;
        private Format _HeaderFormat;
        private Format _FooterFormat;
        private bool _UseEventLevelFormat;
        private Encoding _Encoding;
        private readonly TraceFilterCollection _Filters = new TraceFilterCollection();
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The default trace format for messages.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Format cannot be null.</exception>
        [TraceSetting("Format", ConverterType = typeof(FormatConverter))]
        [DefaultValue(_DefaultFormat)]
        public Format Format
        {
            get { return _Format; }
            set 
            {
                Fosol.Common.Validation.Assert.IsNotNull(value, "value");
                _Format = value; 
            }
        }

        /// <summary>
        /// get/set - The trace format for TraceLevel.Debug messages.
        /// </summary>
        [TraceSetting("DebugFormat", ConverterType = typeof(FormatConverter))]
        public Format DebugFormat
        {
            get { return _DebugFormat; }
            set
            {
                _DebugFormat = value;
                _UseEventLevelFormat = UseEventLevelFormat();
            }
        }

        /// <summary>
        /// get/set - The trace format for TraceLevel.Information messages.
        /// </summary>
        [TraceSetting("InformationFormat", ConverterType = typeof(FormatConverter))]
        public Format InformationFormat
        {
            get { return _InformationFormat; }
            set
            {
                _InformationFormat = value;
                _UseEventLevelFormat = UseEventLevelFormat();
            }
        }

        /// <summary>
        /// get/set - The trace format for TraceLevel.Warning messages.
        /// </summary>
        [TraceSetting("WarningFormat", ConverterType = typeof(FormatConverter))]
        public Format WarningFormat
        {
            get { return _WarningFormat; }
            set
            {
                _WarningFormat = value;
                _UseEventLevelFormat = UseEventLevelFormat();
            }
        }

        /// <summary>
        /// get/set - The trace format for TraceLevel.Error messages.
        /// </summary>
        [TraceSetting("ErrorFormat", ConverterType = typeof(FormatConverter))]
        public Format ErrorFormat
        {
            get { return _ErrorFormat; }
            set 
            {
                _ErrorFormat = value;
                _UseEventLevelFormat = true;
            }
        }

        /// <summary>
        /// get/set - The trace format for TraceLevel.Critical messages.
        /// </summary>
        [TraceSetting("CriticalFormat", ConverterType = typeof(FormatConverter))]
        public Format CriticalFormat
        {
            get { return _CriticalFormat; }
            set 
            { 
                _CriticalFormat = value;
                _UseEventLevelFormat = UseEventLevelFormat();
            }
        }

        /// <summary>
        /// get/set - The trace format for TraceLevel.Start messages.
        /// </summary>
        [TraceSetting("StartFormat", ConverterType = typeof(FormatConverter))]
        public Format StartFormat
        {
            get { return _StartFormat; }
            set 
            { 
                _StartFormat = value;
                _UseEventLevelFormat = UseEventLevelFormat();
            }
        }

        /// <summary>
        /// get/set - The trace format for TraceLevel.Resume messages.
        /// </summary>
        [TraceSetting("StopFormat", ConverterType = typeof(FormatConverter))]
        public Format StopFormat
        {
            get { return _StopFormat; }
            set 
            { 
                _StopFormat = value;
                _UseEventLevelFormat = UseEventLevelFormat();
            }
        }

        /// <summary>
        /// get/set - The trace format for TraceLevel.Suspend messages.
        /// </summary>
        [TraceSetting("SuspendFormat", ConverterType = typeof(FormatConverter))]
        public Format SuspendFormat
        {
            get { return _SuspendFormat; }
            set 
            { 
                _SuspendFormat = value;
                _UseEventLevelFormat = UseEventLevelFormat();
            }
        }

        /// <summary>
        /// get/set - The trace format for TraceLevel.Resume messages.
        /// </summary>
        [TraceSetting("ResumeFormat", ConverterType = typeof(FormatConverter))]
        public Format ResumeFormat
        {
            get { return _ResumeFormat; }
            set 
            { 
                _ResumeFormat = value;
                _UseEventLevelFormat = UseEventLevelFormat();
            }
        }

        /// <summary>
        /// get/set - The format for the header message.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">HeaderFormat cannot be null.</exception>
        [TraceSetting("HeaderFormat", ConverterType = typeof(FormatConverter))]
        [DefaultValue(_DefaultHeaderFormat)]
        public Format HeaderFormat
        {
            get { return _HeaderFormat; }
            set 
            {
                Fosol.Common.Validation.Assert.IsNotNull(value, "value");
                _HeaderFormat = value; 
            }
        }

        /// <summary>
        /// get/set - The format for the footer message.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">FooterFormat cannot be null.</exception>
        [TraceSetting("FooterFormat", ConverterType = typeof(FormatConverter))]
        [DefaultValue(_DefaultFooterFormat)]
        public Format FooterFormat
        {
            get { return _FooterFormat; }
            set
            {
                Fosol.Common.Validation.Assert.IsNotNull(value, "value");
                _FooterFormat = value;
            }
        }

        /// <summary>
        /// get/set - The encoding messages should use.
        /// </summary>
        [DefaultValue("Default")]
        [TraceSetting("Encoding", ConverterType = typeof(Common.Converters.EncodingConverter))]
        public Encoding Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }

        /// <summary>
        /// get - Collection of TraceFilter objects for this TraceListener.
        /// </summary>
        internal TraceFilterCollection Filters
        {
            get { return _Filters; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Performs any initialization code after the TraceListener has been constructed.
        /// </summary>
        protected internal virtual void Initialize()
        {

        }

        /// <summary>
        /// Fires the BeforeWrite event.
        /// Validates TraceEvent with TraceFilter.
        /// Executes the OnWrite function.
        /// Fires the AfterWrite event.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        public void Write(TraceEvent trace)
        {
            _Lock.EnterWriteLock();
            try
            {
                if (OnBeforeWrite(trace))
                {
                    if (Validate(trace))
                    {
                        OnWrite(trace);
                        OnAfterWrite(trace);
                    }
                }
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Fires the BeforeWrite event.
        /// Provides a way to cancel writes.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        /// <returns>True if the write should continue.</returns>
        protected virtual bool OnBeforeWrite(TraceEvent trace)
        {
            if (BeforeWrite != null)
            {
                var args = new Events.WriteEventArgs(trace);
                BeforeWrite(this, args);
                return !args.Cancel;
            }
            return true;
        }

        /// <summary>
        /// This method performs the actual write for the listener.
        /// Override this method on all custom TraceListeners.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        protected abstract void OnWrite(TraceEvent trace);

        /// <summary>
        /// Fires the AfterWrite event.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        protected virtual void OnAfterWrite(TraceEvent trace)
        {
            if (AfterWrite != null)
                AfterWrite(this, new Events.WriteEventArgs(trace));
        }

        /// <summary>
        /// Validates the TraceFilters to determine if the TraceEvent should be written.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        /// <returns>True if valid.</returns>
        public bool Validate(TraceEvent trace)
        {
            if (this.Filters.Count == 0)
                return true;

            if (this.Filters.Validate(trace))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if any of the special formats have been set.
        /// </summary>
        /// <returns>'True' if any of the special formats have been set.</returns>
        private bool UseEventLevelFormat()
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
        /// Apply the correct format to the TraceEvent message.
        /// </summary>
        /// <param name="trace">TraceEvent object being passed to the listener.</param>
        /// <returns>Dynamically generated message.</returns>
        protected string Render(TraceEvent trace)
        {
            if (trace.Position == TraceSpot.Message)
            {
                if (!_UseEventLevelFormat)
                    return this.Format.Render(trace);
                else if (trace.Level == TraceLevel.Critical && this.CriticalFormat != null)
                    return this.CriticalFormat.Render(trace);
                else if (trace.Level == TraceLevel.Error && this.ErrorFormat != null)
                    return this.ErrorFormat.Render(trace);
                else if (trace.Level == TraceLevel.Warning && this.WarningFormat != null)
                    return this.WarningFormat.Render(trace);
                else if (trace.Level == TraceLevel.Information && this.InformationFormat != null)
                    return this.InformationFormat.Render(trace);
                else if (trace.Level == TraceLevel.Debug && this.DebugFormat != null)
                    return this.DebugFormat.Render(trace);
                else if (trace.Level == TraceLevel.Start && this.StartFormat != null)
                    return this.StartFormat.Render(trace);
                else if (trace.Level == TraceLevel.Stop && this.StopFormat != null)
                    return this.StopFormat.Render(trace);
                else if (trace.Level == TraceLevel.Suspend && this.SuspendFormat != null)
                    return this.SuspendFormat.Render(trace);
                else if (trace.Level == TraceLevel.Resume && this.ResumeFormat != null)
                    return this.ResumeFormat.Render(trace);
                else
                    return this.Format.Render(trace);
            }
            else if (trace.Position == TraceSpot.Header)
                return this.HeaderFormat.Render(trace);
            else if (trace.Position == TraceSpot.Footer)
                return this.FooterFormat.Render(trace);
            else
                return this.Format.Render(trace);
        }

        /// <summary>
        /// Flush the TraceListener.
        /// </summary>
        public virtual void Flush()
        {

        }

        /// <summary>
        /// Close the TraceListener.
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// Dipose the TraceListener.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Get the TraceFilter identified by the specified name.
        /// </summary>
        /// <param name="name">Unique name that identifies the TraceFilter.</param>
        /// <returns>TraceFilter object.</returns>
        public TraceFilter GetFilter(string name)
        {
            return this.Filters.First(f => f.Key.Equals(name)).Value;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        public delegate void WriteEventHandler(object sender, Events.WriteEventArgs args);

        /// <summary>
        /// This event is fired before the OnWrite function can be called.
        /// </summary>
        public event WriteEventHandler BeforeWrite;

        /// <summary>
        /// This event is fired after the OnWrite function is called.
        /// </summary>
        public event WriteEventHandler AfterWrite;
        #endregion
    }
}
