using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.ChandraToueg
{
    public class CTProcess : Process
    {
        public CTProcess(Archiver archiver, Proposer proposer)
            : base(archiver, proposer) { }

        public override void Bind(Instance r)
        {
            if (r.Deciders.Contains(this))
                BindAsCoordinator(r);
            else
                BindAsProcess(r);
        }

        private void BindAsCoordinator(Instance r)
        {
            WaitQuorum(r, MessageType.Propose, msgs =>
            {
                var v = PickMostRecentValue(
                    msgs.Where(m => Archiver.CanCommit(m.Value))
                );

                Broadcast(r, MessageType.Select, v);
            });

            WaitQuorum(r, MessageType.Ack, msgs =>
            {
                var v = PickMostRecentValue(msgs);
                Broadcast(r, MessageType.Decide, v);
                Terminate(r, v);
            });
        }

        private void BindAsProcess(Instance r)
        {
            WaitQuorum(r, MessageType.Select, msgs =>
            {
                var v = msgs.Single().Value;
                Broadcast(r, MessageType.Ack, v);
            });

            WaitQuorum(r, MessageType.Decide, msgs =>
            {
                var v = msgs.Single().Value;
                Terminate(r, v);
            });
        }

        private object PickMostRecentValue(IEnumerable<Message> msgs)
        {
            return msgs
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault()?.Value;
        }
    }
}
