namespace Gamepackage
{
    public static class Tables
    {
        public static ProbabilityTable<UniqueIdentifier> BanditWeapons = new ProbabilityTable<UniqueIdentifier>()
        {
            Resolution = TableResolutionStrategy.OneOf,
            Values = new System.Collections.Generic.List<ProbabilityTableTuple<UniqueIdentifier>>()
            {
                 new ProbabilityTableTuple<UniqueIdentifier>()
                 {
                     NumberOfRolls = 1,
                     Value = UniqueIdentifier.ITEM_LONGSWORD,
                     Weight = 100,
                 }
            }
        };
    }
}
