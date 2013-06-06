using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Exceptions
{
    /// <summary>
    /// An error occured while parsing the FormatKeywordAttributeRequiredException objects from the formatted string and a required parameter is missing.
    /// </summary>
    public sealed class FormatKeywordAttributeRequiredException
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
        /// Creates a new instance of a FormatKeywordAttributeRequiredException.
        /// </summary>
        public FormatKeywordAttributeRequiredException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a FormatKeywordAttributeRequiredException.
        /// </summary>
        /// <param name="message">A message to describe the error.</param>
        public FormatKeywordAttributeRequiredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of a FormatKeywordAttributeRequiredException.
        /// </summary>
        /// <param name="message">A message to describe the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public FormatKeywordAttributeRequiredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a FormatKeywordAttributeRequiredException.
        /// </summary>
        /// <param name="keyword">The name of the keyword that cause the error.</param>
        /// <param name="parameter">The name of the paramter that caused the error.</param>
        public FormatKeywordAttributeRequiredException(string keyword, string parameter)
            : base(String.Format(Resources.Strings.Configuration_Exception_TraceKeyword_Attribute_Required, keyword, parameter))
        {
        }

        /// <summary>
        /// Creates a new instance of a FormatKeywordAttributeRequiredException.
        /// </summary>
        /// <param name="keyword">The name of the keyword that cause the error.</param>
        /// <param name="parameter">The name of the paramter that caused the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public FormatKeywordAttributeRequiredException(string keyword, string parameter, Exception innerException)
            : base(String.Format(Resources.Strings.Configuration_Exception_TraceKeyword_Attribute_Required, keyword, parameter), innerException)
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
