// ===============================================================================
//
// Copyright (c) 2011 Aaron Dandy 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// ===============================================================================


using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.Core.Test
{
    /// <summary>
    /// Tests for LineString2.
    /// </summary>
    [TestFixture]
    public class LineString2Test
    {

        private Point2[] _points;

        [SetUp]
        public void SetUp() {
            _points = new[]{
			    new Point2(0,0),
			    new Point2(3,3),
			    new Point2(2,6),
			    new Point2(0,5),
			    new Point2(2,1),
			    new Point2(6,6)
		    };
        }

        [Test]
        public void SegmentCountTest() {
            var target = new LineString2();
            Assert.AreEqual(0, target.SegmentCount);
            target = new LineString2(new[] { Point2.Zero });
            Assert.AreEqual(0, target.SegmentCount);
            target = new LineString2(new[] { Point2.Zero, Point2.Zero.Add(Vector2.XUnit) });
            Assert.AreEqual(1, target.SegmentCount);
            target = new LineString2(_points);
            Assert.AreEqual(5, target.SegmentCount);
        }

        [Test]
        public void MagnitudeTest() {
            var target = new LineString2(_points);
            var expected = 0.0;
            for (int i = 0; i < target.SegmentCount; i++) {
                expected += target.GetSegment(i).GetMagnitude();
            }
            var actual = target.GetMagnitude();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MagnitudeSquaredTest() {
            var target = new LineString2(_points);
            var expected = 0.0;
            for (int i = 0; i < target.SegmentCount; i++) {
                expected += target.GetSegment(i).GetMagnitude();
            }
            expected = expected * expected;
            var actual = target.GetMagnitudeSquared();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IntersectsTest() {
            var target = new LineString2(_points);
            Assert.IsTrue(target.Intersects(new Point2(1, 3)));
            Assert.IsFalse(target.Intersects(new Point2(1, 5)));
            Assert.IsTrue(target.Intersects(new Point2(6, 6)));
        }

        [Test]
        public void GetSegmentTest() {
            var target = new LineString2(_points);
            Assert.AreEqual(new Point2(2, 6), target.GetSegment(1).B);
            Assert.AreEqual(new Point2(2, 6), target.GetSegment(2).A);
        }

        [Test]
        public void GetMinimumBoundingRectangleTest() {
            var target = new LineString2(_points);
            var expected = new Mbr(0, 0, 6, 6);
            var actual = target.GetMbr();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCentroidTest() {
            var target = new LineString2(_points);
            var expected = new Point2(
                ((1.5 * 18.0) + (2.5 * 10.0) + (1 * 5.0) + (1 * 20.0) + (4 * 41.0)) / 94.0,
                ((1.5 * 18.0) + (4.5 * 10.0) + (5.5 * 5.0) + (3 * 20.0) + (3.5 * 41.0)) / 94.0
            );
            var actual = target.GetCentroid();
            Assert.AreEqual(expected.X, actual.X, 0.0000001);
            Assert.AreEqual(expected.Y, actual.Y, 0.0000001);
        }

        [Test]
        public void DistanceTest() {
            var target = new LineString2(_points);
            Assert.AreEqual(0, target.Distance(new Point2(1, 3)));
            Assert.AreEqual(
                target.GetSegment(2).Distance(new Point2(1, 5)),
                target.Distance(new Point2(1, 5))
            );
            Assert.AreEqual(0, target.Distance(new Point2(6, 6)));
        }

        [Test]
        public void DistanceSquaredTest() {
            var target = new LineString2(_points);
            Assert.AreEqual(0, target.DistanceSquared(new Point2(1, 3)));
            Assert.AreEqual(
                target.GetSegment(2).DistanceSquared(new Point2(1, 5)),
                target.DistanceSquared(new Point2(1, 5))
            );
            Assert.AreEqual(0, target.DistanceSquared(new Point2(6, 6)));
        }

        [Test]
        public void LineStringPointsConstructorTest() {
            var target = new LineString2(_points);
            Assert.AreEqual(_points.Length, target.Count);
            Assert.AreEqual(_points.Length - 1, target.SegmentCount);
        }

        [Test]
        public void LineStringExpectedConstructorTest() {
            var target = new LineString2(10);
            Assert.AreEqual(0, target.Count);
            Assert.AreEqual(0, target.SegmentCount);
        }

        [Test]
        public void LineStringDefaultConstructorTest() {
            var target = new LineString2();
            Assert.AreEqual(0, target.Count);
            Assert.AreEqual(0, target.SegmentCount);
        }

    }
}

#pragma warning restore 1591