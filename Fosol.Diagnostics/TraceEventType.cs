using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public enum TraceEventType
    {
        Debug = 0,
        Information = 1,
        Warning = 2,
        Error = 3,
        Critical = 4,
        Start = 5,
        Stop = 6,
        Suspend = 7,
        Resume = 8
    }
}
