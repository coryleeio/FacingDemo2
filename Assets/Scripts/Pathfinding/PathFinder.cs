
using UnityEngine;

namespace Gamepackage
{
    public class PathFinder
    {
        public DiagonalOptions DiagonalSetting;
        private PathSolver solver = new PathSolver();

        public Path FindPath(Point start, Point end, Grid<Tile> grid)
        {
            return solver.FindPath(start, end, DiagonalSetting, grid);
        }

        public void Init(DiagonalOptions diagonalSetting)
        {
            Debug.Log("Pathfinding: Started");
            DiagonalSetting = diagonalSetting;
        }
    }
}



