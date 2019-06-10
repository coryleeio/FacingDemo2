namespace Gamepackage
{
    public static class EnchantmentFactory
    {
        public static Enchantment Build(string uniqueIdentifier)
        {
            var enchantmentTemplate = Context.ResourceManager.Load<EnchantmentTemplate>(uniqueIdentifier);
            var enchantment = new Enchantment()
            {
                EnchantmentIdentifier = uniqueIdentifier,
                NumberOfChargesRemaining = MathUtil.ChooseRandomIntInRange(enchantmentTemplate.MinCharges, enchantmentTemplate.MaxCharges)
            };

            return enchantment;
        }
    }
}
