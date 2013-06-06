using Fosol.Common.Formatters.Keywords;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// The Timestamp from the trace event.
    /// </summary>
    [FormatKeyword("timestamp", true)]
    public sealed class TimestampKeyword
        : TraceKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a StackFormatKeyword object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public TimestampKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the timestamp of the TraceEvent.
        /// </summary>
        /// <param name="traceEvent">Information object containing data for the keyword.</param>
        /// <returns>The trace event call stack.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent != null)
                return traceEvent.DateTime.Ticks.ToString();
            else
                return DateTime.Now.Ticks.ToString();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
