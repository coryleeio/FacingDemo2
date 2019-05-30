using System.Collections.Generic;

namespace Gamepackage
{
    public class AppliedEffect : Effect
    {
        public UniqueIdentifier EffectAppliedId;
        public List<AttackType> ValidCombatContextsForApplication = new List<AttackType>();

        public override string DisplayName
        {
            get
            {
                return "effect.applied." + LocalizationName + ".name";
            }
        }

        public override string Description
        {
            get
            {
                return "effect.applied." + LocalizationName + ".description";
            }
        }

        public bool CanApply(EntityStateChange outcome)
        {
            return outcome.Target != null && ValidCombatContextsForApplication.Contains(outcome.AttackType);
        }

        public EntityStateChange Apply(EntityStateChange usageContext)
        {
            EntityStateChange applyToTargetContext = new EntityStateChange();
            applyToTargetContext.Source = usageContext.Source;
            applyToTargetContext.Target = usageContext.Target;
            applyToTargetContext.AppliedEffects.Add(EffectFactory.Build(EffectAppliedId));
            CombatUtil.ApplyEntityStateChange(applyToTargetContext);
            return usageContext;
        }

        public override void ShowAddMessages(Entity entity)
        {
            // An applied effect is an internal wrapper and should not be messaged
            // the effect it is applying will be messaged
        }

        public override void ShowRemoveMessages(Entity entity)
        {
            // An applied effect is an internal wrapper and should not be messaged
            // the effect it is applying will be messaged
        }
    }
}
