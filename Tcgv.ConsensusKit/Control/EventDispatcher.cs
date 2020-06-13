using System;
using System.Collections.Generic;
using System.Threading;

namespace Tcgv.ConsensusKit.Control
{
    public class EventDispatcher<TKey, TValue>
    {
        public EventDispatcher()
        {
            sync = new object();
            bindings = new Dictionary<TKey, List<EventBinding<TValue>>>();
        }

        public void Attach(TKey key, Func<TValue, TValue> filter, Action<TValue> action)
        {
            Attch(key, filter, action, EventRecurrency.Multiple);
        }

        public void AttachSingle(TKey key, Func<TValue, TValue> filter, Action<TValue> action)
        {
            Attch(key, filter, action, EventRecurrency.Single);
        }

        public void Trigger(TKey key, TValue value)
        {
            lock (sync)
            {
                if (bindings.ContainsKey(key))
                {
                    var active = new List<EventBinding<TValue>>(bindings[key].Count);
                    foreach (var b in bindings[key])
                    {
                        var filtered = b.Filter(value);
                        if (filtered != null)
                            ThreadPool.QueueUserWorkItem((x) => b.Action(filtered));
                        if (filtered == null || b.Recurrency == EventRecurrency.Multiple)
                            active.Add(b);
                    }
                    bindings[key] = active;
                }
            }
        }

        private void Attch(TKey key, Func<TValue, TValue> filter, Action<TValue> action, EventRecurrency r)
        {
            lock (sync)
            {
                if (!bindings.ContainsKey(key))
                    bindings.Add(key, new List<EventBinding<TValue>>());
                bindings[key].Add(new EventBinding<TValue>(filter, action, r));
            }
        }

        private Dictionary<TKey, List<EventBinding<TValue>>> bindings;
        private object sync;
    }
}
