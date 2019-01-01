using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class UseItemOnSelf : Action
    {
        [JsonIgnore]
        public Entity Source;
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
            stateChange.CombatContext = CombatContext.OnUse;
            stateChange.Source = Source;
            stateChange.Target = Source;
            stateChange.AppliedEffects.AddRange(Item.Effects);

            if(Item.OnUseText != null)
            {
                Context.UIController.TextLog.AddText(Item.OnUseText);
            }

            CombatUtil.ApplyEntityStateChange(stateChange);
            CombatUtil.ConsumeItemCharges(Source, Item);
        }

        public override bool IsValid()
        {
            Assert.IsNotNull(Source);
            Assert.IsNotNull(Item);
            Assert.IsTrue(Item.IsUsable);
            return base.IsValid();
        }
    }
}
