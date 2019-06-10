using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Gamepackage
{
    public class ItemInspectionWindow : UIComponent
    {
        public List<AttributeRow> attributeRows = new List<AttributeRow>();
        public List<AbilityRow> abilityRows = new List<AbilityRow>();
        private bool hasInit = false;

        public override void Hide()
        {
            GetComponent<ItemInspectionWindow>().gameObject.SetActive(false);
            Context.UIController.RemoveWindow(this);
        }

        public void ShowFor(Item item)
        {
            if(!hasInit)
            {
                var window = this.transform.GetComponentInChildren<BorderedWindow>();
                window.CloseButton.WireUp(this);
                hasInit = true;
            }
            if(attributeRows.Count == 0)
            {
                attributeRows.AddRange(gameObject.GetComponentsInChildren<AttributeRow>());
                Assert.IsTrue(attributeRows.Count == 4);
            }
            var displayAttributes = DisplayUtil.GetDisplayAttributesForItem(item);
            for(var i = 0; i < attributeRows.Count; i++)
            {
                if (i < attributeRows.Count)
                {
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
            var displayAbilities = DisplayUtil.GetDisplayAbilitiesForItem(item);
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
            SetItemName(item.Name.Localize());
            ShowHideAbilityHeader(displayAbilities.Count > 0);
            SetPicture(item.Template.ItemAppearance.InventorySprite);

            var description = Window.ContentPanel.transform.Find("DescriptionPanel").Find("Text").GetComponent<Text>();
            description.text = item.Template.TemplateDescription.Localize();
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
            return GetTopPanel().transform.Find("StatsPanel").Find("StatsGrid");
        }

        private UnityEngine.Transform GetTopPanel()
        {
            return Window.ContentPanel.transform.Find("TopPanel");
        }

        public void ShowHideAbilityHeader(bool show)
        {
            var header = GetStatsPanel().Find("StatsGrid")
                .Find("Panel").Find("AbilitiesPanel").Find("FieldDescriptor")
                .GetComponent<Text>();
            header.gameObject.SetActive(show);
        }

        private UnityEngine.Transform GetStatsPanel()
        {
            return GetTopPanel().Find("StatsPanel");
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
