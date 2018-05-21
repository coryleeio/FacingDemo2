using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class TurnSystem
    {
        public GameStateManager GameStateManager { get; set; }
        public StateMachine StateMachine { get; set; }
        private Token NextActor;
        private TokenAction StartedAction;
        public TinyIoCContainer Container { get; set; }

        public void Enter()
        {
            NextActor = null;
        }

        public void Process()
        {
            Game game = GameStateManager.Game;
            Level level = game.CurrentLevel;
            if (NextActor == null)
            {
                foreach (var token in level.Tokens)
                {
                    if (token.HasActed)
                    {
                        continue;
                    }
                    var isPlayerThatNeedsToAct = game.IsPlayerTurn && token.IsPlayer;
                    var isNPCThatNeedsToAct = !game.IsPlayerTurn && !token.IsPlayer;
                    if (isPlayerThatNeedsToAct || isNPCThatNeedsToAct)
                    {
                        Debug.Log("NextActor set to " + token.Id + " " + token.View.gameObject.name);
                        NextActor = token;
                        break;
                    }
                }
                if (NextActor == null)
                {
                    EndTurn(game);
                }
            }
            else
            {
                if (!NextActor.IsPlayer)
                {
                    // AI calculates and requeues its next action
                    NextActor.ActionQueue.Clear();
                    NextActor.ActionQueue.Enqueue(Container.Resolve<Wait>());
                }

                if (NextActor.ActionQueue.Count != 0)
                {
                    // We have atleast one action to do(the player may have multiple, the AI has 1)
                    var action = NextActor.ActionQueue.Peek();

                    if (!action.HasStarted)
                    {
                        if (!NextActor.IsPlayer)
                        {
                            // I f we are the AI, see if we can pay the cost of the action we want to do, and end our turn if not
                            if (action.TimeCost >= NextActor.TimeAccrued)
                            {
                                NextActor.TimeAccrued = NextActor.TimeAccrued - action.TimeCost;
                            }
                            else
                            {
                                // AIs turn ends when it wants to do something, but does not have the time accrued to do it.
                                EndTurnForToken(game, NextActor);
                                return;
                            }
                        }

                        action.HasStarted = true;
                        action.Enter();
                    }
                    if (action.IsComplete)
                    {
                        action.Exit();

                        // The turn is over, if the token is a NPC it may have multiple moves queued up though
                        if (NextActor.IsPlayer)
                        {
                            // If it's a player we are done, but we need to push time to all npcs
                            foreach (var tokenToPushTime in level.Tokens)
                            {
                                if (!tokenToPushTime.IsPlayer)
                                {
                                    tokenToPushTime.TimeAccrued = tokenToPushTime.TimeAccrued + action.TimeCost;
                                    // Mark non players as having not acted for the next turn we are about to switch to.
                                    tokenToPushTime.HasActed = false;
                                }
                            }
                        }
                        NextActor.ActionQueue.Dequeue();
                        if (NextActor.ActionQueue.Count == 0)
                        {
                            EndTurnForToken(game, NextActor);
                            return;
                        }
                    }
                    else
                    {
                        // Action is still underway
                        action.Process();
                    }
                }
                else
                {
                    // we are still waiting for the player to fill in their input, they have to fill in an input that they can't yet complete
                    Debug.Log("Waiting for input");
                    NextActor.ActionQueue.Clear();
                    NextActor.ActionQueue.Enqueue(Container.Resolve<Wait>());
                }
            }
        }

        private void EndTurnForToken(Game game, Token token)
        {
            Debug.Log("EndTurnForToken turn for " + token.Id + " " + token.View.gameObject.name);
            NextActor.HasActed = true;
            NextActor = null;
            if (token.IsPlayer)
            {
                game.IsPlayerTurn = !game.IsPlayerTurn;
            }
        }

        private void EndTurn(Game game)
        {
            game.CurrentTurn += 1;
            Debug.Log("Turn is now: " + game.CurrentTurn);
            game.IsPlayerTurn = true;
            game.CurrentLevel.Player.HasActed = false;
            NextActor = null;
        }
    }
}
