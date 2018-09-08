namespace Gamepackage
{
    public class CurePoison : Effect
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

        public override EffectTriggerType EffectApplicationTrigger
        {
            get
            {
                return EffectTriggerType.OnUseSelf;
            }
        }

        public override string RemovalText
        {
            get
            {
                return "";
            }
        }

        public override bool CanTrigger(EntityStateChange ctx)
        {
            return ctx.Targets.Count == 1;
        }

        public override EntityStateChange Trigger(EntityStateChange ctx)
        {
            var target = ctx.Targets[0];
            var effectsToRemove = CombatUtil.GetEntityEffectsByType(target, (effectInQuestion) => { return effectInQuestion is Poison; });
            CombatUtil.RemoveEntityEffects(target, effectsToRemove);
            Context.UIController.TextLog.AddText(string.Format("Your body is cleansed of all toxins", target.Name));
            return ctx;
        }
    }
}
