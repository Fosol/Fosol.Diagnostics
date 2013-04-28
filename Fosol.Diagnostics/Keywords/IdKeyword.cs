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
    [TraceKeyword("id")]
    public sealed class IdKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The string format of the TraceEventType property.
        /// </summary>
        [TraceKeywordProperty("format", new string[] { "f" })]
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
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>Rendered keyword value.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            return traceEvent.Id.ToString(this.Format);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
