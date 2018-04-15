namespace Gamepackage
{
    interface IPrototypeFactory
    {
        Token Build(TokenPrototype prototype);
        Token Build(string uniqueIdentifier);
    }
}
