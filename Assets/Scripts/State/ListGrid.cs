using Gamepackage;
using System.Collections.Generic;

namespace Gamepackage
{
    public class ListGrid<TNode> : Grid<List<TNode>>
    {
        public ListGrid(int width, int height, int defaultNumberOfItems = 0) : base(width, height)
        {
            for(var x = 0; x < width; x++)
            {
                for(var y = 0; y < height; y++)
                {
                    this[x, y] = new List<TNode>(defaultNumberOfItems);
                }
            }
        }
    }
}
