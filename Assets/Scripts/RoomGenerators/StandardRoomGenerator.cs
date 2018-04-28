namespace Gamepackage
{
    public class StandardRoomGenerator : IRoomGenerator
    {
        public int MinimumWidth { get; set; }
        public int MinimumHeight { get; set; }
        public int MaximumHeight { get; set; }
        public int MaximumWidth { get; set; }
        public Tileset Tileset { get; set; }

        public void Generate()
        {
            throw new System.NotImplementedException();
        }
    }
}
