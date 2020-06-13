using System;

namespace Tcgv.ConsensusKit.Control
{
    public class EventBinding<TValue>
    {
        public EventBinding(
            Func<TValue, TValue> filter,
            Action<TValue> action,
            EventRecurrency recurrency)
        {
            Filter = filter;
            Action = action;
            Recurrency = recurrency;
        }

        public Func<TValue, TValue> Filter { get; }
        public Action<TValue> Action { get; }
        public EventRecurrency Recurrency { get; }
    }
}
