using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Events
{
    /// <summary>
    /// TraceListener Write event information.
    /// </summary>
    public sealed class WriteEventArgs
        : EventArgs
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The data being passed to the DoWrite method.
        /// </summary>
        public TraceEvent TraceEvent { get; private set; }

        /// <summary>
        /// get/set - Controls whether the DoWrite method will be cancelled.
        /// </summary>
        public bool Cancel { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a WriteEventArgs.
        /// </summary>
        /// <param name="traceEvent">Data being passed to the DoWrite method.</param>
        public WriteEventArgs(TraceEvent traceEvent)
        {
            this.TraceEvent = traceEvent;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
