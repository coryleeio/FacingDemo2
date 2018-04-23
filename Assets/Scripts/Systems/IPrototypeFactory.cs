namespace Gamepackage
{
    public interface IPrototypeFactory
    {
        Token BuildToken(TokenPrototype prototype);
        Token BuildToken(string uniqueIdentifier);

        Item BuildItem(ItemPrototype prototype);
        Item BuildItem(string uniqueIdentifier);

        void BuildGrid(Level level);
        void LoadTypes();
    }
}
