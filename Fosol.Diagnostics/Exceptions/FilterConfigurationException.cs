using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Exceptions
{
    public class FilterConfigurationException
        : TraceConfigurationException
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public FilterConfigurationException()
            : base()
        {

        }

        public FilterConfigurationException(string message)
            : base(message)
        {

        }

        public FilterConfigurationException(string message, Exception inner)
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
