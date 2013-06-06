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
    /// The ThreadId of the trace event.
    /// </summary>
    [FormatKeyword("threadId")]
    public sealed class ThreadIdKeyword
        : TraceKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The maximum length the thread id value may be.
        /// </summary>
        [FormatKeywordProperty("length", new string[] { "l", "len" })]
        public int Length { get; set; }

        /// <summary>
        /// get/set - Whether to lowercase thread id value.
        /// </summary>
        [FormatKeywordProperty("lowercase", new string[] { "lc", "low", "lower" })]
        public bool ToLower { get; set; }

        /// <summary>
        /// get/set - Whether to uppercase thread id value.
        /// </summary>
        [FormatKeywordProperty("uppercase", new string[] { "uc", "up", "upper" })]
        public bool ToUpper { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ThreadIdKeyword object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public ThreadIdKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the thread Id from the TraceEvent.
        /// </summary>
        /// <param name="traceEvent">Information object containing data for the keyword.</param>
        /// <returns>A message.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent != null)
            {
                var builder = new StringBuilder(traceEvent.Thread.ThreadId);

                // Limit length of message.
                if (this.Length > 0 && this.Length < builder.Length)
                    builder.Remove(this.Length, builder.Length - this.Length);

                // Lower case message.
                if (this.ToLower)
                    return builder.ToString().ToLower();

                // Upper case message.
                if (this.ToUpper)
                    return builder.ToString().ToUpper();

                return builder.ToString();
            }

            return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
