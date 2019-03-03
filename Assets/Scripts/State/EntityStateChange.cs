using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    // An outcome of an action applied to a single target
    // this may be a theoretical outcome
    // call CombatUtil.ApplyStateChange to apply it
    public class EntityStateChange
    {
        public Entity Source;
        public Entity Target;
        public Item AttackingItem;

        // Where the entire attack that generated this state change was aimed.
        public Point TargetPositionOfAttack;

        // Some effects look at this to decide if they should modify incoming or outbound attacks
        // this is mostly set by the attack action and shouldn't need to be set most of the time 
        // for effects or other types of damage.
        public AttackType AttackType = AttackType.NotSet;

        public AttackParameters AttackParameters;
        public List<Effect> AppliedEffects = new List<Effect>();
        public List<Effect> RemovedEffects = new List<Effect>();
        public int HealthChange;
        public bool WasShortCircuited = false;
        public LinkedList<string> LogMessages = new LinkedList<string>();

        // Effect messages are displayed AFTER the attack, but they are added to the list first
        // so putting them in another list makes it easier to sort it out.
        public LinkedList<string> LateMessages = new LinkedList<string>();
        public LinkedList<FloatingTextMessage> FloatingTextMessage = new LinkedList<FloatingTextMessage>();
    }

    public class FloatingTextMessage
    {
        public string Message;
        public Color Color;
        public int FontSize = 35;
        public Entity target;
        public bool AllowLeftRightDrift = true;
    }
}
