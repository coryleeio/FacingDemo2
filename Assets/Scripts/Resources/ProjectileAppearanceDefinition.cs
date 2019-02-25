using UnityEngine;

namespace Gamepackage
{
    public class ProjectileAppearanceDefinition
    {
        public GameObject Prefab;
        public float Lifetime = 0.5f;
        public bool InheritRotation = false;
        public ProjectileBehaviour ProjectileBehaviour;
        public float PerTileTravelTime = 0.5f;
    }
}
