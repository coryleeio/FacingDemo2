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
            Entity.Behaviour.IsDoneThisTurn = true;
            if (Entity.IsPlayer)
            {
                // If the entity who ended their turn is the player we need to push the time accrued by
                // our actions to them, and change to the NPC turn.
                foreach (var entityToPushTime in Game.CurrentLevel.Entitys)
                {
                    if (entityToPushTime.IsNPC)
                    {
                        entityToPushTime.Behaviour.TimeAccrued = Entity.Behaviour.TimeAccrued;
                    }
                }
                Game.IsPlayerTurn = !Game.IsPlayerTurn;
            }
        }
    }
}
