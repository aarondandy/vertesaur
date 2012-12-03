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
using Vertesaur.Contracts;

#pragma warning disable 1591

namespace Vertesaur.Core.Test {
	[TestFixture]
	public class MbrTest {

		[Test]
		public void YRangeTest() {
			var r = new Mbr(1, 2, 3, 4);
			Assert.AreEqual(2, r.Y.Low);
			Assert.AreEqual(4, r.Y.High);

		}

		[Test]
		public void YMinTest() {
			var r = new Mbr(1, 2, 3, 4);
			Assert.AreEqual(2, r.YMin);
		}

		[Test]
		public void YMaxTest() {
			var r = new Mbr(1, 2, 3, 4);
			Assert.AreEqual(4, r.YMax);
		}

		[Test]
		public void XRangeTest() {
			var r = new Mbr(1, 2, 3, 4);
			Assert.AreEqual(1, r.X.Low);
			Assert.AreEqual(3, r.X.High);
		}

		[Test]
		public void XMinTest() {
			var r = new Mbr(1, 2, 3, 4);
			Assert.AreEqual(1, r.XMin);
		}

		[Test]
		public void XMaxTest() {
			var r = new Mbr(1, 2, 3, 4);
			Assert.AreEqual(3, r.XMax);
		}

		[Test]
		public void WidthTest() {
			var r = new Mbr(1, 6, 3, 5);
			Assert.AreEqual(2, r.Width);
		}

		[Test]
		public void HeightTest() {
			var r = new Mbr(1, 2, 6, 5);
			Assert.AreEqual(3, r.Height);
		}

		[Test]
		public void IntersectsPointTest() {
			var target = new Mbr(new Point2(1, 2), new Point2(3, 4));
			Assert.IsFalse(target.Intersects(new Point2(0, 0)));
			Assert.IsFalse(target.Intersects(new Point2(2, 1)));
			Assert.IsFalse(target.Intersects(new Point2(0, 3)));
			Assert.IsFalse(target.Intersects(new Point2(-1, -1)));
			Assert.IsFalse(target.Intersects(new Point2(10, 10)));
			Assert.IsTrue(target.Intersects(new Point2(1, 2)));
			Assert.IsTrue(target.Intersects(new Point2(3, 4)));
			Assert.IsTrue(target.Intersects(new Point2(3, 2)));
			Assert.IsTrue(target.Intersects(new Point2(1, 4)));
			Assert.IsTrue(target.Intersects(new Point2(2, 3)));
		}

		[Test]
		public void IntersectsMbrTest() {
			var a = new Mbr(1, 1, 5, 6);
			var b = new Mbr(0, 0, 3, 2);
			var c = new Mbr(2, 3, 4, 5);
			Assert.IsTrue(a.Intersects(b));
			Assert.IsTrue(a.Intersects(c));
			Assert.IsFalse(b.Intersects(c));
		}

		[Test]
		public void GetAreaTest() {
			var target = new Mbr(1, 2, 3, 5);
			Assert.AreEqual((2 * 3), target.GetArea());
		}

		[Test]
		public void MbrConstructorRangeTest() {
			var target = new Mbr(new Range(1, 3), new Range(4, 2));
			Assert.AreEqual(1, target.XMin);
			Assert.AreEqual(2, target.YMin);
			Assert.AreEqual(3, target.XMax);
			Assert.AreEqual(4, target.YMax);
		}
		[Test]
		public void MbrConstructor4CoordinateTest() {
			var target = new Mbr(1, 2, 3, 4);
			Assert.AreEqual(1, target.XMin);
			Assert.AreEqual(2, target.YMin);
			Assert.AreEqual(3, target.XMax);
			Assert.AreEqual(4, target.YMax);
		}
		[Test]
		public void MbrConstructor1PointTest() {
			var target = new Mbr(new Point2(1, 2));
			Assert.AreEqual(1, target.XMin);
			Assert.AreEqual(2, target.YMin);
			Assert.AreEqual(1, target.XMax);
			Assert.AreEqual(2, target.YMax);
		}
		[Test]
		public void MbrConstructor2PointTest() {
			var target = new Mbr(new Point2(1, 2), new Point2(3, 4));
			Assert.AreEqual(1, target.XMin);
			Assert.AreEqual(2, target.YMin);
			Assert.AreEqual(3, target.XMax);
			Assert.AreEqual(4, target.YMax);
		}

		[Test]
		public void DistancePointTest() {
			var target = new Mbr(new Point2(1, 2), new Point2(3, 4));
			Assert.AreEqual(System.Math.Sqrt(1 + 4), target.Distance(new Point2(0, 0)));
			Assert.AreEqual(1, target.Distance(new Point2(2, 1)));
			Assert.AreEqual(1, target.Distance(new Point2(0, 3)));
			Assert.AreEqual(System.Math.Sqrt(4 + 9), target.Distance(new Point2(-1, -1)));
			Assert.AreEqual(System.Math.Sqrt(36 + (7 * 7)), target.Distance(new Point2(10, 10)));
			Assert.AreEqual(0, target.Distance(new Point2(1, 2)));
			Assert.AreEqual(0, target.Distance(new Point2(3, 4)));
			Assert.AreEqual(0, target.Distance(new Point2(3, 2)));
			Assert.AreEqual(0, target.Distance(new Point2(1, 4)));
			Assert.AreEqual(0, target.Distance(new Point2(2, 3)));
		}
		[Test]
		public void DistanceSquaredPointTest() {
			var target = new Mbr(new Point2(1, 2), new Point2(3, 4));
			Assert.AreEqual((1 + 4), target.DistanceSquared(new Point2(0, 0)));
			Assert.AreEqual(1, target.DistanceSquared(new Point2(2, 1)));
			Assert.AreEqual(1, target.DistanceSquared(new Point2(0, 3)));
			Assert.AreEqual((4 + 9), target.DistanceSquared(new Point2(-1, -1)));
			Assert.AreEqual((36 + (7 * 7)), target.DistanceSquared(new Point2(10, 10)));
			Assert.AreEqual(0, target.DistanceSquared(new Point2(1, 2)));
			Assert.AreEqual(0, target.DistanceSquared(new Point2(3, 4)));
			Assert.AreEqual(0, target.DistanceSquared(new Point2(3, 2)));
			Assert.AreEqual(0, target.DistanceSquared(new Point2(1, 4)));
			Assert.AreEqual(0, target.DistanceSquared(new Point2(2, 3)));
		}
		[Test]
		public void DistanceMbrTest() {
			var a = new Mbr(0, 0, 2, 2);
			var b = new Mbr(1, 1, 3, 3);
			var c = new Mbr(3, 4, 5, 5);
			var d = new Mbr(8, 8, 10, 10);
			Assert.AreEqual(0, a.Distance(b));
			Assert.AreEqual(System.Math.Sqrt(1 + 4), a.Distance(c));
			Assert.AreEqual(System.Math.Sqrt(36 + 36), a.Distance(d));
			Assert.AreEqual(1, b.Distance(c));
			Assert.AreEqual(System.Math.Sqrt(25 + 25), b.Distance(d));
			Assert.AreEqual(System.Math.Sqrt(9 + 9), c.Distance(d));
		}
		[Test]
		public void DistanceSquaredMbrTest() {
			var a = new Mbr(0, 0, 2, 2);
			var b = new Mbr(1, 1, 3, 3);
			var c = new Mbr(3, 4, 5, 5);
			var d = new Mbr(8, 8, 10, 10);
			Assert.AreEqual(0, a.DistanceSquared(b));
			Assert.AreEqual((1 + 4), a.DistanceSquared(c));
			Assert.AreEqual((36 + 36), a.DistanceSquared(d));
			Assert.AreEqual(1, b.Distance(c));
			Assert.AreEqual((25 + 25), b.DistanceSquared(d));
			Assert.AreEqual((9 + 9), c.DistanceSquared(d));
		}

		[Test]
		public void GetCentroidTest() {
			var target = new Mbr(1, 2, 3, 4);
			Assert.AreEqual(new Point2(2, 3), target.GetCentroid());
		}

		[Test]
		public void EqualsSameTypeTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 4, 5);
			var c = new Mbr(b.X, b.Y);
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(a.Equals(c));
			Assert.IsFalse(b.Equals(a));
			Assert.IsTrue(b.Equals(c));
			Assert.IsFalse(c.Equals(a));
			Assert.IsTrue(c.Equals(b));
		}

		[Test]
		public void EqualsObjectTypeTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 4, 5);
			var c = new Mbr(b.X, b.Y);
			Assert.IsFalse(a.Equals((object)b));
			Assert.IsFalse(a.Equals((object)c));
			Assert.IsFalse(b.Equals((object)a));
			Assert.IsTrue(b.Equals((object)c));
			Assert.IsFalse(c.Equals((object)a));
			Assert.IsTrue(c.Equals((object)b));
		}

		[Test]
		public void EqualsOpSameTypeTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 4, 5);
			var c = new Mbr(b.X, b.Y);
			Assert.IsFalse(a == b);
			Assert.IsFalse(a == c);
			Assert.IsFalse(b == a);
			Assert.IsTrue(b == c);
			Assert.IsFalse(c == a);
			Assert.IsTrue(c == b);
		}

		[Test]
		public void NotEqualsOpSameTypeTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 4, 5);
			var c = new Mbr(b.X, b.Y);
			Assert.IsTrue(a != b);
			Assert.IsTrue(a != c);
			Assert.IsTrue(b != a);
			Assert.IsFalse(b != c);
			Assert.IsTrue(c != a);
			Assert.IsFalse(c != b);
		}

		[Test]
		public void TouchesMbrTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 4, 4);
			var c = new Mbr(3, 4, 5, 6);
			var d = new Mbr(3, 0, 5, 6);
			var e = new Mbr(0, 4, 4, 6);
			Assert.IsFalse(a.Touches(a));
			Assert.IsFalse(a.Touches(b));
			Assert.IsTrue(a.Touches(c));
			Assert.IsTrue(a.Touches(d));
			Assert.IsTrue(a.Touches(e));
		}

		[Test]
		public void TouchesPointTest() {
			var a = new Mbr(1, 2, 3, 4);
			Assert.IsFalse(a.Touches(new Point2(2, 3)));
			Assert.IsFalse(a.Touches(Point2.Zero));
			Assert.IsFalse(a.Touches(Point2.Invalid));
			Assert.IsTrue(a.Touches(new Point2(1, 2)));
			Assert.IsTrue(a.Touches(new Point2(3, 4)));
			Assert.IsTrue(a.Touches(new Point2(1, 4)));
			Assert.IsTrue(a.Touches(new Point2(3, 2)));
			Assert.IsTrue(a.Touches(new Point2(2, 4)));
			Assert.IsTrue(a.Touches(new Point2(3, 3)));
		}

		[Test]
		public void CrossesMbrTest() {
			Assert.Inconclusive();
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 2, 5);
			var c = new Mbr(2, 3, 4, 3);
			var d = new Mbr(2, 3, 2, 3);
			Assert.IsFalse(a.Crosses(a));
			Assert.IsFalse(a.Crosses(b));
			Assert.IsFalse(a.Crosses(c));
			Assert.IsFalse(b.Crosses(a));
			Assert.IsFalse(b.Crosses(b));
			Assert.IsFalse(b.Crosses(c));
			Assert.IsFalse(c.Crosses(a));
			Assert.IsFalse(c.Crosses(b));
			Assert.IsFalse(c.Crosses(c));
			Assert.IsFalse(d.Crosses(a));
			Assert.IsFalse(d.Crosses(b));
			Assert.IsFalse(d.Crosses(c));
			Assert.IsFalse(d.Crosses(d));
		}

		[Test]
		public void WithinMbrTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 4, 5);
			var c = new Mbr(1.5, 2.5, 2.5, 3.5);
			var d = new Mbr(1.5, 2, 2.5, 4);
			Assert.IsTrue(a.Within(a));
			Assert.IsFalse(a.Within(b));
			Assert.IsFalse(a.Within(c));
			Assert.IsFalse(a.Within(d));
			Assert.IsFalse(b.Within(a));
			Assert.IsTrue(b.Within(b));
			Assert.IsFalse(b.Within(c));
			Assert.IsFalse(b.Within(d));
			Assert.IsTrue(c.Within(a));
			Assert.IsFalse(c.Within(b));
			Assert.IsTrue(c.Within(c));
			Assert.IsTrue(c.Within(d));
			Assert.IsTrue(d.Within(a));
			Assert.IsFalse(d.Within(b));
			Assert.IsFalse(d.Within(c));
			Assert.IsTrue(d.Within(d));
		}

		[Test]
		public void ContainsMbrTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 4, 5);
			var c = new Mbr(1.5, 2.5, 2.5, 3.5);
			var d = new Mbr(1.5, 2, 2.5, 4);
			Assert.IsTrue(a.Contains(a));
			Assert.IsFalse(b.Contains(a));
			Assert.IsFalse(c.Contains(a));
			Assert.IsFalse(d.Contains(a));
			Assert.IsFalse(a.Contains(b));
			Assert.IsTrue(b.Contains(b));
			Assert.IsFalse(c.Contains(b));
			Assert.IsFalse(d.Contains(b));
			Assert.IsTrue(a.Contains(c));
			Assert.IsFalse(b.Contains(c));
			Assert.IsTrue(c.Contains(c));
			Assert.IsTrue(d.Contains(c));
			Assert.IsTrue(a.Contains(d));
			Assert.IsFalse(b.Contains(d));
			Assert.IsFalse(c.Contains(d));
			Assert.IsTrue(d.Contains(d));
		}

		[Test]
		public void DisjointMbrTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 4, 5);
			var c = new Mbr(3, 2, 5, 4);
			var d = new Mbr(3, 4, 5, 6);
			var e = new Mbr(5, 6, 7, 8);

			Assert.IsFalse(a.Disjoint(a));
			Assert.IsFalse(a.Disjoint(b));
			Assert.IsFalse(b.Disjoint(a));
			Assert.IsFalse(a.Disjoint(c));
			Assert.IsFalse(c.Disjoint(a));
			Assert.IsFalse(a.Disjoint(d));
			Assert.IsFalse(d.Disjoint(a));
			Assert.IsTrue(a.Disjoint(e));
			Assert.IsTrue(e.Disjoint(a));
		}

		[Test]
		public void SpatiallyEqualMbrTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 4, 5);
			var c = new Mbr(b.X, b.Y);
			Assert.IsFalse(((ISpatiallyEquatable<Mbr>)a).SpatiallyEqual(b));
			Assert.IsFalse(((ISpatiallyEquatable<Mbr>)a).SpatiallyEqual(c));
			Assert.IsFalse(((ISpatiallyEquatable<Mbr>)b).SpatiallyEqual(a));
			Assert.IsTrue(((ISpatiallyEquatable<Mbr>)b).SpatiallyEqual(c));
			Assert.IsFalse(((ISpatiallyEquatable<Mbr>)c).SpatiallyEqual(a));
			Assert.IsTrue(((ISpatiallyEquatable<Mbr>)c).SpatiallyEqual(b));
		}

		[Test]
		public void WithinPointTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 2, 3);
			var c = new Mbr(3, 3, 3, 3);
			var p = new Point2(2, 3);
			Assert.IsFalse(a.Within(p));
			Assert.IsTrue(b.Within(p));
			Assert.IsFalse(c.Within(p));
		}

		[Test]
		public void ContainsPointTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 2, 3);
			var c = new Mbr(5, 6, 7, 8);
			var p = new Point2(2, 3);
			Assert.IsTrue(a.Contains(p));
			Assert.IsTrue(b.Contains(p));
			Assert.IsFalse(c.Contains(p));
		}

		[Test]
		public void DisjointPointTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 2, 3);
			var c = new Mbr(5, 6, 7, 8);
			var p = new Point2(2, 3);
			Assert.IsFalse(a.Disjoint(p));
			Assert.IsFalse(b.Disjoint(p));
			Assert.IsTrue(c.Disjoint(p));
		}

		[Test]
		public void SpatiallyEqualPointTest() {
			var a = new Mbr(1, 2, 3, 4);
			var b = new Mbr(2, 3, 2, 3);
			var c = new Mbr(5, 6, 7, 8);
			var p = new Point2(2, 3);
			Assert.IsFalse(a.SpatiallyEqual(p));
			Assert.IsTrue(b.SpatiallyEqual(p));
			Assert.IsFalse(c.SpatiallyEqual(p));
		}

		[Test]
		public void CrossesPointTest() {
			Assert.Inconclusive();
		}

		[Test]
		public void OverlapsPointTest() {
			Assert.Inconclusive();
		}

	}
}

#pragma warning restore 1591
