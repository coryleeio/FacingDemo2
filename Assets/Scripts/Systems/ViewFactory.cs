using System;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.Modules.AttachmentTools;
using Spine;
using UnityEngine.Assertions;
using Spine.Unity;

namespace Gamepackage
{
    public class ViewFactory
    {
        private Material DefaultSpriteMaterial;
        private Sprite MissingSprite;

        public ViewFactory()
        {
            if (DefaultSpriteMaterial == null)
            {
                DefaultSpriteMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            }
            if (MissingSprite == null)
            {
                MissingSprite = Resources.Load<Sprite>("Missing");
            }
        }

        public Entity BuildEntity(UniqueIdentifier identifier)
        {
            var entity = EntityFactory.Build(identifier);
            entity.Position = new Point(0, 0);
            return entity;
        }

        public List<Entity> BuildEncounter(UniqueIdentifier identifier)
        {
             return EncounterFactory.Build(identifier);
        }

        public void BuildView(Entity entity, bool DestroyOldView = true)
        {
            if (entity.View != null && entity.View.ViewGameObject != null && DestroyOldView)
            {
                UnityEngine.GameObject.Destroy(entity.View.ViewGameObject);
            }
            var itemsToEquip = new Dictionary<SpriteAttachment, Sprite>();
            if(entity.Inventory != null)
            {
                var equippedItemsBySlot = entity.Inventory.EquippedItemBySlot;
                foreach(var pair in equippedItemsBySlot)
                {
                    var item = pair.Value;
                    foreach(var spriteSlotPair in item.ItemAppearance.WornItemSpritePerSlot)
                    {
                        var spriteSlot = spriteSlotPair.Key;
                        var sprite = spriteSlotPair.Value;
                        itemsToEquip[spriteSlot] = sprite;
                    }
                }
            }

            var defaultMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            var go = new GameObject();
            go.name = entity.PrototypeIdentifier.ToString();
            go.transform.position = MathUtil.MapToWorld(entity.Position);
            entity.View.ViewGameObject = go;

            if(entity.IsCombatant && entity.View.ViewPrototypeUniqueIdentifier != UniqueIdentifier.VIEW_CORPSE)
            {
                var healthbarPrefab = Resources.Load<GameObject>("UI/Healthbar/Healthbar");
                var healthbarGameObject = GameObject.Instantiate(healthbarPrefab);
                healthbarGameObject.transform.SetParent(go.transform);
                healthbarGameObject.transform.localPosition = Vector3.zero;
                healthbarGameObject.GetComponent<HealthBar>().Entity = entity;
            }

            if(entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_MARKER_RED)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("RedMarker");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_MARKER_GREEN)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("GreenMarker");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_MARKER_YELLOW)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("YellowMarker");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_MARKER_BLUE)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("BlueMarker");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_STAIRCASE_UP)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/StaircaseUp");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_STAIRCASE_DOWN)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/StaircaseDown");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_HUMAN_ASIAN)
            {
                var newGo = BuildSplineView("Spine/Export/Humanoid_SkeletonData", itemsToEquip, "HumanAsian", Animations.Idle, entity.Direction);
                SetSpineDefaults(go, newGo);
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_HUMAN_WHITE)
            {
                var newGo = BuildSplineView("Spine/Export/Humanoid_SkeletonData", itemsToEquip, "HumanWhite", Animations.Idle, entity.Direction);
                SetSpineDefaults(go, newGo);
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_GHOST)
            {
                var newGo = BuildSplineView("Spine/Export/Humanoid_SkeletonData", itemsToEquip, "Ghost", Animations.Idle, entity.Direction);
                SetSpineDefaults(go, newGo);
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_HUMAN_BLACK)
            {
                var newGo = BuildSplineView("Spine/Export/Humanoid_SkeletonData", itemsToEquip, "HumanBlack", Animations.Idle, entity.Direction);
                SetSpineDefaults(go, newGo);
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_SKELETON_WHITE)
            {
                var newGo = BuildSplineView("Spine/Export/Humanoid_SkeletonData", itemsToEquip, "SkeletonWhite", Animations.Idle, entity.Direction);
                SetSpineDefaults(go, newGo);
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_BEE)
            {
                var newGo = BuildSplineView("Spine/Export/Bee_SkeletonData", itemsToEquip, "Template", Animations.Idle, entity.Direction);
                SetSpineDefaults(go, newGo);
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_STAIRCASE_DOWN)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/StaircaseDown");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_STAIRCASE_DOWN)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/StaircaseDown");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_STAIRCASE_DOWN)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/StaircaseDown");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_CORPSE)
            {
                var relevantItems = entity.Inventory.ChooseRandomItemsFromInventory(3);
                foreach (var item in relevantItems)
                {
                    var childGo = new GameObject();
                    childGo.name = item.DisplayName + "View";
                    var spriteRenderer = childGo.AddComponent<SpriteRenderer>();
                    childGo.transform.localEulerAngles = item.CorpseIconEulerAngles;
                    childGo.transform.position = item.CorpsePositionOffset;
                    childGo.transform.localScale = item.CorpseIconScale;
                    childGo.transform.SetParent(go.transform, false);
                    spriteRenderer.sprite = item.ItemAppearance.InventorySprite;
                    spriteRenderer.material = defaultMaterial;
                }
            }

            if(entity.Body != null && entity.Body.Floating && entity.View.ViewPrototypeUniqueIdentifier != UniqueIdentifier.VIEW_CORPSE)
            {
                var skeletonAnimation = go.transform.GetComponentInChildren<SkeletonAnimation>();
                var target = skeletonAnimation != null ? skeletonAnimation.gameObject : go;
                target.AddComponent<Floating>();
            }

            if (entity.IsCombatant)
            {
                foreach (var pair in entity.Inventory.EquippedItemBySlot)
                {
                    var item = pair.Value;
                    foreach (var effect in item.Effects)
                    {
                        effect.ApplyPersistentVisualEffects(entity);
                    }
                }
            }
        }

        private static void SetSpineDefaults(GameObject go, GameObject newGo)
        {
            newGo.transform.SetParent(go.transform, false);
            newGo.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            newGo.transform.localPosition = new Vector3(0, -0.30f, 0);
        }

        public void BuildMapTiles(Level level)
        {
            var folder = GameObjectUtils.MakeFolder("Grid");
            for (int x = 0; x < level.BoundingBox.Width; x++)
            {
                for (var y = 0; y < level.BoundingBox.Height; y++)
                {
                    var point = new Point(x, y);
                    var tileInfo = level.Grid[x, y];
                    var tileSet = Context.ResourceManager.GetPrototype<Tileset>(tileInfo.TilesetIdentifier);

                    if (tileInfo.TileType == TileType.Floor)
                    {
                        SpriteRenderer renderer = BuildTileSpriteRenderer(folder, tileSet.FloorSprite, point);
                    }
                    else if (tileInfo.TileType == TileType.Wall)
                    {
                        var northEastPointTileType = GetTileInfoForOffset(level, point, MathUtil.NorthEastOffset);
                        var southEastPointTileType = GetTileInfoForOffset(level, point, MathUtil.SouthEastOffset);
                        var southWestPointTileType = GetTileInfoForOffset(level, point, MathUtil.SouthWestOffset);
                        var northWestPointTileType = GetTileInfoForOffset(level, point, MathUtil.NorthWestOffset);

                        if (
                            northEastPointTileType == TileType.Wall &&
                            southEastPointTileType == TileType.Wall &&
                            southWestPointTileType == TileType.Wall &&
                            northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.TeeSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             (northWestPointTileType == TileType.Floor || northWestPointTileType == TileType.Empty)
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthEastTeeSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthEastTeeSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Floor || northEastPointTileType == TileType.Empty) &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthWestTeeSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             (southEastPointTileType == TileType.Floor || southEastPointTileType == TileType.Empty) &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthWestTeeSprite, point);
                        }

                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Floor &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Empty
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthWestWallSprite, point);
                        }

                        else if (
                            northEastPointTileType == TileType.Empty &&
                            southEastPointTileType == TileType.Wall &&
                            southWestPointTileType == TileType.Floor &&
                            northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthEastWallSprite, point);
                        }

                        // We default to SE and SW incase we later want to shorten the walls on the southern side 
                        // if we cant determine which wall should be used opt for the SE and SW ones, as they
                        // might be shorter to prevent vision from being obscured.
                        else if (
                             (northEastPointTileType == TileType.Wall || northEastPointTileType == TileType.Floor) &&
                             (southEastPointTileType == TileType.Floor || southEastPointTileType == TileType.Empty) &&
                             (southWestPointTileType == TileType.Wall || southWestPointTileType == TileType.Floor) &&
                             (northWestPointTileType == TileType.Floor || northWestPointTileType == TileType.Empty)
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthEastWallSprite, point);
                        }

                        else if (
                             (northEastPointTileType == TileType.Floor || northEastPointTileType == TileType.Empty) &&
                             (southEastPointTileType == TileType.Wall || southEastPointTileType == TileType.Floor) &&
                             (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
                             (northWestPointTileType == TileType.Wall || northWestPointTileType == TileType.Floor)
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthWestWallSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             (southWestPointTileType == TileType.Empty || southWestPointTileType == TileType.Floor) &&
                             (northWestPointTileType == TileType.Empty || northWestPointTileType == TileType.Floor)
                         )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.WestCornerSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Empty || northEastPointTileType == TileType.Floor) &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             (northWestPointTileType == TileType.Empty || northWestPointTileType == TileType.Floor)
                         )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthCornerSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Empty || northEastPointTileType == TileType.Floor) &&
                             (southEastPointTileType == TileType.Empty || southEastPointTileType == TileType.Floor) &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                         )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.EastCornerSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             (southEastPointTileType == TileType.Empty || southEastPointTileType == TileType.Floor) &&
                             (southWestPointTileType == TileType.Empty || southWestPointTileType == TileType.Floor) &&
                             northWestPointTileType == TileType.Wall
                         )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthCornerSprite, point);
                        }
                        else
                        {
                            BuildTileSpriteRenderer(folder, MissingSprite, point);
                        }
                    }
                }
            }
        }

        public static GameObject BuildSplineView(string SkeletonDataPath, Dictionary<SpriteAttachment, Sprite> spritesBySlot, string SkinName = null, Animations beginningAnimation = Animations.Idle, Direction beginningDirection = Direction.SouthEast)
        {
            if(spritesBySlot == null)
            {
                spritesBySlot = new Dictionary<SpriteAttachment, Sprite>();
            }
            Texture2D runtimeAtlas;
            Material runtimeMaterial;
            var skeletonDataAsset = Resources.Load<SkeletonDataAsset>(SkeletonDataPath);
            var skeletonData = skeletonDataAsset.GetSkeletonData(false);
            var idleAnimation = skeletonData.FindAnimation(StringUtil.GetAnimationNameForDirection(beginningAnimation, beginningDirection));
            var templateSkin = skeletonData.FindSkin("Template");
            var bodySkin = SkinName != null ? skeletonData.FindSkin(SkinName) : skeletonData.DefaultSkin;
            var skeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(skeletonDataAsset);
            skeletonAnimation.Initialize(false);

            var sourceMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            var skeleton = skeletonAnimation.Skeleton;
            var customSkin = new Skin("custom skin");

            foreach (var pair in spritesBySlot)
            {
                var attachmentKey = pair.Key;
                Sprite sprite = pair.Value;
                int spriteSlotIndex = skeleton.FindSlotIndex(SpriteSlotFromAttachment(attachmentKey));

                if(spriteSlotIndex == -1)
                {
                    Debug.LogWarning("Could not find sprite attachment on template: " + attachmentKey.ToString());
                    continue;
                }
                Attachment templateAttachment = templateSkin.GetAttachment(spriteSlotIndex, attachmentKey.ToString());
                Attachment newAttachment = templateAttachment.GetRemappedClone(sprite, sourceMaterial, true, false, true);
                RegionAttachment regionAttachment = newAttachment as RegionAttachment;
                Assert.IsNotNull(regionAttachment);
                regionAttachment.SetPositionOffset(sprite.textureRectOffset);
                regionAttachment.UpdateOffset();
                customSkin.SetAttachment(spriteSlotIndex, attachmentKey.ToString(), newAttachment);
            }

            var repackedSkin = new Skin("repacked skin");
            repackedSkin.Append(templateSkin);
            repackedSkin.Append(bodySkin);
            repackedSkin.Append(customSkin);
            repackedSkin = repackedSkin.GetRepackedSkin("repacked skin", sourceMaterial, out runtimeMaterial, out runtimeAtlas);

            skeleton.SetSkin(repackedSkin); // Assign the repacked skin to your Skeleton.
            skeleton.SetSlotsToSetupPose(); // Use the pose from setup pose.

            skeletonAnimation.AnimationState.SetAnimation(0, idleAnimation, true);
            skeletonAnimation.Update(0); // Use the pose in the currently active animation.
            Resources.UnloadUnusedAssets();
            return skeletonAnimation.gameObject;
        }

        private static TileType GetTileInfoForOffset(Level level, Point position, Point offset)
        {
            var offsetPoint = MathUtil.GetPointByOffset(position, offset);
            if (!level.BoundingBox.Contains(offsetPoint))
            {
                return TileType.Empty;
            }
            return level.Grid[offsetPoint.X, offsetPoint.Y].TileType;
        }

        private SpriteRenderer BuildTileSpriteRenderer(GameObject folder, Sprite sprite, Point position)
        {
            GameObject o = new GameObject();
            o.name = "Tile";
            var renderer = o.AddComponent<SpriteRenderer>();
            renderer.material = DefaultSpriteMaterial;
            o.transform.SetParent(folder.transform);
            renderer.sprite = sprite;
            o.transform.localPosition = MathUtil.MapToWorld(position);
            Context.SpriteSortingSystem.RegisterTile(renderer, position);
            return renderer;
        }

        private static string SpriteSlotFromAttachment(SpriteAttachment attachment)
        {
            if (attachment == SpriteAttachment.HelmetBackSE || attachment == SpriteAttachment.HelmetBackNE)
            {
                return SpriteSlot.HelmetBack.ToString();
            }
            else if (attachment == SpriteAttachment.HelmetFrontSE || attachment == SpriteAttachment.HelmetFrontNE)
            {
                return SpriteSlot.HelmetFront.ToString();
            }
            else if (attachment == SpriteAttachment.ChestBackSE || attachment == SpriteAttachment.ChestBackNE)
            {
                return SpriteSlot.ChestBack.ToString();
            }
            else if (attachment == SpriteAttachment.ChestFrontSE || attachment == SpriteAttachment.ChestFrontNE)
            {
                return SpriteSlot.ChestFront.ToString();
            }
            else if (attachment == SpriteAttachment.LeftArm)
            {
                return SpriteSlot.LeftArm.ToString();
            }
            else if (attachment == SpriteAttachment.LeftArmBackSE || attachment == SpriteAttachment.LeftArmBackNE)
            {
                return SpriteSlot.LeftArmBack.ToString();
            }
            else if (attachment == SpriteAttachment.LeftArmFrontSE || attachment == SpriteAttachment.LeftArmFrontNE)
            {
                return SpriteSlot.LeftArmFront.ToString();
            }
            else if (attachment == SpriteAttachment.LeftLeg)
            {
                return SpriteSlot.LeftLeg.ToString();
            }
            else if (attachment == SpriteAttachment.LeftLegBackSE || attachment == SpriteAttachment.LeftLegBackNE)
            {
                return SpriteSlot.LeftLegBack.ToString();
            }
            else if (attachment == SpriteAttachment.LeftLegFrontSE || attachment == SpriteAttachment.LeftLegFrontNE)
            {
                return SpriteSlot.LeftLegFront.ToString();
            }
            else if (attachment == SpriteAttachment.RightArm)
            {
                return SpriteSlot.RightArm.ToString();
            }
            else if (attachment == SpriteAttachment.RightArmBackSE || attachment == SpriteAttachment.RightArmBackNE)
            {
                return SpriteSlot.RightArmBack.ToString();
            }
            else if (attachment == SpriteAttachment.RightArmFrontSE || attachment == SpriteAttachment.RightArmFrontNE)
            {
                return SpriteSlot.RightArmFront.ToString();
            }
            else if (attachment == SpriteAttachment.RightLeg)
            {
                return SpriteSlot.RightLeg.ToString();
            }
            else if (attachment == SpriteAttachment.RightLegBackSE || attachment == SpriteAttachment.RightLegBackNE)
            {
                return SpriteSlot.RightLegBack.ToString();
            }
            else if (attachment == SpriteAttachment.RightLegFrontSE || attachment == SpriteAttachment.RightLegFrontNE)
            {
                return SpriteSlot.RightLegFront.ToString();
            }
            else if (attachment == SpriteAttachment.RightLegFrontSE || attachment == SpriteAttachment.RightLegFrontNE)
            {
                return SpriteSlot.RightLegFront.ToString();
            }
            else if (attachment == SpriteAttachment.MainHandBack)
            {
                return SpriteSlot.MainHandBack.ToString();
            }
            else if (attachment == SpriteAttachment.MainHandFront)
            {
                return SpriteSlot.MainHandFront.ToString();
            }
            else if (attachment == SpriteAttachment.OffHandFront)
            {
                return SpriteSlot.OffHandFront.ToString();
            }
            else if (attachment == SpriteAttachment.OffHandBack)
            {
                return SpriteSlot.OffHandBack.ToString();
            }
            else return attachment.ToString();
        }
    }
}

