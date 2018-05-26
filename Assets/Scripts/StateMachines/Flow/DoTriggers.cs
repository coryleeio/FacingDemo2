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
            Token trigger = GetNextTriggerWithTargets();
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

        public Token GetNextTriggerWithTargets()
        {
            Token triggerToReturn = null;
            foreach (var triggerToken in Context.GameStateManager.Game.CurrentLevel.Tokens)
            {
                if(triggerToReturn != null)
                {
                    break;
                }
                if (triggerToken.Trigger != null && triggerToken.Trigger.IsStartable && !triggerToken.Trigger.Completed)
                {
                    var positionsToCheck = MathUtil.GetPointsByOffset(triggerToken.Position, triggerToken.Trigger.Offsets);
                    foreach (var pawnToken in Context.GameStateManager.Game.CurrentLevel.Tokens)
                    {
                        if (pawnToken == triggerToken)
                        {
                            continue;
                        }
                        if (pawnToken.HasMovedSinceLastTriggerCheck && positionsToCheck.Contains(pawnToken.Position))
                        {
                            triggerToReturn = triggerToken;
                            triggerToReturn.Trigger.TargetIds.Add(pawnToken.Id);
                        }
                    }
                }
            }
            return triggerToReturn;
        }

        public void Exit()
        {
            foreach(var token in Context.GameStateManager.Game.CurrentLevel.Tokens)
            {
                token.HasMovedSinceLastTriggerCheck = false;
                if(token.Trigger != null)
                {
                    token.Trigger.Reset(); // Stop being completed for the next pass //  clear targets
                }
            }
        }
    }
}
