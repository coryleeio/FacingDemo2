namespace Gamepackage
{
    public class RoomPrototype : IResource
    {
        public string UniqueIdentifier { get; set; }
        public IRoomGenerator RoomGenerator { get; set; }
    }
}
