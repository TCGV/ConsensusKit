using System.Collections.Generic;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.Nakamoto
{
    public class NkInstance : Instance
    {
        public NkInstance(
            HashSet<Process> proposers,
            HashSet<Process> deciders,
            MessageBuffer buffer,
            int randomDispatchDelay)
            : base(proposers, deciders, buffer, randomDispatchDelay) { }

        public override bool HasQuorum(HashSet<Message> msgs)
        {
            return false;
        }
    }
}
