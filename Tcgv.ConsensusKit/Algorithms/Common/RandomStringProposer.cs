using System;
using Tcgv.ConsensusKit.Actors;

namespace Tcgv.ConsensusKit.Algorithms.Common
{
    public class RandomStringProposer : Proposer
    {
        public override object GetProposal()
        {
            if (base.GetProposal() == null)
                Set(Guid.NewGuid().ToString());
            return base.GetProposal();
        }
    }
}
