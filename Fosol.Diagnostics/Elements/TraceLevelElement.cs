using Fosol.Common.Parsers;
using Fosol.Common.Parsers.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Elements
{
    /// <summary>
    /// Provides a way to render the TraceLevel information.
    /// </summary>
    [Element("level")]
    public sealed class TraceLevelElement
        : TraceElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The string format of the TraceLevel property.
        /// </summary>
        [ElementProperty("format", new string[] { "f" })]
        public string Format { get; set; }

        /// <summary>
        /// get/set - Whether to lowercase.
        /// </summary>
        [ElementProperty("lowercase", new string[] { "lc", "low", "lower" })]
        public bool ToLower { get; set; }

        /// <summary>
        /// get/set - Whether to uppercase.
        /// </summary>
        [ElementProperty("uppercase", new string[] { "uc", "up", "upper" })]
        public bool ToUpper { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceLevelElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary of attributes.</param>
        public TraceLevelElement(StringDictionary attributes = null)
            : base(attributes)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Renders the log event type information for the trace event.
        /// </summary>
        /// <param name="traceEvent">Information object containing data for the keyword.</param>
        /// <returns>Log event type for the trace event.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent != null)
            {
                if (this.ToLower)
                    return traceEvent.Level.ToString(this.Format).ToLower();

                if (this.ToUpper)
                    return traceEvent.Level.ToString(this.Format).ToUpper();

                return traceEvent.Level.ToString(this.Format);
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
