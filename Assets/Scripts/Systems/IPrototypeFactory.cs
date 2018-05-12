namespace Gamepackage
{
    public interface IPrototypeFactory
    {
        Token BuildToken(TokenPrototype prototype);
        Token BuildToken(string uniqueIdentifier);

        Item BuildItem(ItemPrototype prototype);
        Item BuildItem(string uniqueIdentifier);

        void BuildMapTiles(Level level);
        void LoadTypes();
    }
}
