using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class LevelPrototypes
    {
        public static List<LevelPrototype> LoadAll()
        {
            return new List<LevelPrototype>()
            {
                new LevelPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.LEVEL_1,
                    LevelIndex = 1,
                    DefaultTilesetUniqueIdentifier = UniqueIdentifier.TILESET_STONE,
                    NumberOfRooms = Random.Range(3, 5),
                },
                new LevelPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.LEVEL_2,
                    LevelIndex = 2,
                    DefaultTilesetUniqueIdentifier = UniqueIdentifier.TILESET_STONE,
                    NumberOfRooms = Random.Range(3, 5),
                },
            };
        }
    }
}
