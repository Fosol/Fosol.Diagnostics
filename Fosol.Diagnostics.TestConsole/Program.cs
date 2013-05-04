using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fosol.Diagnostics.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = Fosol.Diagnostics.TraceManager.GetWriter();
            log.Write(TraceEventType.Start, "Start");
            log.Write(TraceEventType.Debug, "Debug");
            log.Write(TraceEventType.Information, "Information");
            log.Write(TraceEventType.Warning, "Warning");
            log.Write(TraceEventType.Error, "Error");
            log.Write(TraceEventType.Critical, "Critical");
            log.Write(TraceEventType.Stop, "Stop");

            Console.ReadKey();
        }
    }
}
