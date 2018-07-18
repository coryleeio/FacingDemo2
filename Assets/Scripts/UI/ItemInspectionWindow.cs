using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Gamepackage
{
    public class ItemInspectionWindow : UIComponent
    {
        public List<AttributeRow> attributeRows = new List<AttributeRow>();
        public List<AbilityRow> abilityRows = new List<AbilityRow>();

        public override void Hide()
        {
            GetComponent<ItemInspectionWindow>().gameObject.SetActive(false);
            Context.UIController.RemoveWindow(this);
        }

        public void ShowFor(Item item)
        {
            if(attributeRows.Count == 0)
            {
                attributeRows.AddRange(gameObject.GetComponentsInChildren<AttributeRow>());
                Assert.IsTrue(attributeRows.Count == 6);
            }
            var displayAttributes = StringUtil.GetDisplayAttributesForItem(item);
            for(var i = 0; i < attributeRows.Count; i++)
            {
                if (i < attributeRows.Count)
                {
                    if (attributeRows[i].GetComponent<TooltipMarker>() == null)
                    {
                        attributeRows[i].gameObject.AddComponent<TooltipMarker>();
                    }
                    if (i < displayAttributes.Count)
                    {
                        attributeRows[i].Set(displayAttributes[i].Key, displayAttributes[i].Value);
                    }
                    else
                    {
                        attributeRows[i].Clear();
                    }
                }
            }
            if(abilityRows.Count == 0)
            {
                abilityRows.AddRange(gameObject.GetComponentsInChildren<AbilityRow>());
                Assert.IsTrue(abilityRows.Count == 2);
            }
            var displayAbilities = StringUtil.GetDisplayAbilitiesForItem(item);
            for (var i = 0; i < abilityRows.Count; i++)
            {
                if (i < abilityRows.Count)
                {
                    if (i < displayAbilities.Count)
                    {
                        abilityRows[i].Set(displayAbilities[i]);
                    }
                    else
                    {
                        abilityRows[i].Clear();
                    }
                }
            }
            SetItemName(item.DisplayName);
            ShowHideAbilityHeader(displayAbilities.Count > 0);
            SetPicture(item.ItemAppearance.InventorySprite);

            var description = transform.Find("InnerPanel").Find("DescriptionPanel").Find("Text").GetComponent<Text>();
            description.text = item.Description;
            Show();
        }

        public void SetItemName(string input)
        {
            var name = GetStatsGrid()
                .Find("ItemName")
                .GetComponent<Text>();
            name.text = input;
        }

        public void SetPicture(UnityEngine.Sprite image)
        {
            var imageElement = GetTopPanel().Find("PicturePanel").Find("Image").GetComponent<Image>();
            imageElement.sprite = image;
        }

        private UnityEngine.Transform GetStatsGrid()
        {
            return GetTopPanel().Find("StatsPanel").Find("StatsGrid");
        }

        private UnityEngine.Transform GetTopPanel()
        {
            return transform.Find("InnerPanel").Find("TopPanel");
        }

        public void ShowHideAbilityHeader(bool show)
        {
            var header = transform.Find("InnerPanel").Find("TopPanel").Find("StatsPanel").Find("StatsGrid")
                .Find("Panel").Find("AbilitiesPanel").Find("FieldDescriptor")
                .GetComponent<Text>();
            header.gameObject.SetActive(show);
        }

        public override void Refresh()
        {

        }

        public override void Show()
        {
            GetComponent<ItemInspectionWindow>().gameObject.SetActive(true);
            Context.UIController.PushWindow(this);
        }
    }
}
