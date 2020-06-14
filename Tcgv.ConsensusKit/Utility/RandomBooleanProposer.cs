using System;
using Tcgv.ConsensusKit.Actors;

namespace Tcgv.ConsensusKit.Algorithms.Utility
{
    public class RandomBooleanProposer : Proposer
    {
        public override object GetProposal()
        {
            if (base.GetProposal() == null)
                Set(Guid.NewGuid().GetHashCode() % 2 == 0);
            return base.GetProposal();
        }
    }
}
