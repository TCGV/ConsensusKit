using Tcgv.ConsensusKit.Actors;

namespace Tcgv.ConsensusKit.Control
{
    public abstract class Protocol
    {
        public Protocol(Process[] processes)
        {
            Processes = processes;
        }

        public Process[] Processes { get; }

        public abstract void Execute(int iterations, int millisecondsTimeout);
    }
}
