using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    public sealed class StaticKeyword
        : TraceKeywordBase, IStaticKeyword
    {
        #region Variables
        private string _Text;
        #endregion

        #region Properties
        public string Text
        {
            get
            {
                return _Text;
            }
            protected set
            {
                _Text = value;
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public override string ToString()
        {
            return this.Text;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
