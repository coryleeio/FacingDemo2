namespace Gamepackage
{
    public class AIController
    {
        public void GetActionsForTurn(Entity entity)
        {
            var behaviour = entity.Behaviour;
            var game = ServiceLocator.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;

            if (behaviour.BehaviourImplUniqueIdentifier == UniqueIdentifier.BEHAVIOUR_BRUTE)
            {
                if (!ServiceLocator.VisibilitySystem.CanSee(level, entity, player))
                {
                    if (behaviour.LastKnownPlayerPosition != null && Point.Distance(entity.Position, behaviour.LastKnownPlayerPosition) > 2)
                    {
                        ServiceLocator.CombatSystem.TryToMoveToward(entity, behaviour.LastKnownPlayerPosition);
                    }
                    else
                    {
                        behaviour.LastKnownPlayerPosition = null;
                        ServiceLocator.CombatSystem.Wait(entity);
                    }
                }
                else
                {
                    behaviour.LastKnownPlayerPosition = new Point(player.Position.X, player.Position.Y);
                    if (ServiceLocator.CombatSystem.CanMelee(entity, player))
                    {
                        ServiceLocator.CombatSystem.AttackInMelee(entity, player);
                    }
                    else
                    {
                        ServiceLocator.CombatSystem.TryToMoveToward(entity, player.Position);
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
