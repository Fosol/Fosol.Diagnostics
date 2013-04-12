using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    public sealed class ConsoleListener
        : TraceListener
    {
        #region Variables
        private const string _DefaultFormat = "{source} {level}: {id}: {message}";
        #endregion

        #region Properties
        public string Format
        {
            get
            {
                if (Attributes.ContainsKey("format"))
                    return Attributes["format"];
                else
                    return _DefaultFormat;
            }
            set
            {
                Attributes["format"] = value;
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
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
            WriteLine(message);
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
