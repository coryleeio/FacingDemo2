using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class TraverseStaircase : TriggerAction
    {
        public enum Params
        {
            TARGET_POSX,
            TARGET_POSY,
            TARGET_LEVEL_ID,
        }

        public Dictionary<string, string> Parameters = new Dictionary<string, string>(0);
        public override void Enter()
        {
            base.Enter();
            var levelId = Convert.ToInt32(Parameters[Params.TARGET_LEVEL_ID.ToString()]);
            var posX = Convert.ToInt32(Parameters[Params.TARGET_POSX.ToString()]);
            var posY = Convert.ToInt32(Parameters[Params.TARGET_POSY.ToString()]);
            var targetIncludesPlayer = false;
            foreach (var target in Targets)
            {
                if(target.IsPlayer)
                {
                    targetIncludesPlayer = true;
                }
                var oldLevel = ServiceLocator.GameStateManager.Game.CurrentLevel;
                if (target.Behaviour != null)
                {
                    target.Behaviour.ActionList.Clear();
                    if(target.EntityPrototype.BlocksPathing)
                    {
                        oldLevel.TilesetGrid[target.Position].Walkable = true;
                    }
                }
                if(target.View.ViewGameObject != null)
                {
                    GameObject.Destroy(target.View.ViewGameObject);
                }
                var newLevel = ServiceLocator.GameStateManager.Game.Dungeon.Levels[levelId];
                var pos = new Point(posX, posY);
                ServiceLocator.EntitySystem.Deregister(target, oldLevel);
                target.Position = pos;
            }
            if(targetIncludesPlayer)
            {
                ServiceLocator.GameStateManager.Game.CurrentLevelIndex = levelId;
                ServiceLocator.GameStateManager.Game.FurthestLevelReached = levelId;
                ServiceLocator.Application.StateMachine.ChangeState(ApplicationStateMachine.GamePlayState);
            }
        }

        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }

        private List<Point> _offsets = new List<Point>() { new Point(0, 0) };
        public override List<Point> Offsets
        {
            get
            {
                return _offsets;

            }
        }

        public override bool IsStartable
        {
            get
            {
                return true;
            }
        }
    }
}
