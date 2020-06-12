using System;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.ChandraToueg
{
    public class CTInstance : Instance
    {
        public CTInstance(HashSet<Process> proposers, HashSet<Process> deciders, MessageBuffer buffer)
            : base(proposers, deciders, buffer) { }

        public override bool HasQuorum(HashSet<Message> msgs)
        {
            var x = msgs.FirstOrDefault();
            if (x != null)
            {
                var mCount = msgs.Count;
                var dCount = Deciders.Count;
                var majority = Math.Ceiling((Proposers.Count + dCount) / 2.0d);

                switch (x.Type)
                {
                    case MessageType.Propose:
                    case MessageType.Ack:
                    case MessageType.Nack:
                        return mCount >= majority;
                    case MessageType.Select:
                    case MessageType.Decide:
                        return mCount >= dCount;
                }
            }
            return false;
        }
    }
}
