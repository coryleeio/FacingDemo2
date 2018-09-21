using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class EntityStateChange
    {
        public Entity Source;
        public List<Entity> Targets = new List<Entity>();
        public AttackParameters AttackParameters;
        public List<Effect> AppliedEffects = new List<Effect>();
        public List<Effect> RemovedEffects = new List<Effect>();
        public int Damage;
        public bool WasShortCircuited = false;
        public LinkedList<string> LogMessages = new LinkedList<string>();

        // Effect messages are displayed AFTER the attack, but they are added to the list first
        // so putting them in another list makes it easier to sort it out.
        public LinkedList<string> LateMessages = new LinkedList<string>();
        public LinkedList<FloatingTextMessage> FloatingTextMessage = new LinkedList<FloatingTextMessage>();

        public void ShortCircuit()
        {
            Damage = 0;
            WasShortCircuited = true;
            LogMessages.Clear();
            LateMessages.Clear();
            FloatingTextMessage.Clear();
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
