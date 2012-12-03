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
	/// Test cases for the 3D point.
	/// </summary>
	[TestFixture]
	public class Point3Test {

		[Test]
		public void DefaultConstructorTest() {
			var p = new Point3();
			Assert.AreEqual(0, p.X);
			Assert.AreEqual(0, p.Y);
			Assert.AreEqual(0, p.Z);
		}

		[Test]
		public void ComponentConstructorTest() {
			var p = new Point3(1, 2, 3);
			Assert.AreEqual(1, p.X);
			Assert.AreEqual(2, p.Y);
			Assert.AreEqual(3, p.Z);
		}

		[Test]
		public void DistancePoint3Test() {
			var a = new Point3(1, 2, 5);
			var b = new Point3(3, 5, 1);
			Assert.AreEqual(
				System.Math.Sqrt(29),
				a.Distance(b)
			);
			Assert.AreEqual(
				System.Math.Sqrt(29),
				b.Distance(a)
			);
		}

		[Test]
		public void DistanceSquaredPoint3Test() {
			var a = new Point3(1, 2, 9);
			var b = new Point3(3, 5, 5);
			Assert.AreEqual(
				29,
				a.DistanceSquared(b)
			);
			Assert.AreEqual(
				29,
				b.DistanceSquared(a)
			);
		}

		[Test]
		public void CompareToTest() {
			var a = new Point3(1, 2, 0);
			var b = new Point3(3, 4, 10);
			Assert.IsTrue(a.CompareTo(b) < 0);
			Assert.IsTrue(b.CompareTo(a) > 0);
			Assert.AreNotEqual(a, b);
			b = new Point3(1, 1, 1);
			Assert.IsTrue(b.CompareTo(a) < 0);
			Assert.IsTrue(a.CompareTo(b) > 0);
			Assert.AreNotEqual(a, b);
			b = new Point3(1, 2, 0);
			Assert.AreEqual(0, a.CompareTo(b));
		}
		[Test]
		public void EqualsOpTest() {
			var a = new Point3(1, 2, 1);
			var b = new Point3(3, 4, 2);
			var c = new Point3(3, 4, 2);
			Assert.IsFalse(a == b);
			Assert.IsFalse(a == c);
			Assert.IsFalse(b == a);
			Assert.IsTrue(b == c);
			Assert.IsFalse(c == a);
			Assert.IsTrue(c == b);
		}

		[Test]
		public void NotEqualsOpTest() {
			var a = new Point3(1, 2, 4);
			var b = new Point3(3, 4, 9);
			var c = new Point3(3, 4, 9);
			Assert.IsTrue(a != b);
			Assert.IsTrue(a != c);
			Assert.IsTrue(b != a);
			Assert.IsFalse(b != c);
			Assert.IsTrue(c != a);
			Assert.IsFalse(c != b);
		}

		[Test]
		public void EqualsPoint3Test() {
			var a = new Point3(1, 2, 9);
			var b = new Point3(3, 4, 6);
			var c = new Point3(3, 4, 6);
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(a.Equals(c));
			Assert.IsFalse(b.Equals(a));
			Assert.IsTrue(b.Equals(c));
			Assert.IsFalse(c.Equals(a));
			Assert.IsTrue(c.Equals(b));
		}

		[Test]
		public void EqualsICoordinatePairTest() {
			var a = new Point3(1, 2, 0);
			var b = new Point3(3, 4, 1);
			var c = new Point3(3, 4, 1);
			ICoordinateTriple<double> nil = null;
			Assert.IsFalse(a.Equals((ICoordinateTriple<double>)b));
			Assert.IsFalse(a.Equals((ICoordinateTriple<double>)c));
			Assert.IsFalse(b.Equals((ICoordinateTriple<double>)a));
			Assert.IsTrue(b.Equals((ICoordinateTriple<double>)c));
			Assert.IsFalse(c.Equals((ICoordinateTriple<double>)a));
			Assert.IsTrue(c.Equals((ICoordinateTriple<double>)b));
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			Assert.IsFalse(a.Equals(nil));
			// ReSharper restore ConditionIsAlwaysTrueOrFalse
		}

		[Test]
		public void EqualsObjectTest() {
			var a = new Point3(1, 2, 0);
			var b = new Point3(3, 4, 9);
			var c = new Point3(3, 4, 9);
			Assert.IsFalse(a.Equals((object)(new Vector3(b))));
			Assert.IsFalse(a.Equals((object)(new Vector3(c))));
			Assert.IsFalse(((object)b).Equals(a));
			Assert.IsTrue(((object)b).Equals(new Vector3(c)));
			Assert.IsFalse(c.Equals((object)a));
			Assert.IsTrue(c.Equals((object)b));
		}

		[Test]
		public void IntersectsPointTest() {
			IRelatableIntersects<Point3> a = new Point3(1, 2, 3);
			Assert.IsFalse(a.Intersects(new Point3(3, 4, 5)));
			Assert.IsTrue(a.Intersects(new Point3(1, 2, 3)));
			Assert.IsFalse(a.Intersects(new Point3(1, 3, 5)));
		}

		[Test]
		public void DisjointPointTest() {
			IRelatableDisjoint<Point3> a = new Point3(1, 2, 3);
			Assert.IsTrue(a.Disjoint(new Point3(3, 4, 5)));
			Assert.IsFalse(a.Disjoint(new Point3(1, 2, 3)));
			Assert.IsTrue(a.Disjoint(new Point3(1, 3, 5)));
		}

		[Test]
		public void TouchesPointTest() {
			IRelatableTouches<Point3> a = new Point3(1, 2, 3);
			Assert.IsFalse(a.Touches(new Point3(3, 4, 5)));
			Assert.IsFalse(a.Touches(new Point3(1, 2, 3)));
			Assert.IsFalse(a.Touches(new Point3(1, 3, 5)));
		}

		[Test]
		public void CrossesPointTest() {
			IRelatableCrosses<Point3> a = new Point3(1, 2, 3);
			Assert.IsFalse(a.Crosses(new Point3(3, 4, 5)));
			Assert.IsFalse(a.Crosses(new Point3(1, 2, 3)));
			Assert.IsFalse(a.Crosses(new Point3(1, 3, 5)));
		}

		[Test]
		public void WithinPointTest() {
			IRelatableWithin<Point3> a = new Point3(1, 2, 3);
			Assert.IsFalse(a.Within(new Point3(3, 4, 5)));
			Assert.IsTrue(a.Within(new Point3(1, 2, 3)));
			Assert.IsFalse(a.Within(new Point3(1, 3, 5)));
		}

		[Test]
		public void ContainsPointTest() {
			IRelatableContains<Point3> a = new Point3(1, 2, 3);
			Assert.IsFalse(a.Contains(new Point3(3, 4, 5)));
			Assert.IsTrue(a.Contains(new Point3(1, 2, 3)));
			Assert.IsFalse(a.Contains(new Point3(1, 3, 5)));
		}

		[Test]
		public void OverlapsPointTest() {
			IRelatableOverlaps<Point3> a = new Point3(1, 2, 3);
			Assert.IsFalse(a.Overlaps(new Point3(3, 4, 5)));
			Assert.IsFalse(a.Overlaps(new Point3(1, 2, 3)));
			Assert.IsFalse(a.Overlaps(new Point3(1, 3, 5)));
		}

		[Test]
		public void AddTest() {
			var a = new Point3(1, 3, 5);
			var v = new Vector3(2, 4, 6);
			var b = a.Add(v);
			Assert.AreEqual(a.X + v.X, b.X);
			Assert.AreEqual(a.Y + v.Y, b.Y);
			Assert.AreEqual(a.Z + v.Z, b.Z);
		}

		[Test]
		public void DifferenceTest() {
			var a = new Point3(1, 3, 5);
			var b = new Point3(2, 4, 6);
			var d = a.Difference(b);
			Assert.AreEqual(a.X - b.X, d.X);
			Assert.AreEqual(a.Y - b.Y, d.Y);
			Assert.AreEqual(a.Z - b.Z, d.Z);
		}

	}
}

#pragma warning restore 1591