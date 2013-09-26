using Fosol.Common.Parsers;
using Fosol.Common.Parsers.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Elements
{
    /// <summary>
    /// TraceCreatedElement places a date time value into the message.
    /// </summary>
    [Element("created")]
    public sealed class TraceCreatedElement
        : TraceElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The format string for the DateTime value.
        /// </summary>
        [DefaultValue("G")]
        [ElementProperty("format", new string[] { "f" })]
        public string Format { get; set; }

        /// <summary>
        /// get/set - Whether to display the ticks value instead of the DateTime.
        /// </summary>
        [ElementProperty("ticks")]
        public bool Ticks { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DateTimeElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public TraceCreatedElement(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the date time value of the TraceEvent.
        /// </summary>
        /// <param name="trace">Information object containing data for the keyword.</param>
        /// <returns>Message DateTime value.</returns>
        public override string Render(TraceEvent trace)
        {
            if (trace != null)
            {
                if (this.Ticks)
                    return trace.CreatedDate.Ticks.ToString(this.Format);
                else
                    return trace.CreatedDate.ToString(this.Format);
            }

            if (this.Ticks)
                return Fosol.Common.Optimization.FastDateTime.Now.Ticks.ToString(this.Format);
            else
                return Fosol.Common.Optimization.FastDateTime.Now.ToString(this.Format);
        }
        #endregion

        #region Events
        #endregion
    }
}
