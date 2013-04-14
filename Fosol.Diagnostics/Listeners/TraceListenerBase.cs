using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Abstract class for all extended Trace Listener objects.
    /// </summary>
    public abstract class TraceListenerBase
        : TraceListener
    {
        #region Variables
        protected readonly ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();
        private const string _DefaultFormat = "{source} {type}: {id}: {message}{newline}";
        private readonly string[] _SupportedAttributes;
        private bool _IsInitialized;
        private bool _IsInitializing;
        private LogFormat _Format;
        private bool _UseWriteLine;
        #endregion

        #region Properties
        /// <summary>
        /// get - Whether the Initialized method has been executed.
        /// </summary>
        public bool IsInitialized
        {
            get { return GetValue(ref _IsInitialized); }
            private set { SetValue(ref _IsInitialized, value); }
        }

        /// <summary>
        /// get - Whether it is currenting initializing.
        /// </summary>
        private bool IsInitializing
        {
            get { return GetValue(ref _IsInitializing); }
            set { SetValue(ref _IsInitializing, value); }
        }

        /// <summary>
        /// get - Message string format.
        /// </summary>
        [DefaultValue(_DefaultFormat)]
        [TraceListenerProperty("format", typeof(Converters.LogFormatConverter))]
        public LogFormat Format
        {
            get
            {
                return GetValue(ref _Format);
            }
            set
            {
                SetValue(ref _Format, value, "format");
            }
        }

        /// <summary>
        /// get - Whether the listener should by default use the WriteLine method instead of the Write method.
        /// By default it will use the Write method.
        /// </summary>
        [TraceListenerProperty("useWriteLine")]
        public bool UseWriteLine
        {
            get
            {
                return GetValue(ref _UseWriteLine);
            }
            private set
            {
                SetValue(ref _UseWriteLine, value, "useWriteLine");
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize supported attributes.
        /// </summary>
        public TraceListenerBase()
        {
            _SupportedAttributes = TraceListenerBase.GetSupportedAttributes(this.GetType());
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets an array of supported attributes by extracting the properties and fields that have been marked with the TraceListenerPropertyAttribute.
        /// </summary>
        /// <param name="type">Type of listener.</param>
        /// <returns>An array of supported attributes.</returns>
        protected static string[] GetSupportedAttributes(Type type)
        {
            var properties = (
                from p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.GetCustomAttributes(typeof(TraceListenerPropertyAttribute), true).FirstOrDefault() != null
                select new
                {
                    Property = p,
                    Parameter = p.GetCustomAttributes(typeof(TraceListenerPropertyAttribute), false).FirstOrDefault() as TraceListenerPropertyAttribute,
                    DefaultValue = p.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute
                });

            return properties.Select(p => p.Parameter.Name).ToArray();
        }

        /// <summary>
        /// Initialize the property values that are linked to configuration attributes.
        /// This method is called by the GetValue method.
        /// Regrettably it must be called repeatedly due to not having access to the base TraceListener SetAttributes() method.
        /// </summary>
        internal void InternalIntialize()
        {
            _LockSlim.EnterUpgradeableReadLock();
            try
            {
                if (!_IsInitializing && !_IsInitialized)
                {
                    this.IsInitializing = true;
                    TraceListenerPropertyAttribute.ApplyAttributes(this);
                    Intialize();
                    this.IsInitialized = true;
                    this.IsInitializing = false;
                }
            }
            finally
            {
                _LockSlim.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Subclasses should override this method to apply settings.
        /// This method will get called the first time GetValue() is called (which is normally when a trace event is being handled).
        /// </summary>
        protected virtual void Intialize()
        {

        }

        /// <summary>
        /// Check if the configuration attriute has been set or changed.
        /// If the local property already contains the same value it will return false.
        /// Ensures thread safety.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="prop">Property variable.</param>
        /// <param name="attributeName">Name of the attribute for this property.</param>
        /// <returns>True if the configuration attribute has been set or changed.</returns>
        protected bool UseAttributeValue<T>(ref T prop, string attributeName)
        {
            if (Attributes.ContainsKey(attributeName)
                && (prop == null || !Attributes[attributeName].Equals(GetValue(ref prop).ToString(), StringComparison.InvariantCulture)))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the property value.
        /// Ensures thread safety.
        /// Also calls the Initialize() method.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="prop">References to property variable.</param>
        /// <returns>Value of property.</returns>
        protected T GetValue<T>(ref T prop)
        {
            _LockSlim.EnterReadLock();
            try
            {
                return prop;
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Sets the property value and the configured attribute value.
        /// Ensures thread safety.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="prop">Reference to the property variable.</param>
        /// <param name="value">Value to set the property and attribute with.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Returns the originally passed value.</returns>
        protected T SetValue<T>(ref T prop, T value, string attributeName = null)
        {
            _LockSlim.EnterWriteLock();
            try
            {
                prop = value;

                if (!string.IsNullOrEmpty(attributeName))
                    Attributes[attributeName] = value.ToString();

                return value;
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns an array of supported attributes.
        /// The supported attributes are dynamically generated based on the usage of the TraceListenerPropertyAttribute.
        /// </summary>
        /// <returns>An array of supported attributes.</returns>
        protected override string[] GetSupportedAttributes()
        {
            return _SupportedAttributes;
        }

        /// <summary>
        /// All Trace*() methods send the trace event to this method.
        /// </summary>
        /// <param name="logEvent"></param>
        protected virtual void WriteTrace(LogEvent logEvent)
        {
            // By default all trace events are sent to the Write() method.
            // This allows the format string to control the layout.
            if (!this.UseWriteLine)
                Write(this.Format.Render(logEvent));
            else
                WriteLine(this.Format.Render(logEvent));
        }

        /// <summary>
        /// Repackages and sends a LogEvent to the WriteTrace() method.
        /// Override the TraceWrite() method if you need to do something specific to the message other than format it.
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
                return;

            // It is annoying for this to be here, but it's the only way to ensure it's called.
            InternalIntialize();
            WriteTrace(new LogEvent(eventCache, source, eventType, id, data));
        }

        /// <summary>
        /// Repackages and sends a LogEvent to the WriteTrace() method.
        /// Override the TraceWrite() method if you need to do something specific to the message other than format it.
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
                return;

            // It is annoying for this to be here, but it's the only way to ensure it's called.
            InternalIntialize();
            WriteTrace(new LogEvent(eventCache, source, eventType, id, data));
        }

        /// <summary>
        /// Repackages and sends a LogEvent to the WriteTrace() method.
        /// Override the TraceWrite() method if you need to do something specific to the message other than format it.
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
                return;

            // It is annoying for this to be here, but it's the only way to ensure it's called.
            InternalIntialize();
            WriteTrace(new LogEvent(eventCache, source, eventType, id));
        }

        /// <summary>
        /// Repackages and sends a LogEvent to the WriteTrace() method.
        /// Override the TraceWrite() method if you need to do something specific to the message other than format it.
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
                return;

            // It is annoying for this to be here, but it's the only way to ensure it's called.
            InternalIntialize();
            WriteTrace(new LogEvent(eventCache, source, eventType, id, format, args));
        }

        /// <summary>
        /// Repackages and sends a LogEvent to the WriteTrace() method.
        /// Override the TraceWrite() method if you need to do something specific to the message other than format it.
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;

            // It is annoying for this to be here, but it's the only way to ensure it's called.
            InternalIntialize();
            WriteTrace(new LogEvent(eventCache, source, eventType, id, message));
        }

        /// <summary>
        /// Repackages and sends a LogEvent to the WriteTrace() method.
        /// Override the TraceWrite() method if you need to do something specific to the message other than format it.
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="relatedActivityId"></param>
        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
