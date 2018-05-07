
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TinyIoC;

namespace Gamepackage
{
    [Serializable]
    public class Token : IHandleMessage, IWasBuiltFromAPrototype, IPlaceable, IHaveAnId, IResolvableReferences
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
                TokenView.MapPosition = value;
            }
            get
            {
                return Shape.Position;
            }
        }

        public int Id { get; set; }

        public Shape Shape;

        public List<string> Tags = new List<string>();

        public Motor Motor;

        public Inventory Inventory;

        public Equipment Equipment;

        public Behaviour Behaviour;

        public TokenView TokenView;

        public Persona Persona;

        public TriggerBehaviour TriggerBehaviour;

        public string PrototypeUniqueIdentifier { get; set; }

        [JsonIgnore]
        public Rectangle BoundingRectangle
        {
            get
            {
                return Shape.BoundingRectangle;
            }
        }

        [JsonIgnore]
        public Rectangle Rectangle
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public void HandleMessage(Message messageToHandle)
        {
            Motor.HandleMessage(messageToHandle);
            Inventory.HandleMessage(messageToHandle);
            Equipment.HandleMessage(messageToHandle);
            Behaviour.HandleMessage(messageToHandle);
            TokenView.HandleMessage(messageToHandle);
            Persona.HandleMessage(messageToHandle);
            TriggerBehaviour.HandleMessage(messageToHandle);
        }

        public void Resolve(IResourceManager resourceManager)
        {
            Behaviour.Owner = this;
            Equipment.Owner = this;
            Inventory.Owner = this;
            Motor.Owner = this;
            Persona.Owner = this;
            TriggerBehaviour.Owner = this;
            TokenView.Owner = this;
            Motor.Resolve(resourceManager);
            Inventory.Resolve(resourceManager);
            Equipment.Resolve(resourceManager);
            Behaviour.Resolve(resourceManager);
            TokenView.Resolve(resourceManager);
            Persona.Resolve(resourceManager);
            TriggerBehaviour.Resolve(resourceManager);
            Shape.Recalculate();
        }

        public void PrepareForPlay(TinyIoCContainer container, ITokenSystem tokenSystem)
        {
            container.BuildUp(Behaviour);
            container.BuildUp(Equipment);
            container.BuildUp(Inventory);
            container.BuildUp(Motor);
            container.BuildUp(Persona);
            container.BuildUp(TriggerBehaviour);
            container.BuildUp(TokenView);
            tokenSystem.Register(this);
        }
    }
}