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

        public override void InjectContext(ApplicationContext context)
        {
            base.InjectContext(context);
            if(BehaviourImpl != null)
            {
                BehaviourImpl.Entity = Entity;
                BehaviourImpl.InjectContext(context);
            }
            foreach (var action in ActionList)
            {
                action.Entity = Entity;
                action.InjectContext(Context);
            }
        }
    }
}
