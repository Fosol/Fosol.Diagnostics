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
            var stop = false;
            var log = Fosol.Diagnostics.TraceManager.GetWriter();

            do
            {
                log.Write(TraceEventType.Start, "Start");
                log.Write(TraceEventType.Debug, "Debug");
                log.Write(TraceEventType.Information, "Information");
                log.Write(TraceEventType.Warning, "Warning");
                log.Write(TraceEventType.Error, "Error");
                log.Write(TraceEventType.Critical, "Critical");
                log.Write(TraceEventType.Stop, "Stop");
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                    stop = true;
            }
            while (!stop);

            log.Flush();
        }
    }
}
