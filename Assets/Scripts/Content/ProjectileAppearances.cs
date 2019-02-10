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
            spriteRenderer.sortingLayerID = UnityEngine.SortingLayer.NameToID(SortingLayer.Overlays.ToString());
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
            var daggerPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Dagger"));
            var macePrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Mace"));
            var lightningPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Lightning1"));
            lightningPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
            var lightningSpriteChangerPrefab = lightningPrefab.AddComponent<SpriteChanger>();
            lightningSpriteChangerPrefab.Sprites.Add(Resources.Load<Sprite>("Sprites/Lightning1"));
            lightningSpriteChangerPrefab.Sprites.Add(Resources.Load<Sprite>("Sprites/Lightning2"));
            lightningSpriteChangerPrefab.Sprites.Add(Resources.Load<Sprite>("Sprites/Lightning3"));
            lightningSpriteChangerPrefab.timePerSprite = 0.25f;
            lightningSpriteChangerPrefab.ChangeMethod = ChangeMethodType.Random;

            var fireballPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Fireball"));
            var bigFirePrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/BigFire"));

            var potionBreakPrefab = Resources.Load<GameObject>("Prefabs/PotionBreakPrefab");
            potionBreakPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            var greenPotionPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/GreenPotion"));
            greenPotionPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            var redPotionPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/RedPotion"));
            redPotionPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            var purplePotionPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/PurplePotion"));
            purplePotionPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            var bluePotionPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/BluePotion"));
            bluePotionPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);


            var arrowPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Arrow"));
            arrowPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            var purpleBallPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/PurpleBall"));
            purpleBallPrefab.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            var coinPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/LuckyCoin"));
            coinPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            var bowPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/Bow"));
            var swirlPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/SwirlStaff"));
            var hookStaffPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/HookStaff"));
            var actionStaffPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/ActionStaff"));
            var orbScepterPrefab = BuildDefaultParticle(Resources.Load<Sprite>("Sprites/OrbScepter"));

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

            var mace = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_MACE,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = macePrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(mace);

            var dagger = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_DAGGER,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = daggerPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(dagger);

            var potionBreakDefinition = new ProjectileAppearanceDefinition()
            {
                Prefab = potionBreakPrefab,
                Lifetime = 0.6f,
                ProjectileBehaviour = ProjectileBehaviour.None,
                InheritRotation = true,
            };

            var greenPotion = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_GREEN_POTION,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = greenPotionPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin
                },
                OnHitDefinition = potionBreakDefinition
            };
            retVal.Add(greenPotion);

            var redPotion = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_RED_POTION,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = redPotionPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                },
                OnHitDefinition = potionBreakDefinition
            };
            retVal.Add(redPotion);
            var purplePotion = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_PURPLE_POTION,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = purplePotionPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                },
                OnHitDefinition = potionBreakDefinition
            };
            retVal.Add(purplePotion);
            var bluePotion = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_BLUE_POTION,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = bluePotionPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                },
                OnHitDefinition = potionBreakDefinition
            };
            retVal.Add(bluePotion);

            var arrow = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ARROW,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = arrowPrefab,
                    Lifetime = 0.0f,
                    PerTileTravelTime = 0.2f,
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

            var purpleBall = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_PURPLE_BALL,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = purpleBallPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                    PerTileTravelTime = 2.0f
                }
            };
            retVal.Add(purpleBall);

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

            var hookStaff = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_HOOK_STAFF,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = hookStaffPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(hookStaff);

            var orbScepter = new ProjectileAppearance()
            {
                UniqueIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ORB_SCEPTER,
                ProjectileDefinition = new ProjectileAppearanceDefinition()
                {
                    Prefab = orbScepterPrefab,
                    Lifetime = 0.0f,
                    ProjectileBehaviour = ProjectileBehaviour.Spin,
                }
            };
            retVal.Add(orbScepter);

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
