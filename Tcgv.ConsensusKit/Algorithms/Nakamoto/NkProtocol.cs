using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.Nakamoto
{
    public class NkProtocol : Protocol
    {
        public NkProtocol(NkProcess[] processes)
            : base(processes)
        {
        }

        public NkProtocol(IEnumerable<NkProcess> processes)
            : this(processes.ToArray()) { }

        public override Instance[] Execute(
            int iterations,
            int millisecondsTimeout,
            int randomDispatchDelay = 0)
        {
            var instances = new Instance[iterations];

            var buffer = new MessageBuffer();

            for (int i = 0; i < iterations; i++)
            {
                var r = new NkInstance(
                    new HashSet<Process>(Processes),
                    new HashSet<Process>(Processes),
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
