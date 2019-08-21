using Newtonsoft.Json;

namespace Gamepackage
{
    public class Skill
    {
        public string SkillIdentifier;

        [JsonIgnore]
        public SkillTemplate Template
        {
            get
            {
                if (SkillIdentifier == null || SkillIdentifier == "")
                {
                    return null;
                }
                return Context.ResourceManager.Load<SkillTemplate>(SkillIdentifier);
            }
        }
    }
}
