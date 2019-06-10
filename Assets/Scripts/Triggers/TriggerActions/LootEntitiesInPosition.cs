using System.Collections.Generic;

namespace Gamepackage
{
    public class LootEntitiesInPosition : ITriggerableActionImpl
    {
        public string ActionNameLocalizationKey
        {
            get
            {
                return "loot.action";
            }
        }

        public bool CanPerform(Entity TriggerThatWentOff, Entity CauseOfTrigger, Dictionary<string, string> Data)
        {
            var lootableEntities = LootableEntities(TriggerThatWentOff);
            return lootableEntities.Count > 0;
        }

        private static List<Entity> LootableEntities(Entity TriggerThatWentOff) => Trigger.EntitiesInTrigger(TriggerThatWentOff).FindAll(Filters.LootableEntities);

        public void Perform(Entity TriggerThatWentOff, Entity CauseOfTrigger, Dictionary<string, string> Data)
        {
            var lootableEntities = LootableEntities(TriggerThatWentOff);
            if (lootableEntities.Count > 0)
            {
                Context.UIController.LootWindow.ShowFor(lootableEntities);
                Context.UIController.InputHint.Hide();
            }
        }
    }
}
