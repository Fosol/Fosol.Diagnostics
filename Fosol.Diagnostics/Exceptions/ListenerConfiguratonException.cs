using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Exceptions
{
    public class ListenerConfiguratonException
        : TraceConfigurationException
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ListenerConfiguratonException()
            : base()
        {

        }

        public ListenerConfiguratonException(string message)
            : base(message)
        {

        }

        public ListenerConfiguratonException(string message, Exception inner)
            : base(message, inner)
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
