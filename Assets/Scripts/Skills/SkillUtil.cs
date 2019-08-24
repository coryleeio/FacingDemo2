using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public static class SkillUtil
    {
        public static void ExerciseSkills(Entity entity, List<string> SkillIdentifiers, int NumberOfTurns)
        {
            foreach(var identifier in SkillIdentifiers)
            {
                ExerciseSkill(entity, identifier, NumberOfTurns);
            }
        }

        public static void ExerciseSkill(Entity entity, string SkillIdentifier, int NumberOfTurns)
        {
            if(SkillIdentifier == null)
            {
                return;
            }
            if (entity.SkillsByIdentifier.ContainsKey(SkillIdentifier))
            {
                Debug.Log("Exercised " + SkillIdentifier);
                entity.SkillsByIdentifier[SkillIdentifier].ExercisedUntilTurn = Context.Game.CurrentTurn + NumberOfTurns;
            }
        }

        public static List<Skill> ExercisedSkills(Entity entity)
        {
            var ret = new List<Skill>();
            foreach (var skill in entity.SkillsByIdentifier.Values)
            {
                if (skill.Exercised)
                {
                    ret.Add(skill);
                }
            }
            return ret;
        }

        public static void PopulateSkills(Entity entity)
        {
            var allSkills = Context.ResourceManager.LoadAll<SkillTemplate>();
            PopulateSkills(allSkills, entity);
        }

        private static void PopulateSkills(List<SkillTemplate> skills, Entity entity)
        {
            foreach (var skill in skills)
            {
                if (!entity.SkillsByIdentifier.ContainsKey(skill.Identifier))
                {
                    var newSkill = SkillFactory.Build(skill.Identifier);
                    entity.SkillsByIdentifier.Add(newSkill.SkillIdentifier, newSkill);
                }
            }
        }
    }
}
