namespace Gamepackage
{
    public class Poison : TickingEffect
    {
        public override string DisplayName
        {
            get
            {
                return "Poison";
            }
        }

        public override string Description
        {
            get
            {
                return "You are poisoned...";
            }
        }

        public override EffectTriggerType EffectApplicationTrigger
        {
            get
            {
                return EffectTriggerType.OnTick;
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

        public virtual int PoisonAmount
        {
            get
            {
                return 3;
            }
        }

        public override void Tick(Entity entity)
        {
            base.Tick(entity);
            AttackContext ctx = new AttackContext
            {
                AttackParameters = new AttackParameters
                {
                    DamageType = DamageTypes.POISON,
                    AttackMessage = "Poison hurts you for {2} point of damage!",
                    DyeNumber = 1,
                    DyeSize = PoisonAmount,
                    Bonus = 0
                }
            };
            ctx.Targets.Add(entity);
            CombatUtil.Apply(ctx);
        }
    }
}
