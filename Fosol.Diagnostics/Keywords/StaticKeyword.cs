using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// A StaticKeyword is a basic text value that may include parameters.
    /// </summary>
    public abstract class StaticKeyword
        : TraceKeywordBase, IStaticKeyword
    {
        #region Variables
        private string _Text;
        #endregion

        #region Properties
        /// <summary>
        /// get - The static text that this Keyword will return.
        /// </summary>
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
        /// <summary>
        /// Creates a new instance of a StaticKeyword object.
        /// Remember to populate the Text property in your subclass.
        /// </summary>
        public StaticKeyword()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a StaticKeyword object.
        /// Remember to populate the Text property in your subclass.
        /// </summary>
        /// <param name="text">Original string value that created this keyword.</param>
        public StaticKeyword(string text)
            : base()
        {
            this.Text = text;
        }

        /// <summary>
        /// Creates a new instance of a StaticKeyword object.
        /// Remember to populate the Text property in your subclass.
        /// </summary>
        /// <param name="attributes">StringDictionary of attributes to include with this keyword.</param>
        public StaticKeyword(StringDictionary attributes)
            : base(attributes)
        {
        }

        /// <summary>
        /// Creates a new instance of a StaticKeyword object.
        /// Remember to populate the Text property in your subclass.
        /// </summary>
        /// <param name="value">Original string value that created this keyword.</param>
        /// <param name="attributes">StringDictionary of attributes to include with this keyword.</param>
        public StaticKeyword(string text, StringDictionary attributes)
            : base(attributes)
        {
            this.Text = text;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a formatted string value to create this keyword.
        /// </summary>
        /// <returns>String value.</returns>
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
