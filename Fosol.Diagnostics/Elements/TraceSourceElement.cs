using Fosol.Common.Parsers;
using Fosol.Common.Parsers.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Elements
{
    /// <summary>
    /// TraceSourceElement returns the TraceEvent.SourceType value.
    /// The SourceType is generally the Type of the class that originally sent the message to the listeners.
    /// </summary>
    [Element("source")]
    public sealed class TraceSourceElement
        : TraceElement
    {
        #region Variables
        private string _Key;
        #endregion

        #region Properties
        [DefaultValue("FullName")]
        [ElementProperty("Key", new[] { "k" })]
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceSourceElement object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public TraceSourceElement(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates the source text from this LogMessage object.
        /// </summary>
        /// <param name="trace">Information object containing data for the keyword.</param>
        /// <returns>Message Source value.</returns>
        public override string Render(TraceEvent trace)
        {
            if (trace != null && trace.Writer.SourceType != null)
            {
                var prop = (
                    from p in trace.Writer.SourceType.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    where p.Name.Equals(this.Key, StringComparison.InvariantCulture)
                    select p).FirstOrDefault();

                if (prop != null)
                    return string.Format("{0}", prop.GetValue(trace.Writer.SourceType));
            }

            return null;
        }
        #endregion

        #region Events
        #endregion
    }
}
