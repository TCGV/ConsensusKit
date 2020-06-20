using Tcgv.ConsensusKit.Formatting;
using Tcgv.ConsensusKit.Security;

namespace Tcgv.ConsensusKit.Algorithms.Nakamoto.Data
{
    public class Block
    {
        public Block(object v, Block prev)
        {
            Value = v;
            PreviousId = prev?.Id ?? "";
            Height = (prev?.Height ?? 0) + 1;
            Id = Hex.BytesToHex(SHA256.Hash($"{Value}:{PreviousId}:{Height}"));
            PoW = 0;
        }

        public string Id { get; }
        public object Value { get; }
        public string PreviousId { get; }
        public int Height { get; }
        public long PoW { get; private set; }

        internal void IncrementPoW()
        {
            PoW++;
        }

        public static Block Genesis
        {
            get
            {
                return new Block("GENESIS", null);
            }
        }
    }
}
