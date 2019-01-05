using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class TraverseStaircase : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.traverse.stairs.name".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "effect.traverse.stairs.description".Localize();
            }
        }

        public enum Params
        {
            TARGET_POSX,
            TARGET_POSY,
            TARGET_LEVEL_ID,
        }

        public override bool CanTriggerOnPress()
        {
            return true;
        }

        public override ActionOutcome TriggerOnPress(ActionOutcome outcome)
        {
            var foundPlayer = false;
            if (outcome.Target != null && outcome.Target.IsPlayer)
            {
                foundPlayer = true;
            }
            if (!foundPlayer)
            {
                return outcome;
            }

            var Parameters = outcome.Source.Trigger.TriggerParameters;
            var levelId = Convert.ToInt32(Parameters[Params.TARGET_LEVEL_ID.ToString()]);
            var posX = Convert.ToInt32(Parameters[Params.TARGET_POSX.ToString()]);
            var posY = Convert.ToInt32(Parameters[Params.TARGET_POSY.ToString()]);
            var targetIsPlayer = false;
            if (outcome.Target != null && outcome.Target.IsPlayer)
            {
                targetIsPlayer = true;
            }

            var targetsForLevelChange = new List<Entity>
            {
                outcome.Target
            };

            if (targetIsPlayer)
            {
                var oldLevel = Context.GameStateManager.Game.CurrentLevel;
                foreach (var entityInLevel in oldLevel.Entitys)
                {
                    // Add player followers to list of targets
                    // we want to bring them along when we change levels
                    if (entityInLevel.Behaviour != null && entityInLevel.Behaviour.ActingTeam == Team.PLAYER && !entityInLevel.Behaviour.IsPlayer)
                    {
                        targetsForLevelChange.Add(entityInLevel);
                    }
                }
                Context.GameStateManager.Game.CurrentLevel.UnindexAll();
            }

            foreach (var target in targetsForLevelChange)
            {
                target.Behaviour.NextAction = null;
                if (Context.FlowSystem.Steps.First != null)
                {
                    Context.FlowSystem.Steps.First.Value.Actions.Clear();
                }

                var oldLevel = Context.GameStateManager.Game.CurrentLevel;
                if (target.BlocksPathing)
                {
                    oldLevel.Grid[target.Position].Walkable = true;
                }
                if (target.View.ViewGameObject != null)
                {
                    GameObject.Destroy(target.View.ViewGameObject);
                }
                var newLevel = Context.GameStateManager.Game.Dungeon.Levels[levelId];
                var pos = new Point(posX, posY);
                oldLevel.Entitys.Remove(target);
                target.Position = pos;
                newLevel.Entitys.Add(target);
            }
            if (targetIsPlayer)
            {
                Context.PlayerController.ActionList.Clear();
                Context.GameStateManager.Game.CurrentLevelIndex = levelId;
                if (levelId > Context.GameStateManager.Game.FurthestLevelReached)
                {
                    Context.GameStateManager.Game.FurthestLevelReached = levelId;
                }
                Context.Application.StateMachine.ChangeState(ApplicationStateMachine.GamePlayState);
            }
            return outcome;
        }
    }
}
