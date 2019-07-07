using System;

namespace Gamepackage
{
    [Serializable]
    public class Rectangle
    {
        public Point Position;
        public int Height;
        public int Width;

        public bool Intersects(Rectangle other)
        {
            return Intersects(this, other);
        }

        public static bool Intersects(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Position.X < rect2.Position.X + rect2.Width &&
                    rect1.Position.X + rect1.Width > rect2.Position.X &&
                    rect1.Position.Y < rect2.Position.Y + rect2.Height &&
                    rect1.Height + rect1.Position.Y > rect2.Position.Y;
        }

        public bool Contains(Point p)
        {
            return Contains(this, p);
        }

        public bool Contains(Rectangle b)
        {
            return Contains(this, b);
        }

        public static bool Contains(Rectangle a, Rectangle b)
        {
            return ((b.Position.X + b.Width) < (a.Position.X + a.Width)
                    && (b.Position.X) > (a.Position.X)
                    && (b.Position.Y) > (a.Position.Y)
                    && (b.Position.Y + b.Height) < (a.Position.Y + a.Height));
        }

        public static bool Contains(Rectangle rect, Point p)
        {
            return p.X >= rect.Position.X && p.X < rect.Position.X + rect.Width &&
            p.Y >= rect.Position.Y && p.Y < rect.Position.Y + rect.Height;
        }

        public bool Adjacent(Rectangle other)
        {
            return Adjacent(this, other);
        }

        public static bool Adjacent(Rectangle first, Rectangle second)
        {
            return
            first.Position.X + first.Width == second.Position.X ||
            first.Position.Y + first.Height == second.Position.Y ||
            second.Position.X + second.Width == first.Position.X ||
            second.Position.Y + second.Height == first.Position.Y;
        }
    }

}
