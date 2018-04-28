using Priority_Queue;
namespace Gamepackage
{
    public class PathNode : FastPriorityQueueNode
    {
        public int X;
        public int Y;
        public int F;
        public int G;
        public int H;
        public Point Position
        {
            get
            {
                return new Point(X, Y);
            }
        }
        public PathNode parent;
    }
}