using System.Collections.Generic;

namespace Gamepackage
{
    public class Trigger : Component
    {
        public UniqueIdentifier TriggerActionPrototypeUniqueIdentifier;

        public TriggerAction TriggerAction;

        public Trigger() {}

        public override void Rewire(Entity entity)
        {
            base.Rewire(entity);
            if(TriggerAction != null)
            {
                TriggerAction.Rewire(entity);
            }
            else
            {
                throw new NotImplementedException("Something is wrong, you shouldn't have a trigger component without a trigger action.");
            }
        }
    }
}
