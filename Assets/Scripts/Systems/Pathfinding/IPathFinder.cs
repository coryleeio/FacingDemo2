namespace Gamepackage
{
    public interface IPathFinder
    {
        void StartPath(Point start, Point end, Grid<GraphNode> Grid, OnPathComplete handler);

        void Init(DiagonalOptions setting, int numberOfThreads);

        void Process();

        void Cleanup();
    }
}
