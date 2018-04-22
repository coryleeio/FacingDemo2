namespace Gamepackage
{
    public class EquipmentTable : IResource
    {
        public string UniqueIdentifier { get; set; }
        public ProbabilityTable<ItemPrototype> ProbabilityTable;
    }
}