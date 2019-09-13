using System.Collections.Generic;

namespace Gamepackage
{
    public class DialogFactory
    {
        public static Dialog Build(string templateIdentifier)
        {
            Dialog dialog = new Dialog();
            dialog.TemplateIdentifier = templateIdentifier;
            dialog.CurrentNode = dialog.Template;
            return dialog;
        }

        public static List<Dialog> BuildAll(List<string> templateIdentifiers)
        {
            var retval = new List<Dialog>();
            foreach (var templateIdentifier in templateIdentifiers)
            {
                retval.Add(DialogFactory.Build(templateIdentifier));
            }
            return retval;
        }
    }
}
