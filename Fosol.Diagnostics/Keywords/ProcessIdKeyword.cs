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
    /// The ProcessId from the trace event.
    /// </summary>
    [FormatKeyword("processId")]
    public sealed class ProcessIdKeyword
        : TraceKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ProcessIdKeyword object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public ProcessIdKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the call stack value of the trace event.
        /// </summary>
        /// <param name="traceEvent">Information object containing data for the keyword.</param>
        /// <returns>The trace event call stack.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent != null)
                return traceEvent.Process.ProcessId.ToString();

            return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
