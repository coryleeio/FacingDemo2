using System.Collections.Generic;

namespace Gamepackage
{
    public class TokenSystem
    {
        public GameStateManager GameStateManager { get; set; }

        public TokenSystem()
        {

        }

        private Dictionary<int, Token> TokenMap = new Dictionary<int, Token>();

        public void Register(Token token, Level level)
        {
            if(token.Id == 0)
            {
                token.Id = GameStateManager.Game.NextId;
            }
            if(!TokenMap.ContainsKey(token.Id))
            {
                TokenMap.Add(token.Id, token);
            }
            if(!level.Tokens.Contains(token))
            {
                level.Tokens.Add(token);
            }
            level.IndexToken(token, token.Position);
        }

        public void Clear()
        {
            TokenMap.Clear();
        }

        public Token GetTokenById(int id)
        {
            return TokenMap[id];
        }

        public void Deregister(Token token, Level level)
        {
            if(TokenMap.ContainsKey(token.Id))
            {
                TokenMap.Remove(token.Id);
            }
            if (level.Tokens.Contains(token))
            {
                level.Tokens.Remove(token);
            }
            level.UnindexToken(token, token.Position);
        }
    }
}