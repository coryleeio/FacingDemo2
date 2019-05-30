
using Newtonsoft.Json;
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

        public Direction Direction;

        public Body Body;

        public Trigger Trigger;

        public View View;

        public Behaviour Behaviour;

        public Inventory Inventory;

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        public bool BlocksPathing = false;

        public bool AlwaysVisible = false;

        // Tags assigned at creation or at polymorph into a new entity type.
        public List<Tags> EntityInnateTags;

        // Tags acquired through gameplay, for players or NPCs, not lost on polymorph.
        public List<Tags> EntityAcquiredTags;

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

        [JsonIgnore]
        public Sortable Sortable
        {
            get
            {
                if (View == null)
                {
                    return null;
                }
                else
                {
                    return View.Sortable;
                }
            }
        }

        public List<Effect> GetEffects()
        {
            return GetEffects(null);
        }

        public List<Effect> GetEffects(Predicate<Effect> predicate)
        {
            return CombatUtil.GetEntityEffectsByType(this, predicate);
        }

        public List<Tags> Tags
        {
            get
            {
                return CombatUtil.GetTagsOnEntity(this);
            }
        }

        public int CalculateValueOfAttribute(Attributes attr)
        {
            int total = 0;
            if (Body != null)
            {
                Body.Attributes.TryGetValue(attr, out total);
            }

            if (Inventory != null)
            {
                foreach (var pair in Inventory.EquippedItemBySlot)
                {
                    var item = pair.Value;
                    foreach (var attribute in item.Attributes)
                    {
                        if (attribute.Key == attr)
                        {
                            total += attribute.Value;
                        }
                    }
                    foreach (var effect in item.EffectsGlobal)
                    {
                        int amountToAdd = 0;
                        effect.Attributes.TryGetValue(attr, out amountToAdd);
                        total += amountToAdd;
                    }
                }
            }
            return total;
        }
    }
}