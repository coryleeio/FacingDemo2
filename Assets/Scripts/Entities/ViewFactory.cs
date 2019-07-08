using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;
using Spine.Unity.Modules.AttachmentTools;
using Spine;
using UnityEngine.Assertions;


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

        private static void BuildSprite(GameObject go, string sprite)
        {
            var defaultMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Context.ResourceManager.Load<Sprite>(sprite);
            spriteRenderer.material = defaultMaterial;
            go.AddComponent<EntityView>();
        }

        private static void BuildPrefab(GameObject go, string Sprite)
        {
            var pref = Resources.Load<GameObject>(Sprite);
            var newGameObject = GameObject.Instantiate<GameObject>(pref);
            newGameObject.transform.SetParent(go.transform, false);
            newGameObject.transform.localPosition = Vector3.zero;
            newGameObject.AddComponent<EntityView>();
        }

        private static void SetDefaultShadowValues(Entity entity)
        {
            SetShadowScale(entity, 1f);
            entity.ShadowTransformY = -.33f;
        }

        private static void SetShadowScale(Entity entity, float value)
        {
            if (entity != null)
            {
                entity.ShadowScaleX = value;
                entity.ShadowScaleY = value;
                entity.ShadowScaleZ = value;
            }
        }

        public static void RebuildView(Entity entity, bool DestroyOldView = true)
        {
            if (entity.ViewGameObject != null && DestroyOldView)
            {
                UnityEngine.GameObject.Destroy(entity.ViewGameObject);
            }
            var itemsToEquip = new Dictionary<SpriteAttachment, Sprite>();
            if (entity.Inventory != null)
            {
                var equippedItemsBySlot = entity.Inventory.EquippedItemBySlot;
                foreach (var pair in equippedItemsBySlot)
                {
                    var item = pair.Value;
                    foreach (var spriteSlotPair in item.Template.ItemAppearance.WornItemSpritePerSlot)
                    {
                        var spriteSlot = spriteSlotPair.Key;
                        var sprite = spriteSlotPair.Value;
                        itemsToEquip[spriteSlot] = sprite;
                    }
                }
            }

            // Override this further down if you need a larger or smaller shadow
            SetDefaultShadowValues(entity);

            var defaultMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            var go = BuildDefaultGameObject(entity);
            var sortable = BuildDefaultSortable(entity);
            var template = entity.ViewTemplate;

            if (template != null && Context.ResourceManager.Contains<Sprite>(template.ResourceIdentifier))
            {
                BuildSprite(go, template.ResourceIdentifier);
                SetShadowScale(entity, template.ShadowScale);
                sortable.Weight = template.SortableWeight;
            }

            else if (template != null && Context.ResourceManager.Contains<SkeletonDataAsset>(template.ResourceIdentifier))
            {
                var skeletonDataAsset = Context.ResourceManager.Load<SkeletonDataAsset>(template.ResourceIdentifier);
                var newGo = BuildSplineView(skeletonDataAsset, itemsToEquip, template.SpineSkinName, Animations.Idle, entity.Direction);
                SetSpineDefaults(go, newGo);
                SetShadowScale(entity, template.ShadowScale);
                newGo.transform.localScale = new Vector3(template.Scale, template.Scale, template.Scale);
            }

            else if (template != null && Context.ResourceManager.Contains<MultitileViewTemplate>(template.ResourceIdentifier))
            {
                var multitileViewTemplate = Context.ResourceManager.Load<MultitileViewTemplate>(template.ResourceIdentifier);
                SetShadowScale(entity, template.ShadowScale);

                var newGameObject = new GameObject();
                newGameObject.name = "Constructed Multisprite View " + template.ResourceIdentifier;
                newGameObject.transform.SetParent(go.transform, false);
                newGameObject.transform.localPosition = Vector3.zero;
                newGameObject.AddComponent<EntityView>();

                foreach (var comp in multitileViewTemplate.MultitileViewTemplateComponent)
                {
                    var componentGameObject = new GameObject();
                    componentGameObject.name = "Component for " + template.ResourceIdentifier;
                    componentGameObject.transform.SetParent(newGameObject.transform, false);
                    componentGameObject.transform.localPosition = Vector3.zero;
                    componentGameObject.transform.localPosition = new Vector3(comp.EngineOffsetX, comp.EngineOffsetY, 0.0f);
                    var sr = componentGameObject.AddComponent<SpriteRenderer>();
                    sr.material = defaultMaterial;
                    sr.sprite = comp.Sprite;
                    var componentSortable = componentGameObject.AddComponent<Sortable>();
                    componentSortable._positionRelativeToParent = new Point(comp.GridOffsetX, comp.GridOffsetY);
                    componentSortable.Weight = comp.Weight;
                    componentSortable.Height = comp.Height;
                    componentSortable.Layer = comp.SortingLayer;
                }
            }

            else if (entity.ViewTemplateIdentifier == "VIEW_CORPSE")
            {
                var relevantItems = InventoryUtil.ChooseRandomItemsFromInventory(entity, 3);
                foreach (var item in relevantItems)
                {
                    var childGo = new GameObject();
                    childGo.name = item.Name + "View";
                    var spriteRenderer = childGo.AddComponent<SpriteRenderer>();
                    childGo.transform.localEulerAngles = item.CorpseIconEulerAngles;
                    childGo.transform.position = item.CorpsePositionOffset;
                    childGo.transform.localScale = item.CorpseIconScale;
                    childGo.transform.SetParent(go.transform, false);
                    var childSortable = childGo.AddComponent<Sortable>();
                    childSortable.Layer = SortingLayer.EntitiesAndProps;
                    spriteRenderer.sprite = item.Template.ItemAppearance.InventorySprite;
                    spriteRenderer.material = defaultMaterial;
                    childGo.AddComponent<EntityView>();
                }
                sortable.Weight = 0;
            }

            if (entity.IsCombatant && entity.ViewTemplateIdentifier != "VIEW_CORPSE")
            {
                BuildHealthbar(entity);
                BuildCastShadow(entity, go);
                BuildFloating(entity, go);
            }

            var shouldShowEffects = entity.IsCombatant && (!entity.IsCombatant || (entity.IsCombatant && !entity.IsDead));
            if (shouldShowEffects)
            {
                var effects = entity.GetAllTypesOfEffects();
                foreach (var effect in effects)
                {
                    effect.EffectImpl.ApplyPersistentVisualEffects(entity);
                }
            }
        }

        private static void BuildCastShadow(Entity entity, GameObject go)
        {
            if (entity.CastsShadow)
            {
                var shadowPrefab = Resources.Load<GameObject>("Prefabs/Shadow");
                var shadowGameObject = GameObject.Instantiate<GameObject>(shadowPrefab);
                shadowGameObject.transform.SetParent(go.transform, false);
                shadowGameObject.transform.localScale = new Vector3(entity.ShadowScaleX, entity.ShadowScaleY, entity.ShadowScaleZ);
                shadowGameObject.transform.localPosition = new Vector3(0f, entity.ShadowTransformY, 0f);
            }
        }

        private static void BuildFloating(Entity entity, GameObject go)
        {
            if (entity.Floating)
            {
                var skeletonAnimation = go.transform.GetComponentInChildren<SkeletonAnimation>();
                var target = skeletonAnimation != null ? skeletonAnimation.gameObject : go;
                target.AddComponent<Floating>();
            }
        }

        private static void BuildHealthbar(Entity entity)
        {
            var healthbarPrefab = Resources.Load<GameObject>("Prefabs/UI/Healthbar");
            var healthbarGameObject = GameObject.Instantiate(healthbarPrefab);
            healthbarGameObject.transform.SetParent(Context.UIController.gameObject.transform, false);
            var healthBarComponent = healthbarGameObject.GetComponent<HealthBar>();
            healthBarComponent.Entity = entity;
            entity.HealthBar = healthBarComponent;
            ViewUtils.UpdateHealthBarColor(entity);
        }

        public static GameObject InstantiateProjectileAppearance(ProjectileAppearanceTemplateComponent def, Point position, Direction direction, GameObject referenceObject, Sprite spriteOverride = null)
        {
            if (def.Sprites.Count == 0 && spriteOverride == null)
            {
                return null;
            }

            GameObject myGameObject = new GameObject();
            Sprite sprite = null;
            if (def.Sprites.Count > 0)
            {
                sprite = def.Sprites[0];
            }
            else if (def != null && def.Sprites.Count == 0 && spriteOverride != null)
            {
                sprite = spriteOverride;
            }
            myGameObject.name = string.Format("{0} Generated ProjectileAppearance Prefab", sprite.name);
            var spriteRenderer = myGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 30000;
            spriteRenderer.sortingLayerID = UnityEngine.SortingLayer.NameToID(SortingLayer.Overlays.ToString());
            spriteRenderer.sprite = sprite;
            myGameObject.transform.localScale = new Vector3(def.ScaleX, def.ScaleY, def.ScaleZ);
            if (def.Animated)
            {
                var spriteChanger = myGameObject.AddComponent<SpriteChanger>();
                foreach (var sp in def.Sprites)
                {
                    spriteChanger.Sprites.Add(sp);
                }
                spriteChanger.timePerSprite = def.AnimationTimeSpentShowingEachSprite;
                spriteChanger.ChangeMethod = def.AnimationChangeType;
            }

            var defaultMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            spriteRenderer.material = defaultMaterial;

            myGameObject.transform.position = MathUtil.Offscreen;
            if (position != null)
            {
                myGameObject.transform.position = MathUtil.MapToWorld(position);
            }
            if (def.SpriteFacingBehavior == SpriteFacingBehavior.Spin)
            {
                myGameObject.AddComponent<ProjectileRotator>();
            }
            else if (def.SpriteFacingBehavior == SpriteFacingBehavior.FaceDirection)
            {
                myGameObject.transform.eulerAngles = MathUtil.GetProjectileRotation(direction);
            }
            if (def.InheritRotation)
            {
                myGameObject.transform.localRotation = referenceObject.transform.localRotation;
            }
            if (def.TotalTimeVisible > 0.0f)
            {
                myGameObject.AddComponent<Expires>().Lifetime = def.TotalTimeVisible;
            }
            return myGameObject;
        }

        private static GameObject BuildDefaultGameObject(Entity entity)
        {
            var go = new GameObject();
            go.name = entity.TemplateIdentifier.ToString();
            go.transform.position = MathUtil.MapToWorld(entity.Position);
            entity.ViewGameObject = go;
            return go;
        }

        private static Sortable BuildDefaultSortable(Entity entity)
        {
            var sortable = entity.ViewGameObject.AddComponent<Sortable>();
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
            newGo.transform.localPosition = new Vector3(0, -0.30f, 0);
            newGo.AddComponent<EntityView>();
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
                    var tileSet = Context.ResourceManager.Load<TilesetTemplate>(tileInfo.TilesetIdentifier);

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
                            BuildFloorTile(folder, tileSet.FloorSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             (northWestPointTileType == TileType.Floor || northWestPointTileType == TileType.Empty)
                        )
                        {
                            BuildWallTile(folder, tileSet.SouthEastTeeSprite, point);
                            BuildFloorTile(folder, tileSet.FloorSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildWallTile(folder, tileSet.NorthEastTeeSprite, point);
                            BuildFloorTile(folder, tileSet.FloorSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Floor || northEastPointTileType == TileType.Empty) &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildWallTile(folder, tileSet.SouthWestTeeSprite, point);
                            BuildFloorTile(folder, tileSet.FloorSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             (southEastPointTileType == TileType.Floor || southEastPointTileType == TileType.Empty) &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildWallTile(folder, tileSet.NorthWestTeeSprite, point);
                            BuildFloorTile(folder, tileSet.FloorSprite, point);
                        }

                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Floor &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Empty
                        )
                        {
                            BuildWallTile(folder, tileSet.NorthWestWallSprite, point);
                            BuildFloorTile(folder, tileSet.FloorSprite, point);
                        }

                        else if (
                            northEastPointTileType == TileType.Empty &&
                            southEastPointTileType == TileType.Wall &&
                            southWestPointTileType == TileType.Floor &&
                            northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildWallTile(folder, tileSet.NorthEastWallSprite, point);
                            BuildFloorTile(folder, tileSet.FloorSprite, point);
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
                            if (southEastPointTileType == TileType.Floor)
                            {
                                BuildFloorTile(folder, tileSet.FloorSprite, point);
                            }
                        }

                        else if (
                             (northEastPointTileType == TileType.Floor || northEastPointTileType == TileType.Empty) &&
                             (southEastPointTileType == TileType.Wall || southEastPointTileType == TileType.Floor) &&
                             (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
                             (northWestPointTileType == TileType.Wall || northWestPointTileType == TileType.Floor)
                        )
                        {
                            BuildWallTile(folder, tileSet.SouthWestWallSprite, point);
                            if (southWestPointTileType == TileType.Floor)
                            {
                                BuildFloorTile(folder, tileSet.FloorSprite, point);
                            }
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             (southWestPointTileType == TileType.Empty || southWestPointTileType == TileType.Floor) &&
                             (northWestPointTileType == TileType.Empty || northWestPointTileType == TileType.Floor)
                         )
                        {
                            BuildWallTile(folder, tileSet.WestCornerSprite, point);
                            if (southWestPointTileType == TileType.Floor || northWestPointTileType == TileType.Floor)
                            {
                                BuildFloorTile(folder, tileSet.FloorSprite, point);
                            }
                        }
                        else if (
                             (northEastPointTileType == TileType.Empty || northEastPointTileType == TileType.Floor) &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             (northWestPointTileType == TileType.Empty || northWestPointTileType == TileType.Floor)
                         )
                        {
                            BuildWallTile(folder, tileSet.NorthCornerSprite, point);
                            if (northEastPointTileType == TileType.Empty || southEastPointTileType == TileType.Empty)
                            {
                                BuildFloorTile(folder, tileSet.FloorSprite, point);
                            }
                        }
                        else if (
                             (northEastPointTileType == TileType.Empty || northEastPointTileType == TileType.Floor) &&
                             (southEastPointTileType == TileType.Empty || southEastPointTileType == TileType.Floor) &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                         )
                        {
                            BuildWallTile(folder, tileSet.EastCornerSprite, point);
                            if (northEastPointTileType == TileType.Floor || southEastPointTileType == TileType.Floor)
                            {
                                BuildFloorTile(folder, tileSet.FloorSprite, point);
                            }
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             (southEastPointTileType == TileType.Empty || southEastPointTileType == TileType.Floor) &&
                             (southWestPointTileType == TileType.Empty || southWestPointTileType == TileType.Floor) &&
                             northWestPointTileType == TileType.Wall
                         )
                        {
                            BuildWallTile(folder, tileSet.SouthCornerSprite, point);

                            if (southEastPointTileType == TileType.Floor || southWestPointTileType == TileType.Floor)
                            {
                                BuildFloorTile(folder, tileSet.FloorSprite, point);
                            }
                        }
                        else
                        {
                            BuildWallTile(folder, MissingSpriteMaterial(), point);
                            BuildFloorTile(folder, tileSet.FloorSprite, point);
                        }

                    }
                }
            }
        }

        public static GameObject BuildSplineView(SkeletonDataAsset skeletonDataAsset, Dictionary<SpriteAttachment, Sprite> spritesBySlot, string SkinName = null, Animations beginningAnimation = Animations.Idle, Direction beginningDirection = Direction.SouthEast)
        {
            if (spritesBySlot == null)
            {
                spritesBySlot = new Dictionary<SpriteAttachment, Sprite>();
            }
            Texture2D runtimeAtlas;
            Material runtimeMaterial;
            var skeletonData = skeletonDataAsset.GetSkeletonData(false);
            var idleAnimation = skeletonData.FindAnimation(DisplayUtil.GetAnimationNameForDirection(beginningAnimation, beginningDirection));
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

            skeletonAnimation.gameObject.name = SkinName;
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

        private static void BuildMaskTile(GameObject folder, string spriteResourceName, Point position)
        {
            var spr = Resources.Load<Sprite>(spriteResourceName);
            GameObject o = new GameObject();
            o.name = "TileMask";
            var renderer = o.AddComponent<SpriteRenderer>();
            var sortable = o.AddComponent<Sortable>();
            renderer.material = GetDefaultSpriteMaterial();
            o.transform.SetParent(folder.transform);
            renderer.sprite = spr;
            o.transform.localPosition = MathUtil.MapToWorld(position);
            sortable.Position = new Point(position);
            sortable.Layer = SortingLayer.Ground;
            sortable.SortOrder = 1;
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
