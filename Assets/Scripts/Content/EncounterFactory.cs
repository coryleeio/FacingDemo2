using System.Collections.Generic;

namespace Gamepackage
{
    public class EncounterFactory
    {
        public static List<Entity> Build(UniqueIdentifier identifier)
        {
            var retvals = new List<Entity>();
            if (identifier == UniqueIdentifier.ENCOUNTER_BEE_SWARM)
            {
                retvals.Add(Context.PrototypeFactory.BuildEntity(UniqueIdentifier.ENTITY_GIANT_BEE));
                retvals.Add(Context.PrototypeFactory.BuildEntity(UniqueIdentifier.ENTITY_GIANT_BEE));
                retvals.Add(Context.PrototypeFactory.BuildEntity(UniqueIdentifier.ENTITY_GIANT_BEE));
                retvals.Add(Context.PrototypeFactory.BuildEntity(UniqueIdentifier.ENTITY_QUEEN_BEE));
            }
            else
            {
                throw new NotImplementedException();
            }
            return retvals;
        }
    }
}
