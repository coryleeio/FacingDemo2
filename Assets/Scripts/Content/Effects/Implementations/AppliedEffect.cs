using System.Collections.Generic;

namespace Gamepackage
{
    public class AppliedEffect : Effect
    {
        public UniqueIdentifier EffectAppliedId;
        public string AppliedDisplayName;
        public string AppliedDisplayDescription;
        public List<AttackType> ValidCombatContextsForApplication = new List<AttackType>();

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
    }
}
