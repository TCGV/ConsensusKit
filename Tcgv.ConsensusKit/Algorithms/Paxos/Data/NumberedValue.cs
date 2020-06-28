namespace Tcgv.ConsensusKit.Algorithms.Paxos.Data
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

        public override string ToString()
        {
            return $"(v = {Value}, n = {Number})";
        }
    }
}
