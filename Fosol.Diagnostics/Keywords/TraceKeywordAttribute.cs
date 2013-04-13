using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// Defines the keyword name value within the special syntax ${[name]}
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TraceKeywordAttribute
        : Attribute
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The name of the keyword.
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceKeywordAttribute.
        /// </summary>
        /// <param name="name">Name of the keyword.</param>
        public TraceKeywordAttribute(string name)
        {
            this.Name = name;
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        #endregion
    }
}
