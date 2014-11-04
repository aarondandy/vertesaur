using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.Core.Test
{

    /// <summary>
    /// Tests for multi-point.
    /// </summary>
    [TestFixture]
    public class Multipoint2Test
    {

        private Point2[] _points;

        [SetUp]
        public void SetUp() {
            _points = new[]{
			    new Point2(0,0),
			    new Point2(-1,-1),
			    new Point2(2,1),
			    new Point2(0,4)
		    };
        }

        [Test]
        public void IntersectsTest() {
            var target = new MultiPoint2(_points);
            foreach (Point2 p in _points) {
                Assert.IsTrue(target.Intersects(p));
                if (p.X != p.Y) {
                    Assert.IsFalse(target.Intersects(new Point2(p.Y, p.X)));
                }
            }
        }

        [Test]
        public void GetMinimumBoundingRectangleTest() {
            var target = new MultiPoint2(_points);
            var mbr = new Mbr(_points[0]);
            for (int i = 1; i < _points.Length; i++) {
                mbr = mbr.Encompass(_points[i]);
            }
            Assert.AreEqual(mbr, target.GetMbr());
        }

        [Test]
        public void GetCentroidTest() {
            var target = new MultiPoint2(_points);
            Assert.AreEqual(new Point2(.25, 1), target.GetCentroid());
        }

        [Test]
        public void DistanceTest() {
            var target = new MultiPoint2(_points);
            foreach (Point2 p in _points) {
                Assert.AreEqual(0, target.Distance(p));
            }
            Assert.AreEqual(System.Math.Sqrt(5), target.Distance(new Point2(-1, 2)));
            Assert.AreEqual(System.Math.Sqrt(4), target.Distance(new Point2(0, 2)));
            Assert.AreEqual(System.Math.Sqrt(2), target.Distance(new Point2(1, 2)));
            Assert.AreEqual(1, target.Distance(new Point2(2, 2)));
        }

        [Test]
        public void DistanceSquaredTest() {
            var target = new MultiPoint2(_points);
            foreach (Point2 p in _points) {
                Assert.AreEqual(0, target.DistanceSquared(p));
            }
            Assert.AreEqual(5, target.DistanceSquared(new Point2(-1, 2)));
            Assert.AreEqual(4, target.DistanceSquared(new Point2(0, 2)));
            Assert.AreEqual(2, target.DistanceSquared(new Point2(1, 2)));
            Assert.AreEqual(1, target.DistanceSquared(new Point2(2, 2)));
        }

        [Test]
        public void MultipointPointsConstructorTest() {
            var target = new MultiPoint2(_points);
            Assert.AreEqual(_points.Length, target.Count);
        }

        [Test]
        public void MultipointExpectedSizeConstructorTest() {
            var target = new MultiPoint2(10);
            Assert.AreEqual(0, target.Count);
        }

        [Test]
        public void MultipointDefaultConstructorTest() {
            var target = new MultiPoint2();
            Assert.AreEqual(0, target.Count);
        }

    }
}

#pragma warning restore 1591

