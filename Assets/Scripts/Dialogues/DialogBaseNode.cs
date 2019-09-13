using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Gamepackage
{
    public abstract class DialogBaseNode
    {
        public string Identifier;
        public string Text;
        public int Weight = 1;
        public List<DialogBaseNode> Children = new List<DialogBaseNode>();
        public List<Conditional> Conditions = new List<Conditional>();
        public Dictionary<string, string> Set = new Dictionary<string, string>();

        public bool AllConditionsAreMet(Game game, Dialog state)
        {
            return Conditions.FindAll(x => x.ConditionalImpl.Satisfied(game, state, x.Parameters)).ToList().Count == Conditions.Count;
        }

        [JsonIgnore]
        public bool Valid
        {
            get
            {
                return Identifier != null && SelfValid && ChildrenValid;
            }
        }

        [JsonIgnore]
        public abstract bool SelfValid
        {
            get;
        }

        [JsonIgnore]
        public bool ChildrenValid
        {
            get
            {
                return Children.FindAll(x => x.Valid).ToList().Count == Children.Count;
            }
        }
    }
}
