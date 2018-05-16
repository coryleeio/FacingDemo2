namespace Gamepackage
{
    public class InventoryTable : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public ProbabilityTable<ItemPrototype> ProbabilityTable;
    }
}