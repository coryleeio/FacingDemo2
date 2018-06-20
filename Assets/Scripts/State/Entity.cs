
using Newtonsoft.Json;
using System;

namespace Gamepackage
{
    [Serializable]
    public class Entity
    {
        public Entity() { }

        public int Id { get; set; }

        public string Name;

        public Point Position;

        public Body Body;

        public Trigger Trigger;

        public Motor Motor;

        public View View;

        public Behaviour Behaviour;

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        [JsonIgnore]
        public bool IsPlayer
        {
            get
            {
                return Behaviour != null && Behaviour.IsPlayer;
            }
        }

        [JsonIgnore]
        public bool IsNPC
        {
            get
            {
                return Behaviour != null && !Behaviour.IsPlayer;
            }
        }

        [JsonIgnore]
        public bool IsCombatant
        {
            get
            {
                return Body != null;
            }
        }

        public void Rereference()
        {
            if(Trigger != null)
            {
                Trigger.Rereference(this);
            }
            if(Body != null)
            {
                Body.Rereference(this);
            }
            if(View != null)
            {
                View.Rereference(this);
            }
            if(Motor != null)
            {
                Motor.Rereference(this);
            }
            if(Behaviour != null)
            {
                Behaviour.Rereference(this);
            }
        }

        [JsonIgnore]
        private EntityPrototype _entityPrototype;

        [JsonIgnore]
        public EntityPrototype EntityPrototype
        {
            get
            {
                if (_entityPrototype == null)
                {
                    _entityPrototype = ServiceLocator.ResourceManager.GetPrototype<EntityPrototype>(PrototypeIdentifier);
                }
                return _entityPrototype;
            }
        }
    }
}