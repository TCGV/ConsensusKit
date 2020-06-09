using System;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.ChandraToueg
{
    public class CTProcess : Process
    {
        public CTProcess()
            : base(new CTArchiver()) { }

        public override string GetProposal()
        {
            if (proposal == null)
                proposal = Guid.NewGuid().ToString();
            return proposal;
        }

        public override void Bind(Instance r)
        {
            if (r.Deciders.Contains(this))
                BindAsCoordinator(r);
            else
                BindAsProcess(r);
        }

        protected override void Start(Instance r)
        {
            var msg = new Message(
                this, MessageType.Propose, r, GetProposal()
            );
            r.Broadcast(msg);
        }

        private void BindAsCoordinator(Instance r)
        {
            r.WaitQuorum(MessageType.Propose, (msgs) =>
            {
                var v = PickValue(
                    msgs.Where(m => Archiver.IsValidProposal(m.Value))
                );

                var msg = new Message(
                    this, MessageType.Select, r, v
                );

                r.Broadcast(msg);
            });

            r.WaitQuorum(MessageType.Ack, (msgs) =>
            {
                proposal = PickValue(msgs);

                var msg = new Message(
                    this, MessageType.Decide, r, proposal
                );

                r.Broadcast(msg);

                Terminate(r, proposal);
            });
        }

        private void BindAsProcess(Instance r)
        {
            r.WaitQuorum(MessageType.Select, (msgs) =>
            {
                var v = msgs.Single().Value;

                var msg = new Message(
                    this, MessageType.Ack, r, v
                );

                r.Broadcast(msg);
            });

            r.WaitQuorum(MessageType.Decide, (msgs) =>
            {
                var v = msgs.Single().Value;
                Terminate(r, v);
            });
        }

        private string PickValue(IEnumerable<Message> msgs)
        {
            return msgs
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault()?.Value;
        }

        private string proposal;
    }
}
