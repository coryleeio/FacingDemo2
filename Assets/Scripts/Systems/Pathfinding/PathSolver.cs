using Priority_Queue;
using System;
using System.Collections.Generic;
namespace Gamepackage
{
    public class PathSolver
    {
        private static readonly int _searchLimit = 2000;
        private static readonly int _max_open_nodes = 2000;
        private static readonly int _orthogonal_weight = 10;  // 1 * 10          orthogonal weight multiplied by 10 to remain an int
        private static readonly int _diagonal_weight = 14;    // sqrt(2) * 10    diagonal weight multiplied by 10 to remain an int
        private FastPriorityQueue<PathNode> _open = new FastPriorityQueue<PathNode>(_max_open_nodes); // open min-priority queue sorted by lowest F
        private List<PathNode> _closed = new List<PathNode>();

        public Path FindPath(Point start, Point end, DiagonalOptions diagonalOptions, Grid<GraphNode> grid)
        {
            _open.Clear();
            _closed.Clear();
            var path = new Path();
            path.Reset();

            if (!grid.PointInGrid(start) || !grid.PointInGrid(end))
            {
                path.Errors.Add("Start or endpoint were not in the grid");
                return path;
            }

            if (!grid[end].Walkable)
            {
                path.Errors.Add("endpoint was not walkable");
                return path;
            }

            var parentNode = new PathNode();
            parentNode.G = 0;
            parentNode.H = 2;
            parentNode.F = parentNode.G + parentNode.H;
            parentNode.Position = new Point(start.X, start.Y);
            parentNode.parent = null;
            path.StartNode = parentNode;
            _open.Enqueue(parentNode, parentNode.F);

            while (_open.Count > 0)
            {
                parentNode = _open.Dequeue();
                if (parentNode.Position == end)
                {
                    path.Found = true;
                    break;
                }

                if (_closed.Count > _searchLimit)
                {
                    path.Errors.Add("Reached the search limit!");
                    return path;
                }

                var neighborPoints = MathUtil.GetPointsByOffset(parentNode.Position, MathUtil.OrthogonalOffsets);
                if(diagonalOptions == DiagonalOptions.DiagonalsWithCornerCutting || diagonalOptions == DiagonalOptions.DiagonalsWithoutCornerCutting)
                {
                    neighborPoints.AddRange(MathUtil.GetPointsByOffset(parentNode.Position, MathUtil.DiagonalOffsets));
                }


                foreach (var neighborPoint in neighborPoints)
                {
                    var neighbor = grid[neighborPoint];
                    if (!neighbor.Walkable)
                    {
                        continue;
                    }

                    var neighborNode = new PathNode();
                    neighborNode.Position = new Point(neighborPoint.X, neighborPoint.Y);

                    var moveWasDiagonal = parentNode.Position.X != neighborNode.Position.X && parentNode.Position.Y != neighborNode.Position.Y;

                    var newGValueForPath = parentNode.G + neighbor.Weight;

                    if (moveWasDiagonal)
                    {
                        if (diagonalOptions == DiagonalOptions.DiagonalsWithoutCornerCutting)
                        {
                            // If we dont allow cutting corners and this is a corner cut, skip this node,
                            // as it cannot be traversed from this position
                            var foundCornerCut = false;
                            foreach (var offset in MathUtil.DiagonalOffsets)
                            {
                                // If this is the correct offset, and either of its orthogonal offsets are not walkable
                                // skip this node as we are cutting a corner to walk here
                                var p1 = new Point(parentNode.Position.X + offset.X, parentNode.Position.Y);
                                var p2 = new Point(parentNode.Position.X, parentNode.Position.Y + offset.Y);
                                if (parentNode.Position.X + offset.X == neighborNode.Position.X && parentNode.Position.Y + offset.Y == neighborNode.Position.Y && (!grid[p1].Walkable || !grid[p2].Walkable))
                                {
                                    foundCornerCut = true;
                                    break; // break offset loop
                                }
                            }
                            if (foundCornerCut)
                            {
                                continue; // skip this node
                            }
                        }
                        newGValueForPath += _diagonal_weight;
                    }
                    else
                    {
                        newGValueForPath += _orthogonal_weight;
                    }

                    if (newGValueForPath == parentNode.G)
                    {
                        continue;
                    }

                    PathNode foundInOpen = null;
                    foreach (var openNode in _open)
                    {
                        if (openNode.Position.X == neighborNode.Position.X && openNode.Position.Y == neighborNode.Position.Y)
                        {
                            foundInOpen = openNode;
                            break;
                        }
                    }

                    if (foundInOpen != null && foundInOpen.G <= newGValueForPath)
                    {
                        // Found this in open, and its a worse path, skip this node
                        continue;
                    }

                    PathNode foundInClosed = null;
                    foreach (var closedNode in _closed)
                    {
                        if (closedNode.Position.X == neighborNode.Position.X && closedNode.Position.Y == neighborNode.Position.Y)
                        {
                            foundInClosed = closedNode;
                            break;
                        }
                    }

                    if (foundInClosed != null && foundInClosed.G <= newGValueForPath)
                    {
                        // Found in closed, and it is a worse path, skip this node
                        continue;
                    }

                    // New node not in the open list, or 
                    // or a node in the open list for which we have found a better path
                    // so calculate G H and F
                    // and put it in the queue, this is technically a duplicate PathNode, but since its at a lower F value
                    // it does not need to be removed from the priority queue
                    neighborNode.parent = parentNode;
                    neighborNode.G = newGValueForPath;
                    neighborNode.H = (int)(2 * (Math.Pow((neighborNode.Position.X - end.X), 2) + Math.Pow((neighborNode.Position.Y - end.Y), 2)));
                    neighborNode.F = neighborNode.G + neighborNode.H;

                    _open.Enqueue(neighborNode, neighborNode.F);
                }
                _closed.Add(parentNode);
            }

            if (path.Found)
            {
                // Recursively traverse each parent from the endpoint, then reverse the nodes, this is our path
                while (parentNode.parent != null)
                {
                    path.Nodes.Add(parentNode);
                    parentNode = parentNode.parent;
                }
                path.Nodes.Reverse();
                return path;
            }
            _closed.Clear();
            return path;
        }
    }
}