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
    /// Renders a guid into the message.
    /// </summary>
    [TraceKeyword("guid")]
    public sealed class GuidKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The format string for the Guid.
        /// </summary>
        [DefaultValue("N")]
        [TraceKeywordProperty("format", new string[] { "f" })]
        public string Format { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a GuidKeyword.
        /// </summary>
        /// <param name="attribute">StringDictionary object.</param>
        public GuidKeyword(StringDictionary attributes)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a Guid id value.
        /// </summary>
        /// <param name="logEvent">LogEvent object.</param>
        /// <returns>Guid id value.</returns>
        public override string Render(TraceEvent logEvent)
        {
            return Guid.NewGuid().ToString(this.Format);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
