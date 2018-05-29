namespace Gamepackage
{
    public class EndTurn : EntityAction
    {
        public override int TimeCost
        {
            get
            {
                return 0;
            }
        }

        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }

        public override bool IsAMovementAction
        {
            get
            {
                return false;
            }
        }

        public override bool IsStartable
        {
            get
            {
                return true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            Entity.IsDoneThisTurn = true;
            if (Entity.IsPlayer)
            {
                // If the entity who ended their turn is the player we need to push the time accrued by
                // our actions to them, and change to the NPC turn.
                foreach (var entityToPushTime in Game.CurrentLevel.Entitys)
                {
                    if (!entityToPushTime.IsPlayer)
                    {
                        entityToPushTime.CombatantComponent.TimeAccrued = Entity.CombatantComponent.TimeAccrued;
                    }
                }
                Game.IsPlayerTurn = !Game.IsPlayerTurn;
            }

        }
    }
}
