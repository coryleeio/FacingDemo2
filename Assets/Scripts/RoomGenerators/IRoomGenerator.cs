namespace Gamepackage
{ 
    public interface IRoomGenerator
    {
        int MinimumWidth {  get; set; }
        int MinimumHeight { get; set; }
        int MaximumHeight { get; set; }
        int MaximumWidth { get; set; }
        Tileset Tileset { get; set; }
        Room Generate(Level level, Rectangle rectangle);
    }
}
