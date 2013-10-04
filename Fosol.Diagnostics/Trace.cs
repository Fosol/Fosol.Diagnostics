using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Trace is a static class that provides the simplist access to the principle diagnostics objects.
    /// </summary>
    public static class Trace
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Fetch the default TraceManager.
        /// </summary>
        /// <returns>TraceManager object.</returns>
        public static TraceManager GetManager()
        {
            return TraceManager.GetDefault();
        }

        /// <summary>
        /// Fetch the TraceManager for the specified custom section name or external section configuration file.
        /// Use this method if your section name is not the default.
        /// Use this method if your section configuration file is not referenced within your app.config file.
        /// </summary>
        /// <param name="sectionNameOrFilename">Custom section name, or full path to the section configuration file.</param>
        /// <returns>TraceManager object.</returns>
        public static TraceManager GetManager(string sectionNameOrFilename)
        {
            return new TraceManager(sectionNameOrFilename);
        }

        /// <summary>
        /// Fetch the TraceManager for the specified external configuration file and the custom section name.
        /// Use this method if your application is not using the default app.config file.
        /// </summary>
        /// <param name="externalConfigFilename">Full path to the external configuration file.  This configuration file must be a System.Configuration.Configuration object.</param>
        /// <param name="sectionName">Custom section name.</param>
        /// <returns>TraceManager object.</returns>
        public static TraceManager GetManager(string externalConfigFilename, string sectionName)
        {
            return new TraceManager(externalConfigFilename, sectionName);
        }

        /// <summary>
        /// Fetch a TraceWriter for the default TraceManager.
        /// Only use this method if you are using the default TraceManager.
        /// </summary>
        /// <param name="source">The source type of the object creating the writer.</param>
        /// <param name="data">TraceData object containing information to include with the TraceWriter.</param>
        /// <returns>TraceWriter object.</returns>
        public static TraceWriter GetWriter(Type source, TraceData data = null)
        {
            return TraceManager.GetDefault().GetWriter(source, data);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
