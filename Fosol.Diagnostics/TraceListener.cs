using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public abstract class TraceListener
        : IDisposable
    {
        #region Variables
        private const string _DefaultFormat = "{source} {type}: {id}: {message}{newline}";
        private TraceFormat _Format;
        private Encoding _Encoding;
        #endregion

        #region Properties
        [DefaultValue(_DefaultFormat)]
        [TraceProperty("format", typeof(Converters.LogFormatConverter))]
        public TraceFormat Format
        {
            get { return _Format; }
            set { _Format = value; }
        }

        [DefaultValue(Encoding.Default)]
        [TraceProperty("encoding", typeof(Common.Converters.EncodingConverter))]
        public Encoding Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        #endregion

        #region Constructors
        public TraceListener()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Apply the arguments to the properties.
        /// </summary>
        /// <param name="args"></param>
        internal void Initialize(IEnumerable<KeyValuePair<string, object>> args)
        {
            var properties = (
                from p in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                where p.GetCustomAttribute(typeof(TracePropertyAttribute), true) != null
                select new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute(typeof(TracePropertyAttribute), true) as TracePropertyAttribute
                });

            foreach (var property in properties)
            {
                var prop = property.Property;
                var trace_attr = property.Attribute;

                var arg = args.FirstOrDefault(a => prop.Name.Equals(a.Key, StringComparison.InvariantCulture)).Value;
                if (arg != null)
                {
                    // Use the property converter if specified.
                    if (trace_attr.Converter != null)
                        prop.SetValue(this, trace_attr.Converter.ConvertFrom(arg));
                    else
                        prop.SetValue(this, arg);
                    continue;
                }

                // Check for a default value.
                var default_attr = prop.GetCustomAttribute(typeof(DefaultValueAttribute), true) as DefaultValueAttribute;
                if (default_attr != null)
                {
                    // Use the property converter if specified.
                    if (trace_attr.Converter != null)
                        prop.SetValue(this, trace_attr.Converter.ConvertFrom(default_attr.Value));
                    else
                        prop.SetValue(this, default_attr.Value);
                    continue;
                }

                if (trace_attr.IsRequired)
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Attribute_Required, trace_attr.Name));
            }

            Initialize();
        }

        public virtual void Initialize()
        {

        }

        public abstract void Write(string message);

        internal void Write(TraceEvent traceEvent)
        {
            Write(this.Format.Render(traceEvent));
        }

        public virtual void Close()
        {
        }

        public virtual void Flush()
        {
        }

        public virtual void Dispose()
        {
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
