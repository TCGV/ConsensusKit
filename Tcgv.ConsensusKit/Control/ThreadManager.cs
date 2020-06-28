using System;
using System.Collections.Generic;
using System.Threading;

namespace Tcgv.ConsensusKit.Control
{
    public static class ThreadManager
    {
        static ThreadManager()
        {
            active = 0;
            empty = new ManualResetEvent(true);
        }

        public static void Enqueue(Action action)
        {
            Increment();
            ThreadPool.QueueUserWorkItem((x) =>
            {
                action();
                Decrement();
            });
        }

        public static void ForEach<T>(IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                Enqueue(() => action(item));
        }

        public static void Start(Action action)
        {
            Increment();
            new Thread(() =>
            {
                action();
                Decrement();
            }).Start();
        }

        public static void Join(int millisecondsTimeout)
        {
            empty.WaitOne(millisecondsTimeout);
        }

        private static void Increment()
        {
            Interlocked.Increment(ref active);
            empty.Reset();
        }

        private static void Decrement()
        {
            if (Interlocked.Decrement(ref active) == 0)
                empty.Set();
        }

        private static ManualResetEvent empty;
        private static int active;
    }
}
