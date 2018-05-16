using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    FloorSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneFloor"),
                    TeeSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneTee"),
                    NorthCornerSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneCornerNorth"),
                    EastCornerSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneCornerEast"),
                    SouthCornerSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneCornerSouth"),
                    WestCornerSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneCornerWest"),
                    NorthEastWallSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneNorthEastWall"),
                    SouthEastWallSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneSouthEastWall"),
                    SouthWestWallSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneSouthWestWall"),
                    NorthWestWallSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneNorthWestWall"),
                    NorthEastTeeSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneNorthEastTee"),
                    SouthEastTeeSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneSouthEastTee"),
                    SouthWestTeeSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneSouthWestTee"),
                    NorthWestTeeSprite=Resources.Load<Sprite>("Tilesets/Stone/StoneNorthWestTee"),
                },
            };
        }
    }
}
