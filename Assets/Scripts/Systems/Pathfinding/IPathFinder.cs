namespace Gamepackage
{
    public interface IPathFinder
    {
        void StartPath(Point start, Point end, OnPathComplete handler);

        void StartPath(int startX, int startY, int endX, int endY, OnPathComplete handler);

        void Init(int gridSizeX, int gridSizeY, GridGraph.DiagonalOptions diagonalSetting, int numberOfThreads);

        void Process();

        void Cleanup();
    }
}
