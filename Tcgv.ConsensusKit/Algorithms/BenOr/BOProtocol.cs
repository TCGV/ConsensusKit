using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.BenOr
{
    public class BOProtocol : Protocol
    {
        public BOProtocol(BOProcess[] processes, int f)
            : base(processes)
        {
            this.f = f;
        }

        public BOProtocol(IEnumerable<BOProcess> processes, int f)
            : this(processes.ToArray(), f) { }

        public override Instance[] Execute(
            int iterations,
            int millisecondsTimeout,
            int randomDispatchDelay = 0)
        {
            var instances = new Instance[iterations];

            var buffer = new MessageBuffer();

            for (int i = 0; i < iterations; i++)
            {
                var r = new BOInstance(
                    new HashSet<Process>(Processes),
                    new HashSet<Process>(),
                    buffer,
                    randomDispatchDelay,
                    f: f
                );

                r.Execute(millisecondsTimeout);
                instances[i] = r;
            }

            return instances;
        }

        private int f;
    }
}
