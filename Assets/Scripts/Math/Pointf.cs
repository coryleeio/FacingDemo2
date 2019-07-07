using System;
using UnityEngine;

namespace Gamepackage
{
    [Serializable]
    public class Pointf
    {
        public float X;
        public float Y;

        public Pointf(float x, float y)
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
            if (obj.GetType() == typeof(Pointf))
            {
                var objAsPoint = obj as Pointf;
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

        public static bool operator ==(Pointf point1, Pointf point2)
        {
            if (object.ReferenceEquals(point1, null))
            {
                return object.ReferenceEquals(point2, null);
            }

            return point1.Equals(point2);
        }

        public static bool operator !=(Pointf lhs, Pointf rhs)
        {
            return !(lhs == rhs);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public Point Round()
        {
            return new Point(Mathf.RoundToInt(X), Mathf.RoundToInt(Y));
        }
    }
}