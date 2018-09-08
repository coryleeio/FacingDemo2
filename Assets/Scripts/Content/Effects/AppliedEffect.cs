namespace Gamepackage
{
    // This is all the common logic for a on hit effect,
    // It does this by starting a second attack with the applied effect
    // this gives other effects on the entity the chance to block the effect, reduce the damage
    // etc...
    public class AppliedEffect : Effect
    {
        public UniqueIdentifier EffectAppliedId;
        public string AppliedDisplayName;
        public string AppliedDisplayDescription;

        public override string DisplayName
        {
            get
            {
                return AppliedDisplayName;
            }
        }

        public override string Description
        {
            get
            {
                return AppliedDisplayDescription;
            }
        }

        public override EffectTriggerType EffectApplicationTrigger
        {
            get
            {
                return EffectTriggerType.OnHit;
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
            return ctx.Targets.Count >= 1;
        }

        public override EntityStateChange Trigger(EntityStateChange ctx)
        {
            foreach(var target in ctx.Targets)
            {
                EntityStateChange poisonAttack = new EntityStateChange();
                poisonAttack.Targets.Add(ctx.Targets[0]);
                poisonAttack.AppliedEffects.Add(EffectFactory.Build(EffectAppliedId));
                CombatUtil.ApplyEntityStateChange(poisonAttack);
            }
            return ctx;
        }
    }
}
