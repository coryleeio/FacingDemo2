namespace Gamepackage
{
    public class LevelPrototype : IResource
    {
        public string UniqueIdentifier { get; set; }
        public int LevelIndex { get; set; }
        public SpawnTable DefaultSpawnTable;
        public Tileset DefaultTileset;
    }
}
