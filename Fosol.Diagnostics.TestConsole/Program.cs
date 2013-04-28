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
            log.Write(TraceEventType.Debug, "test");

            Console.ReadKey();
        }
    }
}
