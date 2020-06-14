using System.Linq;
using Tcgv.ConsensusKit.Actors;

namespace Tcgv.ConsensusKit.Algorithms.Utility
{
    public class UniqueArchiver : Archiver
    {
        public override bool CanCommit(object value)
        {
            return !Query(value).Any();
        }
    }
}
