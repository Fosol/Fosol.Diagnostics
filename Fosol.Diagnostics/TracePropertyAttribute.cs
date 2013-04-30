using Fosol.Common.Extensions.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Declares the property or field as a configuration attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class TracePropertyAttribute
        : Attribute
    {
        #region Variables
        private readonly string _Name;
        private readonly bool _IsRequired;
        private readonly TypeConverter _Converter;
        #endregion

        #region Properties
        /// <summary>
        /// get - The unique name for this property used in the configuration file.
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// get - This property is required.
        /// </summary>
        public bool IsRequired
        {
            get { return _IsRequired; }
        }

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
        /// Creates a new TraceListenerPropertyAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">The unique name for this property used in the configuration file.</param>
        public TracePropertyAttribute(string name)
        {
            Common.Validation.Assert.IsNotNullOrEmpty(name, "name");
            _Name = name;
        }

        /// <summary>
        /// Creates a new TraceListenerPropertyAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">The unique name for this property used in the configuration file.</param>
        /// <param name="isRequired">If true this will throw an exception if the property/field has not been set in the configuration.</param>
        public TracePropertyAttribute(string name, bool isRequired)
            : this(name)
        {
            _IsRequired = isRequired;
        }

        /// <summary>
        /// Creates a new TraceListenerPropertyAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">The unique name for this property used in the configuration file.</param>
        /// <param name="converterType">TypeConverter to use when assigning the configured value to the property/field.</param>
        public TracePropertyAttribute(string name, Type converterType, params object[] converterArgs)
            : this(name, false, converterType, converterArgs)
        {
        }

        /// <summary>
        /// Creates a new TraceListenerPropertyAttribute object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "name" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "name" cannot be null.</exception>
        /// <param name="name">The unique name for this property used in the configuration file.</param>
        /// <param name="isRequired">If true this will throw an exception if the property/field has not been set in the configuration.</param>
        /// <param name="converterType">TypeConverter to use when assigning the configured value to the property/field.</param>
        public TracePropertyAttribute(string name, bool isRequired, Type converterType, params object[] converterArgs)
            : this(name, isRequired)
        {
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
