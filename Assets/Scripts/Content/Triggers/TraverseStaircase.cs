using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class TraverseStaircase : Trigger
    {
        public enum Params
        {
            TARGET_POSX,
            TARGET_POSY,
            TARGET_LEVEL_ID,
        }

        public override void Enter()
        {
            base.Enter();
            var levelId = Convert.ToInt32(Parameters[Params.TARGET_LEVEL_ID.ToString()]);
            var posX = Convert.ToInt32(Parameters[Params.TARGET_POSX.ToString()]);
            var posY = Convert.ToInt32(Parameters[Params.TARGET_POSY.ToString()]);
            foreach (var target in Targets)
            {
                var newLevel = Context.GameStateManager.Game.Dungeon.Levels[levelId];
                var pos = new Point(posX, posY);
                Context.EntitySystem.Deregister(target, Context.GameStateManager.Game.CurrentLevel);
                target.Position = pos;
                Context.EntitySystem.Register(target, newLevel);
                target.CombatantComponent.ActionQueue.Clear();
            }
            Context.GameStateManager.Game.CurrentLevelIndex = levelId;
            Context.GameStateManager.Game.FurthestLevelReached = levelId;
           Context.Application.StateMachine.ChangeState(Context.GamePlayState);
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
