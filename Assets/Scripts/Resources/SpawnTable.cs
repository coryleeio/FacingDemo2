namespace Gamepackage
{
    public class SpawnTable : IResource
    {
        public string UniqueIdentifier { get; set; }
        public ProbabilityTable<TokenPrototype> ProbabilityTable;
    }
}
