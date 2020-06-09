using System;

namespace Tcgv.ConsensusKit.Control
{
    public class EventBinding<TValue>
    {
        public EventBinding(Action<TValue> action, EventRecurrency recurrency)
        {
            Action = action;
            Recurrency = recurrency;
        }

        public Action<TValue> Action { get; }
        public EventRecurrency Recurrency { get; }
    }
}
