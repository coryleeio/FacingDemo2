using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public enum ShapeType
    {
        Rect, // 0
        Plus, // 1
        HollowPlus // 2
    }

    [Serializable]
    public class Shape
    {
        public Shape()
        { }

        public Shape(ShapeType shapeType, int width, int height, Direction Rotation = Direction.SouthEast)
        {
            _position = new Point(0, 0);
            _shapeType = shapeType;
            _width = width;
            _height = height;
            Recalculate();
        }

        private int _width;
        private int _height;

        private List<Point> _points = new List<Point>(0);
        public List<Point> Points
        {
            set
            {
                _points = value;
            }
            get
            {
                return _points;
            }
        }

        private List<Point> _offsets = new List<Point>(0);
        public List<Point> Offsets
        {
            get
            {
                return _offsets;
            }
        }

        private ShapeType _shapeType;
        public ShapeType ShapeType
        {
            get
            {
                return _shapeType;
            }
            set
            {
                _shapeType = value;
                Recalculate();
            }
        }

        private Direction _direction;
        public Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                Recalculate();
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                Recalculate();
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                Recalculate();
            }
        }

        private Point _position;
        public Point Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = new Point(value.X, value.Y);
                Recalculate();
            }
        }

        public bool Intersects(Shape shape)
        {
            if(BoundingRectangle.Intersects(shape.BoundingRectangle))
            {
                foreach(var point in Points)
                {
                    foreach(var otherPoint in shape.Points)
                    {
                        if (point == otherPoint)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(Point p)
        {
            if(BoundingRectangle.Contains(p))
            {
                foreach(var point in Points)
                {
                    if(point == p)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public Rectangle BoundingRectangle = new Rectangle();

        public void Recalculate()
        {
            Points.Clear();
            Offsets.Clear();
            if (_shapeType == ShapeType.Rect)
            {
                BoundingRectangle.Position = Position;
                BoundingRectangle.Width = Width;
                BoundingRectangle.Height = Height;
                Points.AddRange(MathUtil.PointsInRect(BoundingRectangle));
                Offsets.AddRange(MathUtil.ConvertMapSpaceToLocalMapSpace(Position, Points));
                ApplyRotation(Points);
            }

            else if (_shapeType == ShapeType.Plus || _shapeType == ShapeType.HollowPlus)
            {
                var startingPosition = new Point(Position.X, Position.Y);
                if (_shapeType == ShapeType.Plus)
                {
                    Points.Add(startingPosition);
                }
                var currentN = new Point(startingPosition.X, startingPosition.Y);
                var currentS = new Point(startingPosition.X, startingPosition.Y);
                var currentE = new Point(startingPosition.X, startingPosition.Y);
                var currentW = new Point(startingPosition.X, startingPosition.Y);

                for (var w = 0; w < Width; w++)
                {
                    currentE = MathUtil.GetPointByOffset(currentE, MathUtil.NorthEastOffset);
                    currentW = MathUtil.GetPointByOffset(currentW, MathUtil.NorthWestOffset);
                    Points.Add(currentE);
                    Points.Add(currentW);
                }
                for (var h = 0; h < Height; h++)
                {
                    currentN = MathUtil.GetPointByOffset(currentN, MathUtil.SouthEastOffset);
                    currentS = MathUtil.GetPointByOffset(currentS, MathUtil.SouthWestOffset);
                    Points.Add(currentN);
                    Points.Add(currentS);
                }
                Offsets.AddRange(MathUtil.ConvertMapSpaceToLocalMapSpace(Position, Points));
                BoundingRectangle = MathUtil.BoundingRectangleForPoints(Points);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void ApplyRotation(List<Point> points)
        {
            // this could be done in place to not create garbage
            Points = MathUtil.RotatePointsInDirection(Position, points, Direction);
        }
    }
}
