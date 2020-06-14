using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.Paxos
{
    public class PxProtocol : Protocol
    {
        public PxProtocol(PxProcess[] processes)
            : base(processes) { }

        public PxProtocol(IEnumerable<PxProcess> processes)
            : base(processes.ToArray()) { }

        public override Instance[] Execute(int iterations, int millisecondsTimeout)
        {
            var instances = new Instance[iterations];

            var buffer = new MessageBuffer();

            for (int i = 0; i < iterations; i++)
            {
                var len = 3;
                var proposers = Processes
                    .Skip(i % (Processes.Length - len)).Take(len);
                var accepters = Processes.Except(proposers);

                var r = new PxInstance(
                    new HashSet<Process>(proposers),
                    new HashSet<Process>(accepters),
                    buffer
                );

                r.Execute(millisecondsTimeout);
                instances[i] = r;
            }

            return instances;
        }
    }
}
