using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Events
{
    /// <summary>
    /// EventArgs that are passed when an exception occurs during configuration.
    /// </summary>
    public class ConfigurationExceptionEventArgs
        : EventArgs
    {
        #region Variables
        private readonly Exception _Exception;
        #endregion

        #region Properties
        public Exception Exception
        {
            get { return _Exception; }
        }
        #endregion

        #region Constructors
        public ConfigurationExceptionEventArgs(Exception exception)
        {
            _Exception = exception;
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
