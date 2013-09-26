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
    /// Renders the Id of the trace event.
    /// </summary>
    [Element("id")]
    public sealed class TraceIdElement
        : TraceElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The string format of the TraceEventType property.
        /// </summary>
        [ElementProperty("format", new string[] { "f" })]
        public string Format { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a IdElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary of attributes.</param>
        public TraceIdElement(StringDictionary attributes = null)
            : base(attributes)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Renders the keyword from the trace event.
        /// </summary>
        /// <param name="traceEvent">Information object containing data for the keyword.</param>
        /// <returns>Rendered keyword value.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent != null)
                return traceEvent.Id.ToString(this.Format);
            return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
