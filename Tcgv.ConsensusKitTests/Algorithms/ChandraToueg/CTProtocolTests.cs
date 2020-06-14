using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Algorithms.Utility;

namespace Tcgv.ConsensusKit.Algorithms.ChandraToueg.Tests
{
    [TestClass]
    public class CTProtocolTests
    {
        [TestMethod]
        public void ExecuteTest()
        {
            var processes = new List<CTProcess>();
            for (int i = 0; i < 32; i++)
                processes.Add(new CTProcess(new UniqueArchiver(), new RandomStringProposer()));

            var protocol = new CTProtocol(processes);

            var instances = protocol.Execute(10, -1);

            Assert.AreEqual(10, instances.Length);
            Assert.IsTrue(instances.All(r => r.Value != null));
        }
    }
}