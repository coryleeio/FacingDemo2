using UnityEngine;

namespace Gamepackage
{
    public class LuckyCoinLifeSave : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.lucky.coin.lifesave.name".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "effect.lucky.coin.lifesave.description".Localize();
            }
        }

        public override bool CanAffectIncomingAttack(ActionOutcome outcome)
        {
            if (outcome.Target == null)
            {
                return false;
            }
            var item = outcome.Target.Inventory.ItemByIdentifier(UniqueIdentifier.ITEM_LUCKY_COIN);
            return item != null;
        }

        public override ActionOutcome CalculateAffectIncomingAttackEffects(ActionOutcome outcome)
        {
            var target = outcome.Target;
            var isLethal = target.Body.CurrentHealth - outcome.HealthChange <= 0;
            if (isLethal && MathUtil.ChanceToOccur(50))
            {
                var item = target.Inventory.ItemByIdentifier(UniqueIdentifier.ITEM_LUCKY_COIN);
                outcome.StopThisAction();
                outcome.FloatingTextMessage.AddLast(new FloatingTextMessage()
                {
                    Message = "effect.lucky.coin.lifesave.floating.message".Localize(),
                    Color = Color.green,
                    target = target,
                });
                outcome.LateMessages.AddLast("effect.lucky.coin.lifesave.block.attack".Localize());
                target.Inventory.RemoveItemStack(item);
            }
            return outcome;
        }

    }
}
