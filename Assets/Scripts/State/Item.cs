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

        // Percentage chance /100 that the item survives being shot or thrown
        // if it survives it will appear on the ground wherever it was thrown or shot to
        public int ChanceToSurviveLaunch;
        

        // For ammo it determines what type of ammo the item is
        // For ranged items it determines what type of ammo is fired
        public AmmoType AmmoType;

        public List<AttackParameters> MeleeParameters;
        public int MeleeRange;
        public int MeleeTargetsPierced;

        public List<AttackParameters> ThrowParameters;
        public int ThrownRange;
        public int ThrownTargetsPierced;

        public List<AttackParameters> RangedParameters;
        public int RangedRange;
        public int RangedTargetsPierced;

        public List<AttackParameters> ZapParameters;
        public int ZapRange;
        public int ZappedTargetsPierced;

        public bool IsUsable;
        public string OnUseText;
        public string CustomOnUseText = null;

        public bool HasCharges
        {
            get
            {
                return HasUnlimitedCharges || ExactNumberOfChargesRemaining > 0;
            }
        }

        public bool HasUnlimitedCharges;
        public int ExactNumberOfChargesRemaining;
        public bool DestroyWhenAllChargesAreConsumed;

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

        public bool CanBeUsedInMelee
        {
            get
            {
                return MeleeParameters.Count > 0;
            }
        }

        public bool CanBeThrown
        {
            get
            {
                return ThrowParameters.Count > 0;
            }
        }

        public bool CanBeUsedForRanged
        {
            get
            {
                return RangedParameters.Count > 0 && AmmoType != AmmoType.None;
            }
        }

        public bool CanBeUsedForZap
        {
            get
            {
                return ZapParameters.Count > 0 && HasCharges;
            }
        }

        public bool CanStack(Item other)
        {
            return UniqueIdentifier == other.UniqueIdentifier && (NumberOfItems + other.NumberOfItems < MaxStackSize);
        }
    }
}
