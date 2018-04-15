using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class PrototypeFactory : IPrototypeFactory
    {
        private Dictionary<string, Type> componentTypeMap = new Dictionary<string, Type>();
        private IPrototypeSystem _prototypeSystem;

        public PrototypeFactory(IPrototypeSystem prototypeSystem)
        {
            _prototypeSystem = prototypeSystem;
        }

        public void LoadTypes()
        {
            componentTypeMap.Clear();
            var types = typeof(IComponent).ConcreteFromInterface();
            foreach (var type in types)
            {
                componentTypeMap[type.Name] = type;
            }
        }

        public Token Build(TokenPrototype prototype)
        {
            var token = new Token
            {
                Behaviour = Activator.CreateInstance(componentTypeMap[prototype.BehaviourClassName]) as Behaviour,
                Equipment = Activator.CreateInstance(componentTypeMap[prototype.EquipmentClassName]) as Equipment,
                Inventory = Activator.CreateInstance(componentTypeMap[prototype.InventoryClassName]) as Inventory,
                Motor = Activator.CreateInstance(componentTypeMap[prototype.MotorClassName]) as Motor,
                Persona = Activator.CreateInstance(componentTypeMap[prototype.PersonaClassName]) as Persona,
                TriggerBehaviour = Activator.CreateInstance(componentTypeMap[prototype.TriggerBehaviourClassName]) as TriggerBehaviour,
                View = Activator.CreateInstance(componentTypeMap[prototype.ViewClassName]) as View
            };
            return token;
        }

        public Token Build(string uniqueIdentifier)
        {
            var prototype = _prototypeSystem.GetPrototypeByUniqueIdentifier<TokenPrototype>(uniqueIdentifier);
            return Build(prototype);
        }
    }
}

