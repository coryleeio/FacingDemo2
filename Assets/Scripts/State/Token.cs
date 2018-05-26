
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

        public Point SortingPosition;

        public int TimeAccrued = 0;

        public Pointf LerpCurrentPosition;

        public Pointf LerpTargetPosition;

        public float ElapsedMovementTime;

        public Queue<TokenAction> ActionQueue = new Queue<TokenAction>(0);

        public bool IsMoving;

        public Queue<Point> CurrentPath = new Queue<Point>(0);

        public bool IsDoneThisTurn = false;

        public Point TargetPosition;

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        [JsonIgnore]
        private TokenPrototype _tokenPrototype;

        [JsonIgnore]
        public TokenPrototype TokenPrototype
        {
            get
            {
                if(_tokenPrototype == null)
                {
                    _tokenPrototype = Context.ResourceManager.GetPrototype<TokenPrototype>(PrototypeIdentifier);
                }
                return _tokenPrototype;
            }
        }

        public int Id { get; set; }

        public List<Traits> Traits = new List<Traits>();

        public List<Inventory> Inventory = new List<Inventory>(0);

        public Dictionary<ItemSlot, ItemPrototypes> Equipment = new Dictionary<ItemSlot, ItemPrototypes>();

        public AIBehaviourType BehaviorIdentifier;

        public ViewType ViewType;

        public UniqueIdentifier ViewUniqueIdentifier;

        public UniqueIdentifier TriggerPrototypeUniqueIdentifier;

        public Trigger Trigger;

        public bool HasMovedSinceLastTriggerCheck = false;

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
            foreach(var action in ActionQueue)
            {
                action.InjectContext(Context);
            }
        }
    }
}