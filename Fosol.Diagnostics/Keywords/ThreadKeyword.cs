using Fosol.Common.Formatters.Keywords;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// The ThreadKeyword provides a way to output thread information within the TraceEvent object.
    /// </summary>
    [FormatKeyword("thread", true)]
    public sealed class ThreadKeyword
        : TraceKeyword
    {
        #region Variables
        private string _Key;
        #endregion

        #region Properties
        [DefaultValue("Name")]
        [FormatKeywordProperty("Key", new[] { "k" })]
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ThreadKeyword object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public ThreadKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the thread Id from the TraceEvent.
        /// </summary>
        /// <param name="traceEvent">Information object containing data for the keyword.</param>
        /// <returns>A message.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent != null && traceEvent.Thread != null)
            {
                var prop = (
                    from p in traceEvent.Thread.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    where p.Name.Equals(this.Key, StringComparison.InvariantCulture)
                    select p).FirstOrDefault();

                if (prop != null)
                    return string.Format("{0}", prop.GetValue(traceEvent.Thread));
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
