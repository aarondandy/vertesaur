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

	/// <summary>
	/// Tests for a 2D ray.
	/// </summary>
	[TestFixture]
	public class Ray2Test {

		[Test]
		public void FieldValueTest() {
			var s = new Ray2(new Point2(0, 0), new Point2(2, 3));
			Assert.AreEqual(new Point2(0, 0), s.P);
			Assert.AreEqual(new Vector2(2, 3), s.Direction);
			s = new Ray2(new Point2(0, 0), new Vector2(2, 3));
			Assert.AreEqual(new Point2(0, 0), s.P);
			Assert.AreEqual(new Vector2(2, 3), s.Direction);
		}

		[Test]
		public void CopyConstructorTest() {
			var i = new Ray2(new Point2(1, 2), new Point2(4, 5));
			var j = new Ray2(i);
			Assert.AreEqual(new Point2(1, 2), j.P);
			Assert.AreEqual(new Vector2(3, 3), j.Direction);
		}

		[Test]
		public void DirectionTest() {
			var u = new Ray2(new Point2(2, 1), new Point2(-1, 3));
			Assert.AreEqual(new Vector2(-3, 2), u.Direction);
			u = new Ray2(new Point2(5, -1), new Point2(10, 1));
			Assert.AreEqual(new Vector2(5, 2), u.Direction);
		}

		[Test]
		public void EqualsTest() {
			var z = new Ray2(new Point2(1, 3), new Point2(2, 5));
			var q = new Ray2(new Point2(1, 3), new Point2(2, 5));
			Assert.AreEqual(z, q);
			Assert.AreEqual(q, z);
			q = new Ray2(new Point2(0, 2), new Point2(5, 6));
			Assert.AreNotEqual(z, q);
			Assert.AreNotEqual(q, z);
			q = new Ray2(new Point2(2, 5), new Point2(1, 3));
			Assert.AreNotEqual(z, q);
			Assert.AreNotEqual(q, z);
		}

        [Test]
		public void ATest()
        {
            var l = new Ray2(new Point2(1, 2), new Vector2(3, 4));
            Assert.AreEqual(new Point2(1, 2), ((IRay2<double>)l).P);
        }

		[Test]
		public void MagnitudeTest() {
			var l = new Ray2(new Point2(1, 2), new Vector2(3, 4));
			Assert.AreEqual(double.PositiveInfinity, l.GetMagnitude());
		}

		[Test]
		public void MagnitudeSquaredTest() {
			var l = new Ray2(new Point2(1, 2), new Vector2(3, 4));
			Assert.AreEqual(double.PositiveInfinity, l.GetMagnitudeSquared());
		}

		[Test]
		public void IntersectsPointTest() {
			var l = new Ray2(new Point2(2, 1), new Vector2(2, 1));
			Assert.IsFalse(l.Intersects(new Point2(-2, -1)));
			Assert.IsFalse(l.Intersects(new Point2(0, 0)));
			Assert.IsTrue(l.Intersects(new Point2(6, 3)));
			Assert.IsTrue(l.Intersects(new Point2(2, 1)));
			Assert.IsTrue(l.Intersects(new Point2(4, 2)));
			Assert.IsFalse(l.Intersects(new Point2(1, 2)));
		}

		[Test]
		public void DistanceTest() {
			var l = new Ray2(new Point2(1, 1), new Vector2(2, 1));
			Assert.AreEqual(System.Math.Sqrt(1 + (.5 * .5)), l.Distance(new Point2(1.5, 2.5)));
			Assert.AreEqual(2, l.Distance(new Point2(-1, 1)));
		}

		[Test]
		public void DistanceSquaredTest() {
			var l = new Ray2(new Point2(1, 1), new Vector2(2, 1));
			Assert.AreEqual(1 + (.5 * .5), l.DistanceSquared(new Point2(1.5, 2.5)));
			Assert.AreEqual(4, l.DistanceSquared(new Point2(-1, 1)));
		}

		[Test]
		public void RayConstructorTest1() {
			var l = new Ray2(new Point2(1, 2), new Vector2(3, 4));
			Assert.AreEqual(new Point2(1, 2), l.P);
			//Assert.AreEqual(new Point2(1, 2), ((ICurve2<double>)l).A);
			//Assert.AreEqual(new Point2(4, 6), ((ICurve2<double>)l).B);
			Assert.AreEqual(new Vector2(3, 4), l.Direction);
		}

		[Test]
		public void RayConstructorTest() {
			var l = new Ray2(new Point2(1, 2), new Point2(3, 4));
			Assert.AreEqual(new Point2(1, 2), l.P);
			//Assert.AreEqual(new Point2(1, 2), ((ICurve2<double>)l).A);
			//Assert.AreEqual(new Point2(3, 4), ((ICurve2<double>)l).B);
			Assert.AreEqual(new Vector2(2, 2), l.Direction);
		}


		[Test]
		public void GetMinimumBoundingRectangleTest() {
			var l = new Ray2(new Point2(1, 1), new Vector2(1, 1));
			Assert.AreEqual(double.PositiveInfinity, l.GetMbr().YMax);
			Assert.AreEqual(double.PositiveInfinity, l.GetMbr().XMax);
			Assert.AreEqual(1, l.GetMbr().YMin);
			l = new Ray2(new Point2(1, 1), new Vector2(0, 1));
			Assert.AreEqual(double.PositiveInfinity, l.GetMbr().YMax);
			Assert.AreEqual(1, l.GetMbr().XMax);
			Assert.AreEqual(1, l.GetMbr().YMin);
		}

		private static void AreSpatiallyEqual(object expected, object result) {
			if (result is ISpatiallyEquatable<Ray2> && expected is Ray2) {
				Assert.That(result is ISpatiallyEquatable<Ray2>);
				Assert.IsTrue(
					((ISpatiallyEquatable<Ray2>)result)
					.SpatiallyEqual(expected as Ray2)
				);
			}
			else {
				Assert.AreEqual(expected, result);
			}
		}

		private static void IntersectionTest(Ray2 a, Ray2 b, object expected) {
			var res = a.Intersection(b);
			AreSpatiallyEqual(expected, res);
			res = b.Intersection(a);
			AreSpatiallyEqual(expected, res);
		}

		[Test]
		public void IntersectionCrossTest() {
			var a = new Ray2(Point2.Zero, new Point2(4, 4));
			var b = new Ray2(new Point2(0, 4), new Point2(4, 0));
			IntersectionTest(a, b, new Point2(2, 2));
		}

		[Test]
		public void IntersectionCrossEdgeTest() {
			var a = new Ray2(new Point2(0, 0), new Point2(1, 1));
			var b = new Ray2(new Point2(0.5, 1.5), new Point2(1.5, .5));
			IntersectionTest(a, b, new Point2(1, 1));
		}

		[Test]
		public void IntersectionCrossEnds() {
			var a = new Ray2(new Point2(0, 0), new Point2(1, 1));
			var b = new Ray2(new Point2(0.5, 1.5), new Point2(1, 1));
			IntersectionTest(a, b, new Point2(1, 1));
		}

		[Test]
		public void IntersectionCrossButts() {
			var a = new Ray2(new Point2(0, 0), new Point2(1, 1));
			var b = new Ray2(new Point2(0, 0), new Point2(1, -1));
			IntersectionTest(a, b, Point2.Zero);
		}

		[Test]
		public void IntersectionSame() {
			var a = new Ray2(new Point2(0, 0), new Point2(1, 1));
			var b = new Ray2(new Point2(0, 0), new Point2(1, 1));
			var res = a.Intersection(a);
			Assert.AreEqual(a, res);
			Assert.AreEqual(b, res);
			res = b.Intersection(b);
			Assert.AreEqual(a, res);
			Assert.AreEqual(b, res);
			res = a.Intersection(b);
			Assert.AreEqual(a, res);
			Assert.AreEqual(b, res);
			res = b.Intersection(a);
			Assert.AreEqual(a, res);
			Assert.AreEqual(b, res);
		}

		[Test]
		public void IntersectionSameReversed() {
			var a = new Ray2(new Point2(0, 0), new Point2(1, 1));
			var b = new Ray2(new Point2(1, 1), new Point2(0, 0));
			IntersectionTest(a, b, new Segment2(Point2.Zero, new Point2(1, 1)));
		}

		[Test]
		public void IntersectionParallelButts() {
			var a = new Ray2(new Point2(1, 1), new Point2(2, 2));
			var b = new Ray2(new Point2(1, 1), new Point2(0, 0));
			IntersectionTest(a, b, new Point2(1, 1));
		}

		[Test]
		public void TestSubRaysGoingWithTheGrain() {
			var a = new Ray2(Point2.Zero, new Point2(4, 4));
			var d = new Vector2(2, 2);
			for (var v = -3.0; v <= 0; v++) {
				var b = new Ray2(new Point2(v, v), d);
				IntersectionTest(a, b, a);
			}
			for(var v = 0; v <= 5.0; v++) {
				var b = new Ray2(new Point2(v, v), d);
				IntersectionTest(a, b, b);
			}
		}

		[Test]
		public void TestSubRaysGoingAgainstTheGrain() {
			var a = new Ray2(Point2.Zero, new Point2(4, 4));
			var d = new Vector2(-2, -2);
			var b = new Ray2(new Point2(-1, -1), d);
			IntersectionTest(a, b, null);
			b = new Ray2(Point2.Zero, d);
			IntersectionTest(a, b, Point2.Zero);
			for(var v = 1.0; v <= 4.0; v++) {
				var p = new Point2(v, v);
				b = new Ray2(p, d);
				IntersectionTest(a, b, new Segment2(Point2.Zero, p));
			}
			for(var v = 5.0; v <= 7.0; v++) {
				var p = new Point2(v, v);
				b = new Ray2(p, d);
				IntersectionTest(a, b, new Segment2(Point2.Zero, p));
			}
		}

		[Test]
		public void RayIntersectAtPointAboveRayDefinition() {
			var a = new Ray2(Point2.Zero, new Point2(4, 4));
			var d = new Vector2(1, 0);
			for(var v = -3.0; v <= 1.0; v++) {
				var b = new Ray2(new Point2(v, 5.0), d);
				IntersectionTest(a, b, new Point2(5, 5));
			}
		}

		[Test]
		public void RayIntersectAtPointWithinRayDefinition() {
			var a = new Ray2(Point2.Zero, new Point2(4, 4));
			var d = new Vector2(1, 0);
			for (var v = -3.0; v <= 1.0; v++) {
				var b = new Ray2(new Point2(v, 3), d);
				IntersectionTest(a, b, new Point2(3, 3));
			}
		}

		[Test]
		public void RayIntersectAtPointBelowRayDefinition() {
			var a = new Ray2(Point2.Zero, new Point2(4, 4));
			var d = new Vector2(1, 0);
			for (var v = -3.0; v <= 1.0; v++) {
				var b = new Ray2(new Point2(v, -1), d);
				IntersectionTest(a, b,  null);
			}
		}

	}
}

#pragma warning restore 1591