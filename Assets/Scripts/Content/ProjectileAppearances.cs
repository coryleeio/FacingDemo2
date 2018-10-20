using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public static class ProjectileAppearances
    {
        public static GameObject BuildDefaultParticle(Sprite sprite)
        {
            GameObject myGameObject = new GameObject();
            myGameObject.name = string.Format("{0} Generated ProjectileAppearance Prefab", sprite.name);
            myGameObject.hideFlags = HideFlags.HideInHierarchy;
            var spriteRenderer = myGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 30000;
            spriteRenderer.sprite = sprite;
            myGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            var defaultMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            spriteRenderer.material = defaultMaterial;

            GameObject.DontDestroyOnLoad(myGameObject);
            myGameObject.transform.position = MathUtil.Offscreen;
            return myGameObject;
        }

        public static List<ProjectileAppearance> LoadAll()
        {
            var longswordPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Longsword"));
            var lightningPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Lightning"));
            var fireballPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Fireball"));
            var bigFirePrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/BigFire"));
            var greenPotionPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/GreenPotion"));
            var arrowPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Arrow"));
            var coinPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/LuckyCoin"));
            var bowPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Bow"));
            var swirlPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/SwirlStaff"));
            var actionStaffPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/ActionStaff"));

            var retVal = new List<ProjectileAppearance>();
            var none = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_NONE
            };
            retVal.Add(none);

            var fireball = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_FIREBALL,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = fireballPrefab,
                    Lifetime = 0.0f,
                    PerTileTravelTime = 0.25f,
                    ProjectileBehaviour = ProjectileBehaviour.FaceDirection,
                }
            };
            retVal.Add(fireball);

            var fireExplosion = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_FIRE_EXPLOSION,
                OnEnterDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = bigFirePrefab,
                    Lifetime = 0.1f,
                    PerTileTravelTime = 0.05f,
                    ProjectileBehaviour = ProjectileBehaviour.None,
                },
            };
            retVal.Add(fireExplosion);

            var lightningJet = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_LIGHTNING_JET,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = null, // null, used to specify speed of projectile
                    Lifetime = 0.0f,
                    PerTileTravelTime = 0.05f, 
                },
                OnEnterDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = lightningPrefab,
                    ProjectileBehaviour = ProjectileBehaviour.FaceDirection,
                },
            };
            retVal.Add(lightningJet);

            var longsword = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_LONGSWORD,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = longswordPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(longsword);

            var greenPotion = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_GREEN_POTION,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = greenPotionPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(greenPotion);

            var arrow = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ARROW,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = arrowPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.FaceDirection,
                }
            };
            retVal.Add(arrow);

            var arrowSpin = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ARROW_SPIN,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = arrowPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(arrowSpin);
            var coin = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_COIN,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = coinPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(coin);

            var swirlStaff = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_SWIRL_STAFF,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = swirlPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(swirlStaff);

            var actionStaff = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ACTION_STAFF,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = actionStaffPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(actionStaff);

            var bow = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_BOW,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = bowPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(bow);

            return retVal;
        }
    }
}
