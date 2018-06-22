using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Gamepackage
{
    public class TraverseStaircase : Ability
    {
        public enum Params
        {
            TARGET_POSX,
            TARGET_POSY,
            TARGET_LEVEL_ID,
        }

        public override int TimeCost
        {
            get
            {
                return 0;
            }
        }

        [JsonIgnore]
        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }

        [JsonIgnore]
        public override bool CanPerform
        {
            get
            {
                foreach(var target in Targets)
                {
                    if(target.IsPlayer)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override void Exit()
        {
            base.Exit();
            var Parameters = Source.Trigger.TriggerParameters;
            var levelId = Convert.ToInt32(Parameters[Params.TARGET_LEVEL_ID.ToString()]);
            var posX = Convert.ToInt32(Parameters[Params.TARGET_POSX.ToString()]);
            var posY = Convert.ToInt32(Parameters[Params.TARGET_POSY.ToString()]);
            var targetIncludesPlayer = false;
            foreach (var target in Targets)
            {
                if (target.IsPlayer)
                {
                    targetIncludesPlayer = true;
                }
                var oldLevel = ServiceLocator.GameStateManager.Game.CurrentLevel;
                if (target.BlocksPathing)
                {
                    oldLevel.Grid[target.Position].Walkable = true;
                }
                if (target.View.ViewGameObject != null)
                {
                    GameObject.Destroy(target.View.ViewGameObject);
                }
                var newLevel = ServiceLocator.GameStateManager.Game.Dungeon.Levels[levelId];
                var pos = new Point(posX, posY);
                oldLevel.Entitys.Remove(target);
                target.Position = pos;
                newLevel.Entitys.Add(target);
            }
            if (targetIncludesPlayer)
            {
                ServiceLocator.PlayerController.ActionList.Clear();
                ServiceLocator.GameStateManager.Game.CurrentLevelIndex = levelId;
                if(levelId > ServiceLocator.GameStateManager.Game.FurthestLevelReached)
                {
                    ServiceLocator.GameStateManager.Game.FurthestLevelReached = levelId;
                }
                ServiceLocator.Application.StateMachine.ChangeState(ApplicationStateMachine.GamePlayState);
            }
        }
    }
}
