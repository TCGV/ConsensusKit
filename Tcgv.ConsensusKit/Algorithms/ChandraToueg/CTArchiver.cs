using System.Linq;
using Tcgv.ConsensusKit.Actors;

namespace Tcgv.ConsensusKit.Algorithms.ChandraToueg
{
    public class CTArchiver : Archiver
    {
        public override bool IsValidProposal(string value)
        {
            return !Query(value).Any();
        }
    }
}
