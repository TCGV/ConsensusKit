using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Control
{
    public abstract class Instance
    {
        public Instance(HashSet<Process> proposers, HashSet<Process> deciders, MessageBuffer buffer)
        {
            Proposers = proposers;
            Deciders = deciders;
            Value = null;
            this.buffer = buffer;
            dispatcher = new EventDispatcher<MessageType, HashSet<Message>>();
        }

        public HashSet<Process> Proposers { get; }
        public HashSet<Process> Deciders { get; }
        public object Value { get; private set; }

        public abstract bool HasQuorum(HashSet<Message> msgs);

        public void Execute(int millisecondsTimeout)
        {
            var all = Proposers.Union(Deciders);

            foreach (var p in all)
                p.Bind(this);

            Parallel.ForEach(all, p => p.Execute(this));

            WaitTermination(all, millisecondsTimeout);

            Value = GetAgreedValue(all);
        }

        public void Broadcast(Message msg)
        {
            buffer.Add(this, msg);
            var msgs = buffer.Filter(msg.Type, this);
            if (HasQuorum(msgs))
                dispatcher.Trigger(msg.Type, msgs);
        }

        public void WaitQuorum(MessageType type, Action<HashSet<Message>> onQuorum)
        {
            dispatcher.AttachSingle(type, onQuorum);
        }

        private void WaitTermination(IEnumerable<Process> all, int millisecondsTimeout)
        {
            all.All(p => p.Join(this, millisecondsTimeout));
        }

        private object GetAgreedValue(IEnumerable<Process> all)
        {
            return all
                .Select(a => a.Archiver.Query(this))
                .Distinct()
                .SingleOrDefault();
        }

        private EventDispatcher<MessageType, HashSet<Message>> dispatcher;
        private MessageBuffer buffer;
    }
}
