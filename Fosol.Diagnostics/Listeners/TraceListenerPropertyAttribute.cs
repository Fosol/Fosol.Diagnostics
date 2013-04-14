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

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Declares the property or field as a configuration attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class TraceListenerPropertyAttribute
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
        public TraceListenerPropertyAttribute(string name)
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
        public TraceListenerPropertyAttribute(string name, bool isRequired)
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
        public TraceListenerPropertyAttribute(string name, Type converterType, params object[] converterArgs)
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
        public TraceListenerPropertyAttribute(string name, bool isRequired, Type converterType, params object[] converterArgs)
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
        /// <summary>
        /// Reviews the configuration attributes and applies them to the objects properties and fields.
        /// This requires that the object be marked up with TraceListenerPropertyAttribute objects.
        /// </summary>
        /// <param name="obj">Object to apply settings to.</param>
        internal static void ApplyAttributes<T>(T obj)
            where T : TraceListener
        {
            ApplyAttributes(obj, obj.Attributes);
        }

        /// <summary>
        /// Reviews the configuration attributes and applies them to the objects properties and fields.
        /// This requires that the object be marked up with TraceListenerPropertyAttribute objects.
        /// </summary>
        /// <param name="obj">Object to apply settings to.</param>
        /// <param name="attributes">StringDictionary containing all the attributes.</param>
        internal static void ApplyAttributes<T>(T obj, StringDictionary attributes)
            where T : class
        {
            var type = obj.GetType();
            // Get all the relevant fields that have the LogTargetSettingAttribute.
            var fields = (
                from f in type.GetFields(BindingFlags.Instance)
                where f.GetCustomAttributes(typeof(TraceListenerPropertyAttribute)).FirstOrDefault() != null
                select new
                {
                    Field = f,
                    Attr = f.GetCustomAttributes(typeof(TraceListenerPropertyAttribute)).FirstOrDefault() as TraceListenerPropertyAttribute,
                    DefaultValue = f.GetCustomAttributes(typeof(DefaultValueAttribute)).FirstOrDefault() as DefaultValueAttribute
                });

            foreach (var field in fields)
            {
                // Configuration contains the setting name.
                if (attributes.ContainsKey(field.Attr.Name))
                {
                    if (field.Attr.Converter != null)
                        Common.Helpers.ReflectionHelper.SetValue(field.Field, obj, attributes[field.Attr.Name], field.Attr.Converter);
                    else
                        Common.Helpers.ReflectionHelper.SetValue(field.Field, obj, attributes[field.Attr.Name]);
                }
                // Assign default values.
                else if (field.DefaultValue != null)
                {
                    if (field.DefaultValue.GetType() != field.Field.GetType()
                        && field.Attr.Converter != null)
                        Common.Helpers.ReflectionHelper.SetValue(field.Field, obj, field.DefaultValue.Value, field.Attr.Converter);
                    else
                        Common.Helpers.ReflectionHelper.SetValue(field.Field, obj, field.DefaultValue.Value);
                }
                // Throw exception.
                else if (field.Attr.IsRequired)
                {
                    throw new System.Configuration.ConfigurationErrorsException(string.Format(Resources.Strings.Exception_Configuration_AttributeRequired, field.Attr.Name));
                }
            }

            // Get all the relevant properties that have the LogTargetSettingAttribute.
            var properties = (
                from p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.CanWrite
                    && p.GetCustomAttributes(typeof(TraceListenerPropertyAttribute)).FirstOrDefault() != null
                select new
                {
                    Property = p,
                    Attr = p.GetCustomAttributes(typeof(TraceListenerPropertyAttribute)).FirstOrDefault() as TraceListenerPropertyAttribute,
                    DefaultValue = p.GetCustomAttributes(typeof(DefaultValueAttribute)).FirstOrDefault() as DefaultValueAttribute
                });

            foreach (var prop in properties)
            {
                // Configuration contains the setting name.
                if (attributes.ContainsKey(prop.Attr.Name))
                {
                    if (prop.Attr.Converter != null)
                        Common.Helpers.ReflectionHelper.SetValue(prop.Property, obj, attributes[prop.Attr.Name], prop.Attr.Converter);
                    else
                        Common.Helpers.ReflectionHelper.SetValue(prop.Property, obj, attributes[prop.Attr.Name]);
                }
                // Assign default values.
                else if (prop.DefaultValue != null)
                {
                    if (prop.DefaultValue.GetType() != prop.Property.GetType()
                        && prop.Attr.Converter != null)
                        Common.Helpers.ReflectionHelper.SetValue(prop.Property, obj, prop.DefaultValue.Value, prop.Attr.Converter);
                    else
                        Common.Helpers.ReflectionHelper.SetValue(prop.Property, obj, prop.DefaultValue.Value);
                }
                // Throw exception.
                else if (prop.Attr.IsRequired)
                {
                    throw new System.Configuration.ConfigurationErrorsException(string.Format(Resources.Strings.Exception_Configuration_AttributeRequired, prop.Attr.Name));
                }
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
