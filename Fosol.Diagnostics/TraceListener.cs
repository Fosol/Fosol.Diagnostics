using Fosol.Diagnostics.Listeners;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Abstract class for all extended Trace Listener objects.
    /// </summary>
    public abstract class TraceListener
        : IDisposable
    {
        #region Variables
        protected readonly ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();
        private const string _DefaultFormat = "{source} {type}: {id}: {message}{newline}";
        private readonly string[] _SupportedAttributes;
        private bool _IsInitialized;
        private bool _IsInitializing;
        private TraceFormat _Format;
        private volatile TraceFilter _Filter;
        private StringDictionary _Attributes;
        #endregion

        #region Properties
        /// <summary>
        /// get - Whether the Initialized method has been executed.
        /// </summary>
        public bool IsInitialized
        {
            get { return GetValue(ref _IsInitialized); }
            private set { SetValue(ref _IsInitialized, value); }
        }

        /// <summary>
        /// get - Whether it is currenting initializing.
        /// </summary>
        private bool IsInitializing
        {
            get { return GetValue(ref _IsInitializing); }
            set { SetValue(ref _IsInitializing, value); }
        }

        public string InitializeData { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// get - Message string format.
        /// </summary>
        [DefaultValue(_DefaultFormat)]
        [TraceListenerProperty("format", typeof(Converters.LogFormatConverter))]
        public TraceFormat Format
        {
            get
            {
                return GetValue(ref _Format);
            }
            set
            {
                SetValue(ref _Format, value, "format");
            }
        }

        public TraceFilter Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        public StringDictionary Attributes
        {
            get { return _Attributes; }
        }

        public virtual bool IsThreadSafe
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize supported attributes.
        /// </summary>
        public TraceListener()
        {
            _SupportedAttributes = TraceListener.GetSupportedAttributes(this.GetType());
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets an array of supported attributes by extracting the properties and fields that have been marked with the TraceListenerPropertyAttribute.
        /// </summary>
        /// <param name="type">Type of listener.</param>
        /// <returns>An array of supported attributes.</returns>
        public static string[] GetSupportedAttributes(Type type)
        {
            var properties = (
                from p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.GetCustomAttributes(typeof(TraceListenerPropertyAttribute), true).FirstOrDefault() != null
                select new
                {
                    Property = p,
                    Parameter = p.GetCustomAttributes(typeof(TraceListenerPropertyAttribute), false).FirstOrDefault() as TraceListenerPropertyAttribute,
                    DefaultValue = p.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute
                });

            return properties.Select(p => p.Parameter.Name).ToArray();
        }

        /// <summary>
        /// Initialize the property values that are linked to configuration attributes.
        /// This method is called by the GetValue method.
        /// Regrettably it must be called repeatedly due to not having access to the base TraceListener SetAttributes() method.
        /// </summary>
        internal void InternalIntialize()
        {
            _LockSlim.EnterUpgradeableReadLock();
            try
            {
                if (!_IsInitializing && !_IsInitialized)
                {
                    this.IsInitializing = true;
                    TraceListenerPropertyAttribute.ApplyAttributes(this);
                    Intialize();
                    this.IsInitialized = true;
                    this.IsInitializing = false;
                }
            }
            finally
            {
                _LockSlim.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Subclasses should override this method to apply settings.
        /// This method will get called the first time GetValue() is called (which is normally when a trace event is being handled).
        /// </summary>
        protected virtual void Intialize()
        {

        }

        /// <summary>
        /// Check if the configuration attriute has been set or changed.
        /// If the local property already contains the same value it will return false.
        /// Ensures thread safety.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="prop">Property variable.</param>
        /// <param name="attributeName">Name of the attribute for this property.</param>
        /// <returns>True if the configuration attribute has been set or changed.</returns>
        protected bool UseAttributeValue<T>(ref T prop, string attributeName)
        {
            if (Attributes.ContainsKey(attributeName)
                && (prop == null || !Attributes[attributeName].Equals(GetValue(ref prop).ToString(), StringComparison.InvariantCulture)))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the property value.
        /// Ensures thread safety.
        /// Also calls the Initialize() method.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="prop">References to property variable.</param>
        /// <returns>Value of property.</returns>
        protected T GetValue<T>(ref T prop)
        {
            _LockSlim.EnterReadLock();
            try
            {
                return prop;
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Sets the property value and the configured attribute value.
        /// Ensures thread safety.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="prop">Reference to the property variable.</param>
        /// <param name="value">Value to set the property and attribute with.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Returns the originally passed value.</returns>
        protected T SetValue<T>(ref T prop, T value, string attributeName = null)
        {
            _LockSlim.EnterWriteLock();
            try
            {
                prop = value;

                if (!string.IsNullOrEmpty(attributeName))
                    Attributes[attributeName] = value.ToString();

                return value;
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns an array of supported attributes.
        /// The supported attributes are dynamically generated based on the usage of the TraceListenerPropertyAttribute.
        /// </summary>
        /// <returns>An array of supported attributes.</returns>
        protected string[] GetSupportedAttributes()
        {
            return _SupportedAttributes;
        }

        public abstract void Write(string trace);

        public virtual void Write(TraceEvent trace)
        {
            if (this.Filter != null && !this.Filter.ShouldTrace(trace))
                return;

            Write(this.Format.Render(trace));
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

        internal void SetAttributes(System.Collections.Hashtable attribs)
        {
            TraceListener.VerifyAttributes(this.Attributes, GetSupportedAttributes());
            _Attributes = new StringDictionary();
            this.Attributes.ReplaceHashtable(attribs);
        }

        static void VerifyAttributes(IDictionary attributes, string[] supportedAttributes)
        {
            foreach (var text in attributes.Keys)
            {
                if (supportedAttributes == null
                    || supportedAttributes.Contains(text))
                    throw new ConfigurationErrorsException();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
