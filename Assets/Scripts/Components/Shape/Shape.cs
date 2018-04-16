using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public enum ShapeType
    {
        Rect, // 0
        Plus, // 1
        HollowPlus // 2
    }

    public class Shape
    {
        public Shape()
        { }

        public Shape(ShapeType shapeType, int width, int height)
        {
            _position = new Point(0, 0);
            _shapeType = shapeType;
            _width = width;
            _height = height;
            Recalculate();
        }

        private int _width;
        private int _height;
        private List<Point> points = new List<Point>(0);

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
                foreach(var point in points)
                {
                    foreach(var otherPoint in shape.points)
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
                foreach(var point in points)
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
            points.Clear();
            if (_shapeType == ShapeType.Rect)
            {
                BoundingRectangle.Position = Position;
                BoundingRectangle.Width = Width;
                BoundingRectangle.Height = Height;
                points.AddRange(MathUtil.PointsInRect(BoundingRectangle));
            }
            else if (_shapeType == ShapeType.Plus || _shapeType == ShapeType.HollowPlus)
            {
                var startingPosition = new Point(Position.X, Position.Y);
                if (_shapeType == ShapeType.Plus)
                {
                    points.Add(startingPosition);
                }
                var currentN = MathUtil.GetPointByOffset(startingPosition, MathUtil.NorthOffset);
                var currentS = MathUtil.GetPointByOffset(startingPosition, MathUtil.SouthOffset);
                var currentE = MathUtil.GetPointByOffset(startingPosition, MathUtil.EastOffset);
                var currentW = MathUtil.GetPointByOffset(startingPosition, MathUtil.WestOffset);

                for(var w = 0; w < Width; w++)
                {
                    points.Add(currentE);
                    points.Add(currentW);
                    currentE = MathUtil.GetPointByOffset(currentE, MathUtil.EastOffset);
                    currentW = MathUtil.GetPointByOffset(currentW, MathUtil.WestOffset);
                }
                for (var h = 0; h < Height; h++)
                {
                    points.Add(currentN);
                    points.Add(currentS);
                    currentN = MathUtil.GetPointByOffset(currentN, MathUtil.NorthOffset);
                    currentS = MathUtil.GetPointByOffset(currentS, MathUtil.SouthOffset);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
