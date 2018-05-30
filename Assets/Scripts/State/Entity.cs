
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

        public CombatantComponent CombatantComponent;

        public TriggerComponent TriggerComponent;

        public MovementComponent MovementComponent;

        public ViewComponent ViewComponent;

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        public bool IsDoneThisTurn = false;

        public List<Traits> Traits = new List<Traits>();

        [JsonIgnore]
        public bool IsPlayer
        {
            get
            {
                return Traits.Contains(Gamepackage.Traits.Player);
            }
        }

        [JsonIgnore]
        public bool IsCombatant
        {
            get
            {
                return Traits.Contains(Gamepackage.Traits.Combatant);
            }
        }

        private ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
            if(TriggerComponent != null)
            {
                TriggerComponent.InjectContext(context);
            }
            if(CombatantComponent != null)
            {
                CombatantComponent.InjectContext(context);
            }
            if(ViewComponent != null)
            {
                ViewComponent.InjectContext(context);
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