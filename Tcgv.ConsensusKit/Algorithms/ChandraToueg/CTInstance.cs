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
            var f = msgs.FirstOrDefault();
            if (f != null)
            {
                var mCount = msgs.Count;
                var dCount = Deciders.Count;
                var majority = Math.Ceiling((Proposers.Count + dCount) / 2.0d);

                switch (f.Type)
                {
                    case MessageType.Propose:
                        return mCount >= majority;
                    case MessageType.Select:
                        return mCount >= dCount;
                    case MessageType.Ack:
                        return mCount >= majority;
                    case MessageType.Nack:
                        return mCount >= majority;
                    case MessageType.Decide:
                        return mCount >= dCount;
                }
            }
            return false;
        }
    }
}
