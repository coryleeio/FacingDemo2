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

        public static bool PointsAreOrthogonal(Point a, Point b)
        {
            return (a.X == b.X || a.Y == b.Y);
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

        public static bool IsAdjacent(Point p1, Point p2)
        {
            return OffsetBy(p1, p2, MathUtil.SurroundingOffsets);
        }

        public static bool IsDiagonallyAdjacent(Point p1, Point p2)
        {
            return OffsetBy(p1, p2, MathUtil.DiagonalOffsets);
        }

        public static bool IsOrthogonallyAdjacent(Point p1, Point p2)
        {
            return OffsetBy(p1, p2, MathUtil.OrthogonalOffsets);
        }

        public bool OffsetBy(Point p2, Point[] offsets)
        {
            return OffsetBy(this, p2, offsets);
        }

        public bool OrthgonalTo(Point p2)
        {
            return PointsAreOrthogonal(this, p2);
        }
    }
}