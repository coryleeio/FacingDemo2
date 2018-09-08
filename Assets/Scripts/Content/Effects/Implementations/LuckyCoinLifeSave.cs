using UnityEngine;

namespace Gamepackage
{
    public class LuckyCoinLifeSave : Effect
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

        public override EffectTriggerType EffectApplicationTrigger
        {
            get
            {
                return EffectTriggerType.OnDamageWouldKill;
            }
        }

        public override string RemovalText
        {
            get
            {
                return "";
            }
        }

        public override bool CanTrigger(EntityStateChange ctx)
        {
            if(ctx.Targets.Count > 1)
            {
                return false;
            }
            var target = ctx.Targets[0];
            var item = target.Inventory.ItemByIdentifier(UniqueIdentifier.ITEM_LUCKY_COIN);
            return item != null;
        }

        public override EntityStateChange Trigger(EntityStateChange ctx)
        {
            var target = ctx.Targets[0];
            if (MathUtil.ChanceToOccur(50))
            {
                var item = target.Inventory.ItemByIdentifier(UniqueIdentifier.ITEM_LUCKY_COIN);
                ctx.Damage = 0;
                ctx.WasShortCircuited = true;
                ctx.ShortCircuitedMessage = "The mortal blow was somehow deflected by the lucky coin, sparing your life! The coin shatters...";
                ctx.ShortCircuitedFloatingText = "Got lucky!";
                ctx.ShortCircuitedFloatingTextColor = Color.green;
                target.Inventory.RemoveItemStack(item);
            }
            return ctx;
        }
    }
}
