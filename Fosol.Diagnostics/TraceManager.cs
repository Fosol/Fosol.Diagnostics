using Fosol.Common.Caching;
using Fosol.Common.Extensions.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Principle object to control and configure tracing.
    /// </summary>
    public sealed class TraceManager
    {
        #region Variables
        private readonly System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        private static TraceManager _DefaultManager;
        private Fosol.Common.Configuration.ConfigurationSectionFileWatcher<Configuration.DiagnosticSection> _ConfigWatcher;
        private readonly TraceListenerCollection _Listeners = new TraceListenerCollection();
        private readonly SimpleCache<TraceWriter> _Writers = new SimpleCache<TraceWriter>();
        private bool _ThrowOnError = false;
        private bool _AutoFlush = false;
        private bool _FlushOnExit = true;
        #endregion

        #region Properties
        /// <summary>
        /// get - The Fosol.Common.Configuration.ConfigurationSectionFileWatcher<Configuration.DiagnosticSection> object.
        /// </summary>
        private Fosol.Common.Configuration.ConfigurationSectionFileWatcher<Configuration.DiagnosticSection> ConfigWatcher
        {
            get { return _ConfigWatcher; }
            set { _ConfigWatcher = value; }
        }

        /// <summary>
        /// get - Collection of TraceListener objects.
        /// </summary>
        private TraceListenerCollection Listeners
        {
            get { return _Listeners; }
        }

        /// <summary>
        /// get - Collection of TraceWriter objects.
        /// </summary>
        private SimpleCache<TraceWriter> Writers
        {
            get { return _Writers; }
        }

        /// <summary>
        /// get/set - Whether to throw exceptions when they occur.
        /// If this is set to 'false' it will simply fire the Error event.
        /// </summary>
        public bool ThrowOnError
        {
            get { return _ThrowOnError; }
            set 
            {
                if (this.ConfigWatcher != null)
                {
                    this.ConfigWatcher.ThrowOnError = value;
                }
                _ThrowOnError = value; 
            }
        }

        /// <summary>
        /// get/set - Whether flush is forced after every TraceEvent.
        /// </summary>
        public bool AutoFlush
        {
            get { return _AutoFlush; }
            set { _AutoFlush = value; }
        }

        /// <summary>
        /// get/set - Whether flush is called when the application exits.
        /// </summary>
        public bool FlushOnExit
        {
            get { return _FlushOnExit; }
            set { _FlushOnExit = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Add all the FormatElement objects in the Fosol.Diagnostics library to the ElementLibrary collection.
        /// </summary>
        static TraceManager()
        {
            Fosol.Common.Parsers.ElementLibrary.Add(Assembly.GetExecutingAssembly(), typeof(Elements.TraceElement).Namespace);
        }

        /// <summary>
        /// Creates a new instance of a TraceManager object.
        /// If you use this constructor it will not use a configuration file, you must configure through code.
        /// </summary>
        public TraceManager()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        /// <summary>
        /// Creates a new instance of a TraceManager object.
        /// Initializes the TraceManager with the configuration file.
        /// </summary>
        /// <param name="sectionNameOrFilePath">Configuration section name, or the full path to the configuration file.</param>
        public TraceManager(string sectionNameOrFilePath)
            : this()
        {
            Fosol.Common.Validation.Assert.IsNotNullOrEmpty(sectionNameOrFilePath, "sectionNameOrFilePath");

            _ConfigWatcher = new Fosol.Common.Configuration.ConfigurationSectionFileWatcher<Configuration.DiagnosticSection>(sectionNameOrFilePath);
            _ConfigWatcher.ThrowOnError = this.ThrowOnError;
            _ConfigWatcher.Error += OnError;
            _ConfigWatcher.FileChanged += OnConfigChanged;
            _ConfigWatcher.Start();
            Refresh();
        }


        public TraceManager(string externalConfigFilePath, string sectionName)
            : this()
        {
            Fosol.Common.Validation.Assert.IsNotNullOrEmpty(externalConfigFilePath, "externalConfigFilePath");
            Fosol.Common.Validation.Assert.IsNotNullOrEmpty(sectionName, "sectionName");

            _ConfigWatcher = new Fosol.Common.Configuration.ConfigurationSectionFileWatcher<Configuration.DiagnosticSection>(externalConfigFilePath, sectionName);
            _ConfigWatcher.ThrowOnError = this.ThrowOnError;
            _ConfigWatcher.Error += OnError;
            _ConfigWatcher.FileChanged += OnConfigChanged;
            _ConfigWatcher.Start();
            Refresh();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the default TraceManager.
        /// </summary>
        /// <returns>TraceManager object.</returns>
        public static TraceManager GetDefault()
        {
            if (_DefaultManager == null)
            {
                _DefaultManager = new TraceManager(Configuration.DiagnosticSection.Name);
            }

            return _DefaultManager;
        }

        /// <summary>
        /// Refresh the TraceListener collections.
        /// </summary>
        private void Refresh()
        {
            _Lock.EnterWriteLock();
            try
            {
                if (this.ConfigWatcher.Section != null)
                {
                    this.ThrowOnError = this.ConfigWatcher.Section.ThrowOnError;
                    this.AutoFlush = this.ConfigWatcher.Section.Trace.AutoFlush;
                    this.FlushOnExit = this.ConfigWatcher.Section.Trace.FlushOnExit;

                    // A flush may need to occur before the listeners are cleared.
                    this.Listeners.Clear();

                    // Generate the TraceListener collection.
                    foreach (var listener_config in this.ConfigWatcher.Section.Trace.Listeners)
                    {
                        var shared_listener = this.ConfigWatcher.Section.SharedListeners.FirstOrDefault(l => l.Name.Equals(listener_config.Name));

                        // Merge the trace listener with the shared listener.
                        // This provides a way to modify/override a common configured listener.
                        if (shared_listener != null)
                        {
                            listener_config.Merge(shared_listener);
                        }
                        var listener = CreateListener(listener_config);

                        if (listener != null)
                            this.Listeners.Add(listener);
                    }
                }
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Get the TraceListener from the configuration.
        /// </summary>
        /// <exception cref="Exceptions.ListenerConfigurationException">Failed to create a new instance of a TraceListener object.</exception>
        /// <param name="config">ListenerElement configuration object.</param>
        /// <returns>A new instance of a TraceListener object.</returns>
        internal TraceListener CreateListener(Configuration.ListenerElement config)
        {
            try
            {
                var listener = CreateInstanceOf<TraceListener>(config.ListenerTypeName, config.Constructor);
                // Reference TraceFilters.
                foreach (var filter_config in config.Filters)
                {
                    var shared_filter = this.ConfigWatcher.Section.SharedFilters.FirstOrDefault(f => f.Name.Equals(filter_config.Name));

                    // Merge the filter with the shared filter.
                    // This provides a way to modify/override a common configured filter.
                    if (shared_filter != null)
                    {
                        filter_config.Merge(shared_filter);
                    }
                    var filter = CreateFilter(filter_config);

                    if (filter != null)
                        listener.Filters.Add(filter);
                }
                ApplySettings(listener, config.Settings);
                listener.Initialize();

                return listener;
            }
            catch (Exception ex)
            {
                OnError(this, new Common.Configuration.Events.ConfigurationSectionErrorEventArgs(ex));
                return null;
            }
        }

        /// <summary>
        /// Get the TraceFilter from the configuration.
        /// </summary>
        /// <param name="config">FilterElement configuration object.</param>
        /// <returns>A new instance of a FilterElement object.</returns>
        internal TraceFilter CreateFilter(Configuration.FilterElement config)
        {
            try
            {
                var filter = CreateInstanceOf<TraceFilter>(config.FilterTypeName, config.Constructor);
                filter.Condition = config.Condition;
                ApplySettings(filter, config.Settings);
                filter.Initialize();

                return filter;
            }
            catch (Exception ex)
            {
                OnError(this, new Common.Configuration.Events.ConfigurationSectionErrorEventArgs(ex));
                return null;
            }
        }

        /// <summary>
        /// Checks if the settings contain constructor initialization settings.  
        /// If it does it will attempt to use them to initialize a new instance of the object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Argument "type" must be assignable from Type T.</exception>
        /// <exception cref="System.ArgumentNullException">Argument "type" must be a valid type.</exception>
        /// <typeparam name="T">Type of object to create a new instance of.</typeparam>
        /// <param name="typeName">Type name of object to create a new instance of.</param>
        /// <param name="settings">Configuration.SettingElementCollection object containing constructor settings.</param>
        /// <returns>A new instance of an object of Type T.</returns>
        private T CreateInstanceOf<T>(string typeName, Configuration.SettingElementCollection settings)
        {
            var type = Type.GetType(typeName);

            Fosol.Common.Validation.Assert.IsNotNull(type, "typeName", string.Format(Resources.Strings.Exception_Argument_Type_Invalid, typeName));

            return CreateInstanceOf<T>(type, settings);
        }

        /// <summary>
        /// Checks if the settings contain constructor initialization settings.  
        /// If it does it will attempt to use them to initialize a new instance of the object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Argument "type" must be assignable from Type T.</exception>
        /// <exception cref="Exceptions.SettingConfigurationException">TraceSettingAttribute objects require a ConverterType when applied to constructors.</exception>
        /// <typeparam name="T">Type of object to create a new instance of.</typeparam>
        /// <param name="type">Type of object to create a new instance of.</param>
        /// <param name="settings">Configuration.SettingElementCollection object containing constructor settings.</param>
        /// <returns>A new instance of an object of Type T.</returns>
        private T CreateInstanceOf<T>(Type type, Configuration.SettingElementCollection settings)
        {
            Fosol.Common.Validation.Assert.IsAssignableFromType(type, typeof(T), "type", string.Format(Resources.Strings.Exception_Argument_Type_NotAssignable, type.Name, typeof(T).Name));

            // Determine if the object has TraceSettingAttribute objects on any of its constructors.
            var constructors_with_attributes = (
                from c in type.GetConstructors()
                where c.GetCustomAttributes(typeof(TraceSettingAttribute), true).Length > 0
                select new
                {
                    Constructor = c,
                    Attributes = c.GetCustomAttributes(typeof(TraceSettingAttribute), true),
                });

            // Determine if the configuration included constructor settings.
            foreach (var con in constructors_with_attributes)
            {
                var arguments = new List<object>();

                // Check if the configuration has the correct settings for the constructor.
                foreach (TraceSettingAttribute attr in con.Attributes)
                {
                    var setting = settings.FirstOrDefault(s => s.Name.Equals(attr.Name, StringComparison.CurrentCultureIgnoreCase));

                    if (setting == null)
                    {
                        arguments.Clear();
                        break;
                    }

                    if (attr.ConverterType == null)
                        throw new Exceptions.SettingConfigurationException(string.Format(Resources.Strings.Exception_Setting_Attribute_Missing_ConverterType, attr.Name));

                    var value = default(object);

                    // Convert the setting value with its TypeConverter.
                    if (attr.TryConvert(setting.Value, out value))
                    {
                        arguments.Add(value);
                        continue;
                    }

                    break;
                }

                // There are settings for this constructor so use it.
                if (arguments.Count() > 0)
                    return (T)con.Constructor.Invoke(arguments.ToArray());
            }

            return (T)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Applies the configuration settings to the objects properties.
        /// </summary>
        /// <exception cref="Exceptions.SettingConfigurationException">Invalid setting configuration for this object.</exception>
        /// <param name="obj">Object to update with setting values.</param>
        /// <param name="settings">Configuration.SettingElementCollection object containing property settings.</param>
        private void ApplySettings(object obj, Configuration.SettingElementCollection settings)
        {
            var type = obj.GetType();

            // Fetch all property settings.
            var properties = (
                from p in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                where p.GetCustomAttributes(typeof(TraceSettingAttribute), true).Length > 0
                select new
                {
                    Property = p,
                    Attribute = (TraceSettingAttribute)p.GetCustomAttributes(typeof(TraceSettingAttribute), true).FirstOrDefault(),
                    Required = (RequiredAttribute)p.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault(),
                    Default = (DefaultValueAttribute)p.GetCustomAttributes(typeof(DefaultValueAttribute), true).FirstOrDefault()
                });

            // Apply configuration setting values to the properties.
            foreach (var prop in properties)
            {
                var setting = settings.FirstOrDefault(s => s.Name.Equals(prop.Attribute.Name, StringComparison.CurrentCultureIgnoreCase));

                if (setting == null)
                {
                    // Use the default value.
                    if (prop.Default != null)
                    {
                        object val = null;
                        if (prop.Default.Value.GetType() == prop.Property.PropertyType)
                            prop.Property.SetValue(obj, prop.Default.Value);
                        else if (prop.Attribute.TryConvert(prop.Default.Value, out val))
                            prop.Property.SetValue(obj, val);
                        else if (Fosol.Common.Helpers.ReflectionHelper.TryConvert(prop.Default.Value, prop.Property.PropertyType, ref val))
                            prop.Property.SetValue(obj, val);
                        else
                            prop.Property.SetValue(obj, prop.Default.Value);
                    }

                    // The setting is required but it hasn't been configured.
                    if (prop.Required != null)
                        throw new Exceptions.SettingConfigurationException(string.Format(Resources.Strings.Exception_Configuration_Setting_Required, setting.Name));

                    continue;
                }

                object value = null;

                // Apply the setting value to the property.
                if (prop.Attribute.TryConvert(setting.Value, out value))
                    prop.Property.SetValue(obj, value);
                else if (Fosol.Common.Helpers.ReflectionHelper.TryConvert(setting.Value, prop.Property.PropertyType, ref value))
                    prop.Property.SetValue(obj, value);
                else
                    throw new Exceptions.SettingConfigurationException(string.Format(Resources.Strings.Exception_Configuration_Setting_Value_Invalid, setting.Name));
            }
        }

        /// <summary>
        /// Gets a TraceWriter for the specified type.
        /// If the writer has not been created yet it will generate a new instance of a TraceWriter.
        /// </summary>
        /// <param name="source">Type of object creating the TraceWriter.</param>
        /// <param name="data">TraceData you would like to initialize the TraceWriter with.</param>
        /// <returns>The TraceWriter associated with the type.</returns>
        public TraceWriter GetWriter(Type source, TraceData data = null)
        {
            Fosol.Common.Validation.Assert.IsNotNull(source, "source");
            _Lock.EnterUpgradeableReadLock();
            try
            {
                if (_Writers.ContainsKey(TraceWriter.GenerateCacheKey(source, data)))
                    return _Writers[source.FullName].Value;

                // Create a new TraceWriter and add it to cache.
                _Lock.EnterWriteLock();
                try
                {
                    // Double check to confirm the writer has not already been created.
                    if (_Writers.ContainsKey(TraceWriter.GenerateCacheKey(source, data)))
                        return _Writers[source.FullName].Value;

                    var writer = CreateWriter(source, data);
                    _Writers.Add(writer.GetCacheKey(), writer);
                    return writer;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
            finally
            {
                _Lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Creates a new instance of a TraceWriter.
        /// </summary>
        /// <param name="type">Source Type.</param>
        /// <param name="data">TraceData object.</param>
        /// <returns>A new instance of a TraceWriter.</returns>
        private TraceWriter CreateWriter(Type type, TraceData data = null)
        {
            var writer = new TraceWriter(this, type, data);
            return writer;
        }

        /// <summary>
        /// Write the TraceEvent to every listener that validates the TraceEvent.
        /// Each write is wrapped in it's own try+catch so that a single failure will not cause all other listeners to fail.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        internal void Write(TraceEvent trace)
        {
            _Lock.EnterReadLock();
            try
            {
                foreach (var listener in this.Listeners)
                {
                    try
                    {
                        listener.Write(trace);

                        if (this.AutoFlush)
                            listener.Flush();
                    }
                    catch (Exception ex)
                    {
                        OnError(this, new Common.Configuration.Events.ConfigurationSectionErrorEventArgs(ex));
                    }
                }
            }
            finally
            {
                _Lock.ExitReadLock();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        /// <summary>
        /// The configuration file has been updated externally.  
        /// Refresh the Filters and Listeners.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfigChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// If ThrowOnError=false it will raise this event and pass the exception to the handler(s).
        /// </summary>
        public event EventHandler<Events.ConfigurationExceptionEventArgs> Error;

        /// <summary>
        /// When an exception is thrown it will either throw the exception or it will raise the Error event.
        /// </summary>
        /// <param name="sender">Object sending the error.</param>
        /// <param name="args">ConfigurationSectionErrorEventArgs object.</param>
        void OnError(object sender, Fosol.Common.Configuration.Events.ConfigurationSectionErrorEventArgs args)
        {
            if (this.ThrowOnError)
                throw args.Exception;
            else
                Error.Raise(sender, new Events.ConfigurationExceptionEventArgs(args.Exception));
        }

        /// <summary>
        /// Detects when application exits and if the FlushOnExit='true' it will call the Flush method on each Listener.
        /// </summary>
        /// <param name="sender">Source of this method call.</param>
        /// <param name="e">EventArgs object.</param>
        void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (this.FlushOnExit)
            {
                _Lock.EnterReadLock();
                try
                {
                    foreach (var listener in this.Listeners)
                    {
                        try
                        {
                            listener.Flush();
                        }
                        catch (Exception ex)
                        {
                            OnError(this, new Common.Configuration.Events.ConfigurationSectionErrorEventArgs(ex));
                        }
                    }
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
        }
        #endregion
    }
}
