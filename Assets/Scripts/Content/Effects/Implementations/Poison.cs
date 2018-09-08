namespace Gamepackage
{
    public class Poison : Effect
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

        public override bool CanTrigger(EntityStateChange ctx)
        {
            return ctx.Targets.Count == 1;
        }

        public override EntityStateChange Trigger(EntityStateChange ctx)
        {
            var target = ctx.Targets[0];
            target.Body.Effects.Add(this);
            return ctx;
        }

        public virtual int PoisonAmount
        {
            get
            {
                return 1;
            }
        }

        public override string RemovalText
        {
            get
            {
                return "Your poison fades";
            }
        }

        public override void Tick(Entity entity)
        {
            base.Tick(entity);
            EntityStateChange ctx = new EntityStateChange
            {
                AttackParameters = new AttackParameters
                {
                    DamageType = DamageTypes.POISON,
                    AttackMessage = "Poison hurts {1} for {2} point of damage!",
                    DyeNumber = 1,
                    DyeSize = PoisonAmount,
                    Bonus = 0
                }
            };
            ctx.Targets.Add(entity);
            CombatUtil.ApplyEntityStateChange(ctx);
        }
    }
}
