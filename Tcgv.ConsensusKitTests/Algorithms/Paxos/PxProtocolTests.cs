﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Algorithms.Utility;
using Tcgv.ConsensusKit.Formatting;

namespace Tcgv.ConsensusKit.Algorithms.Paxos.Tests
{
    [TestClass()]
    public class PxProtocolTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            var protocol = CreateProtocol(32);

            var instances = protocol.Execute(10, -1);

            Assert.AreEqual(10, instances.Length);
            Assert.IsTrue(instances.All(r => r.Consensus));
        }

        [TestMethod()]
        public void PrintHistoryTest()
        {
            var protocol = CreateProtocol(32);

            var instances = protocol.Execute(5, -1, 100);

            var str = new Analyzer().PrintHistory(instances);
        }

        private PxProtocol CreateProtocol(int processesCount)
        {
            var processes = new List<PxProcess>();

            for (int i = 0; i < processesCount; i++)
            {
                var p = new PxProcess(new UniqueArchiver(), new RandomStringProposer());
                processes.Add(p);
            }

            return new PxProtocol(processes);
        }
    }
}