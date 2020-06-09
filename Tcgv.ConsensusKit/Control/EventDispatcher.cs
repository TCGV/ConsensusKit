using System;
using System.Collections.Generic;

namespace Tcgv.ConsensusKit.Control
{
    public class EventDispatcher<TKey, TValue>
    {
        public EventDispatcher()
        {
            sync = new object();
            bindings = new Dictionary<TKey, List<EventBinding<TValue>>>();
        }

        public void Attach(TKey key, Action<TValue> action)
        {
            Attch(key, action, EventRecurrency.Multiple);
        }

        public void AttachSingle(TKey key, Action<TValue> action)
        {
            Attch(key, action, EventRecurrency.Single);
        }

        public void Trigger(TKey key, TValue value)
        {
            lock (sync)
            {
                var active = new List<EventBinding<TValue>>(bindings[key].Count);
                foreach (var b in bindings[key])
                {
                    b.Action(value);
                    if (b.Recurrency == EventRecurrency.Multiple)
                        active.Add(b);
                }
                bindings[key] = active;
            }
        }

        private void Attch(TKey key, Action<TValue> action, EventRecurrency r)
        {
            lock (sync)
            {
                if (!bindings.ContainsKey(key))
                    bindings.Add(key, new List<EventBinding<TValue>>());
                bindings[key].Add(new EventBinding<TValue>(action, r));
            }
        }

        private Dictionary<TKey, List<EventBinding<TValue>>> bindings;
        private object sync;
    }
}
