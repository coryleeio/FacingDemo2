using System.Collections.Generic;

namespace Gamepackage
{
    public class TokenSystem : ITokenSystem
    {
        public TokenSystem()
        {

        }

        private Dictionary<int, Token> TokenMap = new Dictionary<int, Token>();
        private int NextId = 0;

        public void Register(Token token)
        {
            RegisterWithoutIncrement(token);
            NextId++;
        }

        private void RegisterWithoutIncrement(Token token)
        {
            TokenMap.Add(token.Id, token);
        }

        public void Clear()
        {
            TokenMap.Clear();
        }

        public Token GetTokenById(int id)
        {
            return TokenMap[id];
        }

        public void Deregister(Token token)
        {
            if(TokenMap.ContainsKey(token.Id))
            {
                TokenMap.Remove(token.Id);
            }
        }

        public void Remember(Token token)
        {
            if (token.Id > NextId)
            {
                NextId = token.Id + 1;
            }
            RegisterWithoutIncrement(token);
        }
    }
}