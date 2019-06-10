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

        public static Dictionary<string, ProjectileAppearanceTemplate> LoadAll()
        {
            var retVal = new Dictionary<string, ProjectileAppearanceTemplate>();
            var none = new ProjectileAppearanceTemplate()
            {
                Identifier = "PROJECTILE_APPEARANCE_NONE",
                ProjectileAppearanceComponents = new Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent>()
            };
            retVal.Add(none.Identifier, none);

            var generate = new ProjectileAppearanceTemplate()
            {
                Identifier = "PROJECTILE_APPEARANCE_AUTO",
                ProjectileAppearanceComponents = new Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent>()
                {
                    { ProjectileComponentTypes.Projectile, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){},
                        ScaleX = 0.5f,
                        ScaleY = 0.5f,
                        ScaleZ = 0.5f,
                        TotalTimeVisible = 0.0f,
                        TimeSpentInEachTile = 0.25f,
                        SpriteFacingBehavior = SpriteFacingBehavior.Spin,
                    }},
                }
            };
            retVal.Add(generate.Identifier, generate);

            var fireball = new ProjectileAppearanceTemplate()
            {
                Identifier = "PROJECTILE_APPEARANCE_FIREBALL",
                ProjectileAppearanceComponents = new Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent>()
                {
                    { ProjectileComponentTypes.Projectile, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){
                            Resources.Load<Sprite>("Sprites/Fireball"),
                        },
                        ScaleX = 0.5f,
                        ScaleY = 0.5f,
                        ScaleZ = 0.5f,
                        TotalTimeVisible = 0.0f,
                        TimeSpentInEachTile = 0.25f,
                        SpriteFacingBehavior = SpriteFacingBehavior.FaceDirection,
                    }},
                }
            };
            retVal.Add(fireball.Identifier, fireball);

            var fireExplosion = new ProjectileAppearanceTemplate()
            {
                Identifier = "PROJECTILE_APPEARANCE_FIRE_EXPLOSION",
                ProjectileAppearanceComponents = new Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent>()
                {
                    { ProjectileComponentTypes.OnEnter, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){
                            Resources.Load<Sprite>("Sprites/BigFire"),
                        },
                        ScaleX = 0.5f,
                        ScaleY = 0.5f,
                        ScaleZ = 0.5f,
                        TotalTimeVisible = 0.1f,
                        TimeSpentInEachTile = 0.05f,
                        SpriteFacingBehavior = SpriteFacingBehavior.None,
                    }},
                }
            };
            retVal.Add(fireExplosion.Identifier, fireExplosion);

            var lightningJet = new ProjectileAppearanceTemplate()
            {
                Identifier = "PROJECTILE_APPEARANCE_LIGHTNING_JET",
                ProjectileAppearanceComponents = new Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent>()
                {
                    { ProjectileComponentTypes.Projectile, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){}, // empty, used to specify speed of projectile
                        TotalTimeVisible = 0.0f,
                        TimeSpentInEachTile = 0.05f,
                    }},
                    { ProjectileComponentTypes.OnEnter, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){
                            Resources.Load<Sprite>("Sprites/Lightning1"),
                            Resources.Load<Sprite>("Sprites/Lightning2"),
                            Resources.Load<Sprite>("Sprites/Lightning3"),
                        },
                        ScaleX = 1f,
                        ScaleY = 1f,
                        ScaleZ = 1f,
                        AnimationTimeSpentShowingEachSprite = 0.3f,
                        TotalTimeVisible = .7f,
                        AnimationChangeType = ChangeMethodType.Random,
                        SpriteFacingBehavior = SpriteFacingBehavior.FaceDirection,
                    }},
                },
            };
            retVal.Add(lightningJet.Identifier, lightningJet);


            var brokenFlask = new ProjectileAppearanceTemplate()
            {
                Identifier = "PROJECTILE_APPEARANCE_BROKEN_FLASK",
                ProjectileAppearanceComponents = new Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent>()
                {
                    { ProjectileComponentTypes.OnHit, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){
                                    Resources.Load<Sprite>("Sprites/PotionBreak1"),
                                    Resources.Load<Sprite>("Sprites/PotionBreak2"),
                                    Resources.Load<Sprite>("Sprites/PotionBreak3"),
                                },
                        AnimationTimeSpentShowingEachSprite = 0.3f,
                        AnimationChangeType = ChangeMethodType.Linear,
                        ScaleX = 0.3f,
                        ScaleY = 0.3f,
                        ScaleZ = 0.3f,
                        TotalTimeVisible = 0.6f,
                        SpriteFacingBehavior = SpriteFacingBehavior.None,
                        InheritRotation = true,
                    }},
                    { ProjectileComponentTypes.Projectile, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){}, // projectile defined with no sprites will use inventory sprite
                        ScaleX = 0.3f,
                        ScaleY = 0.3f,
                        ScaleZ = 0.3f,
                        TotalTimeVisible = 0.0f,
                        TimeSpentInEachTile = 0.25f,
                        SpriteFacingBehavior = SpriteFacingBehavior.Spin,
                    }},
                },
            };
            retVal.Add(brokenFlask.Identifier, brokenFlask);


            var arrow = new ProjectileAppearanceTemplate()
            {
                Identifier = "PROJECTILE_APPEARANCE_ARROW",
                ProjectileAppearanceComponents = new Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent>()
                {
                    { ProjectileComponentTypes.Projectile, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){
                            Resources.Load<Sprite>("Sprites/Arrow"),
                        },
                        ScaleX = 0.3f,
                        ScaleY = 0.3f,
                        ScaleZ = 0.3f,
                        TotalTimeVisible = 0.0f,
                        TimeSpentInEachTile = 0.2f,
                        SpriteFacingBehavior = SpriteFacingBehavior.FaceDirection,
                    }},
                },
            };
            retVal.Add(arrow.Identifier, arrow);

            var arrowSpin = new ProjectileAppearanceTemplate()
            {
                Identifier = "PROJECTILE_APPEARANCE_ARROW_SPIN",
                ProjectileAppearanceComponents = new Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent>()
                {
                    { ProjectileComponentTypes.Projectile, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){
                            Resources.Load<Sprite>("Sprites/Arrow"),
                        },
                        ScaleX = 0.3f,
                        ScaleY = 0.3f,
                        ScaleZ = 0.3f,
                        TotalTimeVisible = 0.0f,
                        SpriteFacingBehavior = SpriteFacingBehavior.Spin,
                    }},
                },
            };
            retVal.Add(arrowSpin.Identifier, arrowSpin);

            var purpleBall = new ProjectileAppearanceTemplate()
            {
                Identifier = "PROJECTILE_APPEARANCE_PURPLE_BALL",
                ProjectileAppearanceComponents = new Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent>()
                {
                    { ProjectileComponentTypes.Projectile, new ProjectileAppearanceTemplateComponent()
                    {
                        Sprites = new List<Sprite>(){
                            Resources.Load<Sprite>("Sprites/PurpleBall"),
                        },
                        ScaleX = 0.7f,
                        ScaleY = 0.7f,
                        ScaleZ = 0.7f,
                        TotalTimeVisible = 0.0f,
                        SpriteFacingBehavior = SpriteFacingBehavior.Spin,
                        TimeSpentInEachTile = 2.0f
                    }},
                },
            };
            retVal.Add(purpleBall.Identifier, purpleBall);
            return retVal;
        }
    }
}
