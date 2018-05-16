using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class Grid<TGrid>
    {
        [JsonProperty]
        private TGrid[,] _grid;

        [JsonProperty]
        private Rectangle _boundingBox;

        // Define the indexer to allow client code to use [] notation.
        public TGrid this[int x, int y]
        {
            get { return this._grid[x, y]; }
            set { this._grid[x, y] = value; }
        }

        public TGrid this[Point p]
        {
            get { return this._grid[p.X, p.Y]; }
            set { this._grid[p.X, p.Y] = value; }
        }

        [JsonIgnore]
        public int SizeX
        {
            get
            {
                return _grid.GetLength(0);
            }
        }

        [JsonIgnore]
        public int SizeY
        {
            get
            {
                return _grid.GetLength(1);
            }
        }

        private Grid()
        {

        }

        public Grid(int sizeX, int sizeY)
        {
            _grid = new TGrid[sizeX, sizeY];
            _boundingBox = new Rectangle()
            {
                Position = new Point(0, 0),
                Width = sizeX,
                Height = sizeY
            };
            Each((x, y, v) =>
            {
                this[x, y] = default(TGrid);
            });
        }

        public bool PointInGrid(Point p)
        {
            return _boundingBox.Contains(p);
        }

        public List<TGrid> Filter(Predicate<TGrid> predicate)
        {
            var lis = new List<TGrid>();
            Each((x, y, v) =>
            {
                if (predicate(this[x, y]))
                {
                    lis.Add(this[x, y]);
                }
            });
            return lis;
        }

        public delegate void GridEachDelegate(int x, int y, TGrid value);
        public void Each(GridEachDelegate fn)
        {
            for (var x = 0; x < SizeX; x++)
            {
                for (var y = 0; y < SizeY; y++)
                {
                    fn(x, y, this[x, y]);
                }
            }
        }
    }
}


