using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Algorithms.Utility;

namespace Tcgv.ConsensusKit.Algorithms.Paxos.Tests
{
    [TestClass()]
    public class PxProtocolTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            var processes = new List<PxProcess>();
            for (int i = 0; i < 32; i++)
                processes.Add(new PxProcess(new Archiver(), new RandomStringProposer()));

            var protocol = new PxProtocol(processes);

            var instances = protocol.Execute(10, -1);

            Assert.AreEqual(10, instances.Length);
            Assert.IsTrue(instances.All(r => r.Consensus));
        }
    }
}