using System.Collections.Generic;
using System.Linq;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Algorithms.BenOr
{
    public class BOInstance : Instance
    {
        public BOInstance(
            HashSet<Process> proposers,
            HashSet<Process> deciders,
            MessageBuffer buffer,
            int randomDispatchDelay,
            int f)
            : base(proposers, deciders, buffer, randomDispatchDelay)
        {
            this.f = f;
        }

        public override bool HasQuorum(HashSet<Message> msgs)
        {
            var x = msgs.FirstOrDefault();
            if (x != null)
            {
                var mCount = msgs.Count;
                var pCount = Proposers.Count;

                switch (x.Type)
                {
                    case MessageType.Propose:
                    case MessageType.Select:
                        return mCount >= pCount - f - 1;
                }
            }
            return false;
        }

        private int f;
    }
}
