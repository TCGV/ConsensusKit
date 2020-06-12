using System.Collections.Generic;
using System.Threading;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Actors
{
    public abstract class Process
    {
        public Process(Archiver archive, Proposer proposer)
        {
            Archiver = archive;
            Proposer = proposer;
            barriers = new Dictionary<Instance, ManualResetEvent>();
        }

        public Archiver Archiver { get; }

        public Proposer Proposer { get; }

        public abstract void Bind(Instance r);

        public void Execute(Instance r)
        {
            lock (barriers)
            {
                if (!IsTerminated(r))
                {
                    barriers.Add(r, new ManualResetEvent(false));
                    Start(r);
                }
            }
        }

        public bool Join(Instance r, int millisecondsTimeout)
        {
            bool b = true;
            if (barriers.ContainsKey(r))
                b = barriers[r].WaitOne(millisecondsTimeout);
            return b;
        }

        protected virtual void Start(Instance r)
        {
            var v = Proposer.GetProposal();

            var msg = new Message(this, MessageType.Propose, v);

            r.Broadcast(msg);
        }

        protected void Broadcast(Instance r, MessageType mType, object v)
        {
            var msg = new Message(this, mType, v);
            r.Broadcast(msg);
        }

        protected void Terminate(Instance r, object v)
        {
            lock (barriers)
            {
                Archiver.Commit(r, v);
                if (barriers.ContainsKey(r))
                    barriers[r].Set();
            }
        }

        protected bool IsTerminated(Instance r)
        {
            return Archiver.IsCommited(r);
        }

        private Dictionary<Instance, ManualResetEvent> barriers;
    }
}
