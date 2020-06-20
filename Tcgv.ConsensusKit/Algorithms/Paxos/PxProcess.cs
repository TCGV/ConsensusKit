using System;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Algorithms.Paxos.Data;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.Paxos
{
    public class PxProcess : Process
    {
        public PxProcess(Archiver archiver, Proposer proposer)
            : base(archiver, proposer)
        {
            proposalNumber = -1;
            minNumber = 0;
        }

        protected override void Start(Instance r)
        {
            accepted = null;

            if (r.Proposers.Contains(this))
            {
                proposalNumber = minNumber + 1;

                Broadcast(r, MessageType.Propose, proposalNumber);
            }
        }

        public override void Bind(Instance r)
        {
            if (r.Proposers.Contains(this))
                BindAsProposer(r);
            else
                BindAsAccepter(r);
        }

        private void BindAsProposer(Instance r)
        {
            WaitQuorum(r, MessageType.Ack, msgs =>
            {
                var v = PickHighestNumberedValue(msgs)?.Value ?? Proposer.GetProposal();

                if (Archiver.CanCommit(v))
                {
                    accepted = new NumberedValue(v, proposalNumber);
                    Broadcast(r, MessageType.Select, accepted);
                }
                else
                {
                    v = null;
                }

                Terminate(r, v);
            });

            WaitMessage(r, MessageType.Nack, msg =>
            {
                if (msg.Value != null)
                {
                    var n = (long)msg.Value;
                    minNumber = Math.Max(n, minNumber);
                }

                Terminate(r, null);
            });
        }

        private void BindAsAccepter(Instance r)
        {
            WaitMessage(r, MessageType.Propose, msg =>
            {
                var n = (long)msg.Value;

                if (n > minNumber)
                {
                    minNumber = n;
                    SendTo(msg.Source, r, MessageType.Ack, accepted);
                }
                else
                {
                    SendTo(msg.Source, r, MessageType.Nack, minNumber);
                }
            });

            WaitMessage(r, MessageType.Select, msg =>
            {
                var x = msg.Value as NumberedValue;

                if (x.Number >= minNumber)
                {
                    accepted = x;
                    Terminate(r, accepted?.Value);
                }
            });
        }

        private NumberedValue PickHighestNumberedValue(IEnumerable<Message> msgs)
        {
            return (from m in msgs
                    where m.Value != null
                    select m.Value as NumberedValue)
                   .OrderByDescending(v => v.Number)
                   .FirstOrDefault();
        }

        private long proposalNumber;
        private long minNumber;
        private NumberedValue accepted;
    }
}
