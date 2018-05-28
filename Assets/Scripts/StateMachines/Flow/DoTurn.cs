using System;
using System.Collections.Generic;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class DoTurn : IStateMachineState
    {
        public ApplicationContext Context { get; set; }
        private List<Token> TokensThatNeedToAct = new List<Token>(0);

        public void Enter()
        {

        }

        public void Process()
        {
            TokensThatNeedToAct.Clear();

            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;

            TokensThatNeedToAct.AddRange(Context.GameStateManager.Game.CurrentLevel.Tokens.FindAll((tok) =>
            {
                var isPlayerThatNeedsToAct = (game.IsPlayerTurn && tok.IsPlayer && !tok.IsDoneThisTurn);
                var isNpcThatNeedsToAct = (!game.IsPlayerTurn && !tok.IsPlayer && !tok.IsDoneThisTurn);
                return isPlayerThatNeedsToAct || isNpcThatNeedsToAct;
            }));

            // All tokens have acted, so end this turn
            if (TokensThatNeedToAct.Count == 0)
            {
                EndTurn();
            }
            else
            {
                foreach (var token in TokensThatNeedToAct)
                {
                    if (!token.IsPlayer && token.ActionQueue.Count == 0)
                    {
                        token.ActionQueue.Enqueue(Context.PrototypeFactory.BuildTokenAction<Wait>(token));
                        token.ActionQueue.Enqueue(Context.PrototypeFactory.BuildTokenAction<EndTurn>(token));
                    }
                    if(token.ActionQueue.Count > 0)
                    {
                        var action = token.ActionQueue.Peek();
                        action.Do();
                        // TokenActions dequeue themselves on exit.
                        // so it may have been dequeued by this point.
                        if (action.Completed && action.IsAMovementAction)
                        {
                            Context.FlowSystem.StateMachine.ChangeState(Context.DoTriggers);
                        }
                    }
                }
            }
        }

        public void Exit()
        {

        }

        private Token GetCurrentToken()
        {
            Game game = Context.GameStateManager.Game;
            Level level = game.CurrentLevel;
            foreach (var token in level.Tokens)
            {
                if (token.IsDoneThisTurn)
                {
                    continue;
                }
                var isPlayerThatNeedsToAct = game.IsPlayerTurn && token.IsPlayer;
                var isNPCThatNeedsToAct = !game.IsPlayerTurn && !token.IsPlayer;
                if (isPlayerThatNeedsToAct || isNPCThatNeedsToAct)
                {
                    Debug.Log("NextActor set to " + token.Id + " " + token.View.gameObject.name);
                    return token;
                }
            }
            return null;
        }

        private void EndTurn()
        {
            var game = Context.GameStateManager.Game;
            game.CurrentTurn += 1;
            Debug.Log("Turn is now: " + game.CurrentTurn);
            game.IsPlayerTurn = true;
            foreach (var token in game.CurrentLevel.Tokens)
            {
                if (token.IsPlayer)
                {
                    token.TimeAccrued = 0;
                }
                token.IsDoneThisTurn = false;
            }
        }
    }
}
