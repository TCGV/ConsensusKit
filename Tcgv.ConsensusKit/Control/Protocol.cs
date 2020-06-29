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

        public abstract Instance[] Execute(
            int iterations,
            int millisecondsTimeout,
            int randomDispatchDelay = 0);
    }
}
