using Newtonsoft.Json;

namespace Gamepackage
{
    public class Skill
    {
        public string SkillIdentifier;
        public int Rank;
        public int Xp;
        public bool Exercised;
        public int ExercisedUntilTurn;

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
