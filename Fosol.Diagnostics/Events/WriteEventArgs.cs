using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Events
{
    public sealed class WriteEventArgs
        : EventArgs
    {
        #region Variables
        private readonly TraceEvent _Trace;
        private bool _Cancel;
        #endregion

        #region Properties
        public TraceEvent Trace
        {
            get { return _Trace; }
        }

        public bool Cancel
        {
            get { return _Cancel; }
            set { _Cancel = value; }
        }
        #endregion

        #region Constructors
        public WriteEventArgs(TraceEvent trace)
        {
            _Trace = trace;
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
