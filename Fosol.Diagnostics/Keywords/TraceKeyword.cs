using Fosol.Common.Extensions.Dictionaries;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// A keyword provides a way to insert dynamic data into a message format.
    /// Don't inherit directly from this abstract class, instead inherit from DynamicKeyword.
    /// </summary>
    public abstract class TraceKeyword
        : Fosol.Common.Formatters.Keywords.FormatDynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceKeyword object.
        /// Initializes the Name property with the KeywordAttribute.Name property.
        /// </summary>
        /// <exception cref="Fosol.Common.Exceptions.AttributeRequiredException">The TraceKeywordAttributeAttribute is required.</exception>
        public TraceKeyword()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceKeyword object.
        /// </summary>
        /// <param name="attributes">Dictionary of attributes to include with this keyword.</param>
        public TraceKeyword(StringDictionary attributes)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the dynamically generated keyword value.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>Dynamically generated keyword value.</returns>
        public abstract string Render(TraceEvent traceEvent);

        /// <summary>
        /// Submit the object data to the Render method with the TraceEvent argument type.
        /// Returns the dynamically generated keyword value.
        /// </summary>
        /// <param name="data">Information to be used when generating the results.</param>
        /// <returns>Dynamically generated keyword value.</returns>
        public override string Render(object data)
        {
            return Render(data as TraceEvent);
        }
        #endregion

        #region Events
        #endregion
    }
}
