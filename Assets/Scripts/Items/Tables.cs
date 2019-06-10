using System.Collections.Generic;

namespace Gamepackage
{
    public static class Tables
    {
        public static List<string> HumanoidWeapons = new List<string>()
        {
            "ITEM_DAGGER",
            "ITEM_LONGSWORD",
            "ITEM_MACE",
            "ITEM_FANG_OF_JAHABI",
            "ITEM_SHORTBOW",
        };

        public static List<string> RandomArrows = new List<string>()
        {
            "ITEM_ARROW",
        };

        public static ProbabilityTable<string> HumanoidClothing = new ProbabilityTable<string>()
        {
            Resolution = TableResolutionStrategy.AnyOf,
            Values = new System.Collections.Generic.List<ProbabilityTableTuple<string>>()
            {
                 new ProbabilityTableTuple<string>()
                 {
                     NumberOfRolls = 1,
                     Value = "ITEM_ROBE_OF_WONDERS",
                     Weight = 25,
                 }
            }
        };
    }
}
