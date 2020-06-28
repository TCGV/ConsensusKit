using System;
using Tcgv.ConsensusKit.Actors;

namespace Tcgv.ConsensusKit.Algorithms.Utility
{
    public class RandomStringProposer : Proposer
    {
        public override object GetProposal()
        {
            if (base.GetProposal() == null)
                Set(GeneratePronounceableName(5));
            return base.GetProposal();
        }

        private string GeneratePronounceableName(int length)
        {
            lock (rnd)
            {
                const string vowels = "aeiou";
                const string consonants = "bcdfghjklmnpqrstvwxyz";

                var name = new char[length];

                for (var i = 0; i < length; i++)
                {
                    if (i % 2 == 0)
                        name[i] = vowels[rnd.Next(vowels.Length)];
                    else
                        name[i] = consonants[rnd.Next(consonants.Length)];
                }

                return new string(name);
            }
        }

        private static Random rnd = new Random();
    }
}
