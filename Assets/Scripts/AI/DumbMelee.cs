using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class DumbMelee : IAI
    {
        private static readonly List<CombatActionType> MeleeAIRelevantContexts = new List<CombatActionType>()
        {
            CombatActionType.Melee,
        };

        public void FigureOutNextAction(Entity entity)
        {
            entity.PathsReturned = 0;
            var level = Context.Game.CurrentLevel;
            entity.NextAction = null;
            AIUtil.CommonAIAttackWithWeapon(level, entity, InventoryUtil.GetMainHandOrDefaultWeapon(entity), MeleeAIRelevantContexts);
        }
    }
}
