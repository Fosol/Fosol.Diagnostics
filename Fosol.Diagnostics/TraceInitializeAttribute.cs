using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TraceInitializeAttribute
        : Attribute
    {
        #region Variables
        #endregion

        #region Properties
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public TypeConverter Converter { get; private set; }
        public bool IsTypeConverter { get; private set; }
        #endregion

        #region Constructors
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
