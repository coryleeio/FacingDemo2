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

        public override void Rereference(Entity entity)
        {
            base.Rereference(entity);
            if(TriggerAction != null)
            {
                TriggerAction.Rereference(entity);
            }
            else
            {
                throw new NotImplementedException("Something is wrong, you shouldn't have a trigger component without a trigger action.");
            }
        }
    }
}
