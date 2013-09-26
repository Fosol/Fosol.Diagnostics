using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Exceptions
{
    public class SettingConfigurationException
        : TraceConfigurationException
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public SettingConfigurationException()
            : base()
        {

        }

        public SettingConfigurationException(string message)
            : base(message)
        {

        }

        public SettingConfigurationException(string message, Exception inner)
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
