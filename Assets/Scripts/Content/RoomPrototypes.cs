using System.Collections.Generic;

namespace Gamepackage
{
    public class RoomPrototypes
    {
        public static List<RoomPrototype> LoadAll()
        {
            return new List<RoomPrototype>()
            {
                new RoomPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.ROOM_STANDARD_STONE_ROOM,
                    AvailableOnLevels = new List<int>(){1,2,3},
                    Mandatory = false,
                    RoomGenerator = new StandardRoomGenerator()
                    {
                        TilesetUniqueIdentifier = UniqueIdentifier.TILESET_STONE,
                        MinimumHeight = 5,
                        MaximumHeight = 9,
                        MinimumWidth = 5,
                        MaximumWidth = 9,
                    }
                }
            };
        }
    }
}
