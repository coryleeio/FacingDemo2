using System;
using System.Collections.Generic;
using TinyIoC;

namespace Gamepackage
{
    public class PrototypeFactory : IPrototypeFactory
    {
        private Dictionary<string, Type> componentTypeMap = new Dictionary<string, Type>();
        private IPrototypeSystem _prototypeSystem;
        private TinyIoCContainer _container;
        private ITokenSystem _tokenSystem;

        public PrototypeFactory(IPrototypeSystem prototypeSystem, TinyIoCContainer container, ITokenSystem tokenSystem)
        {
            _prototypeSystem = prototypeSystem;
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
            var prototype = _prototypeSystem.GetPrototypeByUniqueIdentifier<TokenPrototype>(uniqueIdentifier);
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
            var prototype = _prototypeSystem.GetPrototypeByUniqueIdentifier<ItemPrototype>(uniqueIdentifier);
            return BuildItem(prototype);
        }
    }
}

