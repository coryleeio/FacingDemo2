using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class Body : Component
    {
        public int CurrentHealth;
        public Dictionary<Attributes, int> Attributes = new Dictionary<Attributes, int>(0);
        public List<Effect> Effects = new List<Effect>();
        public bool IsDead = false;
        public bool Floating = false;
        public List<ItemSlot> UsableItemSlots = new List<ItemSlot>(0);
        public AttackTypeParameters MeleeAttackTypeParameters;

        public GameObject MeleeProjectilePrefab;
        public GameObject MeleeOnHitPrefab;
        public GameObject MeleeOnSwingPrefab;
        public float MeleeProjectileTravelTime;

        public bool CanAttackInMelee
        {
            get
            {
                return MeleeAttackTypeParameters != null && MeleeAttackTypeParameters.AttackParameters.Count > 0;
            }
        }
    }
}
