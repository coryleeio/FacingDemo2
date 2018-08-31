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

        public override bool CanApply(AttackContext ctx)
        {
            return ctx.Targets.Count > 1;
        }

        public override AttackContext Apply(AttackContext ctx)
        {
            var target = ctx.Targets[0];
            // apply weak poison here
            return ctx;
        }
    }
}
