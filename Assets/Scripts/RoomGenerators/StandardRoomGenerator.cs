namespace Gamepackage
{
    public class StandardRoomGenerator : IRoomGenerator
    {
        public int MinimumWidth { get; set; }
        public int MinimumHeight { get; set; }
        public int MaximumHeight { get; set; }
        public int MaximumWidth { get; set; }
        public Tileset Tileset { get; set; }

        public Room Generate(Level level, Rectangle rectangle)
        {
            for (var x = rectangle.Position.X; x < rectangle.Position.X + rectangle.Width; x++)
            {
                for (var y = rectangle.Position.Y; y < rectangle.Position.Y + rectangle.Height; y++)
                {
                    if (x == rectangle.Position.X || y == rectangle.Position.Y || x == rectangle.Position.X + rectangle.Width - 1 || y == rectangle.Position.Y + rectangle.Height - 1)
                    {
                        level.TilesetGrid[x, y].TileType = TileType.Wall;
                    }
                    else
                    {
                        level.TilesetGrid[x, y].TileType = TileType.Floor;
                    }
                }
            }
            return new Room()
            {
                BoundingBox = rectangle
            };
        }
    }
}
