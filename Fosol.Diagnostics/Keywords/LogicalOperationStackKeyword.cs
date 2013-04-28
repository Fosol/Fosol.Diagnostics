using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// The LogicalOperationStack from the trace event.
    /// </summary>
    [TraceKeyword("logicalStack")]
    public sealed class LogicalOperationStackKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The maximum length the message may be.
        /// </summary>
        [TraceKeywordProperty("length", new string[] { "l", "len" })]
        public int Length { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a StackTraceKeyword object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public LogicalOperationStackKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the activity StackTrace of the TraceEvent.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>The trace event call stack.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent.ActivityStackTrace != null && traceEvent.ActivityStackTrace.Count > 0)
            {
                var builder = new StringBuilder();

                foreach (var obj in traceEvent.ActivityStackTrace)
                {
                    builder.Append(obj);
                    
                    // Limit length of message.
                    if (this.Length > 0 && this.Length < builder.Length)
                    {
                        builder.Remove(this.Length, builder.Length - this.Length);
                        break;
                    }
                }

                return builder.ToString();
            }
            else
                return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
