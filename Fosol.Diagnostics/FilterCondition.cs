using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Operator conditions on each filter provide a way to modify how multiple filters work together.
    /// </summary>
    public enum FilterCondition
    {
        /// <summary>
        /// Use the default condition set in the shared filters (or use And).
        /// </summary>
        None = 0,
        /// <summary>
        /// All And filters need to be valid.
        /// </summary>
        And = 1,
        /// <summary>
        /// This filter only needs to be valid.
        /// </summary>
        Or = 2,
        /// <summary>
        /// Only this filter can be valid.
        /// </summary>
        Xor = 3
    }
}
