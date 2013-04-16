using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public class TraceFilter
    {
        #region Variables
        #endregion

        #region Properties

        public string InitializeData { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public bool ShouldTrace(TraceEvent trace)
        {
            return true;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
