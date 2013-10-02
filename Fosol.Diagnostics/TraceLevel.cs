using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// TraceEvent.Level options.
    /// TraceLevel provides a way to describe the type of TraceEvent.
    /// </summary>
    public enum TraceLevel
    {
        /// <summary>
        /// A debug message.
        /// </summary>
        Debug = 0x1,
        /// <summary>
        /// An information message.
        /// </summary>
        Information = 0x2,
        /// <summary>
        /// A start message.
        /// </summary>
        Start = 0x4,
        /// <summary>
        /// A stop message.
        /// </summary>
        Stop = 0x8,
        /// <summary>
        /// A suspend message.
        /// </summary>
        Suspend = 0x10,
        /// <summary>
        /// A resume message.
        /// </summary>
        Resume = 0x20,
        /// <summary>
        /// A warning message.
        /// </summary>
        Warning = 0x40,
        /// <summary>
        /// An error message.
        /// </summary>
        Error = 0x80,
        /// <summary>
        /// A critical error message.
        /// </summary>
        Critical = 0x160
    }
}
