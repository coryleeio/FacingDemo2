using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class AbilityContext
    {
        public Entity Source;
        public List<Entity> Targets = new List<Entity>();
        public AttackParameters AttackParameters;
        public List<Ability> OnHitAbilities = new List<Ability>();
        public int Damage;
        public bool WasShortCircuited = false;
        public string ShortCircuitedMessage = null;
        public string ShortCircuitedFloatingText = null;
        public Color ShortCircuitedFloatingTextColor = Color.green;
    }
}
