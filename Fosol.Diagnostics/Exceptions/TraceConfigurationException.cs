using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Exceptions
{
    public class TraceConfigurationException
        : System.Configuration.ConfigurationErrorsException
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public TraceConfigurationException()
            : base()
        {

        }

        public TraceConfigurationException(string message)
            : base(message)
        {

        }

        public TraceConfigurationException(string message, Exception inner)
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
