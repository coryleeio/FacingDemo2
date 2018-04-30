using System.Collections.Generic;

namespace Gamepackage
{
    public class RoomPrototype : IResource
    {
        public string UniqueIdentifier { get; set; }
        public IRoomGenerator RoomGenerator { get; set; }
        public List<string> Tags = new List<string>(0);
        public List<int> AvailableOnLevels = new List<int>(0);
        public bool Mandatory;
        public bool Unique;
    }
}
