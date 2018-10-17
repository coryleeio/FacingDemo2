using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class AttackParameters
    {
        public int DyeSize;
        public int DyeNumber;
        public int Bonus;
        public DamageTypes DamageType;
        public List<Effect> AttackSpecificEffects = new List<Effect>();
        public string AttackMessage = null;
        public UniqueIdentifier ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_NONE;
        public ExplosionParameters ExplosionParameters;

        [JsonIgnore]
        public ProjectileAppearance ProjectileAppearance
        {
            get
            {
                return Context.ResourceManager.GetPrototype<ProjectileAppearance>(ProjectileAppearanceIdentifier);
            }
        }

        public AttackParameters() { }
    }
}
