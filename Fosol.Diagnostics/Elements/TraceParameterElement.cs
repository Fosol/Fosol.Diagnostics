using Fosol.Common.Parsers;
using Fosol.Common.Parsers.Converters;
using Fosol.Common.Parsers.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Elements
{
    /// <summary>
    /// A TraceParameterElement provides a dynamic way to apply static parameter names while including attributes for further logic.
    /// It is useful for database parameters (i.e. {parameter?name=@Id&SqlDbType=NVarChar&value={message}}).
    /// You can use the shortcut syntax too (i.e. {@Id?value={message}}).
    /// </summary>
    [Element("parameter", true)]
    public sealed class TraceParameterElement
        : TraceElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The parameter name.
        /// </summary>
        [ElementProperty("name")]
        [Required(AllowEmptyStrings = false)]
        public string ParameterName { get; set; }

        /// <summary>
        /// get/set - The parameter value.
        /// </summary>
        [ElementProperty("value", typeof(FormatConverter))]
        [Required(AllowEmptyStrings = false)]
        public Format Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceParameterElement.
        /// </summary>
        /// <param name="attributes">StringDictionary containing attribute values.</param>
        public TraceParameterElement(StringDictionary attributes)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Dynamically generate the value this keyword makes.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>String value.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            return this.ParameterName;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
