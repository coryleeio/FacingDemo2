using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamepackage
{
    public class ChangeLevel : ITriggerableActionImpl
    {
        public string ActionNameLocalizationKey
        {
            get
            {
                return "Change level";
            }
        }

        public enum Params
        {
            TARGET_POSX,
            TARGET_POSY,
            TARGET_LEVEL_ID,
        }

        public void Perform(Entity TriggerThatWentOff, Entity CauseOfTrigger, Dictionary<string, string> Data)
        {
            var levelId = Convert.ToInt32(Data[Params.TARGET_LEVEL_ID.ToString()]);
            var posX = Convert.ToInt32(Data[Params.TARGET_POSX.ToString()]);
            var posY = Convert.ToInt32(Data[Params.TARGET_POSY.ToString()]);
            var targetIsPlayer = false;
            if (CauseOfTrigger != null && CauseOfTrigger.IsPlayer)
            {
                targetIsPlayer = true;
            }

            var targetsForLevelChange = new List<Entity>
            {
                CauseOfTrigger
            };

            if (targetIsPlayer)
            {
                var oldLevel = Context.Game.CurrentLevel;
                foreach (var entityInLevel in oldLevel.Entitys)
                {
                    // Add player followers to list of targets
                    // we want to bring them along when we change levels
                    if (entityInLevel.IsCombatant && entityInLevel.ActingTeam == Team.PLAYER && !entityInLevel.IsPlayer)
                    {
                        targetsForLevelChange.Add(entityInLevel);
                    }
                }
                Context.Game.CurrentLevel.UnindexAll();
            }

            foreach (var target in targetsForLevelChange)
            {
                target.NextAction = null;
                if (Context.FlowSystem.Steps.First != null)
                {
                    Context.FlowSystem.Steps.First.Value.Actions.Clear();
                }

                var oldLevel = Context.Game.CurrentLevel;
                oldLevel.ReleasePathfindingAtPosition(target, target.Position);
                if (target.ViewGameObject != null)
                {
                    GameObject.Destroy(target.ViewGameObject);
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
                SceneManager.LoadScene((int)Scenes.Game);
            }
        }

        public bool CanPerform(Entity TriggerThatWentOff, Entity CauseOfTrigger, Dictionary<string, string> Data)
        {
            return true;
        }
    }
}
