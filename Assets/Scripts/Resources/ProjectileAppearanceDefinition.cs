using UnityEngine;

namespace Gamepackage
{
    public class ProjectileAppearanceDefinition
    {
        public GameObject Prefab;
        public float Lifetime = 0.5f;
        public ProjectileBehaviour ProjectileBehaviour;
        public float PerTileTravelTime = 0.5f;

        public ProjectileAppearanceDefinition() { }

        public GameObject Instantiate(Point position, Direction direction)
        {
            if(Prefab == null)
            {
                return null;
            }
            var prefabInstance = GameObject.Instantiate<GameObject>(Prefab);
            if(position != null)
            {
                prefabInstance.transform.position = MathUtil.MapToWorld(position);
            }
            if (ProjectileBehaviour == ProjectileBehaviour.Spin)
            {
                prefabInstance.AddComponent<ProjectileRotator>();
            }
            else if (ProjectileBehaviour == ProjectileBehaviour.FaceDirection)
            {
                prefabInstance.transform.eulerAngles = MathUtil.GetProjectileRotation(direction);
            }
            if (Lifetime > 0.0f)
            {
                prefabInstance.AddComponent<Expires>().Lifetime = Lifetime;
            }
            return prefabInstance;
        }
    }
}
