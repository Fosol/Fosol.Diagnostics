using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    [ConfigurationCollection(typeof(ArgumentElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    internal class ArgumentElementCollection
        : Fosol.Common.Configuration.ConfigurationElementCollection<ArgumentElement>
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public IEnumerable<KeyValuePair<string, object>> GetArguments(string callerName)
        {
            foreach (var arg in this)
            {
                // If a converter name was provided then attempt to use it.
                if (!string.IsNullOrEmpty(arg.ConverterName))
                {
                    var converter_type = Type.GetType(arg.ConverterName, false, false);
                    if (converter_type == null || !converter_type.IsAssignableFrom(typeof(TypeConverter)))
                        throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Argument_Converter_Invalid, callerName, arg.Name, arg.ConverterName));
                    
                    var type = Type.GetType(arg.TypeName, false, false);
                    if (type == null)
                        throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Argument_Type_Invalid, callerName, arg.Name, arg.TypeName));

                    // Check if the converter needs to be initialized with the result type.
                    var ctor = converter_type.GetConstructor(new Type[] { type });
                    if (ctor != null)
                    {
                        var converter = ctor.Invoke(new object[] { type }) as TypeConverter;

                        if (converter != null && converter.CanConvertFrom(typeof(string)) && converter.CanConvertTo(type))
                        {
                            yield return new KeyValuePair<string, object>(arg.Name, converter.ConvertTo(arg.Value, type));
                            continue;
                        }
                        
                        throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Argument_Type_Invalid, callerName, arg.Name, arg.TypeName));
                    }

                    // Use the base constructor for the converter.
                    ctor = converter_type.GetConstructor(new Type[0]);
                    if (ctor != null)
                    {
                        var converter = ctor.Invoke(new object[0]) as TypeConverter;

                        if (converter != null && converter.CanConvertFrom(typeof(string)) && converter.CanConvertTo(type))
                        {
                            yield return new KeyValuePair<string, object>(arg.Name, converter.ConvertTo(arg.Value, type));
                            continue;
                        }
                        
                        throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Argument_Type_Invalid, callerName, arg.Name, arg.TypeName));
                    }
                    
                    // The convert failed.
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Argument_Converter_Invalid, callerName, arg.Name, arg.ConverterName));
                }

                // If the argument provides the type attempt to convert the string value to the type specified.
                if (!string.IsNullOrEmpty(arg.TypeName))
                {
                    var type = Type.GetType(arg.TypeName, false, false);
                    if (type == null)
                        throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Argument_Type_Invalid, callerName, arg.Name, arg.TypeName));

                    KeyValuePair<string, object> value;
                    try
                    {
                        value = new KeyValuePair<string, object>(arg.Name, Convert.ChangeType(arg.Value, type));
                    }
                    catch
                    {
                        throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Argument_Conversion_Failed, callerName, arg.Name, arg.TypeName));
                    }
                    yield return value;
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
