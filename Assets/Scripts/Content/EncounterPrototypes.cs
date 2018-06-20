using System.Collections.Generic;

namespace Gamepackage
{
    public class EncounterPrototypes
    {
        public static List<EncounterPrototype> LoadAll()
        {
            return new List<EncounterPrototype>()
            {
                new EncounterPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.ENCOUNTER_BEE_SWARM,
                    ProbabilityTable = new ProbabilityTable<UniqueIdentifier>()
                    {
                       Resolution = TableResolutionStrategy.AnyOf,
                       Values = new List<ProbabilityTableTuple<UniqueIdentifier>>()
                       {
                           new ProbabilityTableTuple<UniqueIdentifier>()
                           {
                               Weight = 100,
                               NumberOfRolls = 3,
                               Value = UniqueIdentifier.TOKEN_GIANT_BEE
                           },
                           new ProbabilityTableTuple<UniqueIdentifier>()
                           {
                               Weight = 100,
                               NumberOfRolls = 1,
                               Value = UniqueIdentifier.TOKEN_QUEEN_BEE
                           }
                       }
                    }
                },
            };
        }
    }
}
