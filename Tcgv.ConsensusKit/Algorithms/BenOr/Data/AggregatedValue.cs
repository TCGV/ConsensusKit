namespace Tcgv.ConsensusKit.Algorithms.BenOr.Data
{
    public class AggregatedValue
    {
        public AggregatedValue(object value, int count)
        {
            Value = value;
            Count = count;
        }

        public object Value { get; }
        public int Count { get; }
    }
}
