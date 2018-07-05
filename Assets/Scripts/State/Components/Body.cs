using System.Collections.Generic;

namespace Gamepackage
{
    public class Body : Component
    {
        public int CurrentHealth;
        public Dictionary<Attributes, int> Attributes = new Dictionary<Attributes, int>(0);
        public List<Ability> Abilities = new List<Ability>();
        public bool IsDead = false;
        public List<ItemSlot> UsableItemSlots = new List<ItemSlot>(0);
        public List<AttackParameters> Attacks = new List<AttackParameters>(0);

        public int CalculateValueOfAttribute(Attributes attr)
        {
            var totalVal = Attributes[attr];

            if (Entity != null && Entity.Inventory != null)
            {
                foreach (var pair in Entity.Inventory.EquippedItemBySlot)
                {
                    foreach (var attribute in pair.Value.Attributes)
                    {
                        if (attribute.Key == attr)
                        {
                            totalVal += attribute.Value;
                        }
                    }
                }
            }

            return totalVal;
        }
    }
}
