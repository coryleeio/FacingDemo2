using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class ItemTemplate
    {
        public string Identifier;
        public string LocalizationPrefix;
        public int MinStackSize;
        public int MaxStackSize;
        public bool DestroyOnUse;
        public int ChanceToSurviveLaunch;
        public AmmoType AmmoType;

        public string ItemAppearanceIdentifier;
        [JsonIgnore]
        public ItemAppearance ItemAppearance
        {
            get
            {
                return Context.ResourceManager.Load<ItemAppearance>(ItemAppearanceIdentifier);
            }
        }

        // Percentage chance /100 that the item survives being shot or thrown
        // if it survives it will appear on the ground wherever it was thrown or shot to

        public List<ItemSlot> SlotsWearable;
        public List<ItemSlot> SlotsOccupiedByWearing;
        public List<string> TagsThatDescribeThisItem;
        public List<string> TagsAppliedToEntity;

        public Dictionary<CombatActionType, CombatActionDescriptor> CombatActionDescriptor;

        public List<string> PossibleEnchantments;


        public string TemplateName
        {
            get
            {
                return LocalizationPrefix + ".name";
            }
        }

        public string TemplateDescription
        {
            get
            {
                return LocalizationPrefix + ".description";
            }
        }

        public string UseButtonText
        {
            get
            {
                return LocalizationPrefix + ".use.action.text";
            }
        }

        public string ApplyToSelfPlayerText
        {
            get
            {
                return LocalizationPrefix + ".apply.to.self.player";
            }
        }

        public string ApplyToSelfOtherText
        {
            get
            {
                return LocalizationPrefix + ".apply.to.self.other";
            }
        }
    }
}
