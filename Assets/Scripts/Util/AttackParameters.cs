using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    // Contain the parameters specific to one type of swing possible with a weapon
    // being used in a particular way.  i.e. melee, ranged, etc.
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
