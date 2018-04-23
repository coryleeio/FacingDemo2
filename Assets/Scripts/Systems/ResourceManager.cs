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
            LoadTokenPrototypes(dbConnection);
            LoadTilesets(dbConnection);
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
            if (!_prototypesByUniqueIdentifier.ContainsKey(uniqueIdentifier))
            {
                throw new CouldNotResolvePrototypeException(string.Format("Failed to lookup prototype: {0}", uniqueIdentifier));
            }
            return (TPrototype)_prototypesByUniqueIdentifier[uniqueIdentifier];
        }
    }
}
