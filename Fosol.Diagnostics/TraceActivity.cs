using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public struct TraceActivity
    {
        #region Variables
        private readonly Guid _Id;
        private readonly object _Information;
        #endregion

        #region Properties
        public Guid Id
        {
            get { return _Id; }
        }

        public object Information
        {
            get { return _Information; }
        }
        #endregion

        #region Constructors
        public TraceActivity(Guid id, object activityInfo = null)
        {
            _Id = id;
            _Information = activityInfo;
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
