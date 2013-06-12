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
    /// A ParameterKeyword provides a dynamic way to apply static parameter names while including attributes for further logic.
    /// It is useful for database parameters (i.e. {parameter?name=@Id&SqlDbType=NVarChar&value={message}}}).
    /// You can use the shortcut syntax too (i.e. {@Id?value={message}}}).
    /// Note that the Value property/attribute can contain keywords, just be sure to escape the end boundary if it is next to the parameter keyword end boundary.
    /// </summary>
    [FormatKeyword("parameter", true)]
    public sealed class ParameterKeyword
        : TraceKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The parameter name.
        /// </summary>
        [FormatKeywordProperty("name", true)]
        public string ParameterName { get; set; }

        /// <summary>
        /// get/set - The parameter value.
        /// </summary>
        [FormatKeywordProperty("value", true, typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParameterKeyword.
        /// </summary>
        /// <param name="attributes">StringDictionary containing attribute values.</param>
        public ParameterKeyword(StringDictionary attributes)
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
