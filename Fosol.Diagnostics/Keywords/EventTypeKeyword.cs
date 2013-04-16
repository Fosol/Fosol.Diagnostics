using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// Provides a way to render the log type information.
    /// </summary>
    [TraceKeyword("type")]
    public sealed class EventTypeKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The string format of the LogEventType property.
        /// </summary>
        [TraceKeywordProperty("format", new string[] { "f" })]
        public string Format { get; set; }

        /// <summary>
        /// get/set - Whether to lowercase.
        /// </summary>
        [TraceKeywordProperty("lowercase", new string[] { "lc", "low", "lower" })]
        public bool ToLower { get; set; }

        /// <summary>
        /// get/set - Whether to uppercase.
        /// </summary>
        [TraceKeywordProperty("uppercase", new string[] { "uc", "up", "upper" })]
        public bool ToUpper { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a EventTypeKeyword object.
        /// </summary>
        /// <param name="attributes">StringDictionary of attributes.</param>
        public EventTypeKeyword(StringDictionary attributes = null)
            : base(attributes)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Renders the log event type information for the trace event.
        /// </summary>
        /// <param name="logEvent">LogEvent object.</param>
        /// <returns>Log event type for the trace event.</returns>
        public override string Render(TraceEvent logEvent)
        {
            if (this.ToLower)
                return logEvent.EventType.ToString(this.Format).ToLower();

            if (this.ToUpper)
                return logEvent.EventType.ToString(this.Format).ToUpper();

            return logEvent.EventType.ToString(this.Format);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
