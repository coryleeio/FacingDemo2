using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.Modules.AttachmentTools;
using Spine;
using UnityEngine.Assertions;
using Spine.Unity;

namespace Gamepackage
{
    public static class ViewFactory
    {
        private static Material GetDefaultSpriteMaterial()
        {
            return Resources.Load<Material>("Materials/DefaultSpriteMaterial");
        }

        private static Sprite MissingSpriteMaterial()
        {
            return Resources.Load<Sprite>("SpritesManual/Missing");
        }

        public static  void RebuildView(Entity entity, bool DestroyOldView = true)
        {
            if (entity.View != null && entity.View.ViewGameObject != null && DestroyOldView)
            {
                UnityEngine.GameObject.Destroy(entity.View.ViewGameObject);
            }
            var itemsToEquip = new Dictionary<SpriteAttachment, Sprite>();
            if (entity.Inventory != null)
            {
                var equippedItemsBySlot = entity.Inventory.EquippedItemBySlot;
                foreach (var pair in equippedItemsBySlot)
                {
                    var item = pair.Value;
                    foreach (var spriteSlotPair in item.ItemAppearance.WornItemSpritePerSlot)
                    {
                        var spriteSlot = spriteSlotPair.Key;
                        var sprite = spriteSlotPair.Value;
                        itemsToEquip[spriteSlot] = sprite;
                    }
                }
            }

            var defaultMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");

            var go = BuildDefaultGameObject(entity);
            var sortable = BuildDefaultSortable(entity);

            if (entity.IsCombatant && entity.View.ViewPrototypeUniqueIdentifier != UniqueIdentifier.VIEW_CORPSE)
            {
                var healthbarPrefab = Resources.Load<GameObject>("Prefabs/UI/Healthbar");
                var healthbarGameObject = GameObject.Instantiate(healthbarPrefab);
                healthbarGameObject.transform.SetParent(Context.UIController.gameObject.transform, false);
                var healthBarComponent = healthbarGameObject.GetComponent<HealthBar>();
                healthBarComponent.Entity = entity;
                entity.View.HealthBar = healthBarComponent;
            }

            if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_MARKER_RED)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("SpritesManual/RedMarker");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_MARKER_GREEN)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("SpritesManual/GreenMarker");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_MARKER_YELLOW)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("SpritesManual/YellowMarker");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_MARKER_BLUE)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("SpritesManual/BlueMarker");
                spriteRenderer.material = defaultMaterial;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_CHESS_PIECE)
            {
                var pref = Resources.Load<GameObject>("Prefabs/ChessPiecePrefab");
                var ch = GameObject.Instantiate<GameObject>(pref);
                ch.transform.SetParent(go.transform, false);
                ch.transform.localPosition = Vector3.zero;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_RUG)
            {
                var pref = Resources.Load<GameObject>("Prefabs/RugPrefab");
                var ch = GameObject.Instantiate<GameObject>(pref);
                ch.transform.SetParent(go.transform, false);
                ch.transform.localPosition = Vector3.zero;
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_STAIRCASE_UP)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/StaircaseUp");
                spriteRenderer.material = defaultMaterial;
                sortable.Weight = 0; // Behind walking combating entities, but still in the entity layer bc it rises up in front of walls behind it.
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_STAIRCASE_DOWN)
            {
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/StaircaseDown");
                spriteRenderer.material = defaultMaterial;
                sortable.Weight = 0;  // Behind walking combating entities, but still in the entity layer bc it rises up in front of walls behind it.
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
                newGo.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
            else if (entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_LARGE_BEE)
            {
                var newGo = BuildSplineView("Spine/Export/Bee_SkeletonData", itemsToEquip, "Template", Animations.Idle, entity.Direction);
                SetSpineDefaults(go, newGo);
                newGo.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
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
                var relevantItems = InventoryUtil.ChooseRandomItemsFromInventory(entity, 3);
                foreach (var item in relevantItems)
                {
                    var childGo = new GameObject();
                    childGo.name = item.DisplayName + "View";
                    var spriteRenderer = childGo.AddComponent<SpriteRenderer>();
                    childGo.transform.localEulerAngles = item.CorpseIconEulerAngles;
                    childGo.transform.position = item.CorpsePositionOffset;
                    childGo.transform.localScale = item.CorpseIconScale;
                    childGo.transform.SetParent(go.transform, false);
                    var childSortable = childGo.AddComponent<Sortable>();
                    childSortable.Layer = SortingLayer.EntitiesAndProps;
                    spriteRenderer.sprite = item.ItemAppearance.InventorySprite;
                    spriteRenderer.material = defaultMaterial;
                }
                sortable.Weight = 0;
            }

            if (entity.Body != null && entity.Body.Floating && entity.View.ViewPrototypeUniqueIdentifier != UniqueIdentifier.VIEW_CORPSE)
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
                    foreach (var effect in item.EffectsGlobal)
                    {
                        effect.ApplyPersistentVisualEffects(entity);
                    }
                }
            }
        }

        public static GameObject InstantiateProjectileAppearance(ProjectileAppearanceDefinition def, Point position, Direction direction, GameObject referenceObject)
        {
            if (def.Prefab == null)
            {
                return null;
            }
            var prefabInstance = GameObject.Instantiate<GameObject>(def.Prefab);
            if (position != null)
            {
                prefabInstance.transform.position = MathUtil.MapToWorld(position);
            }
            if (def.ProjectileBehaviour == ProjectileBehaviour.Spin)
            {
                prefabInstance.AddComponent<ProjectileRotator>();
            }
            else if (def.ProjectileBehaviour == ProjectileBehaviour.FaceDirection)
            {
                prefabInstance.transform.eulerAngles = MathUtil.GetProjectileRotation(direction);
            }
            if (def.InheritRotation)
            {
                prefabInstance.transform.localRotation = referenceObject.transform.localRotation;
            }
            if (def.Lifetime > 0.0f)
            {
                prefabInstance.AddComponent<Expires>().Lifetime = def.Lifetime;
            }
            return prefabInstance;
        }

        private static GameObject BuildDefaultGameObject(Entity entity)
        {
            var go = new GameObject();
            go.name = entity.PrototypeIdentifier.ToString();
            go.transform.position = MathUtil.MapToWorld(entity.Position);
            entity.View.ViewGameObject = go;
            return go;
        }

        private static Sortable BuildDefaultSortable(Entity entity)
        {
            var sortable = entity.View.ViewGameObject.AddComponent<Sortable>();
            sortable.Layer = SortingLayer.EntitiesAndProps;
            if (entity.Position == null)
            {
                sortable.Position = new Point(0, 0);
            }
            else
            {
                sortable.Position = new Point(entity.Position);
            }

            sortable.PositionRelativeToParent = new Point(Point.Zero);
            // Combatants appear in front of loot, objects on the floor.
            if (entity.IsCombatant)
            {
                entity.Sortable.Weight = 1;
            }
            return sortable;
        }

        private static void SetSpineDefaults(GameObject go, GameObject newGo)
        {
            newGo.transform.SetParent(go.transform, false);
            newGo.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            newGo.transform.localPosition = new Vector3(0, -0.30f, 0);
        }

        public static void BuildMapTiles(Level level)
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
                        BuildFloorTile(folder, tileSet.FloorSprite, point);
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
                            BuildWallTile(folder, tileSet.TeeSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             (northWestPointTileType == TileType.Floor || northWestPointTileType == TileType.Empty)
                        )
                        {
                            BuildWallTile(folder, tileSet.SouthEastTeeSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildWallTile(folder, tileSet.NorthEastTeeSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Floor || northEastPointTileType == TileType.Empty) &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildWallTile(folder, tileSet.SouthWestTeeSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             (southEastPointTileType == TileType.Floor || southEastPointTileType == TileType.Empty) &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildWallTile(folder, tileSet.NorthWestTeeSprite, point);
                        }

                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Floor &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Empty
                        )
                        {
                            BuildWallTile(folder, tileSet.NorthWestWallSprite, point);
                        }

                        else if (
                            northEastPointTileType == TileType.Empty &&
                            southEastPointTileType == TileType.Wall &&
                            southWestPointTileType == TileType.Floor &&
                            northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildWallTile(folder, tileSet.NorthEastWallSprite, point);
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
                            BuildWallTile(folder, tileSet.SouthEastWallSprite, point);
                        }

                        else if (
                             (northEastPointTileType == TileType.Floor || northEastPointTileType == TileType.Empty) &&
                             (southEastPointTileType == TileType.Wall || southEastPointTileType == TileType.Floor) &&
                             (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
                             (northWestPointTileType == TileType.Wall || northWestPointTileType == TileType.Floor)
                        )
                        {
                            BuildWallTile(folder, tileSet.SouthWestWallSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             (southWestPointTileType == TileType.Empty || southWestPointTileType == TileType.Floor) &&
                             (northWestPointTileType == TileType.Empty || northWestPointTileType == TileType.Floor)
                         )
                        {
                            BuildWallTile(folder, tileSet.WestCornerSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Empty || northEastPointTileType == TileType.Floor) &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             (northWestPointTileType == TileType.Empty || northWestPointTileType == TileType.Floor)
                         )
                        {
                            BuildWallTile(folder, tileSet.NorthCornerSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Empty || northEastPointTileType == TileType.Floor) &&
                             (southEastPointTileType == TileType.Empty || southEastPointTileType == TileType.Floor) &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                         )
                        {
                            BuildWallTile(folder, tileSet.EastCornerSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             (southEastPointTileType == TileType.Empty || southEastPointTileType == TileType.Floor) &&
                             (southWestPointTileType == TileType.Empty || southWestPointTileType == TileType.Floor) &&
                             northWestPointTileType == TileType.Wall
                         )
                        {
                            BuildWallTile(folder, tileSet.SouthCornerSprite, point);
                        }
                        else
                        {
                            BuildWallTile(folder, MissingSpriteMaterial(), point);
                        }
                        BuildFloorTile(folder, tileSet.FloorSprite, point);
                    }
                }
            }
        }

        public static GameObject BuildSplineView(string SkeletonDataPath, Dictionary<SpriteAttachment, Sprite> spritesBySlot, string SkinName = null, Animations beginningAnimation = Animations.Idle, Direction beginningDirection = Direction.SouthEast)
        {
            if (spritesBySlot == null)
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

                if (spriteSlotIndex == -1)
                {
                    Debug.LogWarning("Could not find sprite attachment on template: " + attachmentKey.ToString());
                    continue;
                }
                Attachment templateAttachment = templateSkin.GetAttachment(spriteSlotIndex, attachmentKey.ToString());
                Attachment newAttachment = templateAttachment.GetRemappedClone(sprite, sourceMaterial, true, false, true);
                RegionAttachment regionAttachment = newAttachment as RegionAttachment;
                Assert.IsNotNull(regionAttachment, "If this is null there is a decent chance you forgot to set the Ghost image to the slot your looking for on the template skin in spine.");
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

        private static void BuildWallTile(GameObject folder, Sprite wallTile, Point position)
        {
            GameObject o = new GameObject();
            o.name = "Tile";
            var renderer = o.AddComponent<SpriteRenderer>();
            var sortable = o.AddComponent<Sortable>();
            renderer.material = GetDefaultSpriteMaterial();
            o.transform.SetParent(folder.transform);
            renderer.sprite = wallTile;
            o.transform.localPosition = MathUtil.MapToWorld(position);
            sortable.Position = new Point(position);
            sortable.Layer = SortingLayer.EntitiesAndProps;
        }

        private static void BuildFloorTile(GameObject folder, Sprite wallTile, Point position)
        {
            GameObject o = new GameObject();
            o.name = "Ground";
            var renderer = o.AddComponent<SpriteRenderer>();
            var sortable = o.AddComponent<Sortable>();
            renderer.material = GetDefaultSpriteMaterial();
            o.transform.SetParent(folder.transform);
            renderer.sprite = wallTile;
            o.transform.localPosition = MathUtil.MapToWorld(position);
            sortable.Position = new Point(position);
            sortable.Layer = SortingLayer.Ground;
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
            else if (attachment == SpriteAttachment.MainHandBack)
            {
                return SpriteSlot.MainHandBack.ToString();
            }
            else if (attachment == SpriteAttachment.MainHandFront)
            {
                return SpriteSlot.MainHandFront.ToString();
            }
            else if (attachment == SpriteAttachment.OffHandFrontSE || attachment == SpriteAttachment.OffHandFrontNE)
            {
                return SpriteSlot.OffHandFront.ToString();
            }
            else if (attachment == SpriteAttachment.OffHandBackSE || attachment == SpriteAttachment.OffHandBackNE)
            {
                return SpriteSlot.OffHandBack.ToString();
            }
            else return attachment.ToString();
        }
    }
}

