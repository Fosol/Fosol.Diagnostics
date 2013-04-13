using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
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
        private const string _DefaultFormat = "{source} {type}: {id}: {message}{newline}";
        private static readonly string[] _SupportedAttributes;
        private LogFormat _Format;
        private bool _UseWriteLine;
        #endregion

        #region Properties
        /// <summary>
        /// get - Message string format.
        /// </summary>
        [TraceListenerProperty("format")]
        public LogFormat Format
        {
            get
            {
                if (UseAttributeValue<LogFormat>(_Format, "format"))
                    _Format = new LogFormat(Attributes["format"]);
                else if (_Format == null)
                    _Format = new LogFormat(_DefaultFormat);

                return _Format;
            }
            private set
            {
                SetValue<LogFormat>("format", ref _Format, value);
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
                if (UseAttributeValue<bool>(_UseWriteLine, "useWriteLine"))
                    bool.TryParse(Attributes["useWriteLine"], out _UseWriteLine);

                return _UseWriteLine;
            }
            private set
            {
                SetValue<bool>("useWriteLine", ref _UseWriteLine, value);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize supported attributes.
        /// </summary>
        static TraceListenerBase()
        {
            var properties = (
                from p in typeof(ConsoleListener).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.GetCustomAttributes(typeof(TraceListenerPropertyAttribute), true).FirstOrDefault() != null
                select new
                {
                    Property = p,
                    Parameter = p.GetCustomAttributes(typeof(TraceListenerPropertyAttribute), false).FirstOrDefault() as TraceListenerPropertyAttribute,
                    DefaultValue = p.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute
                });

            _SupportedAttributes = properties.Select(p => p.Parameter.Name).ToArray();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check if the configuration attriute has been set or changed.
        /// If the local property already contains the same value it will return false.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="prop">Property variable.</param>
        /// <param name="attributeName">Name of the attribute for this property.</param>
        /// <returns>True if the configuration attribute has been set or changed.</returns>
        private bool UseAttributeValue<T>(T prop, string attributeName)
        {
            if (Attributes.ContainsKey(attributeName)
                && (prop == null || !Attributes[attributeName].Equals(prop.ToString(), StringComparison.InvariantCulture)))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Sets the property value and the configured attribute value.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="prop">Reference to the property variable.</param>
        /// <param name="value">Value to set the property and attribute with.</param>
        private void SetValue<T>(string attributeName, ref T prop, T value)
        {
            prop = value;
            Attributes[attributeName] = value.ToString();
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

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
                return;

            Write(this.Format.Render(new LogEvent(eventCache, source, eventType, id, data)));
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
                return;

            Write(this.Format.Render(new LogEvent(eventCache, source, eventType, id, data)));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
                return;

            Write(this.Format.Render(new LogEvent(eventCache, source, eventType, id)));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
                return;

            Write(this.Format.Render(new LogEvent(eventCache, source, eventType, id, format, args)));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;

            Write(this.Format.Render(new LogEvent(eventCache, source, eventType, id, message)));
        }

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
