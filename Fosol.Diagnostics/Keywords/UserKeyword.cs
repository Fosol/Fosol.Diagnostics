using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// Renders the current username.
    /// </summary>
    [TraceKeyword("user")]
    public sealed class UserKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Outputs the username and domain.
        /// </summary>
        /// <param name="logEvent">LogEvent object.</param>
        /// <returns>The username currently logged in.</returns>
        public override string Render(TraceEvent logEvent)
        {
            return Environment.UserDomainName + "\\" + Environment.UserName;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
