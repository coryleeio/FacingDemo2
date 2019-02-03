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

        // Effects that are applied by all attacks no matter the type
        // also includes stat changing effects, immunities etc
        // anything that is not specific to a particular attack.
        public List<Effect> EffectsGlobal;

        // Percentage chance /100 that the item survives being shot or thrown
        // if it survives it will appear on the ground wherever it was thrown or shot to
        public int ChanceToSurviveLaunch;
        
        // For ammo it determines what type of ammo the item is
        // For ranged items it determines what type of ammo is fired
        public AmmoType AmmoType;

        public Dictionary<AttackType, AttackTypeParameters> AttackTypeParameters = new Dictionary<AttackType, AttackTypeParameters>();

        public bool IsUsable
        {
            get
            {
                return AttackTypeParameters.ContainsKey(AttackType.ApplyToSelf) && AttackTypeParameters[AttackType.ApplyToSelf].AttackParameters.Count > 0;
            }
        }

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

        public bool CanBeUsedInAttackType(AttackType attackType)
        {
            if(attackType == AttackType.Melee)
            {
                return AttackTypeParameters.ContainsKey(AttackType.Melee);
            }
            else if(attackType == AttackType.Ranged)
            {
                return AttackTypeParameters.ContainsKey(AttackType.Ranged) && AmmoType != AmmoType.None;
            }
            else if(attackType == AttackType.Thrown)
            {
                return AttackTypeParameters.ContainsKey(AttackType.Thrown);
            }
            else if(attackType == AttackType.Zapped)
            {
                return AttackTypeParameters.ContainsKey(AttackType.Zapped) && HasCharges;
            }
            else if (attackType == AttackType.ApplyToSelf)
            {
                return AttackTypeParameters.ContainsKey(AttackType.ApplyToSelf) && HasCharges;
            }
            else if(attackType == AttackType.ApplyToOther)
            {
                return AttackTypeParameters.ContainsKey(AttackType.ApplyToOther);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public bool CanStack(Item other)
        {
            return UniqueIdentifier == other.UniqueIdentifier && (NumberOfItems + other.NumberOfItems < MaxStackSize);
        }
    }
}
