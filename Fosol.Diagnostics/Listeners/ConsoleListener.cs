using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    [TraceInitialize("useErrorStream", typeof(bool))]
    public class ConsoleListener
        : TextWriterListener
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ConsoleListener()
            : this(false)
        {

        }

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
