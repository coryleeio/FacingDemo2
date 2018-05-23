
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    [Serializable]
    public class Token : IHasApplicationContext
    {
        public Token() {}

        public Point Position;

        public int TimeAccrued = 0;

        [JsonIgnore]
        public Vector2 LerpCurrentPosition;

        [JsonIgnore]
        public Vector2 LerpTargetPosition;

        [JsonIgnore]
        public float ElapsedMovementTime;

        [JsonIgnore]
        public Queue<TokenAction> ActionQueue= new Queue<TokenAction>(0);

        public bool IsMoving;

        public bool NeedToCheckIfMovementTriggeredTriggers = false;

        public Queue<Point> CurrentPath = new Queue<Point>(0);

        public bool HasActed = false;

        public Point TargetPosition;

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        public int Id { get; set; }

        public List<Traits> Traits = new List<Traits>();

        public List<Inventory> Inventory = new List<Inventory>(0);

        public Dictionary<ItemSlot, ItemPrototypes> Equipment = new Dictionary<ItemSlot, ItemPrototypes>();

        public AIBehaviourType BehaviorIdentifier;

        public ViewType ViewType;

        public UniqueIdentifier ViewUniqueIdentifier;

        public Trigger Trigger;

        public bool TriggerHasBeenChecked = false;
        public bool IsTriggering = false;

        public bool IsPlayer
        {
            get
            {
                return Traits.Contains(Gamepackage.Traits.Player);
            }
        }

        [JsonIgnore]
        public GameObject View;

        public bool IsVisible;

        private ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
            if(Trigger != null)
            {
                Trigger.InjectContext(context);
            }
        }
    }
}