using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class ResourceManager : IResourceManager
    {
        private Dictionary<string, IResource> _prototypesByUniqueIdentifier = new Dictionary<string, IResource>();
        private Dictionary<Type, List<IResource>> _prototypesByType = new Dictionary<Type, List<IResource>>();

        private ILogSystem _logSystem;

        public ResourceManager(ILogSystem logSystem)
        {
            _logSystem = logSystem;
        }

        public void LoadAllPrototypes(IDbConnection dbConnection)
        {
            _prototypesByUniqueIdentifier.Clear();
            LoadItemPrototypes(dbConnection);
            LoadTilesets(dbConnection);
            LoadEquipmentTables(dbConnection);
            LoadInventoryTables(dbConnection);
            LoadBehaviourPrototypes(dbConnection);
            LoadEquipmentPrototypes(dbConnection);
            LoadEquipmentPrototypeContent(dbConnection);
            LoadInventoryPrototypes(dbConnection);
            LoadInventoryPrototypeContent(dbConnection);
            LoadMotorPrototypes(dbConnection);
            LoadPersonaPrototypes(dbConnection);
            LoadTriggerBehaviourPrototypes(dbConnection);
            LoadTokenViewPrototypes(dbConnection);
            LoadTokenPrototypes(dbConnection);
            LoadSpawnTables(dbConnection);
            LoadSpawnTablesContent(dbConnection);
            LoadLevelPrototypes(dbConnection);
            LoadRoomPrototypes(dbConnection);
        }

        private void LoadRoomPrototypes(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from room_prototypes_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var uniqueId = reader.GetString(1);
                var generatorClassName = reader.GetString(2);
                var minimumHeight = reader.GetInt32(3);
                var minimumWidth = reader.GetInt32(4);
                var maximumWidth = reader.GetInt32(5);
                var maximumHeight = reader.GetInt32(6);
                var tilesetUniqueIdentifier = reader.GetString(7);
                string tagsStr = null;
                if(!reader.IsDBNull(8))
                {
                    tagsStr = reader.GetString(8);
                }
                string availableOnLevelsStr = null;
                if(!reader.IsDBNull(9))
                {
                    availableOnLevelsStr = reader.GetString(9);
                }
                var mandatory = reader.GetBoolean(10);
                var unique = reader.GetBoolean(11);

                var tileset = GetPrototypeByUniqueIdentifier<Tileset>(tilesetUniqueIdentifier);

                var generatorTypes = typeof(IRoomGenerator).ConcreteFromInterface();
                IRoomGenerator roomGenerator = null;
                foreach(var ty in generatorTypes)
                {
                    if(ty.Name == generatorClassName)
                    {
                        roomGenerator = Activator.CreateInstance(ty) as IRoomGenerator;
                        roomGenerator.MaximumHeight = maximumHeight;
                        roomGenerator.MaximumWidth = maximumWidth;
                        roomGenerator.MinimumHeight = minimumHeight;
                        roomGenerator.MinimumWidth = minimumWidth;
                        roomGenerator.Tileset = tileset;
                    }
                }
                if(roomGenerator == null)
                {
                    throw new CouldNotResolveTypeException(string.Format("Could not find type: {0}", generatorClassName));
                }

                RoomPrototype prototype = new RoomPrototype()
                {
                    UniqueIdentifier = uniqueId,
                    RoomGenerator = roomGenerator,
                    Mandatory = mandatory,
                    Unique = unique,
                };

                if(tagsStr != null)
                {
                    var tagsARr = tagsStr.Split(',');
                    foreach(var tag in tagsARr)
                    {
                        prototype.Tags.Add(tag);
                    }
                }

                if(availableOnLevelsStr != null)
                {
                    var availableOnLevelsArr = availableOnLevelsStr.Split(',');
                    foreach (var levelStr in availableOnLevelsArr)
                    {
                        prototype.AvailableOnLevels.Add(Convert.ToInt32(levelStr));
                    }
                }
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadLevelPrototypes(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from level_prototypes_view";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var levelIndex = reader.GetInt32(0);
                var uniqueId = reader.GetString(1);
                var defaultSpawnTableUniqueId = reader.GetString(3);
                var defaultTilesetUniqueId = reader.GetString(5);
                var defaultSpawnTable = GetPrototypeByUniqueIdentifier<SpawnTable>(defaultSpawnTableUniqueId);
                var defaultTileSet = GetPrototypeByUniqueIdentifier<Tileset>(defaultTilesetUniqueId);
                LevelPrototype prototype = new LevelPrototype()
                {
                    LevelIndex = levelIndex,
                    UniqueIdentifier = uniqueId,
                    DefaultSpawnTable = defaultSpawnTable,
                    DefaultTileset = defaultTileSet
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadTokenViewPrototypes(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from token_view_prototypes;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                TokenViewPrototype prototype = new TokenViewPrototype()
                {
                    UniqueIdentifier = reader.GetString(1),
                    ClassName = reader.GetString(2)
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadTriggerBehaviourPrototypes(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from trigger_behaviour_prototypes;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                TriggerBehaviourPrototype prototype = new TriggerBehaviourPrototype()
                {
                    UniqueIdentifier = reader.GetString(1),
                    ClassName = reader.GetString(2)
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadPersonaPrototypes(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from persona_prototypes;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                PersonaPrototype prototype = new PersonaPrototype()
                {
                    UniqueIdentifier = reader.GetString(1),
                    ClassName = reader.GetString(2)
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadMotorPrototypes(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from motor_prototypes;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                MotorPrototype prototype = new MotorPrototype()
                {
                    UniqueIdentifier = reader.GetString(1),
                    ClassName = reader.GetString(2)
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadInventoryPrototypes(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from inventory_prototypes;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                InventoryPrototype prototype = new InventoryPrototype()
                {
                    UniqueIdentifier = reader.GetString(1),
                    ClassName = reader.GetString(2)
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadInventoryPrototypeContent(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from inventory_prototype_inventory_tables_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var inventoryPrototypeUniqueId = reader.GetString(1);
                var inventoryTableId = reader.GetString(4);
                var inventoryPrototype = GetPrototypeByUniqueIdentifier<InventoryPrototype>(inventoryPrototypeUniqueId);
                var inventoryTable = GetPrototypeByUniqueIdentifier<InventoryTable>(inventoryTableId);
                inventoryPrototype.InventoryTables.Add(inventoryTable);
            }
            reader.Close();
            dbcmd.Dispose();
        }


        private void LoadEquipmentPrototypeContent(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from equipment_prototype_equipment_tables_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var equipmentPrototypeUniqueId = reader.GetString(1);
                var equipmentTableId = reader.GetString(4);
                var equipmentPrototype = GetPrototypeByUniqueIdentifier<EquipmentPrototype>(equipmentPrototypeUniqueId);
                var equipmentTable = GetPrototypeByUniqueIdentifier<EquipmentTable>(equipmentTableId);
                equipmentPrototype.EquipmentTables.Add(equipmentTable);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadEquipmentPrototypes(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from equipment_prototypes;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                EquipmentPrototype prototype = new EquipmentPrototype()
                {
                    UniqueIdentifier = reader.GetString(1),
                    ClassName = reader.GetString(2)
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadBehaviourPrototypes(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from behaviour_prototypes;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                BehaviourPrototype prototype = new BehaviourPrototype()
                {
                    UniqueIdentifier = reader.GetString(1),
                    ClassName = reader.GetString(2)
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadTilesets(IDbConnection dbconn)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from tilesets;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var uniqueId = reader.GetString(1);
                string floorSpriteName = reader.GetString(2);
                string teeSpritename = reader.GetString(3);
                string northCornerSpriteName = reader.GetString(4);
                string eastCornerSpriteName = reader.GetString(5);
                string southCornerSpriteName = reader.GetString(6);
                string westCornerSpriteName = reader.GetString(7);
                string northEastwallSpriteName = reader.GetString(8);
                string southEastWallSpriteName = reader.GetString(9);
                string southWestWallSpriteName = reader.GetString(10);
                string northWestWallSpriteName = reader.GetString(11);
                string northEastTeeSpriteName = reader.GetString(12);
                string southEastTeeSpriteName = reader.GetString(13);
                string southWestTeeSpriteName = reader.GetString(14);
                string northWestTeeSpriteName = reader.GetString(15);
                Tileset prototype = new Tileset()
                {
                    UniqueIdentifier = uniqueId,
                    FloorSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + floorSpriteName),
                    TeeSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + teeSpritename),
                    NorthCornerSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + northCornerSpriteName),
                    EastCornerSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + eastCornerSpriteName),
                    SouthCornerSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + southCornerSpriteName),
                    WestCornerSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + westCornerSpriteName),
                    NorthEastWallSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + northEastwallSpriteName),
                    SouthEastWallSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + southEastWallSpriteName),
                    SouthWestWallSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + southWestWallSpriteName),
                    NorthWestWallSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + northWestWallSpriteName),
                    NorthEastTeeSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + northEastTeeSpriteName),
                    SouthEastTeeSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + southEastTeeSpriteName),
                    SouthWestTeeSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + southWestTeeSpriteName),
                    NorthWestTeeSprite = Resources.Load<Sprite>("Tilesets" + "/" + uniqueId + "/" + northWestTeeSpriteName)
                };
                Assert.IsNotNull(prototype.UniqueIdentifier);
                Assert.IsNotNull(prototype.FloorSprite);
                Assert.IsNotNull(prototype.TeeSprite);
                Assert.IsNotNull(prototype.NorthCornerSprite);
                Assert.IsNotNull(prototype.EastCornerSprite);
                Assert.IsNotNull(prototype.SouthCornerSprite);
                Assert.IsNotNull(prototype.WestCornerSprite);
                Assert.IsNotNull(prototype.NorthEastWallSprite);
                Assert.IsNotNull(prototype.SouthEastWallSprite);
                Assert.IsNotNull(prototype.SouthWestWallSprite);
                Assert.IsNotNull(prototype.NorthWestWallSprite);
                Assert.IsNotNull(prototype.NorthEastTeeSprite);
                Assert.IsNotNull(prototype.SouthEastTeeSprite);
                Assert.IsNotNull(prototype.SouthWestTeeSprite);
                Assert.IsNotNull(prototype.NorthWestTeeSprite);
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadItemPrototypes(IDbConnection dbconn)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from item_prototypes_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                ItemPrototype prototype = new ItemPrototype()
                {
                    UniqueIdentifier = reader.GetString(0),
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadInventoryTables(IDbConnection dbconn)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from inventory_table_entries_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var uniqueId = reader.GetString(1);
                var resolutionStr = reader.GetString(2);
                var resolution = (TableResolutionStrategy)System.Enum.Parse(typeof(TableResolutionStrategy), resolutionStr);
                var weight = reader.GetInt32(3);
                var itemPrototypeUniqueIdentifier = reader.GetString(5);
                var numberOfRolls = reader.GetInt32(6);

                var itemPrototype = GetPrototypeByUniqueIdentifier<ItemPrototype>(itemPrototypeUniqueIdentifier);

                InventoryTable inventoryTable = null;
                if (!_prototypesByUniqueIdentifier.ContainsKey(uniqueId))
                {
                    inventoryTable = new InventoryTable()
                    {
                        UniqueIdentifier = uniqueId,
                        ProbabilityTable = new ProbabilityTable<ItemPrototype>()
                        {
                            Resolution = resolution
                        }
                    };
                }
                else
                {
                    inventoryTable = _prototypesByUniqueIdentifier[uniqueId] as InventoryTable;
                }
                inventoryTable.ProbabilityTable.Values.Add(new ProbabilityTableTuple<ItemPrototype>()
                {
                    NumberOfRolls = numberOfRolls,
                    Value = itemPrototype,
                    Weight = weight
                });

                if (!_prototypesByUniqueIdentifier.ContainsKey(uniqueId))
                {
                    CacheResource(inventoryTable);
                }
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadEquipmentTables(IDbConnection dbconn)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from equipment_table_entries_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var uniqueId = reader.GetString(1);
                var resolutionStr = reader.GetString(2);
                var resolution = (TableResolutionStrategy)System.Enum.Parse(typeof(TableResolutionStrategy), resolutionStr);
                var weight = reader.GetInt32(3);
                var itemPrototypeUniqueIdentifier = reader.GetString(5);
                var numberOfRolls = reader.GetInt32(6);

                var itemPrototype = GetPrototypeByUniqueIdentifier<ItemPrototype>(itemPrototypeUniqueIdentifier);

                EquipmentTable equipmentTable = null;
                if (!_prototypesByUniqueIdentifier.ContainsKey(uniqueId))
                {
                    equipmentTable = new EquipmentTable()
                    {
                        UniqueIdentifier = uniqueId,
                        ProbabilityTable = new ProbabilityTable<ItemPrototype>()
                        {
                            Resolution = resolution
                        }
                    };
                }
                else
                {
                    equipmentTable = _prototypesByUniqueIdentifier[uniqueId] as EquipmentTable;
                }
                equipmentTable.ProbabilityTable.Values.Add(new ProbabilityTableTuple<ItemPrototype>()
                {
                    NumberOfRolls = numberOfRolls,
                    Value = itemPrototype,
                    Weight = weight
                });

                if (!_prototypesByUniqueIdentifier.ContainsKey(uniqueId))
                {
                    CacheResource(equipmentTable);
                }
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadSpawnTablesContent(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from spawn_table_entries_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var uniqueId = reader.GetString(1);
                var weight = reader.GetInt32(3);
                var itemPrototypeUniqueIdentifier = reader.GetString(5);
                var numberOfRolls = reader.GetInt32(6);
                var itemPrototype = GetPrototypeByUniqueIdentifier<TokenPrototype>(itemPrototypeUniqueIdentifier);

                SpawnTable spawnTable = GetPrototypeByUniqueIdentifier<SpawnTable>(uniqueId);
                spawnTable.ProbabilityTable.Values.Add(new ProbabilityTableTuple<TokenPrototype>()
                {
                    NumberOfRolls = numberOfRolls,
                    Value = itemPrototype,
                    Weight = weight
                });
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadSpawnTables(IDbConnection dbconn)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from spawn_tables;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var uniqueId = reader.GetString(1);
                var resolutionStr = reader.GetString(2);
                var resolution = (TableResolutionStrategy)System.Enum.Parse(typeof(TableResolutionStrategy), resolutionStr);
                string availableOnLevelsStr = null;

                if(!reader.IsDBNull(3))
                {
                    availableOnLevelsStr = reader.GetString(3);
                }

                var mandatory = reader.GetBoolean(4);

                string roomWithTagConstraint = null;

                if(!reader.IsDBNull(5))
                {
                    roomWithTagConstraint = reader.GetString(5);
                }

                var unique = reader.GetBoolean(6);

                SpawnTable spawnTable = null;
                spawnTable = new SpawnTable()
                {
                    UniqueIdentifier = uniqueId,
                    ProbabilityTable = new ProbabilityTable<TokenPrototype>()
                    {
                        Resolution = resolution
                    },
                    Mandatory = mandatory,
                    ConstraintSpawnToRoomWithTag = roomWithTagConstraint,
                    Unique = unique,
                };
                if(availableOnLevelsStr != null)
                {
                    var availableOnLevelsArr = availableOnLevelsStr.Split(',');
                    foreach(var str in availableOnLevelsArr)
                    {
                        spawnTable.AvailableOnLevels.Add(Convert.ToInt32(str));
                    }
                }
                CacheResource(spawnTable);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void CacheResource(IResource prototype)
        {
            if (_prototypesByUniqueIdentifier.ContainsKey(prototype.UniqueIdentifier))
            {
                throw new DuplicatePrototypeIdException(string.Format("Duplicate prototype: {0}", prototype.UniqueIdentifier));
            }
            _logSystem.Log("Found " + prototype.GetType().Name.ToString() +": " + prototype.UniqueIdentifier);
            _prototypesByUniqueIdentifier[prototype.UniqueIdentifier] = prototype;
            if(!_prototypesByType.ContainsKey(prototype.GetType()))
            {
                _prototypesByType[prototype.GetType()] = new List<IResource>();
            }
            _prototypesByType[prototype.GetType()].Add(prototype);
        }

        private void LoadTokenPrototypes(IDbConnection dbconn)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from token_prototypes_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {

                var uniqueId = reader.GetString(0);
                var width = reader.GetInt32(1);
                var height = reader.GetInt32(2);
                var shapeType = (ShapeType)reader.GetInt32(3);

                var behaviourUniqueId = reader.GetString(4);
                var equipmentUniqueId = reader.GetString(5);
                var inventoryUniqueId = reader.GetString(6);
                var motorUniqueId = reader.GetString(7);
                var personaUniqueId = reader.GetString(8);
                var triggerBehaviourUniqueId = reader.GetString(9);
                var tokenViewBehaviourUniqueId = reader.GetString(10);

                var behaviourPrototype = GetPrototypeByUniqueIdentifier<BehaviourPrototype>(behaviourUniqueId);
                var equipmentPrototype = GetPrototypeByUniqueIdentifier<EquipmentPrototype>(equipmentUniqueId);
                var inventoryPrototype = GetPrototypeByUniqueIdentifier<InventoryPrototype>(inventoryUniqueId);
                var motorPrototype = GetPrototypeByUniqueIdentifier<MotorPrototype>(motorUniqueId);
                var personaPrototype = GetPrototypeByUniqueIdentifier<PersonaPrototype>(personaUniqueId);
                var triggerBehaviourPrototype = GetPrototypeByUniqueIdentifier<TriggerBehaviourPrototype>(triggerBehaviourUniqueId);
                var tokenViewPrototype = GetPrototypeByUniqueIdentifier<TokenViewPrototype>(tokenViewBehaviourUniqueId);

                TokenPrototype prototype = new TokenPrototype()
                {
                    UniqueIdentifier = uniqueId,
                    Width = width,
                    Height = height,
                    ShapeType = shapeType,
                    BehaviourPrototype = behaviourPrototype,
                    EquipmentPrototype = equipmentPrototype,
                    InventoryPrototype = inventoryPrototype,
                    MotorPrototype = motorPrototype,
                    PersonaPrototype = personaPrototype,
                    TriggerBehaviourPrototype = triggerBehaviourPrototype,
                    TokenViewPrototype = tokenViewPrototype
                };
                CacheResource(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        public TPrototype GetPrototypeByUniqueIdentifier<TPrototype>(string uniqueIdentifier) where TPrototype : IResource
        {
            try
            {

                if (!_prototypesByUniqueIdentifier.ContainsKey(uniqueIdentifier))
                {
                    throw new CouldNotResolvePrototypeException(string.Format("Failed to lookup prototype: {0}", uniqueIdentifier));
                }

                return (TPrototype)_prototypesByUniqueIdentifier[uniqueIdentifier];
                
            }
            catch (InvalidCastException ex)
            {
                throw new CouldNotResolvePrototypeException(string.Format("Found prototype for {0}, but it is not a {1}", uniqueIdentifier, typeof(TPrototype).Name));
            }
        }

        public List<TPrototype> GetPrototypes<TPrototype>() where TPrototype : IResource
        {
            if(!_prototypesByType.ContainsKey(typeof(TPrototype)))
            {
                return new List<TPrototype>(0);
            }
            var returnList = new List<TPrototype>();
            foreach(var ires in _prototypesByType[typeof(TPrototype)])
            {
                returnList.Add((TPrototype)ires);
            }
            return returnList;
        }
    }
}
