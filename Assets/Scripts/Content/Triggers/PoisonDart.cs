using System.Collections.Generic;

namespace Gamepackage
{
    public class PoisonDart : Trigger
    {
        public UniqueIdentifier EffectToApply;

        public PoisonDart()
        {

        }

        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }

        private List<Point> _offsets = new List<Point>() {new Point(0,0) };
        public override List<Point> Offsets
        {
            get {
                return _offsets;

            }
        }

        public override bool IsStartable
        {
            get
            {
                return true;
            }
        }
    }
}
