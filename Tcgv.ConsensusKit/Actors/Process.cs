using System.Collections.Generic;
using System.Threading;
using Tcgv.ConsensusKit.Control;

namespace Tcgv.ConsensusKit.Actors
{
    public abstract class Process
    {
        public Process(Archiver archive)
        {
            Archiver = archive;
            barriers = new Dictionary<Instance, ManualResetEvent>();
        }
        public Archiver Archiver { get; }

        public abstract string GetProposal();

        public abstract void Bind(Instance r);

        protected abstract void Start(Instance r);

        public void Execute(Instance r)
        {
            if (!IsTerminated(r))
            {
                barriers.Add(r, new ManualResetEvent(false));
                Start(r);
            }
        }

        public bool Join(Instance r, int millisecondsTimeout)
        {
            bool b = true;
            if (barriers.ContainsKey(r))
                b = barriers[r].WaitOne(millisecondsTimeout);
            return b;
        }

        public void Terminate(Instance r, string v)
        {
            Archiver.Commit(r, v);
            if (barriers.ContainsKey(r))
                barriers[r].Set();
        }

        public bool IsTerminated(Instance r)
        {
            return Archiver.IsCommited(r);
        }

        private Dictionary<Instance, ManualResetEvent> barriers;
    }
}
