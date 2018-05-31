using System.Collections.Generic;

namespace Gamepackage
{
    public class Player : Behaviour
    {
        public override List<EntityAction> GetNextActions()
        {
            return new List<EntityAction>(0);
        }

        public override bool IsPlayer
        {
            get
            {
                return true;
            }
        }
    }
}
