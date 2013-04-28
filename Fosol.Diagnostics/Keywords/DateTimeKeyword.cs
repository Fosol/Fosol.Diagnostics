using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// DateTimeKeyword places a date time value into the message.
    /// </summary>
    [TraceKeyword("datetime")]
    public sealed class DateTimeKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The format string for the DateTime value.
        /// </summary>
        [TraceKeywordProperty("format", new string[] { "f" })]
        public string Format { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DateTimeKeyword object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public DateTimeKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the date time value of the TraceEvent.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>Message DateTime value.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            return traceEvent.DateTime.ToString(this.Format);
        }
        #endregion

        #region Events
        #endregion
    }
}
