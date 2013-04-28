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
        internal static TraceListener CreateListener(Type type, Configuration.ArgumentElementCollection args)
        {
            // Use the default constructor to create a new TraceListener.
            if (args == null)
            {
                var ctor = type.GetConstructor(new Type[0]);

                if (ctor != null)
                    return ctor.Invoke(new object[0]) as TraceListener;
            }
            else
            {
                object[] init = new object[args.Count];
                int i = 0;
                // Get the initialize attributes.
                foreach (TraceInitializeAttribute attr in type.GetCustomAttributes(typeof(TraceInitializeAttribute), true))
                {
                    var arg = args.FirstOrDefault(a => a.Name.Equals(attr.Name, StringComparison.InvariantCulture));

                    // Found an initialize attribute that matches the configured argument.
                    if (arg != null)
                    {
                        // Apply the conversion to the initialize attribute.
                        if (attr.IsTypeConverter)
                            init[i] = attr.Converter.ConvertFrom(arg.Value);
                        else
                            init[i] = Convert.ChangeType(arg.Value, attr.Type);
                    }
                    else
                        init[i] = arg.Value;

                    i++;
                }

                // Get the constructor that matches the supplied initialize args (order is important).
                var ctor = type.GetConstructor(init.Select(a => a.GetType()).ToArray());

                if (ctor != null)
                    return ctor.Invoke(init) as TraceListener;
            }

            throw new ConfigurationErrorsException();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
