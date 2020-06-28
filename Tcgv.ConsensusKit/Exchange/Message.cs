using System.Threading;
using Tcgv.ConsensusKit.Actors;

namespace Tcgv.ConsensusKit.Exchange
{
    public class Message
    {
        public Message(Process s, MessageType type, object v)
            : this(s, null, type, v) { }

        public Message(Process s, Process d, MessageType type, object v)
        {
            Sequence = Interlocked.Increment(ref sequence);
            Source = s;
            Destination = d;
            Type = type;
            Value = v;
        }

        public int Sequence { get; }
        public Process Source { get; }
        public Process Destination { get; }
        public MessageType Type { get; }
        public object Value { get; }

        private static int sequence;
    }
}
