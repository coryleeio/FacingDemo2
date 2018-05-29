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
                trigger.TriggerComponent.TriggerAction.Do();
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
                if (triggerEntity.TriggerComponent != null && triggerEntity.TriggerComponent.TriggerAction.IsStartable && !triggerEntity.TriggerComponent.TriggerAction.Completed)
                {
                    var positionsToCheck = MathUtil.GetPointsByOffset(triggerEntity.Position, triggerEntity.TriggerComponent.TriggerAction.Offsets);
                    foreach (var pawnEntity in Context.GameStateManager.Game.CurrentLevel.Entitys)
                    {
                        if(pawnEntity.MovementComponent == null)
                        {
                            continue;
                        }
                        if (pawnEntity == triggerEntity)
                        {
                            continue;
                        }
                        if (pawnEntity.MovementComponent.HasMovedSinceLastTriggerCheck && positionsToCheck.Contains(pawnEntity.Position))
                        {
                            triggerToReturn = triggerEntity;
                            triggerToReturn.TriggerComponent.TriggerAction.TargetIds.Add(pawnEntity.Id);
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
                if(entity.MovementComponent != null)
                {
                    entity.MovementComponent.HasMovedSinceLastTriggerCheck = false;
                }
                
                if(entity.TriggerComponent != null)
                {
                    entity.TriggerComponent.TriggerAction.Reset(); // Stop being completed for the next pass //  clear targets
                }
            }
        }
    }
}
