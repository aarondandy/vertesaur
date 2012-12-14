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
	/// Test cases for the 2D vector.
	/// </summary>
	[TestFixture]
	public class Vector2Test {

		[Test]
		public void ComponentConstructorGetTest() {
			var a = new Vector2(2, 3);
			Assert.AreEqual(2, a.X);
			Assert.AreEqual(3, a.Y);
		}

		[Test]
		public void PointCastTest() {
			var a = new Vector2(2, 3);
			Point2 b = a;
			Vector2 c = b;
			Assert.AreEqual(a.X, c.X);
			Assert.AreEqual(a.Y, c.Y);
		}

		[Test]
		public void CoordinatePairConstructorGetTest() {
			ICoordinatePair<double> p = new Point2(3, 4);
			var a = new Vector2(p);
			Assert.AreEqual(3, a.X);
			Assert.AreEqual(4, a.Y);
		}

		[Test]
		public void MagnitudeTest() {
			var a = new Vector2(3, 4);
			Assert.AreEqual(5, a.GetMagnitude());
			a = new Vector2(-3, 4);
			Assert.AreEqual(5, a.GetMagnitude());
		}

		[Test]
		public void MagnitudeSquaredTest() {
			var a = new Vector2(3, 4);
			Assert.AreEqual(25, a.GetMagnitudeSquared());
			a = new Vector2(-3, 4);
			Assert.AreEqual(25, a.GetMagnitudeSquared());
		}

		[Test]
		public void CompareToTest() {
			var a = new Vector2(1, 2);
			var b = new Vector2(3, 4);
			Assert.IsTrue(a.CompareTo(b) < 0);
			Assert.IsTrue(b.CompareTo(a) > 0);
			Assert.AreNotEqual(a, b);
			b = new Vector2(1, 1);
			Assert.IsTrue(b.CompareTo(a) < 0);
			Assert.IsTrue(a.CompareTo(b) > 0);
			Assert.AreNotEqual(a, b);
			b = new Vector2(1, 2);
			Assert.AreEqual(0, a.CompareTo(b));
		}

		[Test]
		public void EqualsOpTest() {
			var a = new Vector2(1, 2);
			var b = new Vector2(3, 4);
			var c = new Vector2(3, 4);
			Assert.IsFalse(a == b);
			Assert.IsFalse(a == c);
			Assert.IsFalse(b == a);
			Assert.IsTrue(b == c);
			Assert.IsFalse(c == a);
			Assert.IsTrue(c == b);
		}

		[Test]
		public void NotEqualsOpTest() {
			var a = new Vector2(1, 2);
			var b = new Vector2(3, 4);
			var c = new Vector2(3, 4);
			Assert.IsTrue(a != b);
			Assert.IsTrue(a != c);
			Assert.IsTrue(b != a);
			Assert.IsFalse(b != c);
			Assert.IsTrue(c != a);
			Assert.IsFalse(c != b);
		}

		[Test]
		public void EqualsPoint2Test() {
			var a = new Vector2(1, 2);
			var b = new Vector2(3, 4);
			var c = new Vector2(3, 4);
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(a.Equals(c));
			Assert.IsFalse(b.Equals(a));
			Assert.IsTrue(b.Equals(c));
			Assert.IsFalse(c.Equals(a));
			Assert.IsTrue(c.Equals(b));
		}

		[Test]
		public void EqualsICoordinatePairTest() {
			var a = new Vector2(1, 2);
			var b = new Vector2(3, 4);
			var c = new Vector2(3, 4);
			ICoordinatePair<double> nil = null;
			Assert.IsFalse(a.Equals((ICoordinatePair<double>)b));
			Assert.IsFalse(a.Equals((ICoordinatePair<double>)c));
			Assert.IsFalse(b.Equals((ICoordinatePair<double>)a));
			Assert.IsTrue(b.Equals((ICoordinatePair<double>)c));
			Assert.IsFalse(c.Equals((ICoordinatePair<double>)a));
			Assert.IsTrue(c.Equals((ICoordinatePair<double>)b));
// ReSharper disable ConditionIsAlwaysTrueOrFalse
			Assert.IsFalse(a.Equals(nil));
// ReSharper restore ConditionIsAlwaysTrueOrFalse
		}

		[Test]
		public void EqualsObjectTest() {
			var a = new Vector2(1, 2);
			var b = new Vector2(3, 4);
			var c = new Vector2(3, 4);
			Assert.IsFalse(a.Equals((object)(new Point2(b))));
			Assert.IsFalse(a.Equals((object)(new Point2(c))));
			Assert.IsFalse(((object)b).Equals(a));
			Assert.IsTrue(((object)b).Equals(new Point2(c)));
			Assert.IsFalse(c.Equals((object)a));
			Assert.IsTrue(c.Equals((object)b));
		}

		[Test]
		public void NegativeTest() {
			var v = new Vector2(1, 2);
			v = v.GetNegative();
			Assert.AreEqual(-1, v.X);
			Assert.AreEqual(-2, v.Y);
		}

		[Test]
		public void DotTest() {
			var a = new Vector2(5, 2);
			var b = new Vector2(3, -3);
			Assert.AreEqual(9, a.Dot(b));
			Assert.AreEqual(9, b.Dot(a));
		}

		[Test]
		public void PerpendicularDotTest() {
			var a = new Vector2(5, 2);
			var b = new Vector2(3, -3);
			Assert.AreEqual(-21, a.PerpendicularDot(b));
			Assert.AreEqual(21, b.PerpendicularDot(a));
		}

		[Test]
		public void NormalizedTest() {
			var v = new Vector2(3, 4);
			v = v.GetNormalized();
			Assert.AreEqual(3.0 / 5.0, v.X);
			Assert.AreEqual(4.0 / 5.0, v.Y);
		}

		[Test]
		public void ScaledTest() {
			var v = new Vector2(1.5, 2.9);
			const double f = 1.23;
			Assert.AreEqual(v.X * f, v.GetScaled(f).X);
			Assert.AreEqual(v.Y * f, v.GetScaled(f).Y);
		}

		[Test]
		public void AddTest() {
			var a = new Vector2(1, 3);
			var b = new Vector2(5, 7);
			Assert.AreEqual(new Vector2(6, 10), a.Add(b));
		}

		[Test]
		public void SubtractTest() {
			var a = new Vector2(1, 3);
			var b = new Vector2(5, 9);
			Assert.AreEqual(new Vector2(4, 6), b.Difference(a));
		}

		[Test]
		public void OpAddTest() {
			var a = new Vector2(1, 3);
			var b = new Vector2(5, 7);
			Assert.AreEqual(new Vector2(6, 10), a + b);
		}

		[Test]
		public void OpSubtractTest() {
			var a = new Vector2(1, 3);
			var b = new Vector2(5, 9);
			Assert.AreEqual(new Vector2(4, 6), b - a);
		}

		[Test]
		public void PerpendicularCwTest() {
			var a = new Vector2(1, 2);
			a = a.GetPerpendicularClockwise();
			Assert.AreEqual(new Vector2(2, -1), a);
			a = a.GetPerpendicularClockwise();
			Assert.AreEqual(new Vector2(-1, -2), a);
			a = a.GetPerpendicularClockwise();
			Assert.AreEqual(new Vector2(-2, 1), a);
			a = a.GetPerpendicularClockwise();
			Assert.AreEqual(new Vector2(1, 2), a);
		}

		[Test]
		public void PerpendicularCounterClockwiseTest() {
			var a = new Vector2(1, 2);
			a = a.GetPerpendicularCounterClockwise();
			Assert.AreEqual(new Vector2(-2, 1), a);
			a = a.GetPerpendicularCounterClockwise();
			Assert.AreEqual(new Vector2(-1, -2), a);
			a = a.GetPerpendicularCounterClockwise();
			Assert.AreEqual(new Vector2(2, -1), a);
			a = a.GetPerpendicularCounterClockwise();
			Assert.AreEqual(new Vector2(1, 2), a);
		}

	}
}

#pragma warning restore 1591