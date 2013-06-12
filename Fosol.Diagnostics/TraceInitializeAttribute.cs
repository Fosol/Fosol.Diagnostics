using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Provides a way to initialize class constructors within the configuration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TraceInitializeAttribute
        : Attribute
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The name of the initialize parameter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// get - The type of the initialize parameter.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// get - A converter to change the type from a string to an object of the specified type.
        /// </summary>
        public TypeConverter Converter { get; private set; }

        /// <summary>
        /// get - Whether the Type property is a TypeConverter.
        /// </summary>
        public bool IsTypeConverter { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceInitializeAttribute.
        /// </summary>
        /// <param name="name">Name of the constructor parameter.</param>
        /// <param name="type">Type of the constructor parameter.</param>
        /// <param name="args">Arguments to include with the parameter.</param>
        public TraceInitializeAttribute(string name, Type type, params object[] args)
        {
            this.Name = name;

            if (type == typeof(TypeConverter))
            {
                this.IsTypeConverter = true;
                // Use the default parameterless constructor to initialize the TypeConverter.
                if (args == null || args.Length == 0)
                {
                    var ctor = type.GetConstructor(new Type[0]);

                    if (ctor != null)
                        this.Converter = ctor.Invoke(args) as TypeConverter;
                    else
                        throw new ConfigurationErrorsException();
                }
                else
                {
                    var ctor = type.GetConstructor(args.Select(a => a.GetType()).ToArray());

                    if (ctor != null)
                        this.Converter = ctor.Invoke(args) as TypeConverter;
                    else
                        throw new ConfigurationErrorsException();
                }
            }
            
            this.Type = type;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Convert the string value into the appropiate object type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>A new object.</returns>
        public object Convert(string value)
        {
            if (this.IsTypeConverter)
                return this.Converter.ConvertFrom(value);
            else
                return System.Convert.ChangeType(value, this.Type);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
