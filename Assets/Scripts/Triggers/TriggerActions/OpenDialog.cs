using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class OpenDialog : ITriggerableActionImpl
    {
        public string ActionNameLocalizationKey => "inspect.action";

        public enum Params
        {
            DIALOG_TEMPLATE,
            EXISTING_DIALOG_ID,
        }

        public bool CanPerform(Entity TriggerThatWentOff, Entity CauseOfTrigger, Dictionary<string, string> Data)
        {
            return true;
        }

        public void Perform(Entity TriggerThatWentOff, Entity CauseOfTrigger, Dictionary<string, string> Data)
        {
            if(Data.ContainsKey(Params.EXISTING_DIALOG_ID.ToString()))
            {
                var existingDialogId = Convert.ToInt32(Data[Params.EXISTING_DIALOG_ID.ToString()]);
                var dialog = Context.Game.DialogsById[existingDialogId];
                Context.UIController.DialogController.Open(dialog);
            }
            else
            {
                var dialogTemplateIdentifier = Data[Params.DIALOG_TEMPLATE.ToString()];
                var dialog = DialogFactory.Build(dialogTemplateIdentifier);
                Context.UIController.DialogController.Open(dialog);
                Data[Params.EXISTING_DIALOG_ID.ToString()] = dialog.Id.ToString();
            }
        }
    }
}
