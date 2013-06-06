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
    /// Renders the Id of the trace event.
    /// </summary>
    [FormatKeyword("id")]
    public sealed class IdKeyword
        : TraceKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The string format of the TraceEventType property.
        /// </summary>
        [FormatKeywordProperty("format", new string[] { "f" })]
        public string Format { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a IdKeyword object.
        /// </summary>
        /// <param name="attributes">StringDictionary of attributes.</param>
        public IdKeyword(StringDictionary attributes = null)
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
