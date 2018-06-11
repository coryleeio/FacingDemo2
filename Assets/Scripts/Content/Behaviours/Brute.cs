using System.Collections.Generic;

namespace Gamepackage
{
    public class Brute : BehaviourImpl
    {
        public Point LastKnownPlayerPosition;

        protected override void SetActionsForThisTurn()
        {
            var game = ServiceLocator.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;

            if (!ServiceLocator.VisibilitySystem.CanSee(level, Entity, player))
            {
                if(LastKnownPlayerPosition != null && Point.Distance(Entity.Position, LastKnownPlayerPosition) > 2)
                {
                    ServiceLocator.CombatSystem.TryToMoveToward(Entity, LastKnownPlayerPosition);
                }
                else
                {
                    LastKnownPlayerPosition = null;
                    ServiceLocator.CombatSystem.Wait(Entity);
                }
            }
            else
            {
                LastKnownPlayerPosition = new Point(player.Position.X, player.Position.Y);
                if (ServiceLocator.CombatSystem.CanMelee(Entity, player))
                {
                    ServiceLocator.CombatSystem.AttackInMelee(Entity, player);
                }
                else
                {
                    ServiceLocator.CombatSystem.TryToMoveToward(Entity, player.Position);
                }
            }
        }
    }
}
