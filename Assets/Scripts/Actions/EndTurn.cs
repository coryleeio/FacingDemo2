using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class EndTurn : TargetableAction
    {
        public override int TimeCost
        {
            get
            {
                return 0;
            }
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
            Source.Behaviour.IsDoneThisTurn = true;

            var onTickEffects = Source.GetEffects(EffectTriggerType.OnTick);
            foreach(var effect in onTickEffects)
            {
                var tickingEffect = (TickingEffect) effect;
                tickingEffect.Tick(Source);
                if(tickingEffect.ShouldExpire)
                {
                    if(tickingEffect.ShouldExpire)
                    {
                        effectsThatShouldExpire.Add(tickingEffect);
                    }
                }
            }

            Source.RemoveEffects(effectsThatShouldExpire);
        }
    }
}
