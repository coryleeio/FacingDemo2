using System.Collections.Generic;

namespace Gamepackage
{
    public class LevelPrototype : IResource
    {
        public string UniqueIdentifier { get; set; }
        public SpawnTable DefaultSpawnTable;
        public Tileset DefaultTileset;
        public List<LevelRoomPrototype> Rooms = new List<LevelRoomPrototype>(0);
        public List<LevelSpawnPrototype> Spawns = new List<LevelSpawnPrototype>(0);
    }
}
