using System.Collections.Generic;

namespace Gamepackage
{
    public class Body : Component
    {
        public int CurrentHealth;
        public int MaxHealth;
        public bool IsDead = false;
        public List<AttackParameters> Attacks = new List<AttackParameters>(0);
    }
}
