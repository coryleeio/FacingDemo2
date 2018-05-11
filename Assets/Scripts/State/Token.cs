
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
                if(Level != null)
                {
                    Level.UnindexToken(this, Position);
                    Level.IndexToken(this, value);
                }
                Shape.Position = value;
                // Dont set the position on the view here because
                // sorting differs depending on the direction of your movement.
                // as such you need to be able to update the token position
                // before or after a move.
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
        private Level _level;

        [JsonIgnore]
        public Level Level
        {
            get
            {
                return _level;
            }
            set
            {
                if(_level != null)
                {
                    _level.UnindexToken(this, Position);
                }
                _level = value;
                if(value != null)
                {
                    _level.IndexToken(this, Position);
                }
            }
        }

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

        public bool IsPlayer()
        {
            return Tags.Contains(GameTags.Player);
        }

        public void Resolve(TinyIoCContainer container)
        {
            ITokenSystem tokenSystem = container.Resolve<ITokenSystem>();
            Behaviour.Owner = this;
            Equipment.Owner = this;
            Inventory.Owner = this;
            Motor.Owner = this;
            Persona.Owner = this;
            TriggerBehaviour.Owner = this;
            TokenView.Owner = this;

            Motor.Resolve(container);
            Inventory.Resolve(container);
            Equipment.Resolve(container);
            Behaviour.Resolve(container);
            TokenView.Resolve(container);
            Persona.Resolve(container);
            TriggerBehaviour.Resolve(container);

            container.BuildUp(Behaviour);
            container.BuildUp(Equipment);
            container.BuildUp(Inventory);
            container.BuildUp(Motor);
            container.BuildUp(Persona);
            container.BuildUp(TriggerBehaviour);
            container.BuildUp(TokenView);

            Shape.Recalculate();
            tokenSystem.Register(this);
        }
    }
}