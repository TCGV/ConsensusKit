using System;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Control;

namespace Tcgv.ConsensusKit.Exchange
{
    public class Message
    {
        public Message(Process s, MessageType type, Instance r, string v)
        {
            Timestamp = DateTime.UtcNow;
            Source = s;
            Type = type;
            Instance = r;
            Value = v;
        }

        public DateTime Timestamp { get; }
        public Process Source { get; }
        public MessageType Type { get; }
        public Instance Instance { get; }
        public string Value { get; }
    }
}
