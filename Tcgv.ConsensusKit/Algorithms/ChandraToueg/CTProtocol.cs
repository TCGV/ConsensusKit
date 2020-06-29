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

        public override Instance[] Execute(
            int iterations,
            int millisecondsTimeout,
            int randomDispatchDelay = 0)
        {
            var instances = new Instance[iterations];

            var buffer = new MessageBuffer();

            for (int i = 0; i < iterations; i++)
            {
                var coordinator = Processes[i % Processes.Length];
                var proposers = Processes.Except(new[] { coordinator });

                var r = new CTInstance(
                    new HashSet<Process>(proposers),
                    new HashSet<Process>(new[] { coordinator }),
                    buffer,
                    randomDispatchDelay
                );

                r.Execute(millisecondsTimeout);
                instances[i] = r;
            }

            return instances;
        }
    }
}
