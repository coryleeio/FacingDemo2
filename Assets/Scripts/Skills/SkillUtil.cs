using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public static class SkillUtil
    {
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
