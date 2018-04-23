using UnityEngine;

namespace Gamepackage
{
    public class Tileset : IResource
    {
        public string UniqueIdentifier { get; set; }

        public Sprite FloorSprite;
        public Sprite TeeSprite;
        public Sprite NorthCornerSprite;
        public Sprite EastCornerSprite;
        public Sprite SouthCornerSprite;
        public Sprite WestCornerSprite;
        public Sprite NorthEastWallSprite;
        public Sprite SouthEastWallSprite;
        public Sprite SouthWestWallSprite;
        public Sprite NorthWestWallSprite;
        public Sprite NorthEastTeeSprite;
        public Sprite SouthEastTeeSprite;
        public Sprite SouthWestTeeSprite;
        public Sprite NorthWestTeeSprite;
    }
}
