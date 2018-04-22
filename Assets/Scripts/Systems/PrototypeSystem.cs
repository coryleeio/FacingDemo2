using System.Collections.Generic;
using System.Data;

namespace Gamepackage
{
    public class PrototypeSystem : IPrototypeSystem
    {
        private Dictionary<string, IPrototype> _prototypesByUniqueIdentifier = new Dictionary<string, IPrototype>();
        private ILogSystem _logSystem;

        public PrototypeSystem(ILogSystem logSystem)
        {
            _logSystem = logSystem;
        }

        public void LoadAllPrototypes(IDbConnection dbConnection)
        {
            _prototypesByUniqueIdentifier.Clear();
            LoadItemPrototypes(dbConnection);
            LoadTokenPrototypes(dbConnection);
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
                SavePrototype(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        private void SavePrototype(IPrototype prototype)
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
                    ShapeType = (ShapeType) reader.GetInt32(3),
                    BehaviourClassName = reader.GetString(4),
                    EquipmentClassName = reader.GetString(5),
                    InventoryClassName = reader.GetString(6),
                    MotorClassName = reader.GetString(7),
                    PersonaClassName = reader.GetString(8),
                    TriggerBehaviourClassName = reader.GetString(9),
                    ViewClassName = reader.GetString(10)
                };
                SavePrototype(prototype);
            }
            reader.Close();
            dbcmd.Dispose();
        }

        public TPrototype GetPrototypeByUniqueIdentifier<TPrototype>(string uniqueIdentifier) where TPrototype : IPrototype
        {
            if (!_prototypesByUniqueIdentifier.ContainsKey(uniqueIdentifier))
            {
                throw new CouldNotResolvePrototypeException(string.Format("Failed to lookup prototype: {0}", uniqueIdentifier));
            }
            return (TPrototype)_prototypesByUniqueIdentifier[uniqueIdentifier];
        }
    }
}
