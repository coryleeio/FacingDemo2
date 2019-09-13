using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class Dialog
    {
        public static int ID_UNSET = -1;
        public int Id = ID_UNSET;
        public string TemplateIdentifier;
        public string CurrentNodeIdentifier;
        public Dictionary<string, string> Data = new Dictionary<string, string>();

        [JsonIgnore]
        private DialogNode _template;
        [JsonIgnore]
        public DialogNode Template
        {
            get
            {
                if (_template == null)
                {
                    _template = Context.ResourceManager.Load<DialogNode>(TemplateIdentifier);
                }
                return _template;
            }
        }

        [JsonIgnore]
        private DialogBaseNode _currentNode;
        [JsonIgnore]
        public DialogBaseNode CurrentNode
        {
            get
            {
                if(_currentNode == null)
                {
                    var found = GetNode(CurrentNodeIdentifier, Template);
                    if (found == null)
                    {
                        Debug.LogError("Dialog node: " + CurrentNodeIdentifier + " does not exist.");
                    }
                    _currentNode = found;
                }
                return _currentNode;
            }
            set
            {
                if(value == null)
                {
                    CurrentNodeIdentifier = null;
                }
                else
                {
                    CurrentNodeIdentifier = value.Identifier;
                }
                _currentNode = value;
            }
        }

        public DialogBaseNode GetNode(string identifier, DialogBaseNode node)
        {
            if(identifier == node.Identifier)
            {
                return node;
            }
            else
            {
                foreach (var child in node.Children)
                {
                    var childRes = GetNode(identifier, child);
                    if (childRes != null)
                    {
                        return childRes;
                    }
                }
            }
            return null;
        }
    }
}
