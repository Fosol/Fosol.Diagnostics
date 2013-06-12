using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// A TraceEventProcess contains information about the process that sent the TraceEvent.
    /// </summary>
    public sealed class TraceEventProcess
    {
        #region Variables
        private int _Id;
        private string _Name;
        private string _MachineName;
        #endregion

        #region Properties
        /// <summary>
        /// get - The process Id.
        /// </summary>
        public int Id
        {
            get { return _Id; }
            private set { _Id = value; }
        }

        /// <summary>
        /// get - The process name.
        /// </summary>
        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

        /// <summary>
        /// get - The process machine name.
        /// </summary>
        public string MachineName
        {
            get { return _MachineName; }
            private set { _MachineName = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceEventProcess.
        /// </summary>
        public TraceEventProcess()
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            this.Id = process.Id;
            this.Name = process.ProcessName;
            this.MachineName = process.MachineName;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
