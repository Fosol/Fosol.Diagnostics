using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// The UnitTestListener provides a way to perform some useful and simple tests.
    /// </summary>
    public sealed class UnitTestListener
        : TraceListener
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The number of times this TraceListener has been called.
        /// </summary>
        public int Counter { get; private set; }

        /// <summary>
        /// get - The last message rendered by this TraceListener.
        /// </summary>
        public string LastMessage { get; private set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Increments the counter and saves the last message.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        protected override void OnWrite(TraceEvent trace)
        {
            this.Counter++;
            this.LastMessage = this.Render(trace);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
