namespace Gamepackage
{
    interface IPrototypeFactory
    {
        Token BuildToken(TokenPrototype prototype);
        Token BuildToken(string uniqueIdentifier);

        Item BuildItem(ItemPrototype prototype);
        Item BuildItem(string uniqueIdentifier);
    }
}
