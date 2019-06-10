using System.Collections.Generic;

namespace Gamepackage
{
    public class EncounterFactory
    {
        public static List<Entity> Build(string identifier)
        {
            var retvals = new List<Entity>();
            if (identifier == "ENCOUNTER_BEE_SWARM")
            {
                retvals.Add(EntityFactory.Build("ENTITY_GIANT_BEE"));
                retvals.Add(EntityFactory.Build("ENTITY_GIANT_BEE"));
                retvals.Add(EntityFactory.Build("ENTITY_ANIMATED_WEAPON"));
                retvals.Add(EntityFactory.Build("ENTITY_GHOST"));
                retvals.Add(EntityFactory.Build("ENTITY_QUEEN_BEE"));
            }
            else if (identifier == "ENCOUNTER_SKELETONS")
            {
                retvals.Add(EntityFactory.Build("ENTITY_SKELETON"));
                retvals.Add(EntityFactory.Build("ENTITY_SKELETON"));
                retvals.Add(EntityFactory.Build("ENTITY_SKELETON"));
                retvals.Add(EntityFactory.Build("ENTITY_SKELETON"));
            }
            else
            {
                throw new NotImplementedException();
            }
            return retvals;
        }
    }
}
