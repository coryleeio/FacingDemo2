using System;
using System.Linq;
using UnityEngine;

namespace Gamepackage
{
    public static class Vector2Extensions
    {
        public static Pointf ToPointf(this Vector2 vector2)
        {
            return new Pointf(vector2.x, vector2.y);
        }
    }
}
