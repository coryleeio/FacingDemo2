namespace Gamepackage
{
    public class InventoryTable : IResource
    {
        public string UniqueIdentifier { get; set; }
        public ProbabilityTable<ItemPrototype> ProbabilityTable;
    }
}