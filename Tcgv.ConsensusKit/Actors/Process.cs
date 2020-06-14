using System;
using System.Collections.Generic;
using System.Threading;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Actors
{
    public abstract class Process
    {
        public Process(Archiver archiver, Proposer proposer)
        {
            Archiver = archiver;
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
            Broadcast(r, MessageType.Propose, v);
        }

        protected void SendTo(Instance r, Process destination, MessageType mType, object v)
        {
            var msg = new Message(this, destination, mType, v);
            r.Send(msg);
        }

        protected void Broadcast(Instance r, MessageType mType, object v)
        {
            var msg = new Message(this, mType, v);
            r.Send(msg);
        }

        protected void WaitMessage(Instance r, MessageType mType, Action<Message> onMessage)
        {
            r.WaitMessage(mType, this, onMessage);
        }

        protected void WaitQuorum(Instance r, MessageType mType, Action<HashSet<Message>> onQuorum)
        {
            r.WaitQuorum(mType, this, onQuorum);
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
