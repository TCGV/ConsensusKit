using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Control;

namespace Tcgv.ConsensusKit.Actors
{
    public class Archiver
    {
        public Archiver()
        {
            commited = new Dictionary<Instance, object>();
        }

        public virtual bool CanCommit(object value)
        {
            return true;
        }

        public void Commit(Instance r, object v)
        {
            commited.Add(r, v);
        }

        public bool IsCommited(Instance r)
        {
            return commited.ContainsKey(r);
        }

        public object Query(Instance r)
        {
            return commited[r];
        }

        public IEnumerable<Instance> Query(object v)
        {
            return commited
                .Where(p => v.Equals(p.Value))
                .Select(p => p.Key);
        }

        private Dictionary<Instance, object> commited;
    }
}
