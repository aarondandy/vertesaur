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
    /// Tests for 2D lines (the infinitely long kind).
    /// </summary>
    [TestFixture]
    public class Line2Test
    {

        [Test]
        public void FieldValueTest() {
            var s = new Line2(new Point2(0, 0), new Point2(2, 3));
            Assert.AreEqual(new Point2(0, 0), s.P);
            Assert.AreEqual(new Vector2(2, 3), s.Direction);
            s = new Line2(new Point2(0, 0), new Vector2(2, 3));
            Assert.AreEqual(new Point2(0, 0), s.P);
            Assert.AreEqual(new Vector2(2, 3), s.Direction);
        }

        [Test]
        public void CopyConstructorTest() {
            var i = new Line2(new Point2(1, 2), new Point2(4, 5));
            var j = new Line2(i);
            Assert.AreEqual(new Point2(1, 2), j.P);
            Assert.AreEqual(new Vector2(3, 3), j.Direction);
        }

        [Test]
        public void DirectionTest() {
            var u = new Line2(new Point2(2, 1), new Point2(-1, 3));
            Assert.AreEqual(new Vector2(-3, 2), u.Direction);
            u = new Line2(new Point2(5, -1), new Point2(10, 1));
            Assert.AreEqual(new Vector2(5, 2), u.Direction);
        }

        [Test]
        public void EqualsTest() {
            var z = new Line2(new Point2(1, 3), new Point2(2, 5));
            var q = new Line2(new Point2(1, 3), new Point2(2, 5));
            Assert.AreEqual(z, q);
            Assert.AreEqual(q, z);
            q = new Line2(new Point2(0, 2), new Point2(5, 6));
            Assert.AreNotEqual(z, q);
            Assert.AreNotEqual(q, z);
            q = new Line2(new Point2(2, 5), new Point2(1, 3));
            Assert.AreNotEqual(z, q);
            Assert.AreNotEqual(q, z);
        }

        [Test]
        public void PTest() {
            var l = new Line2(new Point2(1, 2), new Vector2(3, 4));
            Assert.AreEqual(new Point2(1, 2), ((ILine2<double>)l).P);
        }

        [Test]
        public void MagnitudeTest() {
            var l = new Line2(new Point2(1, 2), new Vector2(3, 4));
            Assert.AreEqual(double.PositiveInfinity, l.GetMagnitude());
        }

        [Test]
        public void MagnitudeSquaredTest() {
            var l = new Line2(new Point2(1, 2), new Vector2(3, 4));
            Assert.AreEqual(double.PositiveInfinity, l.GetMagnitudeSquared());
        }

        [Test]
        public void IntersectsTest() {
            var l = new Line2(new Point2(2, 1), new Vector2(2, 1));
            Assert.IsTrue(l.Intersects(new Point2(-2, -1)));
            Assert.IsTrue(l.Intersects(new Point2(0, 0)));
            Assert.IsTrue(l.Intersects(new Point2(1, 0.5)));
            Assert.IsTrue(l.Intersects(new Point2(2, 1)));
            Assert.IsTrue(l.Intersects(new Point2(4, 2)));
            Assert.IsFalse(l.Intersects(new Point2(1, 2)));
        }

        [Test]
        public void DistanceTest() {
            var l = new Line2(new Point2(1, 1), new Vector2(2, 1));
            Assert.AreEqual(System.Math.Sqrt(1 + (.5 * .5)), l.Distance(new Point2(.5, 2)));
            Assert.AreEqual(0, l.Distance(new Point2(-1, 0)));
        }

        [Test]
        public void DistanceSquaredTest() {
            var l = new Line2(new Point2(1, 1), new Vector2(2, 1));
            Assert.AreEqual(1 + (.5 * .5), l.DistanceSquared(new Point2(.5, 2)));
            Assert.AreEqual(0, l.DistanceSquared(new Point2(-1, 0)));
        }

        [Test]
        public void LineConstructorPointVectorTest() {
            var l = new Line2(new Point2(1, 2), new Vector2(3, 4));
            //Assert.AreEqual(new Point2(1, 2), ((ICurve2<double>)l).A);
            //Assert.AreEqual(new Point2(4, 6), ((ICurve2<double>)l).B);
            Assert.AreEqual(new Point2(1, 2), l.P);
            Assert.AreEqual(new Vector2(3, 4), l.Direction);
        }

        [Test]
        public void LineConstructorPointPointTest() {
            var l = new Line2(new Point2(1, 2), new Point2(3, 4));
            //Assert.AreEqual(new Point2(1, 2), ((ICurve2<double>)l).A);
            //Assert.AreEqual(new Point2(3, 4), ((ICurve2<double>)l).B);
            Assert.AreEqual(new Vector2(2, 2), l.Direction);
            Assert.AreEqual(new Point2(1, 2), l.P);
        }

        [Test]
        public void GetMinimumBoundingRectangleTest() {
            var l = new Line2(new Point2(1, 1), new Vector2(1, 1));
            Assert.AreEqual(double.PositiveInfinity, l.GetMbr().YMax);
            Assert.AreEqual(double.PositiveInfinity, l.GetMbr().XMax);
            l = new Line2(new Point2(1, 1), new Vector2(0, 1));
            Assert.AreEqual(double.PositiveInfinity, l.GetMbr().YMax);
            Assert.AreEqual(1, l.GetMbr().XMax);
        }

        [Test]
        public void IntersectionResultTest() {
            var a = new Line2(new Point2(3, 3), new Vector2(1, 1));
            var b = new Line2(new Point2(0, 1), new Vector2(0, 1));
            var c = new Line2(new Point2(2, 2), new Vector2(-1, -1));
            var d = new Line2(new Point2(2, 3), new Vector2(-1, -1));

            Assert.AreEqual(new Point2(0, 0), a.Intersection(b));
            Assert.AreEqual(c, a.Intersection(c));
            Assert.AreEqual(c, c.Intersection(a));
            Assert.AreEqual(null, a.Intersection(d));
        }

        [Test]
        public void IntersectionCheckTest() {
            var a = new Line2(new Point2(3, 3), new Vector2(1, 1));
            var b = new Line2(new Point2(0, 1), new Vector2(0, 1));
            var c = new Line2(new Point2(2, 2), new Vector2(-1, -1));
            var d = new Line2(new Point2(2, 3), new Vector2(-1, -1));
            var e = new Line2(new Point2(2, 3), new Vector2(-2, -2));
            var f = new Line2(new Point2(2, 3), new Vector2(0.5, 0.5));

            Assert.IsTrue(a.Intersects(a));
            Assert.IsTrue(a.Intersects(b));
            Assert.IsTrue(a.Intersects(c));
            Assert.IsFalse(a.Intersects(d));
            Assert.IsFalse(a.Intersects(e));
            Assert.IsFalse(a.Intersects(f));
        }

        [Test]
        public void IntersectionRayResultTest() {
            var a = new Line2(new Point2(3, 3), new Vector2(1, 1));
            var b = new Ray2(new Point2(0, 1), new Vector2(0, 1));
            var c = new Ray2(new Point2(2, 2), new Vector2(-1, -1));
            var d = new Ray2(new Point2(2, 3), new Vector2(-1, -1));

            Assert.AreEqual(null, a.Intersection(b));
            Assert.AreEqual(new Point2(0, 0), a.Intersection(b.GetReverse()));
            Assert.AreEqual(c, a.Intersection(c));
            Assert.AreEqual(c.GetReverse(), a.Intersection(c.GetReverse()));
            Assert.AreEqual(null, a.Intersection(d));
            Assert.AreEqual(null, a.Intersection(d.GetReverse()));
        }

        [Test]
        public void IntersectionRayCheckTest() {
            var a = new Line2(new Point2(3, 3), new Vector2(1, 1));
            var b = new Ray2(new Point2(0, 1), new Vector2(0, 1));
            var c = new Ray2(new Point2(2, 2), new Vector2(-1, -1));
            var d = new Ray2(new Point2(2, 3), new Vector2(-1, -1));

            Assert.IsFalse(a.Intersects(b));
            Assert.IsTrue(a.Intersects(b.GetReverse()));
            Assert.IsTrue(a.Intersects(c));
            Assert.IsTrue(a.Intersects(c.GetReverse()));
            Assert.IsFalse(a.Intersects(d));
            Assert.IsFalse(a.Intersects(d.GetReverse()));
        }

    }
}

#pragma warning restore 1591
