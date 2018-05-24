using System;
using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class TokenAction : ASyncAction
    {
        public abstract int TimeCost
        {
            get;
        }

        public int TokenId;
        [JsonIgnore]
        public Token Token
        {
            get
            {
                return Context.TokenSystem.GetTokenById(TokenId);
            }
        }

        [JsonIgnore]
        public Game Game
        {
            get
            {
                return Context.GameStateManager.Game;
            }
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
            Token.TimeAccrued += TimeCost;
            Token.ActionQueue.Dequeue();
        }
    }
}
