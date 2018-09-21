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

        public override bool CanAffectIncomingAttack(EntityStateChange ctx)
        {
            if (ctx.Targets.Count > 1)
            {
                return false;
            }
            var target = ctx.Targets[0];
            var item = target.Inventory.ItemByIdentifier(UniqueIdentifier.ITEM_LUCKY_COIN);
            return item != null;
        }

        public override EntityStateChange AffectIncomingAttackEffects(EntityStateChange ctx)
        {
            var target = ctx.Targets[0];
            var isLethal = target.Body.CurrentHealth - ctx.Damage <= 0;
            if (isLethal && MathUtil.ChanceToOccur(50))
            {
                var item = target.Inventory.ItemByIdentifier(UniqueIdentifier.ITEM_LUCKY_COIN);
                ctx.ShortCircuit();
                ctx.FloatingTextMessage.AddLast(new FloatingTextMessage()
                {
                    Message = "Got lucky!",
                    Color = Color.green,
                    target = target,
                });
                ctx.LateMessages.AddLast("The mortal blow was somehow deflected by the lucky coin, sparing your life! The coin shatters...");
                target.Inventory.RemoveItemStack(item);
            }
            return ctx;
        }

    }
}
