using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class EndTurn : Action
    {
        [JsonIgnore]
        public Entity Source;

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

            var onTickEffects = Source.GetEffects((effectInQuestion) => { return effectInQuestion.CanTick; });
            foreach(var effect in onTickEffects)
            {
                var tickingEffect = effect;
                tickingEffect.Tick(Source);
                if(tickingEffect.Ticker.ShouldExpire)
                {
                    effectsThatShouldExpire.Add(tickingEffect);
                }
            }
            // You dont need to use a state change to do this removal, because nothing should be preventing a ticking effect from expiring
            CombatUtil.RemoveEntityEffects(Source, effectsThatShouldExpire);
        }
    }
}
