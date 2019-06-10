using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    // Applies effects to things when they step in its offsets or trigger it by pressing a button 
    // (available within its offsets).
    public class Trigger
    {
        public string TemplateIdentifier;
        [JsonIgnore]
        public TriggerTemplate Template
        {
            get
            {
                return Context.ResourceManager.Load<TriggerTemplate>(TemplateIdentifier);
            }
        }
        public Dictionary<string, string> Data;

        [JsonIgnore]
        private ITriggerableActionImpl _triggerAction;

        [JsonIgnore]
        public ITriggerableActionImpl TriggerAction
        {
            get
            {
                if (_triggerAction == null && Template.TriggerableActionClassName != null && Template.TriggerableActionClassName != "")
                {
                    return TriggerableAction.Build(Template.TriggerableActionClassName);
                }
                return _triggerAction;
            }
        }


        public Point[] Offsets
        {
            get
            {
                return CombatUtil.TriggerShapeToOffsets(Template.TriggerShape);
            }
        }

        public bool CanPerform(Entity CauseEntity, Entity TriggerEntity)
        {
            var template = TriggerEntity.Trigger.Template;
            var result = true;
            var action = TriggerEntity.Trigger.TriggerAction;
            if (action != null)
            {
                result = action.CanPerform(TriggerEntity, CauseEntity, TriggerEntity.Trigger.Data);
            }
            return result;
        }

        public void Perform(Entity CauseEntity, Entity TriggerEntity)
        {
            Assert.AreEqual(TriggerEntity.Trigger, this);
            var template = TriggerEntity.Trigger.Template;
            var grid = Context.Game.CurrentLevel.Grid;
            var possibleAttackTypes = new List<CombatActionType>();

            if (TriggerEntity.Trigger.TriggerAction != null )
            {
                TriggerEntity.Trigger.TriggerAction.Perform(TriggerEntity, CauseEntity, TriggerEntity.Trigger.Data);
            }
            if (template.CombatActionParameters != null)
            {
                var resolvedParams = new ResolvedCombatActionDescriptor()
                {
                    CombatActionParameters = new DerivedCombatActionParameters(template.CombatActionParameters)
                };
                var calculated = CombatUtil.CalculateAttack(Context.Game.CurrentLevel.Grid,
                        TriggerEntity,
                        CombatActionType.ApplyToOther,
                        null,
                        CauseEntity.Position,
                        resolvedParams
                );
                var step = new FlowStep()
                {
                    Actions = new LinkedList<Action>()
                };
                step.Actions.AddFirst(new CombatAction(calculated));
                Context.FlowSystem.Steps.AddFirst(step);
            }
        }

        public static List<Entity> EntitiesInTrigger(Entity TriggerThatWentOff)
        {
            var level = Context.Game.CurrentLevel;
            var grid = level.Grid;

            var entitiesInPositions = new List<Entity>();
            var positions = MathUtil.GetPointsByOffset(TriggerThatWentOff.Position, TriggerThatWentOff.Trigger.Offsets);
            foreach (var position in positions)
            {
                entitiesInPositions.AddRange(grid[position].EntitiesInPosition);
            }

            return entitiesInPositions;
        }

        public static bool EntityInTrigger(Entity ent, Entity TriggerThatWentOff)
        {
            return EntitiesInTrigger(TriggerThatWentOff).Contains(ent);
        }
    }
}
