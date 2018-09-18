
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Gamepackage
{
    [Serializable]
    public class Entity
    {
        public Entity() { }

        public int Id { get; set; }

        public string Name;

        public Point Position;

        public Body Body;

        public Trigger Trigger;

        public View View;

        public Behaviour Behaviour;

        public Inventory Inventory;

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        public bool BlocksPathing = false;

        [JsonIgnore]
        public bool IsPlayer
        {
            get
            {
                return Behaviour != null && Behaviour.IsPlayer;
            }
        }

        [JsonIgnore]
        public bool IsNPC
        {
            get
            {
                return Behaviour != null && !Behaviour.IsPlayer;
            }
        }

        [JsonIgnore]
        public bool IsCombatant
        {
            get
            {
                return Body != null;
            }
        }

        public List<Effect> GetEffects(Predicate<Effect> predicate)
        {
            return CombatUtil.GetEntityEffectsByType(this, predicate);
        }

        public void RemoveEffects(Effect effectThatShouldExpire)
        {
            RemoveEffects(new List<Effect>(1) { effectThatShouldExpire });
        }

        public void RemoveEffects(List<Effect> effectsThatShouldExpire)
        {
            CombatUtil.RemoveEntityEffects(this, effectsThatShouldExpire);
        }

        public void Rewire()
        {
            if (Trigger != null)
            {
                Trigger.Rewire(this);
            }
            if (Body != null)
            {
                Body.Rewire(this);
            }
            if (Inventory != null)
            {
                Inventory.Rewire(this);
            }
            if (View != null)
            {
                View.Rewire(this);
            }
            if (Behaviour != null)
            {
                Behaviour.Rewire(this);
            }
        }
    }
}