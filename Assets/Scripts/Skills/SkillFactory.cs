using System.Collections.Generic;

namespace Gamepackage
{
    public class SkillFactory
    {
        public static List<Skill> BuildAll(List<string> uniqueIdentifiers)
        {
            var agg = new List<Skill>();
            foreach (var uniqueIdentifier in uniqueIdentifiers)
            {
                agg.Add(Build(uniqueIdentifier));
            }
            return agg;
        }

        public static Skill Build(string uniqueIdentifier)
        {
            if (uniqueIdentifier == null || uniqueIdentifier == "")
            {
                return null;
            }
            var skillTemplate = Context.ResourceManager.Load<SkillTemplate>(uniqueIdentifier);
            var skill = new Skill()
            {
                SkillIdentifier = uniqueIdentifier,
                ExercisedUntilTurn = -1, // So that it is not marked as exercised on the first turn
                Rank = 1,
                Xp = 0,
            };
            return skill;
        }
    }
}
