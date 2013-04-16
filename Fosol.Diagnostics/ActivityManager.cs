using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public sealed class ActivityManager
        : IDisposable
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ActivityManager()
            : this(Guid.NewGuid())
        {

        }

        public ActivityManager(object activityId)
        {
            Common.Validation.Assert.IsNotNull(activityId, "activityId");
            TraceManager.ActivityStack.Start(activityId);
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            TraceManager.ActivityStack.Stop();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
