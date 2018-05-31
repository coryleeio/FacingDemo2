using System.Collections.Generic;

namespace Gamepackage
{
    public class TurnComponent : Component
    {
        public bool IsDoneThisTurn = false;
        public int TimeAccrued = 0;
        public Queue<EntityAction> ActionQueue = new Queue<EntityAction>(0);
        public UniqueIdentifier BehaviourUniqueIdentifier;
        public Behaviour Behaviour;

        public TurnComponent()
        {

        }

        public TurnComponent(TurnComponent other)
        {
            this.IsDoneThisTurn = other.IsDoneThisTurn;
            this.TimeAccrued = other.TimeAccrued;
            this.BehaviourUniqueIdentifier = other.BehaviourUniqueIdentifier;
        }

        public override void InjectContext(ApplicationContext context)
        {
            base.InjectContext(context);
            if(Behaviour != null)
            {
                Behaviour.InjectContext(context);
            }
            foreach (var action in ActionQueue)
            {
                action.InjectContext(Context);
            }
        }
    }
}
