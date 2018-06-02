using System.Collections.Generic;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class DoTriggers : IStateMachineState
    {
        public ApplicationContext Context { get; set; }

        public void Enter()
        {

        }

        public void Process()
        {
            Debug.Log("Doing triggers...");
            Entity trigger = GetNextTriggerWithTargets();
            if(trigger != null)
            {
                trigger.Trigger.TriggerAction.Do();
            }
            else
            {
                // done with triggers
                Context.FlowSystem.StateMachine.ChangeState(Context.DoTurn);
            }
        }

        public Entity GetNextTriggerWithTargets()
        {
            Entity triggerToReturn = null;
            foreach (var triggerEntity in Context.GameStateManager.Game.CurrentLevel.Entitys)
            {
                if(triggerToReturn != null)
                {
                    break;
                }
                if (triggerEntity.Trigger != null && triggerEntity.Trigger.TriggerAction.IsStartable && !triggerEntity.Trigger.TriggerAction.Completed)
                {
                    var positionsToCheck = MathUtil.GetPointsByOffset(triggerEntity.Position, triggerEntity.Trigger.TriggerAction.Offsets);
                    foreach (var pawnEntity in Context.GameStateManager.Game.CurrentLevel.Entitys)
                    {
                        if(pawnEntity.Motor == null)
                        {
                            continue;
                        }
                        if (pawnEntity == triggerEntity)
                        {
                            continue;
                        }
                        if (pawnEntity.Motor.HasMovedSinceLastTriggerCheck && positionsToCheck.Contains(pawnEntity.Position))
                        {
                            triggerToReturn = triggerEntity;
                            triggerToReturn.Trigger.TriggerAction.TargetIds.Add(pawnEntity.Id);
                        }
                    }
                }
            }
            return triggerToReturn;
        }

        public void Exit()
        {
            foreach(var entity in Context.GameStateManager.Game.CurrentLevel.Entitys)
            {
                if(entity.Motor != null)
                {
                    entity.Motor.HasMovedSinceLastTriggerCheck = false;
                }
                
                if(entity.Trigger != null)
                {
                    entity.Trigger.TriggerAction.Reset(); // Stop being completed for the next pass //  clear targets
                }
            }
        }
    }
}
