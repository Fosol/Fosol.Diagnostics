﻿using System;
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
    [TraceKeyword("processId")]
    public sealed class ProcessIdKeyword
        : DynamicKeyword
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
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>The trace event call stack.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            return traceEvent.Process.ProcessId.ToString();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}