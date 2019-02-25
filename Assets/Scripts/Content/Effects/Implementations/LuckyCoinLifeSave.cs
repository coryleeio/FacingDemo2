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

        public override bool CanAffectIncomingAttack(CalculatedAttack calculatedAttack, EntityStateChange outcome)
        {
            if (outcome.Target == null)
            {
                return false;
            }
            var item = InventoryUtil.ItemByIdentifier(outcome.Target, UniqueIdentifier.ITEM_LUCKY_COIN);
            return item != null;
        }

        public override EntityStateChange CalculateAffectIncomingAttackEffects(CalculatedAttack calculatedAttack, EntityStateChange outcome)
        {
            var target = outcome.Target;
            var isLethal = target.Body.CurrentHealth - outcome.HealthChange <= 0;
            if (isLethal && MathUtil.ChanceToOccur(100))
            {
                var item = InventoryUtil.ItemByIdentifier(target, UniqueIdentifier.ITEM_LUCKY_COIN);

                CombatUtil.ShortCiruitAttack(outcome);
                outcome.FloatingTextMessage.AddLast(new FloatingTextMessage()
                {
                    Message = "effect.lucky.coin.lifesave.floating.message".Localize(),
                    Color = Color.green,
                    target = target,
                });
                outcome.LateMessages.AddLast("effect.lucky.coin.lifesave.block.attack".Localize());

                calculatedAttack.ItemStateChanges.Add(new ItemStateChange()
                {
                    Owner = outcome.Target,
                    Item = item,
                    NumberOfItemsConsumed = item.NumberOfItems,
                });
            }
            return outcome;
        }

    }
}
