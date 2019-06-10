using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class Item
    {

        [JsonIgnore]
        public string Name
        {
            get
            {
                var name = Template.TemplateName.Localize();
                if(Enchantment != null)
                {
                    name = string.Format(Enchantment.Template.NameModifier.Localize(), name);
                }
                return name;
            }
        }

        public string TemplateIdentifier;
        [JsonIgnore]
        public ItemTemplate Template
        {
            get
            {
                return Context.ResourceManager.Load<ItemTemplate>(TemplateIdentifier);
            }
        }

        public Enchantment Enchantment;

        [JsonIgnore]
        public bool Stackable
        {
            get
            {
                return Template.MaxStackSize > 1;
            }
        }

        public int NumberOfItems;
        public List<Effect> EffectsGrantedToOwner;
        public Dictionary<Attributes, int> Attributes;

        [JsonIgnore]
        public Dictionary<CombatActionType, CombatActionDescriptor> CombatActionDescriptor
        {
            get
            {
                return Template.CombatActionDescriptor;
            }
        }

        [JsonIgnore]
        public bool IsUsable
        {
            get
            {
                return CombatActionDescriptor.ContainsKey(CombatActionType.ApplyToSelf) && CombatActionDescriptor[CombatActionType.ApplyToSelf] != null;
            }
        }

        [JsonIgnore]
        public bool HasCharges
        {
            get
            {
                // Useable items can be used despite having no enchantment, and thus no charges.
                return Enchantment == null || Enchantment.HasCharges;
            }
        }

        [JsonIgnore]
        public bool Equipable
        {
            get
            {
                return Template.SlotsWearable.Count > 0;
            }
        }

        [JsonIgnore]
        public Vector3 CorpsePositionOffset
        {
            get
            {
                return new Vector3(0f, -0.2f, 0f);
            }
        }

        [JsonIgnore]
        public Vector3 CorpseIconEulerAngles
        {
            get
            {
                return new Vector3(0, 0, 30);
            }
        }

        [JsonIgnore]
        public Vector3 CorpseIconScale
        {
            get
            {
                return new Vector3(0.5f, 0.5f, 0.5f);
            }
        }

        public bool CanBeUsedInInteractionType(CombatActionType InteractionType)
        {
            if (InteractionType == CombatActionType.Melee)
            {
                return CombatActionDescriptor.ContainsKey(CombatActionType.Melee);
            }
            else if (InteractionType == CombatActionType.Ranged)
            {
                return CombatActionDescriptor.ContainsKey(CombatActionType.Ranged) && Template.AmmoType != AmmoType.None;
            }
            else if (InteractionType == CombatActionType.Thrown)
            {
                return CombatActionDescriptor.ContainsKey(CombatActionType.Thrown);
            }
            else if (InteractionType == CombatActionType.Zapped)
            {
                if(Enchantment != null && Enchantment.Template.CombatActionDescriptor.ContainsKey(CombatActionType.Zapped))
                {
                    return Enchantment.Template.CombatActionDescriptor.ContainsKey(CombatActionType.Zapped) && HasCharges;
                }
                return CombatActionDescriptor.ContainsKey(CombatActionType.Zapped) && HasCharges;
            }
            else if (InteractionType == CombatActionType.ApplyToSelf)
            {
                return CombatActionDescriptor.ContainsKey(CombatActionType.ApplyToSelf) && HasCharges;
            }
            else if (InteractionType == CombatActionType.ApplyToOther)
            {
                return CombatActionDescriptor.ContainsKey(CombatActionType.ApplyToOther);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public bool CanStack(Item other)
        {
            return TemplateIdentifier == other.TemplateIdentifier && (NumberOfItems + other.NumberOfItems < this.Template.MaxStackSize);
        }

    }
}
