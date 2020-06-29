using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Algorithms.Utility;
using Tcgv.ConsensusKit.Formatting;

namespace Tcgv.ConsensusKit.Algorithms.BenOr.Tests
{
    [TestClass()]
    public class BOProtocolTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            var protocol = CreateProtocol(32, 4);

            var instances = protocol.Execute(100, -1);

            Assert.AreEqual(100, instances.Length);
            Assert.IsTrue(instances.Any(r => r.Consensus));
            Assert.IsTrue(instances.Any(r => !r.Consensus));
        }

        [TestMethod()]
        public void PrintHistoryTest()
        {
            var protocol = CreateProtocol(10, 1);

            var instances = protocol.Execute(5, -1, 100);

            var str = new Analyzer().PrintHistory(instances);
        }

        private BOProtocol CreateProtocol(int processesCount, int faultyCount)
        {
            var processes = new List<BOProcess>();

            for (int i = 0; i < processesCount; i++)
            {
                var p = new BOProcess(new Archiver(), new RandomBooleanProposer(), f: faultyCount);
                processes.Add(p);
            }

            return new BOProtocol(processes, f: faultyCount);
        }
    }
}