using System.Collections.Generic;

namespace Gamepackage
{
    public class Brute : BehaviourImpl
    {
        protected override void SetActionsForThisTurn()
        {
            var game = ServiceLocator.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;

            if (!ServiceLocator.VisibilitySystem.CanSee(level, Entity, player))
            {
                ServiceLocator.CombatSystem.Wait(Entity);
            }
            else
            {
                if (ServiceLocator.CombatSystem.CanMelee(Entity, player))
                {
                    ServiceLocator.CombatSystem.AttackInMelee(Entity, player);
                }
                else
                {
                    ServiceLocator.CombatSystem.TryToMoveToward(Entity, player);
                }
            }
        }
    }
}
