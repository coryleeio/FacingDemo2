namespace Gamepackage
{
    public interface IPrototypeFactory
    {
        Token BuildToken(TokenPrototype prototype, int levelIndex, Point position);
        Token BuildToken(string uniqueIdentifier, int levelIndex, Point position);

        Item BuildItem(ItemPrototype prototype);
        Item BuildItem(string uniqueIdentifier);

        void LoadTypes();
    }
}
