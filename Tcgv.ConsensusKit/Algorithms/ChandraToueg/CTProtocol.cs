using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.ChandraToueg
{
    public class CTProtocol : Protocol
    {
        public CTProtocol(CTProcess[] processes)
            : base(processes) { }

        public CTProtocol(IEnumerable<CTProcess> processes)
            : base(processes.ToArray()) { }

        public override void Execute(int iterations, int millisecondsTimeout)
        {
            var buffer = new MessageBuffer();

            for (int i = 0; i < iterations; i++)
            {
                var coordinator = Processes[i % Processes.Length];

                var r = new CTInstance(
                    new HashSet<Process>(Processes),
                    new HashSet<Process>(new[] { coordinator }),
                    buffer
                );

                r.Execute(millisecondsTimeout);
            }
        }
    }
}
