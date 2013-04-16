using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    internal sealed class FilterElement
        : Fosol.Common.Configuration.TypeElement
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public FilterElement()
            : base(typeof(TraceFilter))
        {

        }
        #endregion

        #region Methods
        public TraceFilter GetRuntimeObject()
        {
            var trace_filter = (TraceFilter)base.BaseConstructObject();
            trace_filter.InitializeData = base.InitData;
            return trace_filter;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
