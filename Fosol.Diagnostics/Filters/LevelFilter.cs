using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Filters
{
    /// <summary>
    /// A LevelFilter ensures the only TraceEvents that are allowed through to the listeners are those that have a TraceType greater than equal to the configured level.
    /// </summary>
    public class LevelFilter
        : TraceFilter
    {
        #region Variables
        private TraceEventType _Level;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The level of TraceType values that are allowed to be sent to the listeners.
        /// </summary>
        [TraceSetting("Level", true, typeof(EnumConverter), typeof(TraceEventType))]
        public TraceEventType Level
        {
            get { return _Level; }
            set { _Level = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Checks to see if the TraceEvent should be sent to the listeners.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>'True' if the TraceEvent should be send to the listeners.</returns>
        public override bool ShouldTrace(TraceEvent traceEvent)
        {
            if ((int)traceEvent.EventType >= (int)this.Level)
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
