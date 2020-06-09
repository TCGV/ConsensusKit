using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Control;

namespace Tcgv.ConsensusKit.Actors
{
    public abstract class Archiver
    {
        public Archiver()
        {
            commited = new Dictionary<Instance, string>();
        }

        public void Commit(Instance r, string v)
        {
            commited.Add(r, v);
        }

        public abstract bool IsValidProposal(string value);

        public bool IsCommited(Instance r)
        {
            return commited.ContainsKey(r);
        }

        public IEnumerable<Instance> Query(string v)
        {
            return commited
                .Where(p => v.Equals(p.Value))
                .Select(p => p.Key);
        }

        private Dictionary<Instance, string> commited;
    }
}
