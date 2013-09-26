using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor, AllowMultiple = true, Inherited = true)]
    public class TraceSettingAttribute
        : Attribute
    {
        #region Variables
        private Type _ConverterType;
        private TypeConverter _Converter;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A unique name to identify this setting.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// get/set - The Type of the value or the TypeConverter to use to cast the value.
        /// </summary>
        public Type ConverterType 
        {
            get { return _ConverterType; }
            set
            {
                if (typeof(TypeConverter).IsAssignableFrom(value))
                    _Converter = (TypeConverter)Activator.CreateInstance(value);
                _ConverterType = value;
            }
        }

        /// <summary>
        /// get - An instance of the TypeConverter.
        /// </summary>
        public TypeConverter Converter
        {
            get { return _Converter; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceSettingAttribute.
        /// </summary>
        /// <param name="name">A unique name to identify this setting.</param>
        public TraceSettingAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Creates a new instance of a TraceSettingAttribute.
        /// </summary>
        /// <param name="name">A unique name to identify this setting.</param>
        /// <param name="converterType">The Type of the TypeConverter to use to cast values.</param>
        /// <param name="args">Arguments necessary to create an instance of the TypeConverter.</param>
        public TraceSettingAttribute(string name, Type converterType, params object[] args)
            : this(name)
        {
            if (args != null && args.Length > 0)
                _Converter = (TypeConverter)Activator.CreateInstance(converterType, args);
            else
                _Converter = (TypeConverter)Activator.CreateInstance(converterType);
            _ConverterType = converterType;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Attempts to convert the value with the TypeConverter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryConvert(object value, out object result)
        {
            // Use the TypeConverter specified.
            if (_Converter != null)
            {
                if (_Converter.CanConvertFrom(value.GetType()))
                {
                    result = _Converter.ConvertFrom(value);
                    return true;
                }

                result = null;
                return false;
            }

            // Use the Type specified.
            if (this.ConverterType != null)
            {
                result = default(object);
                if (Fosol.Common.Helpers.ReflectionHelper.TryConvert(value, this.ConverterType, ref result))
                    return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Attempts to convert the value with the TypeConverter if it is set.
        /// If there is no TypeConverter it returns the original value.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot convert value to the specified type.</exception>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public object Convert(object value)
        {
            // Use the TypeConverter specified.
            if (_Converter != null)
            {
                if (_Converter.CanConvertFrom(value.GetType()))
                {
                    return _Converter.ConvertFrom(value);
                }

                throw new InvalidOperationException();
            }

            // Use the Type specified.
            if (this.ConverterType != null)
            {
                var result = default(object);
                if (Fosol.Common.Helpers.ReflectionHelper.TryConvert(value, this.ConverterType, ref result))
                    return result;

                throw new InvalidOperationException();
            }

            return value;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
