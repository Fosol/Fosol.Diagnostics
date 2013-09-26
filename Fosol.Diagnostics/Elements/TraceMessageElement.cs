using Fosol.Common.Parsers;
using Fosol.Common.Parsers.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Elements
{
    /// <summary>
    /// The message to write to the log.
    /// </summary>
    [Element("message")]
    public sealed class TraceMessageElement
        : TraceElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The maximum length the message may be.
        /// </summary>
        [ElementProperty("length", new string[] { "l", "len" })]
        public int Length { get; set; }

        /// <summary>
        /// get/set - Whether to lowercase message.
        /// </summary>
        [ElementProperty("lowercase", new string[] { "lc", "low", "lower" })]
        public bool ToLower { get; set; }

        /// <summary>
        /// get/set - Whether to uppercase message.
        /// </summary>
        [ElementProperty("uppercase", new string[] { "uc", "up", "upper" })]
        public bool ToUpper { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a MessageElement object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public TraceMessageElement(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the text value of this keyword.
        /// </summary>
        /// <param name="traceEvent">Information object containing data for the keyword.</param>
        /// <returns>A message.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent != null)
            {
                var builder = new StringBuilder(traceEvent.Message);

                // Limit length of message.
                if (this.Length > 0 && this.Length < builder.Length)
                    builder.Remove(this.Length, builder.Length - this.Length);

                // Lower case message.
                if (this.ToLower)
                    return builder.ToString().ToLower();

                // Upper case message.
                if (this.ToUpper)
                    return builder.ToString().ToUpper();

                return builder.ToString();
            }

            return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
