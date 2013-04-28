using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// A NewlineKeyword is a newline character.
    /// </summary>
    [TraceKeyword("newline")]
    public sealed class NewlineKeyword
        : StaticKeyword
    {
        #region Variables
        public enum LineEnding
        {
            /// <summary>
            /// \r
            /// </summary>
            CR,
            /// <summary>
            /// \n
            /// </summary>
            LF,
            /// <summary>
            /// \r\n
            /// </summary>
            CRLF,
            /// <summary>
            /// Enivronment.Newline
            /// </summary>
            Default,
            /// <summary>
            /// No line ending.
            /// </summary>
            None
        }
        private LineEnding _Mode;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Line ending mode syntax.
        /// </summary>
        [DefaultValue(LineEnding.Default)]
        [TraceKeywordProperty("mode", new string[] { "m" }, typeof(EnumConverter), typeof(LineEnding))]
        public LineEnding Mode
        {
            get { return _Mode; }
            set
            {
                // Update the Text property.
                if (value != _Mode)
                    this.Text = NewlineKeyword.GetLineEnding(value);
                _Mode = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a NewlineKeyword object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public NewlineKeyword(StringDictionary attributes)
            : base(System.Environment.NewLine, attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the line ending string based on the specified mode.
        /// </summary>
        /// <param name="mode">LineEnding mode option.</param>
        /// <returns>Newline string value based on the specified mode.</returns>
        private static string GetLineEnding(LineEnding mode)
        {
            switch (mode)
            {
                case (LineEnding.CR):
                    return "\r";
                case (LineEnding.LF):
                    return "\n";
                case (LineEnding.CRLF):
                    return "\r\n";
                case (LineEnding.None):
                    return string.Empty;
                default:
                    return System.Environment.NewLine;
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
