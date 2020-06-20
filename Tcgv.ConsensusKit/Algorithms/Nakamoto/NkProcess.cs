using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tcgv.ConsensusKit.Actors;
using Tcgv.ConsensusKit.Algorithms.Nakamoto.Data;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;
using Tcgv.ConsensusKit.Security;

namespace Tcgv.ConsensusKit.Algorithms.Nakamoto
{
    public class NkProcess : Process
    {
        public NkProcess(Archiver archiver, Proposer proposer, int k)
            : base(archiver, proposer)
        {
            this.k = k;
            counter = 0;
            sync = new object();
            blockchains = new Dictionary<string, LinkedList<Block>>();
            AddGenesisChain();
        }

        protected override void Start(Instance r)
        {
            new Thread(() =>
            {
                if (Archiver.CanCommit(Proposer.GetProposal()))
                {
                    var b = MineBlock(r);
                    if (b != null)
                    {
                        ProcessBlock(r, b);
                        Broadcast(r, MessageType.Propose, b);
                    }
                }
            }).Start();
        }

        public override void Bind(Instance r)
        {
            WaitMessage(r, MessageType.Propose, msg =>
            {
                var b = msg.Value as Block;
                ProcessBlock(r, b);
            });
        }

        private void ProcessBlock(Instance r, Block b)
        {
            if (VerifyPoW(b))
            {
                var newChain = AddNewChain(b);
                if (newChain != null && b.Height - k0 == k && !IsTerminated(r))
                {
                    Terminate(r, GetBlockAt(newChain, k).Value);
                    CommitChain(newChain);
                }
            }
        }

        private Block MineBlock(Instance r)
        {
            Block b = null;
            var found = false;
            var v = Proposer.GetProposal();
            var c = counter;
            while (!IsTerminated(r) && !found)
            {
                if (b == null || counter > c)
                {
                    c = counter;
                    var h = GetHeadFromLongestChain(v);
                    if (h == null)
                        break;
                    b = new Block(v, h);
                }
                b.IncrementPoW();
                found = VerifyPoW(b);
            }
            return found ? b : null;
        }

        private static bool VerifyPoW(Block b, int leadingZeros = 2)
        {
            var bytes = SHA256.Hash($"{b.Id}{b.PoW}");
            int i = 0;
            for (; i < leadingZeros && bytes[i] == 0; i++) ;
            return i == leadingZeros;
        }

        private Block GetHeadFromLongestChain(object v)
        {
            lock (sync)
            {
                LinkedList<Block> chain = null;
                foreach (var c in GetChainsWithoutValue(v))
                {
                    if (chain == null || c.First.Value.Height > chain.First.Value.Height)
                        chain = c;
                }
                return chain?.First?.Value;
            }
        }

        private static Block GetBlockAt(LinkedList<Block> chain, int index)
        {
            var x = chain.First;
            for (int i = 0; i < index - 1; i++)
                x = x.Next;
            return x.Value;
        }

        private void AddGenesisChain()
        {
            lock (sync)
            {
                var g = Block.Genesis;
                var chain = new LinkedList<Block>();
                chain.AddFirst(g);
                blockchains.Add(g.Id, chain);
                k0 = g.Height;
            }
        }

        private LinkedList<Block> AddNewChain(Block header)
        {
            lock (sync)
            {
                LinkedList<Block> newChain = null;
                if (blockchains.ContainsKey(header.PreviousId))
                {
                    var oldChain = blockchains[header.PreviousId];
                    newChain = new LinkedList<Block>(oldChain);
                    newChain.AddFirst(header);
                    blockchains.Add(header.Id, newChain);
                    counter++;
                }
                return newChain;
            }
        }

        private void CommitChain(LinkedList<Block> newChain)
        {
            lock (sync)
            {
                blockchains.Clear();
                var header = newChain.First.Value;
                blockchains.Add(header.Id, newChain);
                k0++;
            }
        }

        private IEnumerable<LinkedList<Block>> GetChainsWithoutValue(object v)
        {
            return blockchains.Values.Where(x => !x.Any(y => y.Value == v));
        }

        private Dictionary<string, LinkedList<Block>> blockchains;
        private int counter;
        private object sync;
        private int k0;
        private int k;
    }
}
