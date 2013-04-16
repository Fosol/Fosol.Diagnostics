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
    /// SourceKeyword returns the source name from the trace event.
    /// </summary>
    [TraceKeyword("source")]
    public sealed class SourceKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a SourceKeyword object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public SourceKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates the source text from this LogMessage object.
        /// </summary>
        /// <param name="logEvent">LogEvent object.</param>
        /// <returns>Message Source value.</returns>
        public override string Render(TraceEvent logEvent)
        {
            return logEvent.Source;
        }
        #endregion

        #region Events
        #endregion
    }
}
