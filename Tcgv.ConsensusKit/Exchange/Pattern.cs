using Tcgv.ConsensusKit.Control;

namespace Tcgv.ConsensusKit.Exchange
{
    public class Pattern
    {
        public Pattern(MessageType type, Instance r)
        {
            Type = type;
            Instance = r;
        }

        public MessageType Type { get; }
        public Instance Instance { get; }
    }
}
