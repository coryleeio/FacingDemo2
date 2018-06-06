using System.Collections.Generic;

namespace Gamepackage
{
    public class Trigger : Component
    {
        public UniqueIdentifier TriggerActionPrototypeUniqueIdentifier;

        public TriggerAction TriggerAction;

        public Trigger() {}

        public Trigger(Trigger other)
        {
            TriggerActionPrototypeUniqueIdentifier = other.TriggerActionPrototypeUniqueIdentifier;
        }

        public override void InjectContext(Entity entity)
        {
            base.InjectContext(entity);
            if(TriggerAction != null)
            {
                TriggerAction.InjectContext(entity);
            }
            else
            {
                throw new NotImplementedException("Something is wrong, you shouldn't have a trigger component without a trigger action.");
            }
        }
    }
}
