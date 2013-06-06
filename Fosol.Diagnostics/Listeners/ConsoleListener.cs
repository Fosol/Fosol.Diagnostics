using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// The ConsoleListener writes messages to the command console.
    /// </summary>
    [TraceInitialize("UseErrorStream", typeof(bool))]
    public class ConsoleListener
        : TextWriterListener
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ConsoleListener.
        /// Writes messages to the Console.Out stream.
        /// </summary>
        public ConsoleListener()
            : this(false)
        {

        }

        /// <summary>
        /// Creates a new instance of a ConsoleListener.
        /// </summary>
        /// <param name="useErrorStream">
        ///     If 'true' it will write messages to the Console.Error stream.
        ///     If 'false' it will write messages to the Console.Out stream.
        /// </param>
        public ConsoleListener(bool useErrorStream)
            : base(useErrorStream ? Console.Error : Console.Out)
        {

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
