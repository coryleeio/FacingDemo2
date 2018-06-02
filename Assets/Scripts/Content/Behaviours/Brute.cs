using System.Collections.Generic;

namespace Gamepackage
{
    public class Brute : BehaviourImpl
    {
        protected override void SetActionsForThisTurn()
        {
            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;

            if (!Context.VisibilitySystem.CanSee(level, Entity, player))
            {
                Context.CombatSystem.Wait(Entity);
            }
            else
            {
                if (Context.CombatSystem.CanMelee(Entity, player))
                {
                    Context.CombatSystem.AttackInMelee(Entity, player);
                }
                else
                {
                    Context.CombatSystem.TryToMoveToward(Entity, player);
                }
            }
        }
    }
}
