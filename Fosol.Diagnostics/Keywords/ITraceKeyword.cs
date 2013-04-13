using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// A TraceKeyword is is part of a formatted message.
    /// Each TraceKeyword provides either static or dynamic text that will be used to compose a single message.
    /// </summary>
    internal interface ITraceKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The unique name to identify the keyword in a format string.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// get - A dictionary of attributes for this keyword.
        /// </summary>
        StringDictionary Attributes { get; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion
    }
}
