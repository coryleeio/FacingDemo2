using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class Item
    {
        public abstract UniqueIdentifier UniqueIdentifier
        {
            get;
        }

        public abstract UniqueIdentifier ItemAppearanceIdentifier
        {
            get;
        }

        public abstract string DisplayName
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public Item()
        {
            this.NumberOfItems = DefaultStackSize;
        }

        public abstract List<ItemSlot> SlotsWearable
        {
            get;
        }

        public abstract List<ItemSlot> SlotsOccupiedByWearing
        {
            get;
        }

        public abstract int MaxStackSize
        {
            get;
        }

        public abstract int DefaultStackSize
        {
            get;
        }

        public abstract Dictionary<Attributes, int> Attributes
        {
            get;
        }

        public abstract List<Ability> Abilities
        {
            get;
        }

        public abstract List<AttackParameters> AttackParameters
        {
            get;
        }

        public abstract List<AttackParameters> ThrowParameters
        {
            get;
        }

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

        public int NumberOfItems;
    }
}
