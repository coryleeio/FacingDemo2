using System.Collections.Generic;

namespace Gamepackage
{
    public class EncounterPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public ProbabilityTable<UniqueIdentifier> ProbabilityTable;
    }
}
