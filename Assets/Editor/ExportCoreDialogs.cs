using Gamepackage;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using UnityEditor;

public class ExportCoreDialogs
{
    [MenuItem("Tools/Export Core Dialogs")]
    public static void Build()
    {
        DialogNode start = BuildValidStartNode("DIALOG_WELL", "dialog.peer.well");

        DialogOptionNode opt1 = new DialogOptionNode
        {
            Identifier = Guid.NewGuid().ToString(),
            Text = "Use magic!",
        };
        OnlyAllowOnFirstOpen(opt1, ConditionNotMetBehaviour.Hide);
        DialogNode opt1text = new DialogNode
        {
            Identifier = Guid.NewGuid().ToString(),
            Text = "You use magic!"
        };

        DialogOptionNode opt2 = new DialogOptionNode
        {
            Identifier = Guid.NewGuid().ToString(),
            Text = "Use might!",
        };
        OnlyAllowOnFirstOpen(opt2, ConditionNotMetBehaviour.Hide);
        DialogNode opt2text = new DialogNode
        {
            Identifier = Guid.NewGuid().ToString(),
            Text = "You use might!"
        };

        DialogOptionNode opt3 = new DialogOptionNode
        {
            Identifier = Guid.NewGuid().ToString(),
            Text = "Do nothing!",
            ConditionNotMetBehaviour = ConditionNotMetBehaviour.Hide,
        };
        OnlyAllowOnFirstOpen(opt3, ConditionNotMetBehaviour.Disable);

        start.Children.Add(opt1);
        start.Children.Add(opt2);
        start.Children.Add(opt3);
        opt1.Children.Add(opt1text);
        opt2.Children.Add(opt2text);

        SaveDialogFile(start, start.Identifier);
    }

    private static DialogNode BuildValidStartNode(string identifier, string text)
    {
        return new DialogNode
        {
            Identifier = identifier,
            Text = text,
            Set = new System.Collections.Generic.Dictionary<string, string>()
            {
                { DialogBuiltInStrings.Opened, DialogBuiltInStrings.True },
            }
        };
    }

    private static Conditional OnlyAllowOnFirstOpen(DialogOptionNode opt, ConditionNotMetBehaviour behaviour)
    {
        var cond = new Conditional()
        {
            conditionalImplClassName = "Gamepackage.NotEquals",
            Parameters = new System.Collections.Generic.Dictionary<string, string>()
                {
                    { Gamepackage.Equals.Parameters.Key.ToString(), DialogBuiltInStrings.Opened},
                    { Gamepackage.Equals.Parameters.Value.ToString(), DialogBuiltInStrings.True},
                }
        };
        opt.Conditions.Add(cond);
        opt.ConditionNotMetBehaviour = behaviour;
        return cond;
    }

    private static void SaveDialogFile(DialogNode template, string relativeFileName)
    {
        string dialogsDirectory = "Assets/StreamingAssets/Mods/Core/Dialogs";
        Assert.IsTrue(template.Valid);
        if (!Directory.Exists(dialogsDirectory))
        {
            Directory.CreateDirectory(dialogsDirectory);
        }
        var targetFile = System.IO.Path.Combine(dialogsDirectory, relativeFileName);
        if (File.Exists(targetFile))
        {
            File.Delete(targetFile);
        }
        var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
        File.WriteAllText(targetFile, JsonConvert.SerializeObject(template, Formatting.Indented, parameters));
    }
}
