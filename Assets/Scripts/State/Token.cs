
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

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        public int Id { get; set; }

        public List<string> Tags = new List<string>();

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
                return Tags.Contains(GameTags.Player);
            }
        }

        [JsonIgnore]
        public GameObject View;

        public bool IsVisible;
    }
}