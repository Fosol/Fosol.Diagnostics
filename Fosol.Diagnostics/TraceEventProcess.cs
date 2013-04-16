using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public struct TraceEventProcess
    {
        #region Variables
        private readonly int _Id;
        private readonly string _Name;
        private readonly string _MachineName;
        #endregion

        #region Properties
        public int Id
        {
            get { return _Id; }
        }

        public string Name
        {
            get { return _Name; }
        }

        public string MachineName
        {
            get { return _Name; }
        }
        #endregion

        #region Constructors
        public TraceEventProcess(Process process)
        {
            _Id = process.Id;
            _Name = process.ProcessName;
            _MachineName = process.MachineName;
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
