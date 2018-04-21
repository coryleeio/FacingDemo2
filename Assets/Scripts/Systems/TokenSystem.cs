using System.Collections.Generic;

namespace Gamepackage
{
    public class TokenSystem : ITokenSystem
    {
        IGameStateSystem _gameStateSystem;
        public TokenSystem(IGameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;
        }

        private Dictionary<int, Token> TokenMap = new Dictionary<int, Token>();

        public void Register(Token token)
        {
            if(token.Id == 0)
            {
                token.Id = _gameStateSystem.Game.IdManager.NextId;
            }
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
    }
}