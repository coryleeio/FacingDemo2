using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    // Outcome of a given single action applied to a single target
    // this may be a theoretical outcome, but the calculation method should be the same.
    // actual damage and effects applied are calculated when resolve() is called.
    public class ActionOutcome
    {
        public Entity Source;
        public Entity Target;

        // Some effects look at this to decide if they should modify incoming or outbound attacks
        // this is mostly set by the attack action and shouldn't need to be set most of the time 
        // for effects or other types of damage.
        public CombatContext CombatContext = CombatContext.NotSet;
        public AttackParameters AttackParameters;
        public List<Effect> AppliedEffects = new List<Effect>();
        public List<Effect> RemovedEffects = new List<Effect>();
        public int HealthChange;
        public bool isResolved = false;
        public bool WasShortCircuited = false;
        public LinkedList<string> LogMessages = new LinkedList<string>();

        // Effect messages are displayed AFTER the attack, but they are added to the list first
        // so putting them in another list makes it easier to sort it out.
        public LinkedList<string> LateMessages = new LinkedList<string>();
        public LinkedList<FloatingTextMessage> FloatingTextMessage = new LinkedList<FloatingTextMessage>();

        public void StopThisAction()
        {
            HealthChange = 0;
            WasShortCircuited = true;
            LogMessages.Clear();
            LateMessages.Clear();
            FloatingTextMessage.Clear();
            AppliedEffects.Clear();
            RemovedEffects.Clear();
        }

        public void Resolve()
        {
            if(!isResolved)
            {
                var heathChange = 0;
                if (AttackParameters != null)
                {
                    for (var numDyeRolled = 0; numDyeRolled < AttackParameters.DyeNumber; numDyeRolled++)
                    {
                        heathChange += UnityEngine.Random.Range(1, AttackParameters.DyeSize + 1);
                    }
                    heathChange += AttackParameters.Bonus;

                    if (AttackParameters.DamageType == DamageTypes.HEALING)
                    {
                        heathChange *= -1;
                    }
                    HealthChange = heathChange;
                }

                CalculateAffectOutgoingAttack();
                CalculateAffectIncomingAttackEffects();
            }
            isResolved = true;
        }

        private void CalculateAffectOutgoingAttack()
        {
            if (Source != null)
            {
                var sourceEffects = Source.GetEffects();
                foreach (var effect in sourceEffects)
                {
                    if (effect.CanAffectOutgoingAttack(this))
                    {
                        effect.CalculateAffectOutgoingAttack(this);
                    }
                }
            }
        }

        private void CalculateAffectIncomingAttackEffects()
        {
            var targetEffects = Target.GetEffects();
            foreach (var effect in targetEffects)
            {
                if (effect.CanAffectIncomingAttack(this))
                {
                    effect.CalculateAffectIncomingAttackEffects(this);
                }
            }
        }
    }



    public class FloatingTextMessage
    {
        public string Message;
        public Color Color;
        public int FontSize = 35;
        public Entity target;
    }
}
