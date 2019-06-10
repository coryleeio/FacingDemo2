
using UnityEngine;

namespace Gamepackage
{
    public class LuckyCoinLifeSave : EffectImpl
    {
        public override EntityStateChange CalculateAffectIncomingAttackEffects(Effect state, CalculatedCombatAction calculatedAttack, EntityStateChange outcome)
        {
            if(outcome.Missed || outcome.WasBlocked || outcome.WasShortCircuited)
            {
                return outcome;
            }
            var target = outcome.Target;
            var isLethal = target.CurrentHealth - outcome.HealthChange <= 0;
            if (isLethal && MathUtil.ChanceToOccur(100))
            {
                var item = InventoryUtil.ItemByIdentifier(target, "ITEM_LUCKY_COIN");

                CombatUtil.ShortCiruitAttack(outcome);
                outcome.FloatingTextMessage.AddLast(new FloatingTextMessage()
                {
                    Message = (state.Template.LocalizationPrefix + ".floating").Localize(),
                    Color = DisplayUtil.DamageDisplayColor(true),
                    target = target,
                    AllowLeftRightDrift = false,
                });
                outcome.LateMessages.AddLast((state.Template.LocalizationPrefix + ".stop.attack").Localize());

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
