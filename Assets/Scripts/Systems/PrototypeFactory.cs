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
        private ISpriteSortingSystem _spriteSortingSystem;
        private Material DefaultSpriteMaterial;
        private Sprite MissingSprite;

        public PrototypeFactory(IResourceManager resourceManager, TinyIoCContainer container, ITokenSystem tokenSystem, ISpriteSortingSystem spriteSortingSystem)
        {
            _resourceManager = resourceManager;
            _container = container;
            _tokenSystem = tokenSystem;
            _spriteSortingSystem = spriteSortingSystem;
            if(DefaultSpriteMaterial == null)
            {
                DefaultSpriteMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            }
            if(MissingSprite == null)
            {
                MissingSprite = Resources.Load<Sprite>("Missing");
            }
        }

        public void LoadTypes()
        {
            componentTypeMap.Clear();

            List<Type> types = new List<Type>();

            types.AddRange(typeof(Component<BehaviourPrototype>).ConcreteFromAbstract());
            types.AddRange(typeof(Component<EquipmentPrototype>).ConcreteFromAbstract());
            types.AddRange(typeof(Component<InventoryPrototype>).ConcreteFromAbstract());
            types.AddRange(typeof(Component<MotorPrototype>).ConcreteFromAbstract());
            types.AddRange(typeof(Component<PersonaPrototype>).ConcreteFromAbstract());
            types.AddRange(typeof(Component<TokenViewPrototype>).ConcreteFromAbstract());
            types.AddRange(typeof(Component<TriggerBehaviourPrototype>).ConcreteFromAbstract());
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

            token.Behaviour = BuildBehaviour(prototype.BehaviourPrototype);
            token.Equipment = BuildEquipment(prototype.EquipmentPrototype);
            token.Inventory = BuildInventory(prototype.InventoryPrototype);
            token.Motor = BuildMotor(prototype.MotorPrototype);
            token.Persona = BuildPersona(prototype.PersonaPrototype);
            token.TriggerBehaviour = BuildTriggerBehaviour(prototype.TriggerBehaviourPrototype);
            token.TokenView = BuildTokenView(prototype.TokenViewPrototype);

            _container.BuildUp(token.Behaviour);
            _container.BuildUp(token.Equipment);
            _container.BuildUp(token.Inventory);
            _container.BuildUp(token.Motor);
            _container.BuildUp(token.Persona);
            _container.BuildUp(token.TriggerBehaviour);
            _container.BuildUp(token.TokenView);

            token.Behaviour.PrototypeReference = new PrototypeReference<BehaviourPrototype>()
            {
                Prototype = prototype.BehaviourPrototype
            };
            token.Equipment.PrototypeReference = new PrototypeReference<EquipmentPrototype>()
            {
                Prototype = prototype.EquipmentPrototype
            };
            token.Inventory.PrototypeReference = new PrototypeReference<InventoryPrototype>()
            {
                Prototype = prototype.InventoryPrototype
            };
            token.Motor.PrototypeReference = new PrototypeReference<MotorPrototype>()
            {
                Prototype = prototype.MotorPrototype
            };
            token.Persona.PrototypeReference = new PrototypeReference<PersonaPrototype>()
            {
                Prototype = prototype.PersonaPrototype
            };
            token.TriggerBehaviour.PrototypeReference = new PrototypeReference<TriggerBehaviourPrototype>()
            {
                Prototype = prototype.TriggerBehaviourPrototype
            };
            token.TokenView.PrototypeReference = new PrototypeReference<TokenViewPrototype>()
            {
                Prototype = prototype.TokenViewPrototype
            };

            token.Resolve(_resourceManager);
            _tokenSystem.Register(token);
            return token;
        }

        private TokenView BuildTokenView(TokenViewPrototype prototype)
        {
            return Activator.CreateInstance(componentTypeMap[prototype.ClassName]) as TokenView;
        }

        private TriggerBehaviour BuildTriggerBehaviour(TriggerBehaviourPrototype prototype)
        {
            return Activator.CreateInstance(componentTypeMap[prototype.ClassName]) as TriggerBehaviour;
        }

        private Persona BuildPersona(PersonaPrototype prototype)
        {
            return Activator.CreateInstance(componentTypeMap[prototype.ClassName]) as Persona;
        }

        private Motor BuildMotor(MotorPrototype prototype)
        {
            return Activator.CreateInstance(componentTypeMap[prototype.ClassName]) as Motor;
        }

        private Inventory BuildInventory(InventoryPrototype prototype)
        {
            return Activator.CreateInstance(componentTypeMap[prototype.ClassName]) as Inventory;
        }

        private Equipment BuildEquipment(EquipmentPrototype prototype)
        {
            Equipment equipment = Activator.CreateInstance(componentTypeMap[prototype.ClassName]) as Equipment;
            foreach (var table in prototype.EquipmentTables)
            {
                table.ProbabilityTable.Next();
            }

            return equipment;
        }

        private Behaviour BuildBehaviour(BehaviourPrototype prototype)
        {
            return Activator.CreateInstance(componentTypeMap[prototype.ClassName]) as Behaviour;
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
                            else
                            {
                                BuildTileSpriteRenderer(folder, MissingSprite, point);
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

        private SpriteRenderer BuildTileSpriteRenderer(GameObject folder, Sprite sprite, Point position)
        {
            GameObject o = new GameObject();
            o.name = "Tile";
            var renderer = o.AddComponent<SpriteRenderer>();
            renderer.material = DefaultSpriteMaterial;
            o.transform.SetParent(folder.transform);
            renderer.sprite = sprite;
            o.transform.localPosition = MathUtil.MapToWorld(position);
            _spriteSortingSystem.RegisterTile(renderer, position);
            return renderer;
        }

        private Tileset GetTileset(TileInfo tileInfo)
        {
            return _resourceManager.GetPrototypeByUniqueIdentifier<Tileset>(tileInfo.TilesetIdentifier);
        }
    }
}

