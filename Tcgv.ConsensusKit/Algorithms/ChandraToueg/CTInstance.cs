using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.ChandraToueg
{
    public class CTInstance : Instance
    {
        public CTInstance(
            HashSet<Process> proposers,
            HashSet<Process> deciders,
            MessageBuffer buffer,
            int randomDispatchDelay)
            : base(proposers, deciders, buffer, randomDispatchDelay) { }

        public override bool HasQuorum(HashSet<Message> msgs)
        {
            var x = msgs.FirstOrDefault();
            if (x != null)
            {
                var mCount = msgs.Count;
                var dCount = Deciders.Count;
                var majority = 1 + (Proposers.Count / 2);

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
