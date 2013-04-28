using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// The Ticks value of the current date and time.
    /// </summary>
    [TraceKeyword("ticks")]
    public sealed class TicksKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TicksKeyword object.
        /// </summary>
        public TicksKeyword()
            : base()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// The Ticks value of the current date and time.
        /// </summary>
        /// <param name="message">LogMessage object.</param>
        /// <returns>The Ticks value of the current date and time.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            return DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
