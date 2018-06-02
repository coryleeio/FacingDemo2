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

        public override void InjectContext(ApplicationContext context)
        {
            base.InjectContext(context);
            if(TriggerAction != null)
            {
                TriggerAction.Entity = Entity;
                TriggerAction.InjectContext(context);
            }
            else
            {
                throw new NotImplementedException("Something is wrong, you shouldn't have a trigger component without a trigger action.");
            }
        }
    }
}
