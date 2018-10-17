using System.Collections.Generic;

namespace Gamepackage
{
    public class ExplosionParameters : AttackParameters
    {
        public int Radius;
        public List<Effect> AppliedEffects = new List<Effect>();
    }
}
