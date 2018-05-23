using System.Collections.Generic;

namespace Gamepackage
{
    public class TraverseStaircase : Trigger
    {
        public override bool IsComplete
        {
            get
            {
                return true;
            }
        }

        private List<Point> _offsets = new List<Point>() { new Point(0, 0) };
        public override List<Point> Offsets
        {
            get
            {
                return _offsets;

            }
        }
    }
}
