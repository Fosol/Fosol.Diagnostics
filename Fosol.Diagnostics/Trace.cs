using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public static class Trace
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public static TraceManager GetManager()
        {
            return TraceManager.GetDefault();
        }

        public static TraceManager GetManager(string sectionNameOrFilePath)
        {
            return new TraceManager(sectionNameOrFilePath);
        }

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
