namespace Tcgv.ConsensusKit.Actors
{
    public abstract class Proposer
    {
        public virtual object GetProposal()
        {
            return proposal;
        }

        public void Set(object proposal)
        {
            this.proposal = proposal;
        }

        public void Reset()
        {
            proposal = null;
        }

        private object proposal;
    }
}
