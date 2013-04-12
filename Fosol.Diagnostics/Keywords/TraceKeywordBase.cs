using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    public abstract class TraceKeywordBase
    {
        #region Variables
        private string _Name;
        private readonly StringDictionary _Attributes = new StringDictionary();
        #endregion

        #region Properties
        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

        public StringDictionary Attributes
        {
            get
            {
                return _Attributes;
            }
        }
        #endregion

        #region Constructors
        public TraceKeywordBase()
        {
            var attr = (TraceKeywordAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(TraceKeywordAttribute));
            if (attr != null)
                this.Name = attr.Name;
            else
                throw new Common.Exceptions.MissingAttributeException(typeof(TraceKeywordAttribute));
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
