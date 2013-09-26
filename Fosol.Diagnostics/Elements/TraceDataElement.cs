using Fosol.Common.Parsers;
using Fosol.Common.Parsers.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Elements
{
    /// <summary>
    /// TraceDataElement returns a value from the data collection with the specified name.
    /// </summary>
    [Element("data")]
    public sealed class TraceDataElement
        : TraceElement
    {
        #region Variables
        private string _Key;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The unique key name to identify the value that will be return from the data collection.
        /// </summary>
        [ElementProperty("key")]
        [Required(AllowEmptyStrings = false)]
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceDataElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public TraceDataElement(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a trace message.
        /// </summary>
        /// <param name="trace">Information object containing data for the keyword.</param>
        /// <returns>Message Source value.</returns>
        public override string Render(TraceEvent trace)
        {
            if (trace != null && trace.Writer.Data != null)
                return trace.Writer.Data[this.Key].ToString();

            return null;
        }
        #endregion

        #region Events
        #endregion
    }
}
