
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

        public bool IsDoneThisTurn = false;

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        [JsonIgnore]
        private EntityPrototype _entityPrototype;

        [JsonIgnore]
        public EntityPrototype EntityPrototype
        {
            get
            {
                if(_entityPrototype == null)
                {
                    _entityPrototype = Context.ResourceManager.GetPrototype<EntityPrototype>(PrototypeIdentifier);
                }
                return _entityPrototype;
            }
        }

        public List<Traits> Traits = new List<Traits>();

        public List<Inventory> Inventory = new List<Inventory>(0);

        public Dictionary<ItemSlot, ItemPrototypes> Equipment = new Dictionary<ItemSlot, ItemPrototypes>();

        public AIBehaviourType BehaviorIdentifier;

        public ViewType ViewType;

        public UniqueIdentifier ViewUniqueIdentifier;

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

        [JsonIgnore]
        public GameObject View;

        public bool IsVisible;

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
        }
    }
}