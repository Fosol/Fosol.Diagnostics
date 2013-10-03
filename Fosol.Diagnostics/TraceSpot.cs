using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// TraceSpot provides a way to identify the position of a TraceEvent.
    /// Specifically this enum provides a way to determine if a TraceEvent is a header or footer.
    /// </summary>
    enum TraceSpot
    {
        /// <summary>
        /// A default message.
        /// </summary>
        Message = 0,
        /// <summary>
        /// A header message.
        /// </summary>
        Header = 1,
        /// <summary>
        /// A footer message.
        /// </summary>
        Footer = 2
    }
}
