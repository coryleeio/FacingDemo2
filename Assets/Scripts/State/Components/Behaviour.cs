using System.Collections.Generic;

namespace Gamepackage
{
    public class Behaviour : Component
    {
        public bool IsDoneThisTurn = false;
        public int TimeAccrued = 0;
        public LinkedList<EntityAction> ActionList = new LinkedList<EntityAction>();
        public UniqueIdentifier BehaviourImplUniqueIdentifier;
        public BehaviourImpl BehaviourImpl;

        public Behaviour() {}

        public Behaviour(Behaviour other)
        {
            this.IsDoneThisTurn = other.IsDoneThisTurn;
            this.TimeAccrued = other.TimeAccrued;
            this.BehaviourImplUniqueIdentifier = other.BehaviourImplUniqueIdentifier;
        }

        public override void InjectContext(Entity entity)
        {
            base.InjectContext(entity);
            if(BehaviourImpl != null)
            {
                BehaviourImpl.InjectContext(entity);
            }
            foreach (var action in ActionList)
            {
                action.InjectContext(entity);
            }
        }
    }
}
