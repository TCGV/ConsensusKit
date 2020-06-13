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
            quorumDispatcher = new EventDispatcher<MessageType, HashSet<Message>>();
            msgDispatcher = new EventDispatcher<MessageType, Message>();
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

        public void Send(Message msg)
        {
            buffer.Add(this, msg);
            msgDispatcher.Trigger(msg.Type, msg);

            var msgs = buffer.Filter(msg.Type, this);
            quorumDispatcher.Trigger(msg.Type, msgs);
        }

        public void WaitMessage(MessageType type, Process receiver, Action<Message> onMessage)
        {
            msgDispatcher.Attach(type, x =>
            {
                if (ShouldReceive(receiver, x))
                    return x;
                return null;
            }, onMessage);
        }

        public void WaitQuorum(MessageType type, Process receiver, Action<HashSet<Message>> onQuorum)
        {
            quorumDispatcher.AttachSingle(type, x =>
            {
                var y = new HashSet<Message>(
                    x.Where(m => ShouldReceive(receiver, m))
                );
                if (HasQuorum(y))
                    return y;
                return null;
            }, onQuorum);
        }

        private void WaitTermination(IEnumerable<Process> all, int millisecondsTimeout)
        {
            all.All(p => p.Join(this, millisecondsTimeout));
        }

        private object GetAgreedValue(IEnumerable<Process> all)
        {
            var values = all
                .Select(a => a.Archiver.Query(this))
                .Distinct();

            return values.SingleOrDefault();
        }

        private bool ShouldReceive(Process receiver, Message msg)
        {
            return (msg.Destination == null && msg.Source != receiver) || msg.Destination == receiver;
        }

        private EventDispatcher<MessageType, HashSet<Message>> quorumDispatcher;
        private EventDispatcher<MessageType, Message> msgDispatcher;
        private MessageBuffer buffer;
    }
}
