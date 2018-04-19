using UnityEngine;

namespace Gamepackage
{
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return "{ x: " + X + " y: " + Y + " }";
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Point))
            {
                var objAsPoint = obj as Point;
                return X == objAsPoint.X && Y == objAsPoint.Y;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ X.GetHashCode();
                hash = (hash * 16777619) ^ Y.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Point point1, Point point2)
        {
            if (object.ReferenceEquals(point1, null))
            {
                return object.ReferenceEquals(point2, null);
            }

            return point1.Equals(point2);
        }

        public static bool operator !=(Point lhs, Point rhs)
        {
            return !(lhs == rhs);
        }

        public static float Distance(Point p1, Point p2)
        {
            return Mathf.Sqrt(Mathf.Pow(p2.X - p1.X, 2) + Mathf.Pow(p2.Y - p1.Y, 2));
        }

        public static float DistanceSquared(Point p1, Point p2)
        {
            return Mathf.Pow(p2.X - p1.X, 2) + Mathf.Pow(p2.Y - p1.Y, 2);
        }

        public static bool IsOrthogonal(Point a, Point b)
        {
            return (Mathf.Abs(a.X) == Mathf.Abs(b.X) || Mathf.Abs(a.Y) == Mathf.Abs(b.Y));
        }

        public static bool IsDiagonalTo(Point a, Point b)
        {
            if((a.X - b.X) == 0)
            {
                return false;
            }
            return Mathf.Abs( (a.Y - b.Y) / (a.X - b.X) ) == 1;
        }

        public static bool OffsetBy(Point p1, Point p2, Point[] offsets)
        {
            foreach (var offset in offsets)
            {
                if (p1.X + offset.X == p2.X && p1.Y + offset.Y == p2.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsAdjacentTo(Point p1, Point p2)
        {
            return OffsetBy(p1, p2, MathUtil.SurroundingOffsets);
        }

        public static bool IsDiagonallyAdjacentTo(Point p1, Point p2)
        {
            return IsAdjacentTo(p1, p2) && IsDiagonalTo(p1, p2);
        }

        public static bool IsOrthogonallyAdjacentTo(Point p1, Point p2)
        {
            return IsAdjacentTo(p1, p2) && IsOrthogonal(p1, p2);
        }

        public bool OffsetBy(Point p2, Point[] offsets)
        {
            return OffsetBy(this, p2, offsets);
        }

        public bool IsOrthogonalTo(Point p2)
        {
            return IsOrthogonal(this, p2);
        }
        public bool IsDiagonalTo(Point p2)
        {
            return IsDiagonalTo(this, p2);
        }
    }
}