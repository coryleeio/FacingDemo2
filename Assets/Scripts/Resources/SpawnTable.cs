using System.Collections.Generic;

namespace Gamepackage
{
    public class SpawnTable : IResource
    {
        public string UniqueIdentifier { get; set; }
        public ProbabilityTable<TokenPrototype> ProbabilityTable;
        public bool Mandatory;
        public string ConstraintSpawnToRoomWithTag;
        public List<int> AvailableOnLevels = new List<int>(0);
    }
}
