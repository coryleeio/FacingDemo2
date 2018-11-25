using System.Collections.Generic;

namespace Gamepackage
{
    public static class Tables
    {
        public static List<UniqueIdentifier> HumanoidWeapons = new List<UniqueIdentifier>()
        {
            UniqueIdentifier.ITEM_DAGGER,
            UniqueIdentifier.ITEM_LONGSWORD,
            UniqueIdentifier.ITEM_MACE,
            UniqueIdentifier.ITEM_POISON_DAGGER,
        };

        public static ProbabilityTable<UniqueIdentifier> HumanoidClothing = new ProbabilityTable<UniqueIdentifier>()
        {
            Resolution = TableResolutionStrategy.AnyOf,
            Values = new System.Collections.Generic.List<ProbabilityTableTuple<UniqueIdentifier>>()
            {
                 new ProbabilityTableTuple<UniqueIdentifier>()
                 {
                     NumberOfRolls = 1,
                     Value = UniqueIdentifier.ITEM_ROBE_OF_WONDERS,
                     Weight = 25,
                 }
            }
        };
    }
}
