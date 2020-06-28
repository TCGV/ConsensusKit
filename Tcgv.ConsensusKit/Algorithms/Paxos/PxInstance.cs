using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.Paxos
{
    public class PxInstance : Instance
    {
        public PxInstance(
            HashSet<Process> proposers,
            HashSet<Process> deciders,
            MessageBuffer buffer)
            : base(proposers, deciders, buffer) { }

        public override bool HasQuorum(HashSet<Message> msgs)
        {
            var x = msgs.FirstOrDefault();
            if (x != null)
            {
                var mCount = msgs.Count;
                var majority = 1 + (Deciders.Count / 2);
                return mCount >= majority;
            }
            return false;
        }
    }
}
