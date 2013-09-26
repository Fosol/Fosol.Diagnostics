using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Exceptions
{
    public class SourceConfigurationException
        : TraceConfigurationException
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public SourceConfigurationException()
            : base()
        {

        }

        public SourceConfigurationException(string message)
            : base(message)
        {

        }

        public SourceConfigurationException(string message, Exception inner)
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
