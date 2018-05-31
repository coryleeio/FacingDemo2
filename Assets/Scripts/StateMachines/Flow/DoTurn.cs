using System;
using System.Collections.Generic;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class DoTurn : IStateMachineState
    {
        public ApplicationContext Context { get; set; }
        private List<Entity> EntitysThatNeedToAct = new List<Entity>(0);

        public void Enter()
        {

        }

        public void Process()
        {
            EntitysThatNeedToAct.Clear();

            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;

            EntitysThatNeedToAct.AddRange(Context.GameStateManager.Game.CurrentLevel.Entitys.FindAll((tok) =>
            {
                var isPlayerThatNeedsToAct = tok.IsPlayer && (game.IsPlayerTurn && !tok.TurnComponent.IsDoneThisTurn);
                var isNpcThatNeedsToAct = tok.IsNPC && (!game.IsPlayerTurn && !tok.TurnComponent.IsDoneThisTurn);
                return isPlayerThatNeedsToAct || isNpcThatNeedsToAct;
            }));

            // All entities have acted, so end this turn
            if (EntitysThatNeedToAct.Count == 0)
            {
                EndTurn();
            }
            else
            {
                foreach (var entity in EntitysThatNeedToAct)
                {
                    
                    if (!entity.IsPlayer && entity.TurnComponent.ActionQueue.Count == 0)
                    {
                        entity.TurnComponent.ActionQueue.Enqueue(Context.PrototypeFactory.BuildEntityAction<Wait>(entity));
                        entity.TurnComponent.ActionQueue.Enqueue(Context.PrototypeFactory.BuildEntityAction<EndTurn>(entity));
                    }
                    if(entity.TurnComponent.ActionQueue.Count > 0)
                    {
                        var action = entity.TurnComponent.ActionQueue.Peek();
                        action.Do();
                        // EntityActions dequeue themselves on exit.
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

        private Entity GetCurrentEntity()
        {
            Game game = Context.GameStateManager.Game;
            Level level = game.CurrentLevel;
            foreach (var entity in level.Entitys)
            {
                if (entity.TurnComponent == null || entity.TurnComponent.IsDoneThisTurn)
                {
                    continue;
                }
                var isPlayerThatNeedsToAct = game.IsPlayerTurn && entity.IsPlayer;
                var isNPCThatNeedsToAct = !game.IsPlayerTurn && !entity.IsPlayer;
                if (isPlayerThatNeedsToAct || isNPCThatNeedsToAct)
                {
                    return entity;
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
            foreach (var entity in game.CurrentLevel.Entitys)
            {
                if(entity.TurnComponent != null)
                {
                    if (entity.IsPlayer)
                    {
                        entity.TurnComponent.TimeAccrued = 0;
                    }
                    entity.TurnComponent.IsDoneThisTurn = false;
                }
            }
        }
    }
}
