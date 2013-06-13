using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Filters
{
    /// <summary>
    /// An EventTypeFilter provides a way to only allow TraceEvents through if their EventType is allowed.
    /// </summary>
    public sealed class EventTypeFilter
        : TraceFilter
    {
        #region Variables
        private TraceEventType _EventType;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The TraceEventType which will be allowed to trace.
        /// </summary>
        [TraceSetting("EventType", true, typeof(EnumConverter), typeof(TraceEventType))]
        public TraceEventType EventType
        {
            get { return _EventType; }
            set { _EventType = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Check to confirm that the TraceEvent.EventType is equal to the specified Filter.EventType.
        /// If it is return 'true'.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>'True' if the TraceEvent should be sent to the listeners.</returns>
        public override bool ShouldTrace(TraceEvent traceEvent)
        {
            if (traceEvent.EventType == this.EventType)
                return true;
            return false;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
