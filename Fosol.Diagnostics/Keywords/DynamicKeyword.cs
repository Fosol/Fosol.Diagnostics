using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// A DynamicKeyword abstract class provides a framework for dynamically generated keywords.
    /// These keywords will be generated each time the GetText() method is called.
    /// </summary>
    public abstract class DynamicKeyword
        : TraceKeywordBase, IDynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DynamicKeyword object.
        /// </summary>
        /// <param name="attributes">StringDictionary of attributes to include with this keyword.</param>
        public DynamicKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the dynamic value of the keyword.
        /// </summary>
        /// <param name="logEvent">LogEvent containing information for Keyword.</param>
        /// <returns>The dynamic value of the keyword.</returns>
        public abstract string Render(TraceEvent logEvent);
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
