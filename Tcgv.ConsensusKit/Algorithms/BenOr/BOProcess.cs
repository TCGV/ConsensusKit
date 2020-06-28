using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Algorithms.BenOr.Data;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.BenOr
{
    public class BOProcess : Process
    {
        public BOProcess(Archiver archiver, Proposer proposer, int f)
            : base(archiver, proposer)
        {
            this.f = f;
        }

        public override void Bind(Instance r)
        {
            WaitQuorum(r, MessageType.Propose, msgs =>
            {
                var x = PickMostFrequentValue(
                    msgs.Where(m => Archiver.CanCommit(m.Value))
                );

                var v = x.Count > r.Proposers.Count / 2 ? x.Value : null;

                Broadcast(r, MessageType.Select, v);
            });

            WaitQuorum(r, MessageType.Select, msgs =>
            {
                var x = PickMostFrequentValue(msgs.Where(m => m.Value != null));

                if (x.Count >= f + 1)
                {
                    Terminate(r, x.Value);
                    Proposer.Reset();
                }
                else
                {
                    if (x.Count > 0)
                        Proposer.Set(x.Value);
                    else
                        Proposer.Reset();
                }
            });
        }

        private AggregatedValue PickMostFrequentValue(IEnumerable<Message> msgs)
        {
            var x = (from m in msgs
                     group m by m.Value into g
                     select new { g.Key, Count = g.Count() })
                     .OrderByDescending(g => g.Count)
                     .FirstOrDefault();

            return new AggregatedValue(x?.Key, x?.Count ?? 0);
        }

        private int f;
    }
}
