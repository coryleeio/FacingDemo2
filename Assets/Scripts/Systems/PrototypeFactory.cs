using System;
using System.Collections.Generic;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class PrototypeFactory : IPrototypeFactory
    {
        private Dictionary<string, Type> componentTypeMap = new Dictionary<string, Type>();
        private IResourceManager _resourceManager;
        private TinyIoCContainer _container;
        private ITokenSystem _tokenSystem;

        public PrototypeFactory(IResourceManager resourceManager, TinyIoCContainer container, ITokenSystem tokenSystem)
        {
            _resourceManager = resourceManager;
            _container = container;
            _tokenSystem = tokenSystem;
        }

        public void LoadTypes()
        {
            componentTypeMap.Clear();
            var types = typeof(Component).ConcreteFromAbstract();
            foreach (var type in types)
            {
                componentTypeMap[type.Name] = type;
            }
        }

        public Token BuildToken(TokenPrototype prototype)
        {
            var token = new Token();
            token.PrototypeUniqueIdentifier = prototype.UniqueIdentifier;
            token.Shape = new Shape(prototype.ShapeType, prototype.Width, prototype.Height);
            token.Behaviour = Activator.CreateInstance(componentTypeMap[prototype.BehaviourClassName]) as Behaviour;
            token.Equipment = Activator.CreateInstance(componentTypeMap[prototype.EquipmentClassName]) as Equipment;
            token.Inventory = Activator.CreateInstance(componentTypeMap[prototype.InventoryClassName]) as Inventory;
            token.Motor = Activator.CreateInstance(componentTypeMap[prototype.MotorClassName]) as Motor;
            token.Persona = Activator.CreateInstance(componentTypeMap[prototype.PersonaClassName]) as Persona;
            token.TriggerBehaviour = Activator.CreateInstance(componentTypeMap[prototype.TriggerBehaviourClassName]) as TriggerBehaviour;
            token.View = Activator.CreateInstance(componentTypeMap[prototype.ViewClassName]) as View;

            token.Behaviour.Owner = token;
            token.Equipment.Owner = token;
            token.Inventory.Owner = token;
            token.Motor.Owner = token;
            token.Persona.Owner = token;
            token.TriggerBehaviour.Owner = token;
            token.View.Owner = token;

            // Inject components
            _container.BuildUp(token.PrototypeUniqueIdentifier);
            _container.BuildUp(token.Behaviour);
            _container.BuildUp(token.Equipment);
            _container.BuildUp(token.Inventory);
            _container.BuildUp(token.Motor);
            _container.BuildUp(token.Persona);
            _container.BuildUp(token.TriggerBehaviour);
            _container.BuildUp(token.View);

            _tokenSystem.Register(token);
            return token;
        }

        public Token BuildToken(string uniqueIdentifier)
        {
            var prototype = _resourceManager.GetPrototypeByUniqueIdentifier<TokenPrototype>(uniqueIdentifier);
            return BuildToken(prototype);
        }

        public Item BuildItem(ItemPrototype prototype)
        {
            var item = new Item
            {
                PrototypeUniqueIdentifier = prototype.UniqueIdentifier,
            };
            return item;
        }

        public Item BuildItem(string uniqueIdentifier)
        {
            var prototype = _resourceManager.GetPrototypeByUniqueIdentifier<ItemPrototype>(uniqueIdentifier);
            return BuildItem(prototype);
        }

        public void BuildGrid(Level level)
        {
            var folder = GameObjectUtils.MakeFolder("Grid");
            for (int x = 0; x < level.Domain.Width; x++)
            {
                for (var y = 0; y < level.Domain.Height; y++)
                {
                    var point = new Point(x, y);
                    var tileInfo = level.TilesetGrid[x, y];
                    if (tileInfo.TilesetIdentifier != null)
                    {
                        var tileSet = GetTileset(tileInfo);

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
                                southEastPointTileType == TileType.Wall &&
                                (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
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
                                northEastPointTileType == TileType.Wall &&
                                (southEastPointTileType == TileType.Floor || southEastPointTileType == TileType.Empty) &&
                                southWestPointTileType == TileType.Wall &&
                                (northWestPointTileType == TileType.Floor || northWestPointTileType == TileType.Empty)
                            )
                            {
                                BuildTileSpriteRenderer(folder, tileSet.SouthEastWallSprite, point);
                            }

                            else if (
                                (northEastPointTileType == TileType.Floor || northEastPointTileType == TileType.Empty) &&
                                southEastPointTileType == TileType.Wall &&
                                (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
                                northWestPointTileType == TileType.Wall
                            )
                            {
                                BuildTileSpriteRenderer(folder, tileSet.SouthWestWallSprite, point);
                            }
                            else if (
                                 northEastPointTileType == TileType.Wall &&
                                 southEastPointTileType == TileType.Wall &&
                                 southWestPointTileType == TileType.Empty &&
                                 northWestPointTileType == TileType.Empty
                             )
                            {
                                BuildTileSpriteRenderer(folder, tileSet.WestCornerSprite, point);
                            }
                            else if (
                                 northEastPointTileType == TileType.Empty &&
                                 southEastPointTileType == TileType.Wall &&
                                 southWestPointTileType == TileType.Wall &&
                                 northWestPointTileType == TileType.Empty
                             )
                            {
                                BuildTileSpriteRenderer(folder, tileSet.NorthCornerSprite, point);
                            }
                            else if (
                                 northEastPointTileType == TileType.Empty &&
                                 southEastPointTileType == TileType.Empty &&
                                 southWestPointTileType == TileType.Wall &&
                                 northWestPointTileType == TileType.Wall
                             )
                            {
                                BuildTileSpriteRenderer(folder, tileSet.EastCornerSprite, point);
                            }
                            else if (
                                 northEastPointTileType == TileType.Wall &&
                                 southEastPointTileType == TileType.Empty &&
                                 southWestPointTileType == TileType.Empty &&
                                 northWestPointTileType == TileType.Wall
                             )
                            {
                                BuildTileSpriteRenderer(folder, tileSet.SouthCornerSprite, point);
                            }
                        }
                    }
                }
            }
        }

        private static TileType GetTileInfoForOffset(Level level, Point position, Point offset)
        {
            var offsetPoint = MathUtil.GetPointByOffset(position, offset);
            if (!level.Domain.Contains(offsetPoint))
            {
                return TileType.Empty;
            }
            return level.TilesetGrid[offsetPoint.X, offsetPoint.Y].TileType;
        }

        private static SpriteRenderer BuildTileSpriteRenderer(GameObject folder, Sprite sprite, Point position)
        {
            GameObject o = new GameObject();
            o.name = "Tile";
            var renderer = o.AddComponent<SpriteRenderer>();
            o.transform.SetParent(folder.transform);
            renderer.sprite = sprite;
            o.transform.localPosition = MathUtil.MapToWorld(position);
            return renderer;
        }

        private Tileset GetTileset(TileInfo tileInfo)
        {
            return _resourceManager.GetPrototypeByUniqueIdentifier<Tileset>(tileInfo.TilesetIdentifier);
        }
    }
}

