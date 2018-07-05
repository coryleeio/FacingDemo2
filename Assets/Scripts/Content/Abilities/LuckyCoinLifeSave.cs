using UnityEngine;

namespace Gamepackage
{
    public class LuckyCoinLifeSave : Ability
    {
        public override TriggerType TriggeredBy
        {
            get
            {
                return TriggerType.OnDamageWouldKill;
            }
        }

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

        public override bool CanPerform(AbilityTriggerContext abilityTriggerContext)
        {
            var ev = (DamageWouldKillContext)abilityTriggerContext;
            var item = ev.Target.Inventory.ItemByIdentifier(UniqueIdentifier.ITEM_LUCKY_COIN);
            return item != null;
        }

        public override AbilityTriggerContext Perform(AbilityTriggerContext abilityTriggerContext)
        {
            var ev = (DamageWouldKillContext)abilityTriggerContext;
            if (MathUtil.ChanceToOccur(50))
            {
                var item = ev.Target.Inventory.ItemByIdentifier(UniqueIdentifier.ITEM_LUCKY_COIN);
                ev.attackResult.damage = 0;
                ev.attackResult.WasShortCircuited = true;
                ev.attackResult.ShortCircuitedMessage = "The mortal blow was somehow deflected by the lucky coin, sparing your life! The coin shatters...";
                ev.attackResult.ShortCircuitedFloatingText = "Got lucky!";
                ev.attackResult.ShortCircuitedFloatingTextColor = Color.green;
                ev.Target.Inventory.RemoveItem(item);
            }
            return ev;
        }
    }
}
