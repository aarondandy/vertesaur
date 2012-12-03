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
	/// Tests for the Range type.
	/// </summary>
	[TestFixture]
	public class RangeTest {

		[Test]
		public void ConstructorTest() {
			var range = new Range(0, 0);
			Assert.AreEqual(0, range.Low);
			Assert.AreEqual(0, range.High);
			range = new Range(1, 2);
			Assert.AreEqual(1, range.Low);
			Assert.AreEqual(2, range.High);
			range = new Range(4, 3);
			Assert.AreEqual(3, range.Low);
			Assert.AreEqual(4, range.High);
		}

		[Test]
		public void MidpointTest() {
			var range = new Range(1, 3);
			Assert.AreEqual(2, range.Mid);
			range = new Range(-1, 1);
			Assert.AreEqual(0, range.Mid);
			range = new Range(2, -4);
			Assert.AreEqual(-1, range.Mid);
		}

		[Test]
		public void MagnitudeTest() {
			var range = new Range(0, 0);
			Assert.AreEqual(0, range.GetMagnitude());
			Assert.AreEqual(0, range.GetMagnitudeSquared());
			range = new Range(-1, 1);
			Assert.AreEqual(2, range.GetMagnitude());
			Assert.AreEqual(4, range.GetMagnitudeSquared());
			range = new Range(5, -10);
			Assert.AreEqual(15, range.GetMagnitude());
			Assert.AreEqual(15 * 15, range.GetMagnitudeSquared());
		}

		[Test]
		public void DistanceValueTest() {
			var range = new Range(0, 0);
			Assert.AreEqual(0, range.Distance(0));
			Assert.AreEqual(2, range.Distance(2));
			range = new Range(-1, 1);
			Assert.AreEqual(0, range.Distance(1));
			Assert.AreEqual(3, range.Distance(-4));
			Assert.AreEqual(1, range.Distance(-2));
			Assert.AreEqual(1, range.Distance(2));
			range = new Range(5, -10);
			Assert.AreEqual(0, range.Distance(1));
			Assert.AreEqual(0, range.Distance(3));
			Assert.AreEqual(3, range.Distance(8));
			Assert.AreEqual(10, range.Distance(-20));
		}

		[Test]
		public void DistanceSquaredValueTest() {
			var range = new Range(0, 0);
			Assert.AreEqual(0, range.DistanceSquared(0));
			Assert.AreEqual(4, range.DistanceSquared(2));
			range = new Range(-1, 1);
			Assert.AreEqual(0, range.DistanceSquared(1));
			Assert.AreEqual(9, range.DistanceSquared(-4));
			Assert.AreEqual(1, range.DistanceSquared(-2));
			Assert.AreEqual(1, range.DistanceSquared(2));
			range = new Range(5, -10);
			Assert.AreEqual(0, range.DistanceSquared(1));
			Assert.AreEqual(0, range.DistanceSquared(3));
			Assert.AreEqual(9, range.DistanceSquared(8));
			Assert.AreEqual(100, range.DistanceSquared(-20));
		}

		[Test]
		public void DistanceRangeTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			Assert.AreEqual(0, a.Distance(a));
			Assert.AreEqual(0, b.Distance(b));
			Assert.AreEqual(1, a.Distance(b));
			Assert.AreEqual(1, b.Distance(a));
			a = new Range(1, 2);
			b = new Range(3, 2);
			Assert.AreEqual(0, a.Distance(b));
			Assert.AreEqual(0, b.Distance(a));
			b = new Range(-2, -1);
			Assert.AreEqual(2, a.Distance(b));
			Assert.AreEqual(2, b.Distance(a));
		}

		[Test]
		public void DistanceSquaredRangeTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			Assert.AreEqual(0, a.DistanceSquared(a));
			Assert.AreEqual(0, b.DistanceSquared(b));
			Assert.AreEqual(1, a.DistanceSquared(b));
			Assert.AreEqual(1, b.DistanceSquared(a));
			a = new Range(1, 2);
			b = new Range(3, 2);
			Assert.AreEqual(0, a.DistanceSquared(b));
			Assert.AreEqual(0, b.DistanceSquared(a));
			b = new Range(-2, -1);
			Assert.AreEqual(4, a.DistanceSquared(b));
			Assert.AreEqual(4, b.DistanceSquared(a));
		}

		[Test]
		public void EquatableRangeTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			Assert.IsTrue(a.Equals(a));
			Assert.IsTrue(b.Equals(b));
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(b.Equals(a));
			a = new Range(1, 2);
			b = new Range(3, 2);
			Assert.IsTrue(a.Equals(a));
			Assert.IsTrue(b.Equals(b));
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(b.Equals(a));
			b = new Range(2, 1);
			Assert.IsTrue(a.Equals(a));
			Assert.IsTrue(b.Equals(b));
			Assert.IsTrue(a.Equals(b));
			Assert.IsTrue(b.Equals(a));
		}

		[Test]
		public void EquatableIRangeTest() {
			IRange<double> a = new Range(0, 0);
			IRange<double> b = new Range(1, 1);
			// ReSharper disable EqualExpressionComparison
			Assert.IsTrue(a.Equals(a));
			Assert.IsTrue(b.Equals(b));
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(b.Equals(a));
			a = new Range(1, 2);
			b = new Range(3, 2);
			Assert.IsTrue(a.Equals(a));
			Assert.IsTrue(b.Equals(b));
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(b.Equals(a));
			b = new Range(2, 1);
			Assert.IsTrue(a.Equals(a));
			Assert.IsTrue(b.Equals(b));
			Assert.IsTrue(a.Equals(b));
			Assert.IsTrue(b.Equals(a));
			// ReSharper restore EqualExpressionComparison
		}

		[Test]
		public void OperatorEqualsTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			Assert.IsFalse(a == b);
			Assert.IsFalse(b == a);
			a = new Range(1, 2);
			b = new Range(3, 2);
			Assert.IsFalse(a == b);
			Assert.IsFalse(b == a);
			b = new Range(2, 1);
			Assert.IsTrue(a == b);
			Assert.IsTrue(b == a);
		}

		[Test]
		public void OperatorNotEqualsTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			Assert.IsTrue(a != b);
			Assert.IsTrue(b != a);
			a = new Range(1, 2);
			b = new Range(3, 2);
			Assert.IsTrue(a != b);
			Assert.IsTrue(b != a);
			b = new Range(2, 1);
			Assert.IsFalse(a != b);
			Assert.IsFalse(b != a);
		}

		[Test]
		public void EqualsTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			Assert.AreEqual(a, a);
			Assert.AreEqual(b, b);
			Assert.AreNotEqual(a, b);
			Assert.AreNotEqual(b, a);
			a = new Range(1, 2);
			b = new Range(3, 2);
			Assert.AreEqual(a, a);
			Assert.AreEqual(b, b);
			Assert.AreNotEqual(a, b);
			Assert.AreNotEqual(b, a);
			b = new Range(2, 1);
			Assert.AreEqual(a, a);
			Assert.AreEqual(b, b);
			Assert.AreEqual(a, b);
			Assert.AreEqual(b, a);
		}

		[Test]
		public void IntersectionRangeValueTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			Assert.IsTrue(a.Intersects(0));
			Assert.IsFalse(a.Intersects(1));
			Assert.IsFalse(b.Intersects(0));
			Assert.IsTrue(b.Intersects(1));
			a = new Range(3, 1);
			Assert.IsFalse(a.Intersects(-1));
			Assert.IsFalse(a.Intersects(0));
			Assert.IsTrue(a.Intersects(1));
			Assert.IsTrue(a.Intersects(2));
			Assert.IsTrue(a.Intersects(3));
			Assert.IsFalse(a.Intersects(4));
			Assert.IsFalse(a.Intersects(-3));
		}

		[Test]
		public void IntersectionRangeRangeTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			var c = new Range(-1, 3);
			var d = new Range(4, 2);
			Assert.IsFalse(a.Intersects(b));
			Assert.IsTrue(a.Intersects(a));
			Assert.IsTrue(b.Intersects(b));
			Assert.IsTrue(c.Intersects(d));
			Assert.IsTrue(c.Intersects(a));
			Assert.IsTrue(b.Intersects(c));
			Assert.IsFalse(b.Intersects(d));
		}

		[Test]
		public void WithinRangeValueTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			Assert.IsTrue(a.Within(0));
			Assert.IsFalse(a.Within(1));
			Assert.IsFalse(b.Within(0));
			Assert.IsTrue(b.Within(1));
			a = new Range(3, 1);
			Assert.IsFalse(a.Within(-1));
			Assert.IsFalse(a.Within(0));
			Assert.IsFalse(a.Within(1));
			Assert.IsFalse(a.Within(2));
			Assert.IsFalse(a.Within(3));
			Assert.IsFalse(a.Within(4));
			Assert.IsFalse(a.Within(-3));
		}

		[Test]
		public void WithinRangeRangeTest() {
			var a = new Range(0, 0);
			var b = new Range(1, 1);
			var c = new Range(-1, 3);
			var d = new Range(4, 2);
			Assert.IsFalse(a.Within(b));
			Assert.IsTrue(a.Within(a));
			Assert.IsTrue(b.Within(b));
			Assert.IsFalse(c.Within(d));
			Assert.IsFalse(c.Within(a));
			Assert.IsTrue(b.Within(c));
			Assert.IsFalse(b.Within(d));
			Assert.IsTrue(a.Within(c));
			Assert.IsFalse(a.Within(d));
			Assert.IsTrue(new Range(2, 3).Within(d));
		}

		[Test]
		public void TouchesRangeValueTest() {
			var a = new Range(1, 1);
			var b = new Range(1, 2);
			Assert.IsFalse(a.Touches(0));
			Assert.IsFalse(a.Touches(1));
			Assert.IsFalse(a.Touches(2));
			Assert.IsFalse(a.Touches(3));
			Assert.IsFalse(b.Touches(0));
			Assert.IsTrue(b.Touches(1));
			Assert.IsFalse(b.Touches(1.5));
			Assert.IsTrue(b.Touches(2));
			Assert.IsFalse(b.Touches(3));
		}

		[Test]
		public void TouchesRangeRangeTest() {
			var a = new Range(1, 1);
			var b = new Range(1, 2);
			var c = new Range(2, 3);
			var d = new Range(2.1, 2.9);
			var e = new Range(1, 2.5);
			Assert.IsTrue(a.Touches(b));
			Assert.IsFalse(a.Touches(c));
			Assert.IsTrue(b.Touches(a));
			Assert.IsTrue(b.Touches(c));
			Assert.IsFalse(c.Touches(a));
			Assert.IsTrue(c.Touches(b));
			Assert.IsFalse(c.Touches(d));
			Assert.IsFalse(d.Touches(c));
			Assert.IsFalse(c.Touches(e));
			Assert.IsFalse(e.Touches(c));
		}

		[Test]
		public void OverlapValueTest() {
			var a = new Range(1, 2);
			var b = new Range(3, 3);
			Assert.IsFalse(a.Overlaps(0));
			Assert.IsFalse(a.Overlaps(1));
			Assert.IsTrue(a.Overlaps(1.5));
			Assert.IsFalse(a.Overlaps(2));
			Assert.IsFalse(a.Overlaps(3));
			Assert.IsFalse(b.Overlaps(2));
			Assert.IsTrue(b.Overlaps(3));
			Assert.IsFalse(b.Overlaps(4));
		}

		[Test]
		public void OverlapRangeTest() {
			var a = new Range(1, 2);
			var b = new Range(3, 3);
			var c = new Range(0, 1.5);
			var d = new Range(1.5, 3);

			Assert.IsTrue(a.Overlaps(a));
			Assert.IsFalse(a.Overlaps(b));
			Assert.IsTrue(a.Overlaps(c));
			Assert.IsTrue(a.Overlaps(d));
			Assert.IsFalse(b.Overlaps(a));
			Assert.IsTrue(b.Overlaps(b));
			Assert.IsFalse(b.Overlaps(c));
			Assert.IsFalse(b.Overlaps(d));
			Assert.IsTrue(c.Overlaps(a));
			Assert.IsFalse(c.Overlaps(b));
			Assert.IsTrue(c.Overlaps(c));
			Assert.IsFalse(c.Overlaps(d));
			Assert.IsTrue(d.Overlaps(a));
			Assert.IsFalse(d.Overlaps(b));
			Assert.IsFalse(d.Overlaps(c));
			Assert.IsTrue(d.Overlaps(d));
		}

		[Test]
		public void DisjointValueTest() {
			var a = new Range(1, 2);
			var b = new Range(3, 3);
			Assert.IsTrue(a.Disjoint(0));
			Assert.IsFalse(a.Disjoint(1));
			Assert.IsFalse(a.Disjoint(1.5));
			Assert.IsFalse(a.Disjoint(2));
			Assert.IsTrue(a.Disjoint(3));
			Assert.IsTrue(b.Disjoint(2));
			Assert.IsFalse(b.Disjoint(3));
			Assert.IsTrue(b.Disjoint(4));
		}

		[Test]
		public void DisjointRangeTest() {
			var a = new Range(1, 3);
			var b = new Range(1, 2);
			var c = new Range(2, 4);
			var d = new Range(3, 4);
			Assert.IsFalse(a.Disjoint(a));
			Assert.IsFalse(a.Disjoint(b));
			Assert.IsFalse(a.Disjoint(c));
			Assert.IsFalse(a.Disjoint(d));
			Assert.IsFalse(b.Disjoint(a));
			Assert.IsFalse(b.Disjoint(b));
			Assert.IsFalse(b.Disjoint(c));
			Assert.IsTrue(b.Disjoint(d));
			Assert.IsFalse(c.Disjoint(a));
			Assert.IsFalse(c.Disjoint(b));
			Assert.IsFalse(c.Disjoint(c));
			Assert.IsFalse(c.Disjoint(d));
			Assert.IsFalse(d.Disjoint(a));
			Assert.IsTrue(d.Disjoint(b));
			Assert.IsFalse(d.Disjoint(c));
			Assert.IsFalse(d.Disjoint(d));
		}

		[Test]
		public void ContainsValueTest() {
			var a = new Range(1, 2);
			var b = new Range(3);
			Assert.IsFalse(a.Contains(0));
			Assert.IsTrue(a.Contains(1));
			Assert.IsTrue(a.Contains(1.5));
			Assert.IsTrue(a.Contains(2));
			Assert.IsFalse(a.Contains(3));
			Assert.IsFalse(b.Contains(2));
			Assert.IsTrue(b.Contains(3));
			Assert.IsFalse(b.Contains(4));
		}

		[Test]
		public void ContainsRangeTest() {
			var a = new Range(1, 5);
			var b = new Range(1, 3);
			var c = new Range(2, 5);
			var d = new Range(2, 4);
			var e = new Range(2, 6);
			var f = new Range(7, 9);
			Assert.IsTrue(a.Contains(a));
			Assert.IsTrue(a.Contains(b));
			Assert.IsFalse(b.Contains(a));
			Assert.IsTrue(a.Contains(c));
			Assert.IsFalse(c.Contains(a));
			Assert.IsTrue(a.Contains(d));
			Assert.IsFalse(d.Contains(a));
			Assert.IsFalse(a.Contains(e));
			Assert.IsFalse(e.Contains(a));
			Assert.IsFalse(a.Contains(f));
			Assert.IsFalse(f.Contains(a));
		}

		[Test]
		public void CrossesValueTest() {
			var a = new Range(1, 2);
			var b = new Range(3);
			Assert.IsFalse(a.Crosses(0));
			Assert.IsFalse(a.Crosses(1));
			Assert.IsTrue(a.Crosses(1.5));
			Assert.IsFalse(a.Crosses(2));
			Assert.IsFalse(a.Crosses(3));
			Assert.IsFalse(b.Crosses(2));
			Assert.IsFalse(b.Crosses(3));
			Assert.IsFalse(b.Crosses(4));
		}

		[Test]
		public void CrossesRangeTest() {
			var a = new Range(1, 2);
			var b = new Range(2, 6);
			var c = new Range(3, 5);
			var d = new Range(5, 7);
			var e = new Range(3, 3);

			Assert.IsFalse(a.Crosses(a));
			Assert.IsFalse(a.Crosses(b));
			Assert.IsFalse(a.Crosses(c));
			Assert.IsFalse(a.Crosses(d));
			Assert.IsFalse(a.Crosses(e));
			Assert.IsFalse(b.Crosses(a));
			Assert.IsFalse(b.Crosses(b));
			Assert.IsFalse(b.Crosses(c));
			Assert.IsFalse(b.Crosses(d));
			Assert.IsTrue(b.Crosses(e));
			Assert.IsFalse(c.Crosses(a));
			Assert.IsFalse(c.Crosses(b));
			Assert.IsFalse(c.Crosses(c));
			Assert.IsFalse(c.Crosses(d));
			Assert.IsFalse(c.Crosses(e));
			Assert.IsFalse(d.Crosses(a));
			Assert.IsFalse(d.Crosses(b));
			Assert.IsFalse(d.Crosses(c));
			Assert.IsFalse(d.Crosses(d));
			Assert.IsFalse(d.Crosses(e));
			Assert.IsFalse(e.Crosses(a));
			Assert.IsTrue(e.Crosses(b));
			Assert.IsFalse(e.Crosses(c));
			Assert.IsFalse(e.Crosses(d));
			Assert.IsFalse(e.Crosses(e));
		}

		[Test]
		public void GetCenteredTest() {
			Assert.AreEqual(new Range(1, 2), new Range(2, 3).GetCentered(1.5));
			Assert.AreEqual(new Range(-0.5, 0.5), new Range(2, 3).GetCentered(0));
			Assert.AreEqual(new Range(-3.5, -1.5), new Range(4, 6).GetCentered(-2.5));
		}

		[Test]
		public void GetResizedTest() {
			Assert.AreEqual(new Range(1.75, 3.25), new Range(2, 3).GetResized(1.5));
			Assert.AreEqual(new Range(2.5, 2.5), new Range(2, 3).GetResized(0));
			Assert.AreEqual(new Range(3.75, 6.25), new Range(4, 6).GetResized(-2.5));
			Assert.AreEqual(new Range(4.5, 5.5), new Range(4, 6).GetResized(1));
		}

		[Test]
		public void EqualsObjectTest() {
			var a = (object)new Range(0, 0);
			var b = (object)new Range(-1, 3);
			var c = (object)new Range(-1, 3);
			var d = b;
			var e = a;
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(b.Equals(a));
			Assert.IsTrue(a.Equals(e));
			Assert.IsTrue(b.Equals(d));
			Assert.IsFalse(a.Equals(c));
			Assert.IsTrue(b.Equals(c));
			Assert.IsFalse(a.Equals(null));
		}

		[Test]
		public void EqualsSelfTypeTest() {
			var a = new Range(0, 0);
			var b = new Range(-1, 3);
			var c = new Range(-1, 3);
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(b.Equals(a));
			Assert.IsTrue(a.Equals(a));
			Assert.IsTrue(b.Equals(b));
			Assert.IsFalse(a.Equals(c));
			Assert.IsTrue(b.Equals(c));
			Assert.IsFalse(a.Equals(null));
		}

		[Test]
		public void OpEqualTest() {
			var a = new Range(0, 0);
			var b = new Range(-1, 3);
			var c = new Range(-1, 3);
			Assert.IsFalse(a == b);
			Assert.IsFalse(b == a);
			Assert.IsFalse(a == c);
			Assert.IsTrue(b == c);
			Assert.IsTrue(c == b);
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			Assert.IsFalse(a == null);
			Assert.IsFalse(null == a);
			// ReSharper restore ConditionIsAlwaysTrueOrFalse

		}

		[Test]
		public void EncompassValueValue() {
			var a = new Range(1);
			var b = new Range(2);
			var c = new Range(2);
			Assert.AreEqual(new Range(1, 2), a.Encompass(b));
			Assert.AreEqual(new Range(1, 2), b.Encompass(a));
			Assert.AreEqual(new Range(2), b.Encompass(c));
		}

		[Test]
		public void EncompassRangeValue() {

			var a = new Range(1, 2);
			var b = new Range(1.5);
			var c = new Range(3);

			Assert.AreEqual(a, a.Encompass(b));
			Assert.AreEqual(a, b.Encompass(a));
			Assert.AreEqual(new Range(1.5, 3), b.Encompass(c));
			Assert.AreEqual(new Range(1.5, 3), c.Encompass(b));
			Assert.AreEqual(new Range(1, 3), a.Encompass(c));
			Assert.AreEqual(new Range(1, 3), c.Encompass(a));
		}

		[Test]
		public void EncompassRangeRange() {
			var a = new Range(1, 2);
			var b = new Range(1.5, 2.5);
			var c = new Range(1.25, 1.75);
			var d = new Range(3, 4);

			Assert.AreEqual(new Range(1, 2.5), a.Encompass(b));
			Assert.AreEqual(new Range(1, 2.5), b.Encompass(a));
			Assert.AreEqual(a, a.Encompass(c));
			Assert.AreEqual(a, c.Encompass(a));
			Assert.AreEqual(new Range(1, 4), a.Encompass(d));
			Assert.AreEqual(new Range(1, 4), d.Encompass(a));
			Assert.AreEqual(new Range(1.25, 2.5), b.Encompass(c));
			Assert.AreEqual(new Range(1.25, 2.5), c.Encompass(b));
			Assert.AreEqual(new Range(1.5, 4), b.Encompass(d));
			Assert.AreEqual(new Range(1.5, 4), d.Encompass(b));
			Assert.AreEqual(new Range(1.25, 4), c.Encompass(d));
			Assert.AreEqual(new Range(1.25, 4), d.Encompass(c));

		}
	}
}
