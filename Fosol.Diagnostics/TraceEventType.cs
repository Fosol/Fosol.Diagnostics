using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// TraceEvent type values.
    /// </summary>
    public enum TraceEventType
    {
        /// <summary>
        /// Debug - A debugging message.
        /// </summary>
        Debug = 0,
        /// <summary>
        /// Information - An information message.
        /// </summary>
        Information = 1,
        /// <summary>
        /// Start - A start message.
        /// </summary>
        Start = 2,
        /// <summary>
        /// Stop - A stop message.
        /// </summary>
        Stop = 3,
        /// <summary>
        /// Suspend - A suspend message.
        /// </summary>
        Suspend = 4,
        /// <summary>
        /// Resume - A resume message.
        /// </summary>
        Resume = 5,
        /// <summary>
        /// Warning - A warning message.
        /// </summary>
        Warning = 6,
        /// <summary>
        /// Error - An error message.
        /// </summary>
        Error = 7,
        /// <summary>
        /// Critical - a critical error message.
        /// </summary>
        Critical = 8
    }
}
