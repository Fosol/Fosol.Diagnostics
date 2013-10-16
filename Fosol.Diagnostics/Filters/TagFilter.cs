using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Filters
{
    /// <summary>
    /// A TagFilter provides a way to ensure only TraceEvents that contain the tag will be sent to the TraceListeners.
    /// </summary>
    public sealed class TagFilter
        : TraceFilter
    {
        #region Variables
        private string _Key;
        private string _Value;
        private StringComparison _StringComparison;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The tag key name that identifies it.
        /// </summary>
        [TraceSetting("Key")]
        [Required(AllowEmptyStrings = false)]
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        /// <summary>
        /// get/set - The value of the tag.
        /// </summary>
        [TraceSetting("Value")]
        [Required(AllowEmptyStrings = false)]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        /// <summary>
        /// get/set - The StringComparison option to use when comparing the Value property with the TraceEvent.Writer.Tags value.
        /// </summary>
        [TraceSetting("Comparison", ConverterType = typeof(Fosol.Common.Converters.EnumConverter<StringComparison>))]
        [DefaultValue(StringComparison.InvariantCultureIgnoreCase)]
        public StringComparison StringComparison
        {
            get { return _StringComparison; }
            set { _StringComparison = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Validates whether this TraceEvent should be sent to the TraceListeners
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        /// <returns>'True' if the TraceEvent is valid.</returns>
        protected override bool OnValidate(TraceEvent trace)
        {
            if (trace.Writer.Tags.ContainsKey(this.Key))
            {
                var value = trace.Writer.Tags[this.Key];

                if (value.GetType() == typeof(string))
                {
                    if (((string)value).Equals(this.Value, this.StringComparison))
                        return true;
                }
                else
                {
                    if (value.Equals(this.Value))
                        return true;
                }
            }

            return false;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
