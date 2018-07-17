using UnityEngine;

namespace Gamepackage
{
    public class LuckyCoinLifeSave : Ability
    {
        public override string DisplayName
        {
            get
            {
                return "It's really lucky";
            }
        }

        public override string Description
        {
            get
            {
                return "50% chance to survive a hit that would otherwise kill you.";
            }
        }

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
                ev.Target.Inventory.RemoveItemStack(item);
            }
            return ev;
        }
    }
}
