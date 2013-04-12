/*----------------------------------------------------------------------- 
 * MSDN Magazine - Bugslayer Column - John Robbins 
 * A .NET Framework 2.0 class to refresh the TraceSwitch values whenever 
 * app's configuration file changes. 
 * -----------------------------------------------------------------------*/ 
using System; 
using System.IO; 
using System.Text; 
using System.Threading; 
using System.Reflection; 
using System.Diagnostics; 
using System.Collections.Generic; 

namespace Fosol.Diagnostics { 
    /// 
    /// A class that watches the application config file and refreshes all 
    /// values. 
    /// 
    class ConfigFileWatcher
    {
        #region Variables
        // The file watcher itself. 
        private FileSystemWatcher watcher;
        // The name of the configuration file for reference. 
        private String appConfigFile; 
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// Creates the watcher for the config file. 
        public ConfigFileWatcher()
        {
            appConfigFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.ToUpper().Replace(".VSHOST", "");
            Initialize();
        }
        /// Creates the watcher for the file. 
        /// The full path to watch. 
        public ConfigFileWatcher(String fileName)
        {
            appConfigFile = fileName;
            Initialize();
        } 
        #endregion

        #region Methods
        /// Shuts off all configuration monitoring. 
        public void Close()
        {
            // Clear the notifications. 
            watcher.EnableRaisingEvents = false;
            watcher = null;
            appConfigFile = null;
        }

        // The common initialization. 
        private void Initialize()
        {
            Debug.Assert(appConfigFile != null, "appConfigFile != null");
            Debug.Assert(appConfigFile.Length > 0, "appConfigFile.Length > 0");
            if ((null == appConfigFile) || (appConfigFile.Length == 0))
            {
                throw new ArgumentException("appConfigFile");
            }
            // Set the directory to watch and only watch for writes 
            String path = Path.GetDirectoryName(appConfigFile);
            watcher = new FileSystemWatcher(path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            // Set up the event handlers. Make sure to watch 
            // changes made to file, including creation and deletion. 
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.Deleted += new FileSystemEventHandler(watcher_Changed);
            watcher.Created += new FileSystemEventHandler(watcher_Changed);
            // Let fly. 
            watcher.EnableRaisingEvents = true;
        } 
        #endregion

        #region Operators
        #endregion

        #region Events
        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            // Check to see if this is the real application config. 
            if (0 == String.Compare(appConfigFile, e.FullPath, true, Thread.CurrentThread.CurrentCulture))
            {
                // Can this get any simpler? 
                Trace.Refresh();
            }
        } 
        #endregion
    } 
}