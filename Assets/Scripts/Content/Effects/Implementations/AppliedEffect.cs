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

        public bool CanApply(ActionOutcome outcome)
        {
            return outcome.Target != null && ValidCombatContextsForApplication.Contains(outcome.CombatContext);
        }

        public ActionOutcome Apply(ActionOutcome usageContext)
        {
            ActionOutcome applyToTargetContext = new ActionOutcome();
            applyToTargetContext.Source = usageContext.Source;
            applyToTargetContext.Target = usageContext.Target;
            applyToTargetContext.AppliedEffects.Add(EffectFactory.Build(EffectAppliedId));
            CombatUtil.ApplyEntityStateChange(applyToTargetContext);
            return usageContext;
        }
    }
}
