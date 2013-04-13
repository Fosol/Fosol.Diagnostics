using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = Fosol.Diagnostics.LogManager.GetWriter();
            log.Info(1, "information");

            Console.ReadKey();

            log.Flush();
        }
    }
}
