using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class Trigger : ASyncAction
    {


        public abstract List<Point> Offsets
        {
            get;
        }

      
    }
}
