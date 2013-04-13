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
    [TraceKeyword("threadId")]
    public sealed class ThreadIdKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The maximum length the thread id value may be.
        /// </summary>
        [TraceKeywordProperty("length", new string[] { "l", "len" })]
        public int Length { get; set; }

        /// <summary>
        /// get/set - Whether to lowercase thread id value.
        /// </summary>
        [TraceKeywordProperty("lowercase", new string[] { "lc", "low", "lower" })]
        public bool ToLower { get; set; }

        /// <summary>
        /// get/set - Whether to uppercase thread id value.
        /// </summary>
        [TraceKeywordProperty("uppercase", new string[] { "uc", "up", "upper" })]
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
        /// Returns the text value of this keyword.
        /// </summary>
        /// <param name="logEvent">LogEvent object.</param>
        /// <returns>A message.</returns>
        public override string Render(LogEvent logEvent)
        {
            var builder = new StringBuilder(logEvent.EventCache.ThreadId);

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
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
