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
    public sealed class ConsoleListener
        : TraceListener
    {
        #region Variables
        private const string _DefaultFormat = "{source} {level}: {id}: {message}";
        private static readonly string[] _SupportedAttributes;
        private LogFormat _Format;
        #endregion

        #region Properties
        [TraceListenerAttribute("format")]
        public LogFormat Format
        {
            get
            {
                if (Attributes.ContainsKey("format"))
                {
                    // If the format has not been set yet, or has changed through the configuration.
                    if (_Format == null || !Attributes["format"].Equals(_Format.ToString(), StringComparison.InvariantCulture))
                        _Format = new LogFormat(Attributes["format"]);
                }
                else if (_Format == null)
                    _Format = new LogFormat(_DefaultFormat);

                return _Format;
            }
            private set
            {
                _Format = value;
                Attributes["format"] = value.ToString();
            }
        }

        public override bool IsThreadSafe
        {
            get { return true; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize supported attributes.
        /// </summary>
        static ConsoleListener()
        {
            var properties = (
                from p in typeof(ConsoleListener).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.GetCustomAttributes(typeof(TraceListenerAttributeAttribute), true).FirstOrDefault() != null
                select new
                {
                    Property = p,
                    Parameter = p.GetCustomAttributes(typeof(TraceListenerAttributeAttribute), false).FirstOrDefault() as TraceListenerAttributeAttribute,
                    DefaultValue = p.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute
                });

            _SupportedAttributes = properties.Select(p => p.Parameter.Name).ToArray();
        }
        #endregion

        #region Methods
        protected override string[] GetSupportedAttributes()
        {
            return _SupportedAttributes;
        }

        public override void Write(string message)
        {
            Console.Write(message);
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
                return;
            base.TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
                return;
            base.TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
                return;
            base.TraceEvent(eventCache, source, eventType, id);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
                return;
            base.TraceEvent(eventCache, source, eventType, id, format, args);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;
            WriteLine(this.Format);
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
