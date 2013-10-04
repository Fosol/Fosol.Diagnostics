using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fosol.Diagnostics.UnitTests
{
    [TestClass]
    public class TraceManagerTest
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        [TestMethod]
        public void DefaultTraceManagerTest()
        {
            var m1 = Fosol.Diagnostics.TraceManager.GetDefault();
            var m2 = Fosol.Diagnostics.TraceManager.GetDefault();
            var m3 = new Fosol.Diagnostics.TraceManager();

            Assert.AreSame(m1, m2);
            Assert.AreNotSame(m1, m3);
            Assert.IsTrue(m1.ThrowOnError);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
