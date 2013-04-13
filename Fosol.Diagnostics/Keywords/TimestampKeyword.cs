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
    [TraceKeyword("timestamp")]
    public sealed class TimestampKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a StackTraceKeyword object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public TimestampKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the call stack value of the trace event.
        /// </summary>
        /// <param name="logEvent">LogEvent object.</param>
        /// <returns>The trace event call stack.</returns>
        public override string Render(LogEvent logEvent)
        {
            return logEvent.EventCache.Timestamp.ToString();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
