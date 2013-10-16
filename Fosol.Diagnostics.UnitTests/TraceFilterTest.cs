using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fosol.Diagnostics.UnitTests
{
    [TestClass]
    public class TraceFilterTest
    {
        [TestMethod]
        public void TagFilterTest()
        {
            var tags = new Fosol.Diagnostics.TraceTag();
            tags["Source"] = "UnitTest";
            var mng = Fosol.Diagnostics.Trace.GetManager("TraceFilterTest01.config", "fosol.diagnostics");
            var trace = mng.GetWriter(typeof(TraceFilterTest), tags);
            var trace2 = mng.GetWriter(typeof(TraceFilterTest));

            var msg = "Test message";
            trace.Write(msg);
            trace2.Write(msg);

            var listener = mng.GetListener("test") as Fosol.Diagnostics.Listeners.UnitTestListener;

            Assert.IsTrue(listener != null, "TraceListener should exist.");
            Assert.IsTrue(listener.Counter == 1, "TraceListener should only have one message at this point.");
            Assert.IsTrue(listener.LastMessage.Contains(msg), string.Format("TraceListener did not contain message '{0}'.", msg));
            Assert.IsTrue(trace.Tags.ContainsKey("Source"), "TraceListener 'Source' tag was not found.");
            Assert.IsFalse(trace.Tags.ContainsKey("source"), "TraceListener should be case-sensitive; lowercase 'source' tag was found.");
        }
    }
}
