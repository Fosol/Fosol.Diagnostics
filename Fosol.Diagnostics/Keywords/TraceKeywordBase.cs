using Fosol.Common.Extensions.Dictionaries;
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
    /// Don't inherit directly from this abstract class, instead inherit from DynamicKeyword.
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
        /// <exception cref="Fosol.Common.Exceptions.MissingAttributeException">The TraceKeywordAttributeAttribute is required.</exception>
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
        /// <param name="attributes">Dictionary of attributes to include with this keyword.</param>
        public TraceKeywordBase(StringDictionary attributes)
            : this()
        {
            InitAttributes(attributes);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the Attributes collection.
        /// </summary>
        /// <param name="attributes">Dictionary of attributes to include with this keyword.</param>
        protected void InitAttributes(StringDictionary attributes)
        {
            // Get all the valid attributes.
            var properties = (
                from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.GetCustomAttributes(typeof(TraceKeywordPropertyAttribute), false).FirstOrDefault() != null
                select new
                {
                    Property = p,
                    Attribute = p.GetCustomAttributes(typeof(TraceKeywordPropertyAttribute), false).FirstOrDefault() as TraceKeywordPropertyAttribute,
                    DefaultValue = p.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute
                });

            // Loop through the valid properties (this will stop incorrect parameters be provided).
            foreach (var prop in properties)
            {
                string value = null;

                // Check if the parameters have a match.
                if (attributes != null)
                {
                    value = attributes[prop.Attribute.Name];

                    // Go through abbreviations.
                    if (value == null && prop.Attribute.Abbreviations != null)
                    {
                        foreach (var abbr in prop.Attribute.Abbreviations)
                        {
                            // Found a valid abbreviation.
                            if (attributes[abbr] != null)
                            {
                                value = attributes[abbr];
                                break;
                            }
                        }
                    }
                }

                // Parameter was found use it.
                if (value != null)
                {
                    if (prop.Attribute.Converter == null)
                        Common.Helpers.ReflectionHelper.SetValue(prop.Property, this, value);
                    else
                        Common.Helpers.ReflectionHelper.SetValue(prop.Property, this, value, prop.Attribute.Converter);
                    this.Attributes.Add(prop.Attribute.Name, value);
                }
                // Use the default value.
                else if (prop.DefaultValue != null)
                    prop.Property.SetValue(this, prop.DefaultValue.Value);
                // This parameter is required, throw exception.
                else if (prop.Attribute.IsRequired)
                    throw new Exceptions.TraceKeywordAttributeRequiredException(this.Name, prop.Attribute.Name);
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
            if (this.Attributes.Count == 0)
                return "{" + this.Name + "}";
            else
                return string.Format("{{{0}?{1}}}", this.Name, this.Attributes.ToQueryString());
        }

        /// <summary>
        /// Returns the HashCode for this keyword.
        /// This HashCode is composed of the Name and the Parameters.
        /// </summary>
        /// <returns>HashCode for this keyword.</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode()
                + this.Attributes.GetHashCode();
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
                return this.Attributes.IsEqual(keyword.Attributes);

            return false;
        }
        #endregion

        #region Events
        #endregion
    }
}
