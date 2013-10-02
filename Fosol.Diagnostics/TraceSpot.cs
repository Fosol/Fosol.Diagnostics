using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// TraceSpot provides a way to identify the position of a TraceEvent
    /// </summary>
    enum TraceSpot
    {
        Message = 0,
        Header = 1,
        Footer = 2
    }
}
