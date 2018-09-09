using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class PrototypeFactory
    {
        private Material DefaultSpriteMaterial;
        private Sprite MissingSprite;

        public PrototypeFactory()
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

        public TargetableAction BuildEntityAction<TAction> (Entity entity) where TAction : TargetableAction
        {
            var action = Activator.CreateInstance<TAction>();
            Assert.IsNotNull(action, string.Format("Failed to create {0}", typeof(TAction)));
            action.Source = entity;
            return action;
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

        public void BuildView(Entity entity)
        {
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
            else if(entity.View.ViewPrototypeUniqueIdentifier == UniqueIdentifier.VIEW_CORPSE)
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

            if (entity.IsCombatant)
            {
                foreach (var pair in entity.Inventory.EquippedItemBySlot)
                {
                    var item = pair.Value;
                    foreach (var effect in item.Effects.Values)
                    {
                        effect.ApplyPersistentVisualEffects(entity);
                    }
                }
            }
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
    }
}

