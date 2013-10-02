using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Filters
{
    /// <summary>
    /// Provides a way to filter TraceEvents so that only the specified TraceLevel are sent to the TraceListeners.
    /// </summary>
    public class LevelFilter
        : TraceFilter
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Defines which TraceLevels are sent to the TraceListeners.
        /// </summary>
        [TraceSetting("level")]
        [Required]
        public TraceLevel Level { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Check if the TraceEvent.Level is allowed to be sent to the TraceListeners.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        /// <returns>'True' if the TraceEvent should be sent to the TraceListeners.</returns>
        protected override bool OnValidate(TraceEvent trace)
        {
            return (int)trace.Level >= (int)this.Level;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
