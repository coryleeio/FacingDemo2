using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class Tile
    {
        public TileType TileType;
        public string TilesetIdentifier;
        public bool Walkable;
        public int Weight;
        public MapVisibilityState MapVisibilityState;

        [JsonIgnore]
        public Dictionary<int, List<Point>> CachedVisibilityFloodFills = new Dictionary<int, List<Point>>();

        [JsonIgnore]
        public Dictionary<int, List<Point>> CachedFloorFloodFills = new Dictionary<int, List<Point>>();

        [JsonIgnore]
        public List<Entity> EntitiesInPosition = new List<Entity>(0);

        [JsonIgnore]
        public Dictionary<SortingLayer, List<Sortable>> SortablesInPositionByLayer = new Dictionary<SortingLayer, List<Sortable>>();
    }
}
