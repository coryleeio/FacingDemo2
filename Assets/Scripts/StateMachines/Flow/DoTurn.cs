using System;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class DoTurn : IStateMachineState
    {
        public ApplicationContext Context { get; set; }

        public void Enter()
        {

        }

        public void Process()
        {
            Token currentActor = GetCurrentToken();
            
            // All tokens have acted, so end this turn
            if (currentActor == null)
            {
                EndTurn();
            }
            else
            {
                if (currentActor.ActionQueue.Count == 0)
                {
                    // Simulating user and AI input...
                    currentActor.ActionQueue.Enqueue(Context.PrototypeFactory.BuildTokenAction<Wait>(currentActor));
                    currentActor.ActionQueue.Enqueue(Context.PrototypeFactory.BuildTokenAction<EndTurn>(currentActor));
                }

                if (currentActor.ActionQueue.Count != 0)
                {
                    // TokenActions dequeue themselves on exit.
                    var action = currentActor.ActionQueue.Peek();
                    action.Do();
                    if (action.Completed && action.GetType() == typeof(Move))
                    {
                        Context.FlowSystem.StateMachine.ChangeState(Context.DoTriggers);
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
            foreach(var token in game.CurrentLevel.Tokens)
            {
                if(token.IsPlayer)
                {
                    token.TimeAccrued = 0;
                }
                token.IsDoneThisTurn = false;
            }
        }
    }
}
