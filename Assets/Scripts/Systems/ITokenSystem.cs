namespace Gamepackage
{
    public interface ITokenSystem
    {
        void Register(Token token);
        void Clear();
        Token GetTokenById(int id);
        void Deregister(Token entity);
        void Remember(Token entity);
    }
}
