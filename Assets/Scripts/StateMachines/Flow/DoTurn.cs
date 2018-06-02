using System;
using System.Collections.Generic;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class DoTurn : IStateMachineState
    {
        public ApplicationContext Context { get; set; }
        private List<Entity> EntityListCopy = new List<Entity>(0);
        private List<Entity> NotDoneList = new List<Entity>(0);

        public void Enter()
        {

        }

        public void Process()
        {
            EntityListCopy.Clear();
            NotDoneList.Clear();

            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;

            foreach(var entity in level.Entitys)
            {
                if(entity.Behaviour == null)
                {
                    continue;
                }

                var isPlayerOnPlayerTurn = entity.IsPlayer && game.IsPlayerTurn && !entity.Behaviour.IsDoneThisTurn;
                var isNpcOnNpcTurn = entity.IsNPC && !game.IsPlayerTurn && !entity.Behaviour.IsDoneThisTurn;
                
                if ((isPlayerOnPlayerTurn || isNpcOnNpcTurn) || (entity.Behaviour.ActionList.Count > 0 && entity.Behaviour.ActionList.First.Value.IsImmediate))
                {
                    NotDoneList.Add(entity);
                }
                EntityListCopy.Add(entity);
            }

            // All entities have acted, so end this turn
            if (NotDoneList.Count == 0)
            {
                EndTurn();
            }
            else
            {
                // We go through ALL entities, not just the ones that need to act this turn to deal with slides
                // and other actions assigned to you as part of another characters action.
                foreach (var entity in EntityListCopy)
                {
                    if(entity.Behaviour == null)
                    {
                        continue;
                    }
                    if (entity.IsNPC && entity.Behaviour.ActionList.Count == 0 && !entity.Behaviour.IsDoneThisTurn)
                    {
                        entity.Behaviour.BehaviourImpl.GetActionsForTurn();
                    }
                    if(entity.Behaviour.ActionList.Count > 0 && (!entity.Behaviour.IsDoneThisTurn || entity.Behaviour.ActionList.First.Value.IsImmediate))
                    {
                        var action = entity.Behaviour.ActionList.First.Value;
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

        private void EndTurn()
        {
            var game = Context.GameStateManager.Game;
            game.CurrentTurn += 1;
            Debug.Log("Turn is now: " + game.CurrentTurn);
            game.IsPlayerTurn = true;
            foreach (var entity in game.CurrentLevel.Entitys)
            {
                if(entity.Behaviour != null)
                {
                    if (entity.IsPlayer)
                    {
                        entity.Behaviour.TimeAccrued = 0;
                    }
                    entity.Behaviour.IsDoneThisTurn = false;
                }
            }
        }
    }
}
