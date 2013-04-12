using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TraceKeywordAttribute
        : Attribute
    {
        #region Variables
        #endregion

        #region Properties
        public string Name { get; set; }
        #endregion

        #region Constructors
        public TraceKeywordAttribute(string name)
        {
            this.Name = name;
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
