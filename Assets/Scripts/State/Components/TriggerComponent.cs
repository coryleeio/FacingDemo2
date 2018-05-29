using System.Collections.Generic;

namespace Gamepackage
{
    public class TriggerComponent : Component
    {
        public UniqueIdentifier TriggerActionPrototypeUniqueIdentifier;

        public TriggerAction TriggerAction;

        public TriggerComponent()
        {

        }

        public TriggerComponent(TriggerComponent other)
        {
            TriggerActionPrototypeUniqueIdentifier = other.TriggerActionPrototypeUniqueIdentifier;
        }

        public override void InjectContext(ApplicationContext context)
        {
            base.InjectContext(context);
            if(TriggerAction != null)
            {
                TriggerAction.InjectContext(context);
            }
            else
            {
                throw new NotImplementedException("Something is wrong, you shouldn't have a trigger component without a trigger action.");
            }
        }
    }
}
