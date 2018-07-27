using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class MathUtil
    {
        public static float TileHeight = 0.5f;
        public static float TileWidth = 1.0f;

        public static float HalfTileWidth = TileWidth / 2.0f;
        public static float HalfTileHeight = TileHeight / 2.0f;
        public static Point NorthEastOffset = new Point(0, -1);
        public static Point NorthWestOffset = new Point(-1, 0);
        public static Point SouthEastOffset = new Point(1, 0);
        public static Point SouthWestOffset = new Point(0, 1);

        public static Point NorthOffset = new Point(-1, -1);
        public static Point EastOffset = new Point(1, -1);
        public static Point WestOffset = new Point(-1, 1);
        public static Point SouthOffset = new Point(1, 1);

        public static Point[] OrthogonalOffsets = new Point[] { SouthEastOffset, NorthWestOffset, SouthWestOffset, NorthEastOffset };
        public static Point[] DiagonalOffsets = new Point[] { NorthOffset, EastOffset, SouthOffset, WestOffset };
        public static Point[] SurroundingOffsets = new Point[] { SouthEastOffset, NorthWestOffset, SouthWestOffset, NorthEastOffset, NorthOffset, EastOffset, SouthOffset, WestOffset, };

        public static List<Point> RotatePointsInDirection(Point center, List<Point> pointsToRotate, Direction dir)
        {
            var returnValue = new List<Point>(pointsToRotate.Count);
            foreach(var point in pointsToRotate)
            {
                returnValue.Add(RotatePointInDirection(center, point, dir));
            }
            return returnValue;
        }

        public static Point RotatePointInDirection(Point center, Point pointToRotate, Direction dir)
        {
            var radians = 0.0f;
            // You could specify this in radians in the first place to avoid this calculation
            if (dir == Direction.SouthEast)
            {
                radians = Mathf.Deg2Rad * 0;
            }
            else if (dir == Direction.SouthWest)
            {
                radians = Mathf.Deg2Rad * 90;
            }
            else if (dir == Direction.NorthWest)
            {
                radians = Mathf.PI; // 180
            }
            else if (dir == Direction.NorthEast)
            {
                radians = Mathf.Deg2Rad * 270;
            }
            else if (dir == Direction.South)
            {
                radians = Mathf.Deg2Rad * 45;
            }
            else if (dir == Direction.West)
            {
                radians = Mathf.Deg2Rad * 135;
            }
            else if (dir == Direction.North)
            {
                radians = Mathf.Deg2Rad * 225;
            }
            else if(dir == Direction.East)
            {
                radians = Mathf.Deg2Rad * 315;
            }
            else
            {
                throw new NotImplementedException();
            }

            var pointInLocalSpace = ConvertMapSpaceToLocalMapSpace(center, pointToRotate);
            var qx = (float)(pointInLocalSpace.X * Math.Cos(radians) - pointInLocalSpace.Y * Math.Sin(radians));
            var qy = (float)(pointInLocalSpace.X * Math.Sin(radians) + pointInLocalSpace.Y * Math.Cos(radians));
            var rotatedPoint = new Point(Mathf.RoundToInt(qx), Mathf.RoundToInt(qy));
            return ConvertLocalMapSpaceToMapSpace(center, rotatedPoint);
        }

        public static Rectangle BoundingRectangleForPoints(List<Point> points)
        {
            var lowestX = points[0].X;
            var lowestY = points[0].Y;
            var highestX = points[0].X;
            var highestY = points[0].Y;

            foreach(var point in points)
            {
                if(point.X < lowestX)
                {
                    lowestX = point.X;
                }
                if(point.X > highestX)
                {
                    highestX = point.X;
                }
                if(point.Y < lowestY)
                {
                    lowestY = point.Y;
                }
                if(point.Y > highestY)
                {
                    highestY = point.Y;
                }

            }
            return new Rectangle()
            {
                Position = new Point(lowestX, lowestY),
                Width = highestX - lowestX,
                Height = highestY - lowestY
            };
        }

        public static List<Point> ConvertLocalMapSpaceToMapSpace(Point Position, List<Point> Points)
        {
            var returnPoints = new List<Point>(Points.Count);
            foreach (var point in Points)
            {
                returnPoints.Add(ConvertLocalMapSpaceToMapSpace(Position, point));
            }
            return returnPoints;
        }

        public static Point ConvertLocalMapSpaceToMapSpace(Point Position, Point other)
        {
            return new Point(other.X + Position.X, other.Y + Position.Y);
        }

        public static List<Point> ConvertMapSpaceToLocalMapSpace(Point Position, List<Point> Points)
        {
            var returnPoints = new List<Point>(Points.Count);
            foreach (var point in Points)
            {
                returnPoints.Add(ConvertMapSpaceToLocalMapSpace(Position, point));
            }
            return returnPoints;
        }

        public static Point ConvertMapSpaceToLocalMapSpace(Point Position, Point other)
        {
            return new Point(other.X - Position.X, other.Y - Position.Y);
        }

        public static int RelativeCoordinateOffset(int source, int target)
        {
            if (source == target)
            {
                return 0;
            }
            if (target < source)
            {
                return -1;
            }
            return 1;
        }

        public static Direction RelativeDirection(Point source, Point target)
        {
            Point offset = new Point(RelativeCoordinateOffset(source.X, target.X), RelativeCoordinateOffset(source.Y, target.Y));
            if (offset.X == SouthEastOffset.X && offset.Y == SouthEastOffset.Y)
            {
                return Direction.SouthEast;
            }
            if (offset.X == SouthWestOffset.X && offset.Y == SouthWestOffset.Y)
            {
                return Direction.SouthWest;
            }
            if (offset.X == NorthWestOffset.X && offset.Y == NorthWestOffset.Y)
            {
                return Direction.NorthWest;
            }
            if (offset.X == NorthEastOffset.X && offset.Y == NorthEastOffset.Y)
            {
                return Direction.NorthEast;
            }
            if (offset.X == NorthOffset.X && offset.Y == NorthOffset.Y)
            {
                return Direction.North;
            }
            if (offset.X == SouthOffset.X && offset.Y == SouthOffset.Y)
            {
                return Direction.South;
            }
            if (offset.X == EastOffset.X && offset.Y == EastOffset.Y)
            {
                return Direction.East;
            }
            if (offset.X == WestOffset.X && offset.Y == WestOffset.Y)
            {
                return Direction.West;
            }
            return Direction.SouthEast;
        }
        public static List<TChoose> ChooseNRandomElements<TChoose>(int n, List<TChoose> elements)
        {
            List<TChoose> output = new List<TChoose>();
            for (int i = 0; i < n && i < elements.Count; i++)
            {
                TChoose chosen = default(TChoose);
                do
                {
                    chosen = ChooseRandomElement<TChoose>(elements);
                }
                while ((output.Contains(chosen)));
                output.Add(chosen);
            }
            return output;
        }

        // min inclusive, max exclusive
        public static int ChooseRandomIntInRange(int minimum, int maximum)
        {
            return UnityEngine.Random.Range(minimum, maximum);
        }

        public static TChoose ChooseRandomElement<TChoose>(List<TChoose> elements)
        {
            return elements[ChooseRandomIntInRange(0, elements.Count)];
        }

        public static Vector2 MapToWorld(Point p)
        {
            return MapToWorld(p.X, p.Y);
        }

        public static Vector2 MapToWorld(Pointf p)
        {
            return MapToWorld(p.X, p.Y);
        }

        public static Vector2 MapToWorld(int x, int y)
        {
            return MapToWorld(x * 1.0f, y * 1.0f);
        }

        public static Vector2 MapToWorld(float x, float y)
        {
            var newX = (x - y) * HalfTileWidth;
            var newY = (-x - y) * HalfTileHeight;
            return new Vector2(newX, newY);
        }

        public static Vector2 MapToWorld(float x, float y, float height)
        {
            var amountToSubtract = 0.0f;
            amountToSubtract += height;

            var newPos = MathUtil.MapToWorld(x - amountToSubtract, y - amountToSubtract);
            newPos = new Vector2(newPos.x, newPos.y);
            return newPos;
        }

        public static Point WorldToMap(Vector3 v)
        {
            var isoX = v.x;
            var isoY = v.y;

            var cartX = (isoX / HalfTileWidth - isoY / HalfTileHeight) / 2.0f;
            var cartY = (-isoY / HalfTileHeight - isoX / HalfTileWidth) / 2.0f;

            return new Point(Mathf.FloorToInt(cartX), Mathf.FloorToInt(cartY));
        }

        public static Vector3 GetMousePositionInScreenCoordinates()
        {
            return Input.mousePosition;
        }

        private static Vector3 ScreenToWorld(Camera camera, Vector3 screenCoordinates)
        {
            var ray = camera.ScreenPointToRay(screenCoordinates);
            // create a plane at 0,0,0 whose normal points to +Y:
            var hPlane = new Plane(Vector3.back, Vector3.zero);
            float distance;
            // if the ray hits the plane...
            if (hPlane.Raycast(ray, out distance))
            {
                // get the hit point:
                return ray.GetPoint(distance);
            }
            return Vector3.zero;
        }

        public static Vector3 GetMousePositionInWorldCoordinates(Camera camera)
        {
            return ScreenToWorld(camera, GetMousePositionInScreenCoordinates());
        }

        public static Point GetMousePositionOnMap(Camera camera)
        {
            var v = GetMousePositionInWorldCoordinates(camera);
            return WorldToMap(v);
        }
        public enum FloodFillType
        {
            Orthogonal,
            Diagonal,
            Surrounding,
        }

        public static bool ChanceToOccur(int percentile)
        {
            return UnityEngine.Random.Range(0, 100) > percentile;
        }

        public static List<Point> FloodFill(Point p, int radius, ref List<Point> points, FloodFillType floodFillType, Predicate<Point> predicate = null)
        {
            if (predicate == null || predicate(p))
            {
                if (!points.Contains(p))
                {
                    points.Add(p);
                }
            }

            if (radius > 0)
            {
                List<Point> relevantTiles = null;
                switch (floodFillType)
                {
                    case FloodFillType.Orthogonal:
                        relevantTiles = OrthogonalPoints(p);
                        break;
                    case FloodFillType.Diagonal:
                        relevantTiles = DiagonalPoints(p);
                        break;
                    case FloodFillType.Surrounding:
                        relevantTiles = SurroundingPoints(p);
                        break;
                    default:
                        throw new NotImplementedException("Not implemented");
                }

                foreach (var relevantTile in relevantTiles)
                {
                    if (predicate == null || predicate(relevantTile))
                    {
                        FloodFill(relevantTile, radius - 1, ref points, floodFillType, predicate);
                    }
                }
            }

            return points;
        }

        public static List<Point> GetPointsByOffset(Point point, List<Point> offsets)
        {
            var list = new List<Point>();
            foreach (var offset in offsets)
            {
                list.Add(GetPointByOffset(point, offset));
            }
            return list;
        }

        public static List<Point> GetPointsByOffset(Point point, Point[] offsets)
        {
            var list = new List<Point>();
            foreach (var offset in offsets)
            {
                list.Add(GetPointByOffset(point, offset));
            }
            return list;
        }

        public static Point GetPointByOffset(Point point, Point offset)
        {
            var p = new Point(point.X + offset.X, point.Y + offset.Y);
            return p;
        }

        public static List<Point> OrthogonalPoints(Point p)
        {
            return GetPointsByOffset(p, OrthogonalOffsets);
        }

        public static List<Point> DiagonalPoints(Point p)
        {
            return GetPointsByOffset(p, DiagonalOffsets);
        }

        public static List<Point> SurroundingPoints(Point p)
        {
            return GetPointsByOffset(p, SurroundingOffsets);
        }

        public static List<Point> PointsInRects(List<Rectangle> rects)
        {
            var aggregate = new List<Point>();
            foreach (var rect in rects)
            {
                var points = PointsInRect(rect);
                foreach (var point in points)
                {
                    if (!aggregate.Contains(point))
                    {
                        aggregate.Add(point);
                    }
                }
            }
            return aggregate;
        }

        public static List<Point> PointsInRect(Rectangle rect)
        {
            var points = new List<Point>();
            for (int x = rect.Position.X; x < rect.Position.X + rect.Width; x++)
            {
                for (int y = rect.Position.Y; y < rect.Position.Y + rect.Height; y++)
                {
                    points.Add(new Point(x, y));
                }
            }
            return points;
        }

    }
}
