using Mono.Data.Sqlite;
using System;
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
            LoadTokenPrototypes(dbConnection);
        }

        private void LoadTokenPrototypes(IDbConnection dbconn)
        {
            _prototypesByUniqueIdentifier.Clear();
            IDbCommand dbcmd = dbconn.CreateCommand();
            IDataReader reader;

            dbcmd.CommandText = @"
                    SELECT    
                                unique_identifier_id, 
                                behaviour_prototypes.class_name AS behaviour_class_name, 
                                equipment_prototypes.class_name AS equipment_class_name,
                                inventory_prototypes.class_name as inventory_class_name,
                                motor_prototypes.class_name as motor_class_name,
                                persona_prototypes.class_name as persona_class_name,
                                trigger_behaviour_prototypes.class_name as trigger_behaviour_class_name,
                                view_prototypes.class_name as view_class_name
                    FROM      token_prototypes 
                    LEFT JOIN behaviour_prototypes, 
                                equipment_prototypes, 
                                inventory_prototypes,
                                motor_prototypes,
                                persona_prototypes,
                                trigger_behaviour_prototypes,
                                view_prototypes
                    WHERE     behaviour_prototypes.id = token_prototypes.behaviour_prototype_id 
                    AND       equipment_prototypes.id = token_prototypes.equipment_prototype_id
                    AND       inventory_prototypes.id = token_prototypes.inventory_prototype_id
                    AND       motor_prototypes.id = token_prototypes.motor_prototype_id
                    AND       persona_prototypes.id = token_prototypes.persona_prototype_id
                    AND       trigger_behaviour_prototypes.id = token_prototypes.trigger_behaviour_prototype_id
                    AND       view_prototypes.id = token_prototypes.view_prototype_id
            ;";

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
                if(_prototypesByUniqueIdentifier.ContainsKey(prototype.UniqueIdentifier))
                {
                    throw new DuplicatePrototypeIdException(string.Format("Duplicate prototype: {0}", prototype.UniqueIdentifier));
                }
                _prototypesByUniqueIdentifier[prototype.UniqueIdentifier] = prototype;
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
