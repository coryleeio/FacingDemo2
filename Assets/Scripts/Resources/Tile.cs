using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class Tile
    {
        public TileType TileType;
        public UniqueIdentifier TilesetIdentifier;
        public bool Walkable;
        public int Weight;
        public MapVisibilityState MapVisibilityState;

        [JsonIgnore]
        public Dictionary<int, List<Point>> CachedFloodFills = new Dictionary<int, List<Point>>();

        [JsonIgnore]
        public List<Entity> EntitiesInPosition = new List<Entity>(0);
    }
}
