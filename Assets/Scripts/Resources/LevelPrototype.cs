namespace Gamepackage
{
    public class LevelPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public int LevelIndex;
        public int NumberOfRooms;
        public UniqueIdentifier DefaultTilesetUniqueIdentifier;
    }
}
