using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Algorithms.Utility;

namespace Tcgv.ConsensusKit.Algorithms.Nakamoto.Tests
{
    [TestClass()]
    public class NkProtocolTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            var processes = new List<NkProcess>();
            for (int i = 0; i < 32; i++)
                processes.Add(new NkProcess(new UniqueArchiver(), new RandomStringProposer(), k: 5));

            var protocol = new NkProtocol(processes);

            var instances = protocol.Execute(10, -1);

            Assert.AreEqual(10, instances.Length);
            Assert.IsTrue(instances.Any(r => r.Value != null));
        }
    }
}