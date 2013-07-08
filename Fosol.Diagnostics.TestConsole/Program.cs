using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fosol.Diagnostics.TestConsole
{
    class Program
    {

        #region Variables
        //private readonly Fosol.Diagnostics.TraceWriter log = Fosol.Diagnostics.TraceManager.GetWriter(typeof(Program));
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        static void Main(string[] args)
        {
            var program = new Program();

            var stop = false;

            do
            {
                program.MultiThread();
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.Escape)
                    stop = true;
            }
            while (!stop);

            //log.Flush();
        }

        public void SingleThread()
        {
            Write();
        }

        public void MultiThread()
        {
            var threads = new System.Threading.Thread[10];

            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new System.Threading.Thread(new ThreadStart(Write));
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        public void Write()
        {
            var log = Fosol.Diagnostics.TraceManager.GetWriter(typeof(Program));
            log.Write(TraceEventType.Start, "Start");
            log.Write(TraceEventType.Debug, "Debug");
            log.Write(TraceEventType.Information, "Information");
            log.Write(TraceEventType.Warning, "Warning");
            log.Write(TraceEventType.Error, "Error");
            log.Write(TraceEventType.Critical, "Critical");
            log.Write(TraceEventType.Stop, "Stop");
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
