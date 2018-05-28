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
                trigger.Trigger.Do();
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
                if (triggerEntity.Trigger != null && triggerEntity.Trigger.IsStartable && !triggerEntity.Trigger.Completed)
                {
                    var positionsToCheck = MathUtil.GetPointsByOffset(triggerEntity.Position, triggerEntity.Trigger.Offsets);
                    foreach (var pawnEntity in Context.GameStateManager.Game.CurrentLevel.Entitys)
                    {
                        if (pawnEntity == triggerEntity)
                        {
                            continue;
                        }
                        if (pawnEntity.HasMovedSinceLastTriggerCheck && positionsToCheck.Contains(pawnEntity.Position))
                        {
                            triggerToReturn = triggerEntity;
                            triggerToReturn.Trigger.TargetIds.Add(pawnEntity.Id);
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
                entity.HasMovedSinceLastTriggerCheck = false;
                if(entity.Trigger != null)
                {
                    entity.Trigger.Reset(); // Stop being completed for the next pass //  clear targets
                }
            }
        }
    }
}
