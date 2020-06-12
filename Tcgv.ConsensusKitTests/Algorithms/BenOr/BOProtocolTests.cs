using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Tcgv.ConsensusKit.Algorithms.BenOr.Tests
{
    [TestClass()]
    public class BOProtocolTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            var processes = new List<BOProcess>();
            for (int i = 0; i < 32; i++)
                processes.Add(new BOProcess(f: 4));

            var protocol = new BOProtocol(processes, f: 4);

            var instances = protocol.Execute(100, -1);

            Assert.AreEqual(100, instances.Length);
            Assert.IsTrue(instances.Any(r => r.Value == null));
            Assert.IsTrue(instances.Any(r => r.Value != null));
        }
    }
}