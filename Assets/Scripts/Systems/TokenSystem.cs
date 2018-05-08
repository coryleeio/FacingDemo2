using System.Collections.Generic;

namespace Gamepackage
{
    public class TokenSystem : ITokenSystem
    {
        public IGameStateSystem GameStateSystem { get; set; }

        public TokenSystem()
        {

        }

        private Dictionary<int, Token> TokenMap = new Dictionary<int, Token>();

        public void Register(Token token)
        {
            if(token.Id == 0)
            {
                token.Id = GameStateSystem.Game.IdManager.NextId;
            }
            if(!TokenMap.ContainsKey(token.Id))
            {
                TokenMap.Add(token.Id, token);
            }
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
    }
}