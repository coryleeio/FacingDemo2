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
                    Message = "effect.lucky.coin.lifesave.floating.message".Localize(),
                    Color = Color.green,
                    target = target,
                });
                ctx.LateMessages.AddLast("effect.lucky.coin.lifesave.block.attack".Localize());
                target.Inventory.RemoveItemStack(item);
            }
            return ctx;
        }

    }
}
