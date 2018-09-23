using UnityEngine;

namespace Gamepackage
{
    public class ItemAppearance : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }

        // Sprite shown in the inventory for this item
        // Also shown on the ground for the moment.
        public Sprite InventorySprite;

        // Projectile Prefab - Prefab for object projectile
        // OnHitPrefab - Prefab spawned on every target hit by the attack of the appropriate type(melee/ranged/thrown/zapped)
        // OnSwingPrefab - Prefab spawned when you swing the attack of the appropriate type(melee/ranged/thrown/zapped)
        // ProjectileTravelTime - Per tile travel time of attacks with this item of the appropriate type(melee/ranged/thrown/zapped)

        public GameObject MeleeProjectilePrefab;
        public GameObject MeleeOnHitPrefab;
        public GameObject MeleeOnSwingPrefab;
        public float MeleeProjectileTravelTime;

        public GameObject RangedProjectilePrefab;
        public GameObject RangedOnHitPrefab;
        public GameObject RangedOnSwingPrefab;
        public float RangedProjectileTravelTime;

        public GameObject ThrownProjectilePrefab;
        public GameObject ThrownOnHitPrefab;
        public GameObject ThrownOnSwingPrefab;
        public float ThrownProjectileTravelTime;

        public GameObject ZappedProjectilePrefab;
        public GameObject ZappedOnHitPrefab;
        public GameObject ZappedOnSwingPrefab;
        public float ZappedProjectileTravelTime;

        public bool ShouldSpinWhenThrown;
    }
}
