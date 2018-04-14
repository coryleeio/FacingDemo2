using System.Collections.Generic;

namespace Gamepackage
{
    public class TokenSystem
    {

        public TokenSystem()
        {

        }

        public Dictionary<int, Token> TokenMap = new Dictionary<int, Token>();

        public int NextId = 0;


        public void Register(Token token)
        {
            RegisterWithoutIncrement(token);
            NextId++;
        }

        private void RegisterWithoutIncrement(Token token)
        {
            TokenMap.Add(token.id, token);
        }

        public void Clear()
        {
            TokenMap.Clear();
        }

        public Token GetTokenById(int id)
        {
            return TokenMap[id];
        }

        public void Deregister(Token entity)
        {
            if(TokenMap.ContainsKey(entity.id))
            {
                TokenMap.Remove(entity.id);
            }
        }

        public void Remember(Token entity)
        {
            if (entity.id > NextId)
            {
                NextId = entity.id + 1;
            }
            RegisterWithoutIncrement(entity);
        }
    }
}