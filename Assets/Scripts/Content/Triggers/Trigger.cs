using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class Trigger : ASyncAction
    {
        public abstract List<Point> Offsets
        {
            get;
        }
        public List<int> TargetIds = new List<int>(0);
        public Dictionary<string, string> Parameters = new Dictionary<string, string>(0);

        [JsonIgnore]
        public List<Token> Targets
        {
            get
            {
                var list = new List<Token>(TargetIds.Count);
                foreach(var id in TargetIds)
                {
                    var token = Context.TokenSystem.GetTokenById(id);
                    list.Add(token);
                }
                return list;
            }
        }

        public override void Reset()
        {
            base.Reset();
            TargetIds.Clear();
        }
    }
}
