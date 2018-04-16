
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class Token : IHandleMessage, IWasBuiltFromAPrototype, IHaveAnId
    {
        public Token()
        {

        }

        public int Id { get; set; }

        [JsonIgnore]
        public Point Position
        {
            set
            {
                Shape.Position = value;
            }
        }

        public Shape Shape;

        public List<string> Tags = new List<string>();

        public Motor Motor;

        public Inventory Inventory;

        public Equipment Equipment;

        public Behaviour Behaviour;

        public View View;

        public Persona Persona;

        public TriggerBehaviour TriggerBehaviour;

        public string PrototypeUniqueIdentifier { get; set; }

        public void HandleMessage(Message messageToHandle)
        {
            Motor.HandleMessage(messageToHandle);
            Inventory.HandleMessage(messageToHandle);
            Equipment.HandleMessage(messageToHandle);
            Behaviour.HandleMessage(messageToHandle);
            View.HandleMessage(messageToHandle);
            Persona.HandleMessage(messageToHandle);
            TriggerBehaviour.HandleMessage(messageToHandle);
        }
    }
}