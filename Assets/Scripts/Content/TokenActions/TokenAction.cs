using System;
using Newtonsoft.Json;
using UnityEngine;

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
            if(Token.ActionQueue.Contains(this))
            {
                Token.ActionQueue.Dequeue();
            }
        }

        public abstract bool IsAMovementAction
        {
            get;
        }

        public override void FailToStart()
        {
            base.FailToStart();
            Token.ActionQueue.Clear();
        }
    }
}
