
using Newtonsoft.Json;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    [Serializable]
    public class Entity
    {
        // Template part
        public string TemplateIdentifier { get; set; }
        public string Name;
        public bool IsCombatant = false;
        public bool Floating = false;
        public bool CastsShadow = true;
        public bool BlocksPathing = false;
        public bool AlwaysVisible = false;
        public string ViewPrototypeUniqueIdentifier;
        public bool HasBehaviour;
        public Team OriginalTeam;
        public AIType AI;

        // Since you attack with an item - when you attack with your fists, what item should we use?
        public Item DefaultAttackItem;

        public Dictionary<Attributes, int> Attributes = new Dictionary<Attributes, int>();

        // Tags assigned at creation or at polymorph into a new entity type.
        public List<string> EntityInnateTags;

        // Tags acquired through gameplay, for players or NPCs, not lost on polymorph.
        public List<string> EntityAcquiredTags;

        // other templates referenced
        public Trigger Trigger;

        // factory / derived
        public Inventory Inventory;

        public List<Effect> Effects = new List<Effect>();
        public Point Position;
        public Direction Direction;
        public int CurrentHealth;
        public int Id { get; set; }
        public bool IsDead = false;
        public Team ActingTeam;
        public bool IsVisible;
        public bool IsDoneThisTurn = false;
        public int TimeAccrued = 0;
        public bool IsThinking = false;
        public bool IsPlayer;
        public Point LastKnownTargetPosition = null;

        [JsonIgnore]
        public GameObject ViewGameObject;

        [JsonIgnore]
        public HealthBar HealthBar;

        [JsonIgnore]
        public float ShadowScaleX;

        [JsonIgnore]
        public float ShadowScaleY;

        [JsonIgnore]
        public float ShadowScaleZ;

        [JsonIgnore]
        public float ShadowTransformY;

        [JsonIgnore]
        public SkeletonAnimation SkeletonAnimation
        {
            get
            {
                if (ViewGameObject != null)
                {
                    return ViewGameObject.GetComponentInChildren<SkeletonAnimation>();
                }
                return null;
            }
        }

        [JsonIgnore]
        public Sortable Sortable
        {
            get
            {
                if (ViewGameObject != null)
                {
                    return ViewGameObject.GetComponentInChildren<Sortable>();
                }
                return null;
            }
        }

        [JsonIgnore]
        public Action NextAction = null;

        [JsonIgnore]
        public int PathsExpected = 0;

        [JsonIgnore]
        public int PathsReturned = 0;

        [JsonIgnore]
        public bool IsNPC
        {
            get
            {
                return !IsPlayer;
            }
        }

        [JsonIgnore]
        public List<string> Tags
        {
            get
            {
                return CombatUtil.GetTagsOnEntity(this);
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

        public bool CanSee(Point p)
        {
            return Context.VisibilitySystem.CanSee(this, p);
        }

        public int CalculateValueOfAttribute(Attributes attr)
        {
            int total = 0;
            Attributes.TryGetValue(attr, out total);

            if (Inventory != null)
            {
                foreach (var pair in Inventory.EquippedItemBySlot)
                {
                    var item = pair.Value;
                    if(item.IsEnchanted)
                    {
                        foreach (var effect in item.Enchantment.WornEffects)
                        {
                            int amountToAdd = 0;
                            effect.Template.TemplateAttributes.TryGetValue(attr, out amountToAdd);
                            total += amountToAdd;
                        }
                    }

                }
            }
            return total;
        }
    }
}
