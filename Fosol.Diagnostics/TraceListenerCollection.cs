using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    [Serializable]
    public sealed class TraceListenerCollection
        : Dictionary<string, TraceListener>, IDisposable
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        internal TraceListenerCollection()
        {
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
