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
                retvals.Add(Context.ViewFactory.BuildEntity(UniqueIdentifier.ENTITY_GIANT_BEE));
                retvals.Add(Context.ViewFactory.BuildEntity(UniqueIdentifier.ENTITY_GIANT_BEE));
                retvals.Add(Context.ViewFactory.BuildEntity(UniqueIdentifier.ENTITY_ANIMATED_WEAPON));
                retvals.Add(Context.ViewFactory.BuildEntity(UniqueIdentifier.ENTITY_GHOST));
                retvals.Add(Context.ViewFactory.BuildEntity(UniqueIdentifier.ENTITY_QUEEN_BEE));
            }
            else if (identifier == UniqueIdentifier.ENCOUNTER_SKELETONS)
            {
                retvals.Add(Context.ViewFactory.BuildEntity(UniqueIdentifier.ENTITY_SKELETON));
                retvals.Add(Context.ViewFactory.BuildEntity(UniqueIdentifier.ENTITY_SKELETON));
                retvals.Add(Context.ViewFactory.BuildEntity(UniqueIdentifier.ENTITY_SKELETON));
                retvals.Add(Context.ViewFactory.BuildEntity(UniqueIdentifier.ENTITY_SKELETON));
            }
            else
            {
                throw new NotImplementedException();
            }
            return retvals;
        }
    }
}
