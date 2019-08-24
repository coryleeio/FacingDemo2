using Newtonsoft.Json;
using UnityEngine;

namespace Gamepackage
{
    public class Skill
    {
        public int Rank;
        public int Xp;

        public int ExercisedUntilTurn;

        [JsonIgnore]
        public bool Exercised
        {
            get
            {
                return Context.Game.CurrentTurn <= ExercisedUntilTurn;
            }
        }

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

        [JsonIgnore]
        public string Name
        {
            get
            {
                return Template.Name.Localize();
            }
        }

        [JsonIgnore]
        public Sprite Sprite
        {
            get
            {
                return Template.Sprite;
            }
        }

        [JsonIgnore]
        public float SkillXpModifier
        {
            get
            {
                return SkillXpModifier;
            }
        }
    }
}
