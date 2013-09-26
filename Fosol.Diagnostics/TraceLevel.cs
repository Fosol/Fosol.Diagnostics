using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    [Flags]
    public enum TraceLevel
    {
        Debug = 0x1,
        Information = 0x2,
        Start = 0x4,
        Stop = 0x8,
        Suspend = 0x10,
        Resume = 0x20,
        Warning = 0x40,
        Error = 0x80,
        Critical = 0x160
    }
}
