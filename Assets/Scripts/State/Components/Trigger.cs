using System.Collections.Generic;

namespace Gamepackage
{
    public class Trigger : Component
    {
        public Ability Ability;
        public List<Point> Offsets = new List<Point>(0);
        public Dictionary<string, string> TriggerParameters = new Dictionary<string, string>();

        public override void Rewire(Entity entity)
        {
            base.Rewire(entity);
            if(Ability != null)
            {
                Ability.Source = entity;
            }
        }
    }
}
