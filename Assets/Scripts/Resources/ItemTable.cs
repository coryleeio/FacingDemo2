namespace Gamepackage
{
    public class ItemTable : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public ProbabilityTable<ItemPrototype> ProbabilityTable;
    }
}