using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
                processes.Add(new CTProcess());

            var protocol = new CTProtocol(processes);

            protocol.Execute(5, -1);
        }
    }
}