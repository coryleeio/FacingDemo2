using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class Archer : IAI
    {
        private static readonly List<CombatActionType> RangedAIRelevantContexts = new List<CombatActionType>()
        {
            CombatActionType.Melee,
            CombatActionType.Zapped,
            CombatActionType.Thrown,
            CombatActionType.Ranged,
        };

        public void FigureOutNextAction(Entity entity)
        {
            entity.PathsReturned = 0;
            var level = Context.Game.CurrentLevel;
            entity.NextAction = null;
            AIUtil.CommonAIAttackWithWeapon(level, entity, InventoryUtil.GetMainHandOrDefaultWeapon(entity), RangedAIRelevantContexts);
        }
    }
}
