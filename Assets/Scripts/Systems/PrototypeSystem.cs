using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gamepackage
{
    public class PrototypeSystem : IPrototypeSystem
    {
        private Dictionary<string, IPrototype> _prototypesByUniqueIdentifier = new Dictionary<string, IPrototype>();
        private ILogSystem _logSystem;

        public PrototypeSystem(ILogSystem logSystem)
        {

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

            dbcmd.CommandText = @"SELECT * from item_prototypes;";

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
            _prototypesByUniqueIdentifier[prototype.UniqueIdentifier] = prototype;
        }

        private void LoadTokenPrototypes(IDbConnection dbconn)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"SELECT * from token_prototypes;";

            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                TokenPrototype prototype = new TokenPrototype()
                {
                    UniqueIdentifier = reader.GetString(0),
                    BehaviourClassName = reader.GetString(1),
                    EquipmentClassName = reader.GetString(2),
                    InventoryClassName = reader.GetString(3),
                    MotorClassName = reader.GetString(4),
                    PersonaClassName = reader.GetString(5),
                    TriggerBehaviourClassName = reader.GetString(6),
                    ViewClassName = reader.GetString(7)
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
