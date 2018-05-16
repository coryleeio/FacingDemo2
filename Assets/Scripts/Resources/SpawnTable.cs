using System.Collections.Generic;

namespace Gamepackage
{
    public class SpawnTable : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public ProbabilityTable<UniqueIdentifier> ProbabilityTable;
        public bool Mandatory;
        public string ConstraintSpawnToRoomWithTag;
        public List<int> AvailableOnLevels = new List<int>(0);
        public bool Unique;
    }
}
