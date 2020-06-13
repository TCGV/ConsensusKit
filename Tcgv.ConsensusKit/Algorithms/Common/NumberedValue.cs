namespace Tcgv.ConsensusKit.Algorithms.Common
{
    public class NumberedValue
    {
        public NumberedValue(object v, long n)
        {
            Value = v;
            Number = n;
        }

        public object Value { get; }
        public long Number { get; }
    }
}
