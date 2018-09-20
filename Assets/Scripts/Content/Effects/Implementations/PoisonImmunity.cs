namespace Gamepackage
{
    public class PoisonImmunity : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "Cures poison";
            }
        }

        public override string Description
        {
            get
            {
                return "Removes all poisons from your body.";
            }
        }

        public override string RemovalText
        {
            get
            {
                return "You are no longer immune to poison...";
            }
        }

        public override void OnApply(Entity owner)
        {
            base.OnApply(owner);
            var effectsToRemove = CombatUtil.GetEntityEffectsByType(owner, (effectInQuestion) => { return effectInQuestion is Poison; });
            CombatUtil.RemoveEntityEffects(owner, effectsToRemove);
            Context.UIController.TextLog.AddText(string.Format("Your body is cleansed of all toxins", owner.Name));
        }

        public override void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuration(entity, this);
        }
    }
}
