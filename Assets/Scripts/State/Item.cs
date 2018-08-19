using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class Item
    {
        public UniqueIdentifier UniqueIdentifier;
        public UniqueIdentifier ItemAppearanceIdentifier;
        public string DisplayName;
        public string Description;
        public List<ItemSlot> SlotsWearable;
        public List<ItemSlot> SlotsOccupiedByWearing;
        public int MaxStackSize;
        public Dictionary<Attributes, int> Attributes;
        public List<Effect> Effects;
        public List<AttackParameters> AttackParameters;
        public List<AttackParameters> ThrowParameters;

        [JsonIgnore]
        public Vector3 CorpsePositionOffset
        {
            get
            {
                return new Vector3(0f, -0.2f, 0f);
            }
        }

        [JsonIgnore]
        public Vector3 CorpseIconEulerAngles
        {
            get
            {
                return new Vector3(0, 0, 30);
            }
        }

        [JsonIgnore]
        public Vector3 CorpseIconScale
        {
            get
            {
                return new Vector3(0.5f, 0.5f, 0.5f);
            }
        }

        public int NumberOfItems;

        [JsonIgnore]
        public ItemAppearance ItemAppearance
        {
            get
            {
                return Context.ResourceManager.GetPrototype<ItemAppearance>(ItemAppearanceIdentifier);
            }
        }

        public bool CanBeUsedInMelee()
        {
            return AttackParameters.Count > 0;
        }

        public bool CanBeThrown()
        {
            return ThrowParameters.Count > 0;
        }

        public bool CanStack(Item other)
        {
            return UniqueIdentifier == other.UniqueIdentifier && (NumberOfItems + other.NumberOfItems < MaxStackSize);
        }
    }
}
