using System;
using Tcgv.ConsensusKit.Actors;

namespace Tcgv.ConsensusKit.Exchange
{
    public class Message
    {
        public Message(Process s, MessageType type, object v)
        {
            Timestamp = DateTime.UtcNow;
            Source = s;
            Type = type;
            Value = v;
        }

        public DateTime Timestamp { get; }
        public Process Source { get; }
        public MessageType Type { get; }
        public object Value { get; }
    }
}
