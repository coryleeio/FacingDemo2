using System.Collections.Generic;

namespace Gamepackage
{
    public class Body : Component
    {
        public int CurrentHealth;
        public Dictionary<Attributes, int> Attributes = new Dictionary<Attributes, int>(0);
        public List<Effect> Effects = new List<Effect>();
        public bool IsDead = false;
        public int DeadForTurns = 0;
        public List<ItemSlot> UsableItemSlots = new List<ItemSlot>(0);
        public List<AttackParameters> Attacks = new List<AttackParameters>(0);
    }
}
