using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// The MachineNameKeyword output is the machine name.
    /// </summary>
    [TraceKeyword("machineName")]
    public sealed class MachineNameKeyword
        : StaticKeyword
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new MachineNameKeyword object.
        /// Initialize with MachineName.
        /// </summary>
        public MachineNameKeyword()
            : base(Environment.MachineName)
        {
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
