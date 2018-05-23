using System.Collections.Generic;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class DoTriggers : IStateMachineState
    {
        public GameStateManager GameStateManager { get; set; }
        public ResourceManager ResourceManager { get; set; }
        private Token NextActor;
        private TokenAction StartedAction;
        public TinyIoCContainer Container { get; set; }
        private List<Token> Triggers = new List<Token>(0);
        private Token NextTrigger;
        private TriggerBehaviourPrototype NextTriggerPrototype;
        public FlowSystem FlowSystem { get; set; }
        public DoTurn DoTurn { get; set; }

        public void Enter()
        {
            Triggers.Clear();
            foreach (var token in GameStateManager.Game.CurrentLevel.Tokens)
            {
                if (token.TriggerPrototypeUniqueIdentifier != UniqueIdentifier.TRIGGER_NONE)
                {
                    Triggers.Add(token);
                    token.TriggerHasBeenChecked = false;
                    token.IsTriggering = false;
                }
            }
            NextTrigger = null;
        }

        public void Process()
        {
            if (NextTrigger == null)
            {
                foreach (var trigger in Triggers)
                {
                    if (!trigger.TriggerHasBeenChecked)
                    {
                        NextTrigger = trigger;
                    }
                }
                if (NextTrigger == null)
                {
                    DoneWithAllTriggers();
                    return;
                }
            }

            if(NextTriggerPrototype == null || NextTriggerPrototype.UniqueIdentifier != NextTrigger.TriggerPrototypeUniqueIdentifier)
            {
                NextTriggerPrototype = ResourceManager.GetPrototype<TriggerBehaviourPrototype>(NextTrigger.TriggerPrototypeUniqueIdentifier);
            }

            if(!NextTrigger.TriggerHasBeenChecked)
            {
                var pointsForTrigger = MathUtil.GetPointsByOffset(NextTrigger.Position, NextTriggerPrototype.Offsets);
                foreach(var token in GameStateManager.Game.CurrentLevel.Tokens)
                {
                    if(NextTrigger == token)
                    {
                        continue;
                    }
                    if(token.NeedToCheckIfMovementTriggeredTriggers && pointsForTrigger.Contains(token.Position))
                    {
                        NextTrigger.IsTriggering = true;
                    }
                }
                NextTrigger.TriggerHasBeenChecked = true;
            }

            if(!NextTrigger.IsTriggering)
            {
                // Start triggering, or mark as done
                DoneWithThisTrigger();
            }
            else
            {
                // Handle trigger action
                // Start here - detect  if you need to start the trigger action, do Process etc
                // Is there any way to make these not a singleton like tokenactions?
                NextTriggerPrototype.TriggerAction.Enter(NextTrigger.TriggerState);
            }
        }

        private void DoneWithAllTriggers()
        {
            FlowSystem.StateMachine.ChangeState(DoTurn);
        }

        private void DoneWithThisTrigger()
        {
            NextTrigger.TriggerHasBeenChecked = true;
            NextTrigger.IsTriggering = false;
            NextTrigger = null;
        }

        public void Exit()
        {

        }
    }

}
