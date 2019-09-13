using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamepackage
{
    public class DialogController : UIComponent
    {

        private Dialog state;
        private DialogBaseNode node;

        public void Start()
        {
            Window.CloseButton.gameObject.SetActive(false);
        }

        public void Init()
        {
            if (Context.Game != null && Context.Game.CurrentlyOpenDialog != null)
            {
                VisualizeNode(Context.Game.CurrentlyOpenDialog, Context.Game.CurrentlyOpenDialog.CurrentNode);
            }
        }

        private void VisualizeNode(Dialog state, DialogBaseNode node)
        {
            Debug.Log("Visualizing: " + node);
            this.state = state;
            this.node = node;
            Refresh();
            Show();
        }

        public bool DialogOpen
        {
            get
            {
                return Context.Game.CurrentlyOpenDialog == null;
            }
        }

        public void Open(Dialog dialog)
        {
            if (dialog == null)
            {
                return;
            }
            Register(dialog);
            Context.Game.CurrentlyOpenDialog = dialog;
            VisualizeNode(Context.Game.CurrentlyOpenDialog, dialog.CurrentNode);
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void Close()
        {
            ApplySetsToStateFromNode(state, Context.Game.CurrentlyOpenDialog.CurrentNode);
            if (Context.Game.CurrentlyOpenDialog != null && Context.Game.CurrentlyOpenDialog.CurrentNode != null)
            {
                Context.Game.CurrentlyOpenDialog.CurrentNode = Context.Game.CurrentlyOpenDialog.Template;
            }
            Context.Game.CurrentlyOpenDialog = null;
            Hide();
        }

        public override void Hide()
        {
            Context.UIController.DarkOverlay.Hide();
            GetComponent<DialogController>().gameObject.SetActive(false);
            Context.UIController.ClickoutCatcher.Hide();
        }

        public override void Show()
        {
            Context.UIController.DarkOverlay.Show();
            Context.UIController.ClickoutCatcher.Show();
            GetComponent<DialogController>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {
            if (state == null || node == null)
            {
                return;
            }
            Text.text = node.Text.Localize();
            foreach (Transform child in InnerContentPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            var activeButtonCount = 0;
            var allChildren = node.Children;
            var optionsNodes = new List<DialogOptionNode>();
            foreach (var child in allChildren)
            {
                if (child.GetType() == typeof(DialogOptionNode))
                {
                    optionsNodes.Add((DialogOptionNode)child);
                }
            }
            var optIndex = 1;
            foreach (var optionNode in optionsNodes)
            {
                var table = new ProbabilityTable();
                foreach (var optionsChild in optionNode.Children)
                {
                    table.Add(new ProbabilityTableParcel()
                    {
                        Values = new List<string>()
                        {
                            optionsChild.Identifier,
                        },
                        Weight = optionsChild.Weight,
                    });
                }

                bool allConditionsMet = optionNode.AllConditionsAreMet(Context.Game, state);
                if (allConditionsMet || optionNode.ConditionNotMetBehaviour == ConditionNotMetBehaviour.Disable)
                {
                    if(allConditionsMet)
                    {
                        activeButtonCount++;
                    }
                    var created = BuildOptionsButton(Context.Game, state, optionNode, () =>
                    {
                        if (optionNode.Children.Count > 0)
                        {
                            ChangeNode(table.RollAndChooseOne());
                        }
                        else
                        {
                            Close();
                        }
                    }, optionNode.Text, optIndex);
                    optIndex++;
                }
            }

            if (activeButtonCount == 0)
            {
                BuildOptionsButton(Context.Game, state, null, () =>
                {
                    Close();
                }, "dialog.nothing.to.do".Localize(), optIndex);
                optIndex++;
            }
        }

        public GameObject ContentPanel
        {
            get
            {
                return this.transform.Find("BorderedWindow").transform.Find("ContentPanel").gameObject;
            }
        }

        public GameObject InnerContentPanel
        {
            get
            {
                return ContentPanel.transform.Find("InnerContentPanel").gameObject;
            }
        }

        public Text Text
        {
            get
            {
                return ContentPanel.transform.Find("Text").GetComponent<Text>();
            }
        }

        public GameObject BuildOptionsButton(Game game, Dialog state, DialogOptionNode optionNode, UnityEngine.Events.UnityAction act, string text, int index)
        {
            var prefab = Resources.Load<DialogControllerButton>("Prefabs/UI/DialogControllerButton");
            var instance = GameObject.Instantiate<DialogControllerButton>(prefab);
            instance.GetComponent<Button>().onClick.AddListener(act);
            instance.GetComponent<Button>().onClick.AddListener(() =>
            {
                ApplySetsToStateFromNode(state, optionNode);
            });

            var targetText = text;
            var allConditionsMet = optionNode == null || optionNode.AllConditionsAreMet(Context.Game, state);
            if (!allConditionsMet && optionNode != null && optionNode.ConditionNotMetBehaviour == ConditionNotMetBehaviour.Disable)
            {
                targetText = index + ") " + string.Format("dialog.requirement.not.met".Localize(), text.Localize());
            }
            else
            {
                targetText = index + ") " + text.Localize();
            }
            instance.GetComponent<Text>().text = targetText;

            if (!allConditionsMet && optionNode != null && optionNode.ConditionNotMetBehaviour == ConditionNotMetBehaviour.Disable)
            {
                var bt = instance.GetComponent<Button>();
                bt.interactable = false;
            }
            instance.transform.SetParent(InnerContentPanel.transform, false);
            return instance.gameObject;
        }

        private static void ApplySetsToStateFromNode(Dialog state, DialogBaseNode optionNode)
        {
            if (optionNode != null && optionNode.Set != null)
            {
                foreach (var pair in optionNode.Set)
                {
                    state.Data[pair.Key] = pair.Value;
                }
            }
        }

        public GameObject BuildRewardLineItem(string text)
        {
            var prefab = Resources.Load<DialogControllerRewardItem>("Prefabs/UI/DialogControllerReward");
            var instance = GameObject.Instantiate<DialogControllerRewardItem>(prefab);
            instance.GetComponent<Text>().text = text.Localize();
            instance.transform.SetParent(InnerContentPanel.transform, false);
            return instance.gameObject;
        }

        public void ChangeNode(string identifier)
        {
            Context.Game.CurrentlyOpenDialog.CurrentNodeIdentifier = identifier;
            var node = Context.Game.CurrentlyOpenDialog.GetNode(identifier, Context.Game.CurrentlyOpenDialog.Template);
            if (node == null)
            {
                return;
            }

            if (Context.Game.CurrentlyOpenDialog.CurrentNode != null && Context.Game.CurrentlyOpenDialog.CurrentNode.Set != null)
            {
                ApplySetsToStateFromNode(state, Context.Game.CurrentlyOpenDialog.CurrentNode);
            }
            Context.Game.CurrentlyOpenDialog.CurrentNode = node;
            VisualizeNode(Context.Game.CurrentlyOpenDialog, node);
        }

        private static bool DialogIsRegistered(Dialog dialog)
        {
            return dialog.Id != Dialog.ID_UNSET;
        }

        private void Register(Dialog dialog)
        {
            if (!DialogIsRegistered(dialog))
            {
                dialog.Id = Context.Game.NextId;
                Context.Game.DialogsById.Add(dialog.Id, dialog);
            }
        }

        private void Deregister(Dialog dialog)
        {
            if (DialogIsRegistered(dialog))
            {
                Context.Game.DialogsById.Remove(dialog.Id);
            }
        }
    }
}
