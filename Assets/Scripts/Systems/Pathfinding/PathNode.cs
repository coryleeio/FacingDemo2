using Priority_Queue;
namespace Gamepackage
{
    public class PathNode : FastPriorityQueueNode
    {
        public Point Position;
        public int F;
        public int G;
        public int H;
        public PathNode parent;
    }
}