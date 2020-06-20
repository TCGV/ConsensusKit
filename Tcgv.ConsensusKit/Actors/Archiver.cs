using System;
using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Control;

namespace Tcgv.ConsensusKit.Actors
{
    public class Archiver
    {
        public Archiver()
        {
            sync = new object();
            commited = new Dictionary<Instance, object>();
        }

        public virtual bool CanCommit(object value)
        {
            return true;
        }

        public void Commit(Instance r, object v)
        {
            lock (sync)
            {
                if (commited.ContainsKey(r))
                {
                    if (commited[r] != v)
                        throw new InvalidOperationException();
                }
                else
                {
                    commited.Add(r, v);
                }
            }
        }

        public bool IsCommited(Instance r)
        {
            lock (sync)
            {
                return commited.ContainsKey(r);
            }
        }

        public object Query(Instance r)
        {
            lock (sync)
            {
                return commited[r];
            }
        }

        public Instance[] Query(object v)
        {
            lock (sync)
            {
                return commited
                .Where(p => v.Equals(p.Value))
                .Select(p => p.Key)
                .ToArray();
            }
        }

        private Dictionary<Instance, object> commited;
        private object sync;
    }
}
