using Newtonsoft.Json;

namespace Gamepackage
{
    public class Enchantment
    {
        public int NumberOfChargesRemaining;
        public string EnchantmentIdentifier;
        [JsonIgnore]
        public EnchantmentTemplate Template
        {
            get
            {
                if (EnchantmentIdentifier == null || EnchantmentIdentifier == "")
                {
                    return null;
                }
                return Context.ResourceManager.Load<EnchantmentTemplate>(EnchantmentIdentifier);
            }
        }

        [JsonIgnore]
        public bool HasCharges
        {
            get
            {
                // Setting min and max charges to -1 will result in an item with unlimited charges.
                // otherwise you cant use the item once charges hits 0
                return HasUnlimitedCharges || NumberOfChargesRemaining > 0;
            }
        }

        [JsonIgnore]
        public bool HasUnlimitedCharges
        {
            get
            {
                return NumberOfChargesRemaining == -1;
            }
        }
    }
}
