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
            MessageBuffer buffer)
            : base(proposers, deciders, buffer) { }

        public override bool HasQuorum(HashSet<Message> msgs)
        {
            return false;
        }
    }
}
