namespace Gamepackage
{
    public enum Tags
    {
        // This items effects should be evaluated from the inventory, by default items in the inventory are ignored
        // for the purposes of evaluating their effects
        ItemEffectsApplyFromInventory,

        // Possessing this tag means no poison damage hurts you, and all poison is removed.
        PoisonImmunity,
    }
}
