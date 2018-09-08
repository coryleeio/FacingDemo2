using UnityEngine.Assertions;

namespace Gamepackage
{
    public class UseItemOnSelf : TargetableAction
    {
        public Item Item;

        public override int TimeCost
        {
            get
            {
                return 250;
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

            var stateChange = new EntityStateChange();
            stateChange.Source = Source;
            stateChange.Targets.Add(Targets[0]);
            Assert.IsTrue(Source == Targets[0]);
            stateChange.AppliedEffects.AddRange(Item.Effects.FindAll((effectInQuestion) => { return effectInQuestion.EffectApplicationTrigger == EffectTriggerType.OnUseSelf; }));

            if(Item.OnUseText != null)
            {
                Context.UIController.TextLog.AddText(Item.OnUseText);
            }

            CombatUtil.ApplyEntityStateChange(stateChange);
            CombatUtil.ConsumeItemCharges(Targets[0], Item);
        }

        public override bool IsValid()
        {
            Assert.IsNotNull(Source);
            Assert.IsTrue(Targets.Count == 1);
            Assert.IsNotNull(Targets[0]);
            Assert.IsNotNull(Item);
            Assert.IsTrue(Item.IsUsable);
            return base.IsValid();
        }
    }
}
