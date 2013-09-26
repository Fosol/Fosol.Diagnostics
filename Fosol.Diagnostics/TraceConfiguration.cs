using Fosol.Diagnostics.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Provides a principle way to control and manage configuration settings and changes.
    /// Hooks up a FileSystemWatcher to watch the configuration file for changes.
    /// </summary>
    internal class TraceConfiguration
    {
        #region Variables
        private System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        private DiagnosticSection _Section;
        private string _SectionName;
        private FileSystemWatcher _FileWatcher;
        private string _FilePath;
        private bool _ThrowOnError = false;
        #endregion

        #region Properties
        /// <summary>
        /// get - The DiagnosticSection object.
        /// </summary>
        /// <exception cref="Exceptions.TraceConfigurationException">Section cannot be null.</exception>
        public DiagnosticSection Section
        {
            get 
            {
                _Lock.EnterReadLock();
                try
                {
                    return _Section;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            private set
            {
                if (value == null)
                    OnError(this, new Events.ConfigurationExceptionEventArgs(new Exceptions.TraceConfigurationException()));
                else
                {
                    _Lock.EnterWriteLock();
                    try
                    {
                        _Section = value;
                    }
                    finally
                    {
                        _Lock.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// get - The section name.
        /// This property is only populated when the configuration file follows the System.Configuration format.
        /// </summary>
        private string SectionName
        {
            get { return _SectionName; }
            set { _SectionName = value; }
        }

        /// <summary>
        /// get - Path to the configuration file.
        /// </summary>
        public string FilePath
        {
            get { return _FilePath; }
            private set { _FilePath = value; }
        }

        /// <summary>
        /// get/set - Whether to throw exceptions when they occur.
        /// If this is set to 'false' it will simply fire the Error event.
        /// </summary>
        public bool ThrowOnError
        {
            get { return _ThrowOnError; }
            set { _ThrowOnError = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceConfiguration object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Argument "sectionNameOfFilePath" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Argument "sectionNameOfFilePath" cannot be null.</exception>
        /// <param name="sectionNameOrFilePath">Configuration section name or the path to the file.</param>
        public TraceConfiguration(string sectionNameOrFilePath)
        {
            Fosol.Common.Validation.Assert.IsNotNullOrEmpty(sectionNameOrFilePath, "sectionNameOrFilePath");

            Initialize(sectionNameOrFilePath);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Read the configuration file and initialize the Section property.
        /// Hook up the FileSystemWatcher to watch the configuration file.
        /// It will first check to see if the sectionNameOrFilePath points to an existing file.  If it does it will read it.
        /// </summary>
        /// <param name="sectionNameOrFilePath">Configuration section name or the path to the file.</param>
        private void Initialize(string sectionNameOrFilePath)
        {
            try
            {
                var file_exists = System.IO.File.Exists(sectionNameOrFilePath);
                // Check if the sectionNameOfFilePath points to a specific file.
                if (file_exists)
                {
                    this.SectionName = null;
                    this.FilePath = sectionNameOrFilePath;
                    Load();
                }
                // Treat the sectionNameOrFilePath as a section name because the file did not exist.
                else
                {
                    this.SectionName = sectionNameOrFilePath;
                    Load();

                    // If the configuration file points to another location, grab it.
                    if (this.Section != null 
                        && !string.IsNullOrEmpty(this.Section.SectionInformation.ConfigSource))
                        this.FilePath = this.Section.SectionInformation.ConfigSource;
                    // The file location is the default config location.
                    else
                        this.FilePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    
                    file_exists = System.IO.File.Exists(this.FilePath);
                }

                // Only watch the file if it exists.
                if (file_exists)
                {
                    // Hook up the FileSystemWatcher.
                    _FileWatcher = new FileSystemWatcher(Path.GetDirectoryName(this.FilePath), Path.GetFileName(this.FilePath));
                    _FileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
                    _FileWatcher.Created += OnFileCreated;
                    _FileWatcher.Changed += OnFileChanged;
                    _FileWatcher.Deleted += OnFileDeleted;
                    _FileWatcher.Renamed += OnFileRenamed;
                    _FileWatcher.EnableRaisingEvents = true;
                }
            }
            catch (Exception ex)
            {
                OnError(this, new Events.ConfigurationExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Deserialize the file into a Configuration Section.
        /// </summary>
        /// <param name="path">Path to the configuration file.</param>
        /// <returns>A new instance of a DiagnosticSection object.</returns>
        private Configuration.DiagnosticSection DeserializeSectionFromFile(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    var section = new Configuration.DiagnosticSection();
                    section.GetType().GetMethod("DeserializeSection", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(section, new object[] { reader });
                    return section;
                }
            }
        }

        /// <summary>
        /// Refresh the configuration from the file.
        /// </summary>
        public void Refresh()
        {
            if (!string.IsNullOrEmpty(this.SectionName))
                System.Configuration.ConfigurationManager.RefreshSection(this.SectionName);

            Load();
        }

        /// <summary>
        /// Load the configuration into memory.
        /// </summary>
        private void Load()
        {
            _Lock.EnterWriteLock();
            try
            {
                _Section = null;

                if (string.IsNullOrEmpty(this.SectionName))
                {
                    _Section = DeserializeSectionFromFile(this.FilePath);
                }
                else
                {
                    _Section = (Configuration.DiagnosticSection)System.Configuration.ConfigurationManager.GetSection(this.SectionName);
                }
            }
            catch (Exception ex)
            {
                OnError(this, new Events.ConfigurationExceptionEventArgs(ex));
            }
            finally
            {
                _Lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Start watching for configuration file changes.
        /// </summary>
        public void StartWatching()
        {
            if (_FileWatcher != null)
                _FileWatcher.EnableRaisingEvents = true;
            else
                OnError(this, new Events.ConfigurationExceptionEventArgs(new Exceptions.TraceConfigurationException()));
        }

        /// <summary>
        /// Stop watching for configuration file changes.
        /// </summary>
        public void StopWatching()
        {
            if (_FileWatcher != null)
                _FileWatcher.EnableRaisingEvents = false;
            else
                OnError(this, new Events.ConfigurationExceptionEventArgs(new Exceptions.TraceConfigurationException()));
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        /// <summary>
        /// When a change occurs on the configuration file it will raise this event.
        /// </summary>
        public event EventHandler<FileSystemEventArgs> Changed;

        /// <summary>
        /// The configuration file has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnFileChanged(object sender, FileSystemEventArgs args)
        {
            Refresh();
            Fosol.Common.Extensions.Events.EventExtensions.Raise(Changed, sender, args);
        }

        /// <summary>
        /// The configuration file has been created.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnFileCreated(object sender, FileSystemEventArgs args)
        {
            Refresh();
            Fosol.Common.Extensions.Events.EventExtensions.Raise(Changed, sender, args);
        }

        /// <summary>
        /// The configuration file has been deleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnFileDeleted(object sender, FileSystemEventArgs args)
        {
            Refresh();
            Fosol.Common.Extensions.Events.EventExtensions.Raise(Changed, sender, args);
        }

        /// <summary>
        /// The configuration file has been renamed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnFileRenamed(object sender, RenamedEventArgs args)
        {
            Refresh();
            Fosol.Common.Extensions.Events.EventExtensions.Raise(Changed, sender, args);
        }

        /// <summary>
        /// If ThrowOnError=false it will raise this event and pass the exception to the handler(s).
        /// </summary>
        public event EventHandler<Events.ConfigurationExceptionEventArgs> Error;

        /// <summary>
        /// When an exception is thrown it will either throw the exception or it will raise the Error event.
        /// </summary>
        /// <param name="sender">Object sending the error.</param>
        /// <param name="args">ConfigurationExceptionEventArgs object.</param>
        void OnError(object sender, Events.ConfigurationExceptionEventArgs args)
        {
            if (this.ThrowOnError)
                throw args.Exception;
            else
                Fosol.Common.Extensions.Events.EventExtensions.Raise(Error, sender, args);
        }
        #endregion
    }
}
