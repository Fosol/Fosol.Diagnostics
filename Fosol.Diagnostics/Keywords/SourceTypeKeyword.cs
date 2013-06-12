﻿using Fosol.Common.Formatters.Keywords;
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
    /// SourceTypeKeyword returns the TraceEvent.SourceType value.
    /// The SourceType is generally the Type of the class that originally sent the message to the listeners.
    /// </summary>
    [FormatKeyword("sourceType")]
    public sealed class SourceTypeKeyword
        : TraceKeyword
    {
        #region Variables
        private string _Key;
        #endregion

        #region Properties
        [DefaultValue("FullName")]
        [FormatKeywordProperty("Key", new [] { "k" })]
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a SourceTypeKeyword object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public SourceTypeKeyword(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates the source text from this LogMessage object.
        /// </summary>
        /// <param name="traceEvent">Information object containing data for the keyword.</param>
        /// <returns>Message Source value.</returns>
        public override string Render(TraceEvent traceEvent)
        {
            if (traceEvent != null && traceEvent.SourceType != null)
            {
                var prop = (
                    from p in traceEvent.SourceType.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    where p.Name.Equals(this.Key, StringComparison.InvariantCulture)
                    select p).FirstOrDefault();

                if (prop != null)
                    return string.Format("{0}", prop.GetValue(traceEvent.SourceType));
            }

            return null;
        }
        #endregion

        #region Events
        #endregion
    }
}
