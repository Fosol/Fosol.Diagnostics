using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// Renders the application domain name.
    /// </summary>
    [TraceKeyword("appDomain")]
    public sealed class AppDomainKeyword
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
        /// Renders the current application domain name.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>The current application domain name.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
