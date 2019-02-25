using System.Collections.Generic;

namespace Gamepackage
{
    public class Trigger
    {
        public List<Effect> Effects = new List<Effect>();
        public List<Point> Offsets = new List<Point>(0);
        public Dictionary<string, string> TriggerParameters = new Dictionary<string, string>();
    }
}
