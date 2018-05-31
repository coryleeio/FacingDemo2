using System.Collections.Generic;

namespace Gamepackage
{
    public class CombatantComponent : Component
    {
        public int CurrentHealth;
        public int MaxHealth;
        public bool IsDead = false;
        public float ElapsedTimeDead = 0.0f;

        public CombatantComponent()
        {

        }

        public CombatantComponent(CombatantComponent other)
        {
            CurrentHealth = other.CurrentHealth;
            MaxHealth = other.MaxHealth;
        }

        public override void InjectContext(ApplicationContext context)
        {
            base.InjectContext(context);
        }
    }
}
