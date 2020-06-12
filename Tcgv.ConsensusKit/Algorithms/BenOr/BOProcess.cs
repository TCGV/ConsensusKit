using System;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Algorithms.Common;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.BenOr
{
    public class BOProcess : Process
    {
        public BOProcess(int f)
            : base(new Archiver(), new RandomBooleanProposer())
        {
            this.f = f;
        }

        public override void Bind(Instance r)
        {
            r.WaitQuorum(MessageType.Propose, msgs =>
            {
                var t = PickValue(msgs);

                var v = t.Item2 > msgs.Count / 2 ? t.Item1 : null;

                Broadcast(r, MessageType.Select, v);
            });

            r.WaitQuorum(MessageType.Select, msgs =>
            {
                var t = PickValue(msgs.Where(m => m.Value != null));

                if (t.Item2 >= f + 1)
                {
                    Terminate(r, t.Item1);
                    Proposer.Reset();
                }
                else
                {
                    if (t.Item2 > 0)
                        Proposer.Set(t.Item1);
                    else
                        Proposer.Reset();
                    Terminate(r, null);
                }
            });
        }

        private Tuple<object, int> PickValue(IEnumerable<Message> msgs)
        {
            var x = (from m in msgs
                     group m by m.Value into g
                     select new { g.Key, Count = g.Count() })
                     .OrderByDescending(g => g.Count)
                     .FirstOrDefault();
            return new Tuple<object, int>(x?.Key, x?.Count ?? 0);
        }

        private int f;
    }
}
