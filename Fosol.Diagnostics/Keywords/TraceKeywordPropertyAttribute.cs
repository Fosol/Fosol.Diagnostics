using Fosol.Common.Extensions.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// Defines a property as a keyword parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TraceKeywordPropertyAttribute
        : Attribute
    {
        #region Variables
        private readonly string _Name;
        private readonly string[] _Abbr;
        private readonly bool _IsRequired;
        private readonly TypeConverter _Converter;
        #endregion

        #region Properties
        /// <summary>
        /// get - Unique name to identify this parameter in a formatted string.
        /// </summary>
        public string Name { get { return _Name; } }

        /// <summary>
        /// get - Unique abbreviations that can be used instead of the full name.
        /// Be careful when assigning abbreviations to ensure they are not used by another parameter for the same keyword.
        /// </summary>
        public string[] Abbreviations { get { return _Abbr; } }

        /// <summary>
        /// get - Whether this parameter is optional.
        /// </summary>
        [DefaultValue(false)]
        public bool IsRequired { get { return _IsRequired; } }

        /// <summary>
        /// get - Type of TypeConverter to use when setting the property of field.
        /// </summary>
        public TypeConverter Converter
        {
            get { return _Converter; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        public TraceKeywordPropertyAttribute(string name)
            : this(name, null, false, null)
        {
        }

        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        /// <param name="isRequired">Whether the parameter is optional.</param>
        public TraceKeywordPropertyAttribute(string name, bool isRequired)
            : this(name, null, isRequired, null)
        {
        }

        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        /// <param name="converterType">TypeConverter to use to convert configuration values.</param>
        /// <param name="converterArgs">Arguments to supply to the TypeConverter.</param>
        public TraceKeywordPropertyAttribute(string name, Type converterType, params object[] converterArgs)
            : this(name, null, false, converterType, converterArgs)
        {
        }

        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        /// <param name="isRequired">Whether the parameter is optional.</param>
        /// <param name="converterType">TypeConverter to use to convert configuration values.</param>
        /// <param name="converterArgs">Arguments to supply to the TypeConverter.</param>
        public TraceKeywordPropertyAttribute(string name, bool isRequired, Type converterType, params object[] converterArgs)
            : this(name, null, isRequired, converterType, converterArgs)
        {
        }

        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        /// <param name="abbrev">Parameter name abbreviations.</param>
        public TraceKeywordPropertyAttribute(string name, string[] abbrev)
            : this(name, abbrev, false, null)
        {
        }

        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        /// <param name="abbrev">Parameter name abbreviations.</param>
        /// <param name="converterType">TypeConverter to use to convert configuration values.</param>
        /// <param name="converterArgs">Arguments to supply to the TypeConverter.</param>
        public TraceKeywordPropertyAttribute(string name, string[] abbrev, Type converterType, params object[] converterArgs)
            : this(name, abbrev, false, converterType, converterArgs)
        {
        }

        /// <summary>
        /// Creates a new instance of a LogKeywordParameterAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">Parameter name.</param>
        /// <param name="abbrev">Parameter name abbreviations.</param>
        /// <param name="isRequired">Whether the parameter is optional.</param>
        /// <param name="converter">TypeConverter to use to convert configuration values.</param>
        /// <param name="converterArgs">Arguments to supply to the TypeConverter.</param>
        public TraceKeywordPropertyAttribute(string name, string[] abbrev, bool isRequired, Type converterType, params object[] converterArgs)
        {
            Common.Validation.Assert.IsNotNullOrEmpty(name, "name");
            _Name = name;
            _Abbr = abbrev;
            _IsRequired = isRequired;

            if (converterType != null)
            {
                if (converterArgs != null && converterArgs.Length > 0)
                    _Converter = (TypeConverter)Activator.CreateInstance(converterType, converterArgs);
                else if (converterType.HasEmptyConstructor())
                    _Converter = (TypeConverter)Activator.CreateInstance(converterType);
            }
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
