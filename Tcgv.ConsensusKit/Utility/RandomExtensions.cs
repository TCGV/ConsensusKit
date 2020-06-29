using System;
using System.Threading;

namespace Tcgv.ConsensusKit.Utility
{
    public static class RandomExtensions
    {
        public static void Sleep(int randomDelayMilliseconds)
        {
            Thread.Sleep(rnd.Next(randomDelayMilliseconds));
        }

        public static bool Tryout(double p)
        {
            lock (rnd)
            {
                return rnd.NextDouble() < p;
            }
        }

        private static Random rnd = new Random();
    }
}
