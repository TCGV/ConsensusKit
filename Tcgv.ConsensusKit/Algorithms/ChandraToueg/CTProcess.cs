using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Algorithms.Common;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.ChandraToueg
{
    public class CTProcess : Process
    {
        public CTProcess()
            : base(new UniqueArchiver(), new RandomStringProposer()) { }

        public override void Bind(Instance r)
        {
            if (r.Deciders.Contains(this))
                BindAsCoordinator(r);
            else
                BindAsProcess(r);
        }

        private void BindAsCoordinator(Instance r)
        {
            r.WaitQuorum(MessageType.Propose, msgs =>
            {
                var v = PickValue(
                    msgs.Where(m => Archiver.CanCommit(m.Value))
                );

                Broadcast(r, MessageType.Select, v);
            });

            r.WaitQuorum(MessageType.Ack, msgs =>
            {
                var v = PickValue(msgs);
                Broadcast(r, MessageType.Decide, v);
                Terminate(r, v);
            });
        }

        private void BindAsProcess(Instance r)
        {
            r.WaitQuorum(MessageType.Select, msgs =>
            {
                var v = msgs.Single().Value;
                Broadcast(r, MessageType.Ack, v);
            });

            r.WaitQuorum(MessageType.Decide, msgs =>
            {
                var v = msgs.Single().Value;
                Terminate(r, v);
            });
        }

        private object PickValue(IEnumerable<Message> msgs)
        {
            return msgs
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault()?.Value;
        }
    }
}
