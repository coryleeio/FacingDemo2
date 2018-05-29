using System.Collections.Generic;

namespace Gamepackage
{
    public class CombatantComponent : Component
    {
        public int CurrentHealth;
        public int MaxHealth;
        public bool IsDead = false;
        public float ElapsedTimeDead = 0.0f;
        public int TimeAccrued = 0;
        public Queue<EntityAction> ActionQueue = new Queue<EntityAction>(0);

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
            foreach (var action in ActionQueue)
            {
                action.InjectContext(Context);
            }
        }
    }
}
