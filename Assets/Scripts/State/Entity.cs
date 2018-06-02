
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    [Serializable]
    public class Entity : IHasApplicationContext
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

        private ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
            if(Trigger != null)
            {
                Trigger.Entity = this;
                Trigger.InjectContext(context);
            }
            if(Body != null)
            {
                Body.Entity = this;
                Body.InjectContext(context);
            }
            if(View != null)
            {
                View.Entity = this;
                View.InjectContext(context);
            }
            if(Motor != null)
            {
                Motor.Entity = this;
                Motor.InjectContext(context);
            }
            if(Behaviour != null)
            {
                Behaviour.Entity = this;
                Behaviour.InjectContext(context);
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