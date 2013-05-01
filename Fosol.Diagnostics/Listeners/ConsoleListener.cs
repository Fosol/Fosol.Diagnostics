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
        private ConsoleColor _Background;
        #endregion

        #region Properties
        [TraceSetting("background", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor Background
        {
            get 
            { 
                return _Background; 
            }
            set 
            { 
                _Background = value;
                Console.BackgroundColor = value;
            }
        }
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
