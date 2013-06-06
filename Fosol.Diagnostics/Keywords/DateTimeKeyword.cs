using Fosol.Common.Formatters.Keywords;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// DateTimeKeyword places a date time value into the message.
    /// </summary>
    [FormatKeyword("datetime", true)]
    public sealed class DateTimeKeyword
        : TraceKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The format string for the DateTime value.
        /// </summary>
        [DefaultValue("G")]
        [FormatKeywordProperty("format", new string[] { "f" })]
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
        /// <param name="traceEvent">Information object containing data for the keyword.</param>
        /// <returns>Message DateTime value.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent != null)
                return traceEvent.DateTime.ToString(this.Format);

            return DateTime.Now.ToString(this.Format);
        }
        #endregion

        #region Events
        #endregion
    }
}
