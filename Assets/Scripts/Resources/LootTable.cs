namespace Gamepackage
{
    public class LootTable : IResource
    {
        public string UniqueIdentifier { get; set; }
        public ProbabilityTable<ItemPrototype> ProbabilityTable;
    }
}
