using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Writes trace event messages to a console window.
    /// </summary>
    public class ConsoleListener
        : TextWriterListener
    {
        #region Variables
        private bool _UseErrorStream;
        #endregion

        #region Properties
        /// <summary>
        /// get - Whether this console listener uses the Console.Error stream.
        /// </summary>
        public bool UseErrorStream
        {
            get
            {
                return GetValue(ref _UseErrorStream);
            }
            private set
            {
                SetValue(ref _UseErrorStream, value);
            }
        }

        /// <summary>
        /// get - This is thread safe.
        /// </summary>
        public override bool IsThreadSafe
        {
            get { return true; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ConsoleListener object.
        /// Send trace events to the Console.Out stream.
        /// </summary>
        public ConsoleListener()
            : base(Console.Out)
        {

        }

        /// <summary>
        /// Creates a new instance of a ConsoleListener object.
        /// Send trace events to the Console.Error or the Console.Out stream.
        /// </summary>
        /// <param name="useErrorStream">True if you want to send trace events to the Console.Error stream.</param>
        public ConsoleListener(bool useErrorStream)
            : base(useErrorStream ? Console.Error : Console.Out)
        {
            this.UseErrorStream = useErrorStream;
        }
        #endregion

        #region Methods
        #endregion
        
        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
