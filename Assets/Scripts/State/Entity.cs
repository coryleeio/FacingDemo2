
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

        public List<Effect> GetEffects(EffectTriggerType triggertype)
        {
            Assert.IsTrue(triggertype != EffectTriggerType.OnHit, "This function is for everything except on hit.  Melee and ranged attacks implement their own collection of onhit effects because it is more complicated and includes attack specific effects");
            var effects = new List<Effect>();

            if (Trigger != null && Trigger.Effect != null && Trigger.Effect.EffectApplicationTrigger == triggertype)
            {
                effects.Add(Trigger.Effect);
            }

            if(triggertype == EffectTriggerType.OnStep)
            {
                // for now only triggers should use this, no items use it, so this saves us a lot of searching.
                return effects;
            }

            foreach (var pair in Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                foreach (var effect in item.Effects)
                {
                    if (effect.EffectApplicationTrigger == triggertype)
                    {
                        effects.Add(effect);
                    }
                }
            }

            foreach (var item in Inventory.Items)
            {
                if (item != null)
                {
                    foreach (var effect in item.Effects)
                    {
                        if (effect.EffectApplicationTrigger == triggertype)
                        {
                            effects.Add(effect);
                        }
                    }
                }
            }

            if(Body != null)
            {
                foreach(var effect in Body.Effects)
                {
                    if (effect.EffectApplicationTrigger == triggertype)
                    {
                        effects.Add(effect);
                    }
                }
            }
            return effects;
        }

        internal void RemoveEffects(List<Effect> effectsThatShouldExpire)
        {
            // Note that we never remove attack specific effects
            if (Trigger != null && Trigger.Effect != null && effectsThatShouldExpire.Contains(Trigger.Effect))
            {
                Trigger.Effect = null;
            }

            foreach (var pair in Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                item.Effects.RemoveAll((eff) => { return effectsThatShouldExpire.Contains(eff); });
            }

            foreach (var item in Inventory.Items)
            {
                if (item != null)
                {
                    item.Effects.RemoveAll((eff) => { return effectsThatShouldExpire.Contains(eff); });
                }
            }

            if (Body != null)
            {
                Body.Effects.RemoveAll((eff) => { return effectsThatShouldExpire.Contains(eff); });
            }
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