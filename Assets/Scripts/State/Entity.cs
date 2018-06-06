
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    [Serializable]
    public class Entity
    {
        public Entity() { }

        public int Id { get; set; }

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
                return Behaviour != null && Behaviour.BehaviourImpl != null && Behaviour.BehaviourImpl.IsPlayer;
            }
        }

        [JsonIgnore]
        public bool IsNPC
        {
            get
            {
                return Behaviour != null && Behaviour.BehaviourImpl != null && !Behaviour.BehaviourImpl.IsPlayer;
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

        public void InjectContext()
        {
            if(Trigger != null)
            {
                Trigger.InjectContext(this);
            }
            if(Body != null)
            {
                Body.InjectContext(this);
            }
            if(View != null)
            {
                View.InjectContext(this);
            }
            if(Motor != null)
            {
                Motor.InjectContext(this);
            }
            if(Behaviour != null)
            {
                Behaviour.InjectContext(this);
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
                    _entityPrototype = Context.ResourceManager.GetPrototype<EntityPrototype>(PrototypeIdentifier);
                }
                return _entityPrototype;
            }
        }
    }
}