using System.Collections.Generic;

namespace Gamepackage
{
    public class AppliedEffect : Effect
    {
        public UniqueIdentifier EffectAppliedId;
        public string AppliedDisplayName;
        public string AppliedDisplayDescription;
        public List<CombatContext> ValidCombatContextsForApplication = new List<CombatContext>();

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

        public bool CanApply(EntityStateChange ctx)
        {
            return ctx.Targets.Count >= 1 && ValidCombatContextsForApplication.Contains(ctx.CombatContext);
        }

        public EntityStateChange Apply(EntityStateChange usageContext)
        {
            foreach (var target in usageContext.Targets)
            {
                EntityStateChange applyToTargetContext = new EntityStateChange();
                applyToTargetContext.Targets.Add(usageContext.Targets[0]);
                applyToTargetContext.AppliedEffects.Add(EffectFactory.Build(EffectAppliedId));
                CombatUtil.ApplyEntityStateChange(applyToTargetContext);
            }
            return usageContext;
        }
    }
}
