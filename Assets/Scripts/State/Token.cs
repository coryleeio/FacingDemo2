
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    [Serializable]
    public class Token
    {
        public Token() {}

        public Point Position;

        [JsonIgnore]
        public Pointf LerpPosition;

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        public int Id { get; set; }

        public List<Traits> Traits = new List<Traits>();

        public List<Inventory> Inventory = new List<Inventory>(0);

        public Dictionary<ItemSlot, ItemPrototypes> Equipment = new Dictionary<ItemSlot, ItemPrototypes>();

        public AIBehaviourType BehaviorIdentifier;

        public ViewType ViewType;

        public UniqueIdentifier TriggerPrototypeUniqueIdentifier;

        public UniqueIdentifier ViewUniqueIdentifier;

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
    }
}