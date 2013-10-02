using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fosol.Diagnostics.Test.Console
{
    class Program
    {
        #region Variables
        private readonly Fosol.Diagnostics.TraceWriter _Trace = Fosol.Diagnostics.Trace.GetWriter(typeof(Program));
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
                program.SingleThread();
                var key = System.Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                    stop = true;

                //System.Threading.Thread.Sleep(1000);
            }
            while (!stop);
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
            _Trace.Header();
            _Trace.Write(TraceLevel.Start, "Start message");
            _Trace.Write(TraceLevel.Debug, "Debug message");
            _Trace.Write("Information message");
            _Trace.Write(TraceLevel.Warning, "Warning message");
            _Trace.Write(TraceLevel.Error, "Error message");
            _Trace.Write(TraceLevel.Critical, "Critical message");
            _Trace.Write(TraceLevel.Suspend, "Suspend message");
            _Trace.Write(TraceLevel.Resume, "Resume message");
            _Trace.Write(TraceLevel.Stop, "Stop message");
            _Trace.Footer();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
