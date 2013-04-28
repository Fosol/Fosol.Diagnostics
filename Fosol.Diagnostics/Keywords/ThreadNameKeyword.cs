using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// Renders the current thread name.
    /// </summary>
    [TraceKeyword("threadName")]
    public sealed class ThreadNameKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ThreadNameKeyword object.
        /// </summary>
        public ThreadNameKeyword()
            : base()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the thread name.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>Thread name.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            return Thread.CurrentThread.Name;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}