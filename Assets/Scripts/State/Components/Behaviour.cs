using System.Collections.Generic;

namespace Gamepackage
{
    public class Behaviour : Component
    {
        public bool IsDoneThisTurn = false;
        public int TimeAccrued = 0;
        public LinkedList<EntityAction> ActionList = new LinkedList<EntityAction>();
        public UniqueIdentifier BehaviourImplUniqueIdentifier;
        public Point LastKnownPlayerPosition;

        public Behaviour() {}

        public override void Rewire(Entity entity)
        {
            base.Rewire(entity);
            foreach (var action in ActionList)
            {
                action.Rewire(entity);
            }
        }

        public bool IsPlayer
        {
            get
            {
                return BehaviourImplUniqueIdentifier == UniqueIdentifier.BEHAVIOUR_PLAYER;
            }
        }
    }
}
