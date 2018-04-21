
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gamepackage
{
    [Serializable]
    public class Token : IHandleMessage, IWasBuiltFromAPrototype, IHaveAnId
    {
        public Token()
        {

        }

        [JsonIgnore]
        public Point Position
        {
            set
            {
                Shape.Position = value;
            }
        }

        public int Id { get; set; }

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