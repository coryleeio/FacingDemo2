using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public static class Tilesets
    {
        public static List<Tileset> LoadAll()
        {
            return new List<Tileset>()
            {
                new Tileset()
                {
                    UniqueIdentifier = UniqueIdentifier.TILESET_STONE,
                    FloorSprite=Resources.Load<Sprite>("Sprites/StoneFloor"),
                    TeeSprite=Resources.Load<Sprite>("Sprites/StoneTee"),
                    NorthCornerSprite=Resources.Load<Sprite>("Sprites/StoneCornerNorth"),
                    EastCornerSprite=Resources.Load<Sprite>("Sprites/StoneCornerEast"),
                    SouthCornerSprite=Resources.Load<Sprite>("Sprites/StoneCornerSouth"),
                    WestCornerSprite=Resources.Load<Sprite>("Sprites/StoneCornerWest"),
                    NorthEastWallSprite=Resources.Load<Sprite>("Sprites/StoneNorthEastWall"),
                    SouthEastWallSprite=Resources.Load<Sprite>("Sprites/StoneSouthEastWall"),
                    SouthWestWallSprite=Resources.Load<Sprite>("Sprites/StoneSouthWestWall"),
                    NorthWestWallSprite=Resources.Load<Sprite>("Sprites/StoneNorthWestWall"),
                    NorthEastTeeSprite=Resources.Load<Sprite>("Sprites/StoneNorthEastTee"),
                    SouthEastTeeSprite=Resources.Load<Sprite>("Sprites/StoneSouthEastTee"),
                    SouthWestTeeSprite=Resources.Load<Sprite>("Sprites/StoneSouthWestTee"),
                    NorthWestTeeSprite=Resources.Load<Sprite>("Sprites/StoneNorthWestTee"),
                },
            };
        }
    }
}
