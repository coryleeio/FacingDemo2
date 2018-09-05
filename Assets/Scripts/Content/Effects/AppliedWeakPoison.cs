namespace Gamepackage
{
    public class AppliedWeakPoison : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "Applied Weak Poison";
            }
        }

        public override string Description
        {
            get
            {
                return "The business end of this is coated in poison...";
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

        public override bool CanTrigger(AttackContext ctx)
        {
            return ctx.Targets.Count >= 1;
        }

        public override AttackContext Trigger(AttackContext ctx)
        {
            foreach(var target in ctx.Targets)
            {
                AttackContext poisonAttack = new AttackContext();
                poisonAttack.Targets.Add(ctx.Targets[0]);
                poisonAttack.AppliedEffects.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_WEAK_POISON));
                CombatUtil.ApplyAttackResult(poisonAttack);
            }
            return ctx;
        }
    }
}
