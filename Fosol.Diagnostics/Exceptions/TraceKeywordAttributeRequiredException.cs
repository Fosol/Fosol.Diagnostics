using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Exceptions
{
    /// <summary>
    /// An error occured while parsing the TraceKeywordAttributeRequiredException objects from the formatted string and a required parameter is missing.
    /// </summary>
    public sealed class TraceKeywordAttributeRequiredException
        : Exception
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The keyword that caused the error.
        /// </summary>
        public string Keyword { get; private set; }

        /// <summary>
        /// get - The parameter name that caused the error.
        /// </summary>
        public string Parameter { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceKeywordAttributeRequiredException.
        /// </summary>
        public TraceKeywordAttributeRequiredException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceKeywordAttributeRequiredException.
        /// </summary>
        /// <param name="message">A message to describe the error.</param>
        public TraceKeywordAttributeRequiredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceKeywordAttributeRequiredException.
        /// </summary>
        /// <param name="message">A message to describe the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public TraceKeywordAttributeRequiredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceKeywordAttributeRequiredException.
        /// </summary>
        /// <param name="keyword">The name of the keyword that cause the error.</param>
        /// <param name="parameter">The name of the paramter that caused the error.</param>
        public TraceKeywordAttributeRequiredException(string keyword, string parameter)
            : base(String.Format(Resources.Strings.Configuration_Exception_Keyword_Attribute_Required, keyword, parameter))
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceKeywordAttributeRequiredException.
        /// </summary>
        /// <param name="keyword">The name of the keyword that cause the error.</param>
        /// <param name="parameter">The name of the paramter that caused the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public TraceKeywordAttributeRequiredException(string keyword, string parameter, Exception innerException)
            : base(String.Format(Resources.Strings.Configuration_Exception_Keyword_Attribute_Required, keyword, parameter), innerException)
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
