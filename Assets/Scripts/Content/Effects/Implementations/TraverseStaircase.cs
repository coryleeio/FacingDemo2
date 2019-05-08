using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamepackage
{
    public class TraverseStaircase : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.traverse.stairs.name";
            }
        }

        public override string Description
        {
            get
            {
                return "effect.traverse.stairs.description";
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

        public override EntityStateChange TriggerOnPress(EntityStateChange outcome)
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
                var oldLevel = Context.Game.CurrentLevel;
                foreach (var entityInLevel in oldLevel.Entitys)
                {
                    // Add player followers to list of targets
                    // we want to bring them along when we change levels
                    if (entityInLevel.Behaviour != null && entityInLevel.Behaviour.ActingTeam == Team.PLAYER && !entityInLevel.Behaviour.IsPlayer)
                    {
                        targetsForLevelChange.Add(entityInLevel);
                    }
                }
                Context.Game.CurrentLevel.UnindexAll();
            }

            foreach (var target in targetsForLevelChange)
            {
                target.Behaviour.NextAction = null;
                if (Context.FlowSystem.Steps.First != null)
                {
                    Context.FlowSystem.Steps.First.Value.Actions.Clear();
                }

                var oldLevel = Context.Game.CurrentLevel;
                oldLevel.ReleasePathfindingAtPosition(target, target.Position);
                if (target.View.ViewGameObject != null)
                {
                    GameObject.Destroy(target.View.ViewGameObject);
                }
                var newLevel = Context.Game.Dungeon.Levels[levelId];
                var pos = new Point(posX, posY);
                oldLevel.Entitys.Remove(target);
                target.Position = pos;
                newLevel.Entitys.Add(target);
            }
            if (targetIsPlayer)
            {
                Context.PlayerController.ActionList.Clear();
                Context.Game.CurrentLevelIndex = levelId;
                if (levelId > Context.Game.FurthestLevelReached)
                {
                    Context.Game.FurthestLevelReached = levelId;
                }
                var gameSceneRoot = GameObject.FindObjectOfType<GameSceneController>();
                if (gameSceneRoot != null)
                {
                    gameSceneRoot.Stopped = true;
                }
                SceneManager.LoadScene((int) Scenes.Game);
            }
            return outcome;
        }
    }
}
