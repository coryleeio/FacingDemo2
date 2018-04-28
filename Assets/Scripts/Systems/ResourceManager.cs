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
            LoadLevelPrototypes(dbConnection);
            LoadRoomPrototypes(dbConnection);
            LoadLevelPrototypeRoomContent(dbConnection);
            LoadLevelPrototypeSpawnContent(dbConnection);
        }

        private void LoadLevelPrototypeSpawnContent(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from level_prototype_spawn_content_view";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var levelPrototypeUniqueId = reader.GetString(1);
                var spawnTableUniqueIdentifier = reader.GetString(3);
                string tagConstraint = null;
                if(!reader.IsDBNull(4))
                {
                    tagConstraint = reader.GetString(4);
                }

                var levelPrototype = GetPrototypeByUniqueIdentifier<LevelPrototype>(levelPrototypeUniqueId);
                var spawnTable = GetPrototypeByUniqueIdentifier<SpawnTable>(spawnTableUniqueIdentifier);

                levelPrototype.Spawns.Add(new LevelSpawnPrototype()
                {
                    SpawnTable = spawnTable,
                    TagConstraint = tagConstraint
                });
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void LoadLevelPrototypeRoomContent(IDbConnection dbConnection)
        {
            IDbCommand dbcmd = dbConnection.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from level_prototype_room_content_view";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var levelPrototypeUniqueId = reader.GetString(1);
                var roomPrototypeUniqueIdentifier = reader.GetString(3);
                var tagsList = new List<string>();
                if(!reader.IsDBNull(4))
                {
                    var tags = reader.GetString(4);
                    tagsList.AddRange(tags.Split(','));
                }

                var levelPrototype = GetPrototypeByUniqueIdentifier<LevelPrototype>(levelPrototypeUniqueId);
                var roomPrototype = GetPrototypeByUniqueIdentifier<RoomPrototype>(roomPrototypeUniqueIdentifier);

                var levelRoomPrototype = new LevelRoomPrototype()
                {
                    RoomPrototype = roomPrototype
                };
                levelRoomPrototype.Tags.AddRange(tagsList);
                levelPrototype.Rooms.Add(levelRoomPrototype);
            }
            reader.Close();
            dbcmd.Dispose();
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
                };
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
                var uniqueId = reader.GetString(1);
                var defaultSpawnTableUniqueId = reader.GetString(3);
                var defaultTilesetUniqueId = reader.GetString(5);
                var defaultSpawnTable = GetPrototypeByUniqueIdentifier<SpawnTable>(defaultSpawnTableUniqueId);
                var defaultTileSet = GetPrototypeByUniqueIdentifier<Tileset>(defaultTilesetUniqueId);
                LevelPrototype prototype = new LevelPrototype()
                {
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

        private void LoadSpawnTables(IDbConnection dbconn)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from spawn_table_entries_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var uniqueId = reader.GetString(1);
                var resolutionStr = reader.GetString(2);
                var resolution = (TableResolutionStrategy)System.Enum.Parse(typeof(TableResolutionStrategy), resolutionStr);
                var weight = reader.GetInt32(3);
                var itemPrototypeUniqueIdentifier = reader.GetString(5);
                var numberOfRolls = reader.GetInt32(6);

                var itemPrototype = GetPrototypeByUniqueIdentifier<TokenPrototype>(itemPrototypeUniqueIdentifier);

                SpawnTable equipmentTable = null;
                if (!_prototypesByUniqueIdentifier.ContainsKey(uniqueId))
                {
                    equipmentTable = new SpawnTable()
                    {
                        UniqueIdentifier = uniqueId,
                        ProbabilityTable = new ProbabilityTable<TokenPrototype>()
                        {
                            Resolution = resolution
                        }
                    };
                }
                else
                {
                    equipmentTable = _prototypesByUniqueIdentifier[uniqueId] as SpawnTable;
                }
                equipmentTable.ProbabilityTable.Values.Add(new ProbabilityTableTuple<TokenPrototype>()
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

        private void CacheResource(IResource prototype)
        {
            if (_prototypesByUniqueIdentifier.ContainsKey(prototype.UniqueIdentifier))
            {
                throw new DuplicatePrototypeIdException(string.Format("Duplicate prototype: {0}", prototype.UniqueIdentifier));
            }
            _logSystem.Log("Found prototype: " + prototype.UniqueIdentifier);
            _prototypesByUniqueIdentifier[prototype.UniqueIdentifier] = prototype;
        }

        private void LoadTokenPrototypes(IDbConnection dbconn)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from token_prototypes_view;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                TokenPrototype prototype = new TokenPrototype()
                {
                    UniqueIdentifier = reader.GetString(0),
                    Width = reader.GetInt32(1),
                    Height = reader.GetInt32(2),
                    ShapeType = (ShapeType)reader.GetInt32(3),
                    BehaviourClassName = reader.GetString(4),
                    EquipmentClassName = reader.GetString(5),
                    InventoryClassName = reader.GetString(6),
                    MotorClassName = reader.GetString(7),
                    PersonaClassName = reader.GetString(8),
                    TriggerBehaviourClassName = reader.GetString(9),
                    ViewClassName = reader.GetString(10)
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
    }
}
