using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class DoTurn : IStateMachineState
    {
        public ApplicationContext ApplicationContext { get; set; }

        public void Enter()
        {

        }

        public void Process()
        {
            Token currentActor = null;
            Game game = ApplicationContext.GameStateManager.Game;
            Level level = game.CurrentLevel;
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
                    currentActor = token;
                    break;
                }
            }

            if (currentActor == null)
            {
                EndTurn(game);
            }
            if (!currentActor.IsPlayer)
            {
                // AI calculates and requeues its next action
                currentActor.ActionQueue.Clear();
                currentActor.ActionQueue.Enqueue(ApplicationContext.PrototypeFactory.BuildTokenAction<Wait>());
            }

            if (currentActor.ActionQueue.Count != 0)
            {
                // We have atleast one action to do, and our turn is not over
                var action = currentActor.ActionQueue.Peek();

                if (!action.IsComplete && !action.HasStarted)
                {
                    if (!currentActor.IsPlayer)
                    {
                        // I f we are the AI, see if we can pay the cost of the action we want to do, and end our turn if not
                        if (action.TimeCost >= currentActor.TimeAccrued)
                        {
                            currentActor.TimeAccrued = currentActor.TimeAccrued - action.TimeCost;
                        }
                        else
                        {
                            // AIs turn ends when it wants to do something, but does not have the time accrued to do it.
                            EndTurnForToken(game, currentActor);
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
                    if (currentActor.IsPlayer)
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
                    currentActor.ActionQueue.Dequeue();
                    if (currentActor.ActionQueue.Count == 0)
                    {
                        EndTurnForToken(game, currentActor);
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
                currentActor.ActionQueue.Clear();
                currentActor.ActionQueue.Enqueue(ApplicationContext.PrototypeFactory.BuildTokenAction<Wait>());
            }
        }

        private void EndTurnForToken(Game game, Token token)
        {
            Debug.Log("EndTurnForToken turn for " + token.Id + " " + token.View.gameObject.name);
            token.HasActed = true;
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
        }

        public void Exit()
        {

        }
    }
}
