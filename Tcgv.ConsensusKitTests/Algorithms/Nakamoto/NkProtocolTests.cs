using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Algorithms.Utility;
using Tcgv.ConsensusKit.Formatting;

namespace Tcgv.ConsensusKit.Algorithms.Nakamoto.Tests
{
    [TestClass()]
    public class NkProtocolTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            var protocol = CreateProtocol(32, 5);

            var instances = protocol.Execute(10, -1);

            Assert.AreEqual(10, instances.Length);
            Assert.IsTrue(instances.All(r => r.Consensus));
        }

        [TestMethod()]
        public void PrintHistoryTest()
        {
            var protocol = CreateProtocol(10, 5);

            var instances = protocol.Execute(5, -1, 100);

            var str = new Analyzer().PrintHistory(instances);
        }

        private NkProtocol CreateProtocol(int processesCount, int kDepth)
        {
            var processes = new List<NkProcess>();

            for (int i = 0; i < processesCount; i++)
            {
                var p = new NkProcess(new UniqueArchiver(), new RandomStringProposer(), k: kDepth);
                processes.Add(p);
            }

            return new NkProtocol(processes);
        }
    }
}