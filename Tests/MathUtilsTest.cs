using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gamepackage;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class MathUtilsTest
    {
        [TestMethod]
        public void TestBounding()
        {
            Point first = new Point(1, 1);
            Point second = new Point(4, 4);
            var bound = MathUtil.BoundingRectangleForPoints(new List<Point>() { first, second });
            Assert.AreEqual(first, bound.Position);
            Assert.AreEqual(3, bound.Width);
            Assert.AreEqual(3, bound.Height);
        }

        [TestMethod]
        public void TestLocalSpaceConversion()
        {
            Point position = new Point(1, 1);
            Point other = new Point(4, 4);
            var localSpaceOther = MathUtil.ConvertMapSpaceToLocalMapSpace(position, other);
            Assert.AreEqual(new Point(3, 3), localSpaceOther);


            var backToWorldSpace = MathUtil.ConvertLocalMapSpaceToMapSpace(position, localSpaceOther);
            Assert.AreEqual(other, backToWorldSpace);
        }

        [TestMethod]
        public void TestRotationOrthogonal()
        {
            Point position = new Point(0, 0);
            Point other = new Point(1, 0);
            var rotated = MathUtil.RotatePointInDirection(position, other, Direction.SouthWest);
            Assert.AreEqual(new Point(0, 1), rotated);

            position = new Point(1, 1);
            other = new Point(4, 1);
            rotated = MathUtil.RotatePointInDirection(position, other, Direction.SouthWest);
            Assert.AreEqual(new Point(1, 4), rotated);
        }
    }
}
