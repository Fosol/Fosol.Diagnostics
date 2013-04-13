using Fosol.Common.Extensions.NameValueCollections;
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
    /// A keyword provides a way to insert dynamic data into a message format.
    /// Don't inherit directly from this abstract class, instead inherit from LogStaticKeyword or LogDynamicKeyword.
    /// </summary>
    public abstract class TraceKeywordBase
        : ITraceKeyword
    {
        #region Variables
        private string _Name;
        private readonly StringDictionary _Attributes = new StringDictionary();
        #endregion

        #region Properties
        /// <summary>
        /// get - The name value in the KeywordAttribute.
        /// </summary>
        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

        /// <summary>
        /// get - A dictionary of attributes for this keyword.
        /// </summary>
        public StringDictionary Attributes
        {
            get { return _Attributes; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceKeywordBase object.
        /// Initializes the Name property with the KeywordAttribute.Name property.
        /// </summary>
        /// <exception cref="Fosol.Common.Exceptions.MissingAttributeException">The TraceKeywordAttribute is required.</exception>
        public TraceKeywordBase()
        {
            var attr = (TraceKeywordAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(TraceKeywordAttribute));
            if (attr != null)
                this.Name = attr.Name;
            else
                throw new Common.Exceptions.MissingAttributeException(typeof(TraceKeywordAttribute));
        }

        /// <summary>
        /// Creates a new instance of a TraceKeywordBase object.
        /// </summary>
        /// <param name="parameters">NameValueCollection of parameters to include with this keyword.</param>
        public TraceKeywordBase(NameValueCollection parameters)
            : this()
        {
            InitParameters(parameters);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the Parameters collection.
        /// </summary>
        /// <param name="parameters">NameValueCollection of parameters to include with this keyword.</param>
        protected void InitParameters(NameValueCollection parameters)
        {
            // Get all the valid parameter options.
            var properties = (
                from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.GetCustomAttributes(typeof(TraceKeywordBaseParameterAttribute), false).FirstOrDefault() != null
                select new
                {
                    Property = p,
                    Parameter = p.GetCustomAttributes(typeof(TraceKeywordBaseParameterAttribute), false).FirstOrDefault() as TraceKeywordBaseParameterAttribute,
                    DefaultValue = p.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute
                });

            // Loop through the valid properties (this will stop incorrect parameters be provided).
            foreach (var prop in properties)
            {
                string value = null;

                // Check if the parameters have a match.
                if (parameters != null)
                {
                    value = parameters[prop.Parameter.Name];

                    // Go through abbreviations.
                    if (value == null && prop.Parameter.Abbreviations != null)
                    {
                        foreach (var abbr in prop.Parameter.Abbreviations)
                        {
                            // Found a valid abbreviation.
                            if (parameters[abbr] != null)
                            {
                                value = parameters[abbr];
                                break;
                            }
                        }
                    }
                }

                // Parameter was found use it.
                if (value != null)
                {
                    if (prop.Parameter.Converter == null)
                        Common.Helpers.ReflectionHelper.SetValue(prop.Property, this, value);
                    else
                        Common.Helpers.ReflectionHelper.SetValue(prop.Property, this, value, prop.Parameter.Converter);
                    _Parameters.Add(prop.Parameter.Name, value);
                }
                // Use the default value.
                else if (prop.DefaultValue != null)
                    prop.Property.SetValue(this, prop.DefaultValue.Value);
                // This parameter is required, throw exception.
                else if (prop.Parameter.IsRequired)
                    throw new Exceptions.TraceKeywordBaseParameterIsRequiredException(this.Name, prop.Parameter.Name);
            }
        }

        /// <summary>
        /// Returns a formatted string value to create this keyword.
        /// </summary>
        /// <example>
        /// ${datetime:format=u}
        /// </example>
        /// <returns>Special formatted string value.</returns>
        public override string ToString()
        {
            if (this.Parameters.Count == 0)
                return "${" + this.Name + "}";
            else
                return string.Format("${{{0}?{1}}}", this.Name, this.Parameters.ToQueryString());
        }

        /// <summary>
        /// Returns the HashCode for this keyword.
        /// This HashCode is composed of the Name and the Parameters.
        /// </summary>
        /// <returns>HashCode for this keyword.</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode()
                + this.Parameters.GetHashCode();
        }

        /// <summary>
        /// Determine if the object is equal to this keyword.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if they are equal.</returns>
        public override bool Equals(object obj)
        {
            var keyword = obj as TraceKeywordBase;

            if (ReferenceEquals(keyword, null))
                return false;

            if (ReferenceEquals(keyword, this))
                return true;

            if (this.Name == keyword.Name)
                return this.Parameters.IsEqual(keyword.Parameters);

            return false;
        }
        #endregion

        #region Events
        #endregion
    }
}
