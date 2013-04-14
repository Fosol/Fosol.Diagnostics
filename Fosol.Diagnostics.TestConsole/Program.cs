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
            var log = Fosol.Diagnostics.LogManager.GetWriter();

            var value = 0;
            while (true)
            {
                log.Info(value++, "information");
                Thread.Sleep(1000);
            }

            Console.ReadKey();

            log.Flush();
        }
    }
}
