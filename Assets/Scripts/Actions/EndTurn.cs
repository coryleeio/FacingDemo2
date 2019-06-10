using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class EndTurn : Action
    {
        [JsonIgnore]
        public override Entity Source
        {
            get;set;
        }

        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }

        public override void Enter()
        {
            base.Enter();
            var effectsThatShouldExpire = new List<Effect>(0);
            if(Source.HasBehaviour)
            {
                Source.IsDoneThisTurn = true;
            }

            foreach(var effect in Source.GetEffects())
            {
                var tickingEffect = effect;
                tickingEffect.EffectImpl.Tick(tickingEffect, Source);
                if(tickingEffect.ShouldExpire)
                {
                    effectsThatShouldExpire.Add(tickingEffect);
                }
            }
            // You dont need to use a state change to do this removal, because nothing should be preventing a ticking effect from expiring
            CombatUtil.RemoveEntityEffects(Source, effectsThatShouldExpire);
            if (Source.SkeletonAnimation != null)
            {
                var skeletonAnimation = Source.SkeletonAnimation;
                skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Idle, Source.Direction), true);
            }
        }
    }
}
