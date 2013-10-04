using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fosol.Diagnostics.UnitTests
{
    /// <summary>
    /// A number of test cases which use an external app configuration file (System.Configuration.Configuration object).
    /// </summary>
    [TestClass]
    public class ExternalAppConfigTest
    {
        #region Variables
        private TestContext _TextContext;
        #endregion

        #region Properties
        public TestContext TextContext
        {
            get { return _TextContext; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Test the ConsoleListener configured with App01.config file.
        /// </summary>
        [TestMethod]
        public void Console()
        {
            // Send console information to this local stream so that it can be tested.
            var stream = new System.IO.MemoryStream();
            var writer = new System.IO.StreamWriter(stream);
            System.Console.SetOut(writer);

            var log = Fosol.Diagnostics.Trace.GetManager("App01.config", "fosol.diagnostics").GetWriter(typeof(ExternalAppConfigTest));
            log.Write("test message");

            writer.Flush();
            stream.Position = 0;
            var reader = new System.IO.StreamReader(stream);
            var message = reader.ReadToEnd();

            // Example message
            // Information: [-1] Fosol.Diagnostics.UnitTests.ConfigTest: 2013-10-04 9:43:01 AM: test message
            Assert.IsNotNull(message);
            Assert.IsTrue(message.StartsWith("Information"));
            Assert.IsTrue(message.EndsWith("test message\r\n"));
        }

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
