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

using System;
using NUnit.Framework;
using Vertesaur.Contracts;

#pragma warning disable 1591

namespace Vertesaur.Core.Test {

	/// <summary>
	/// Test cases for the 3D vector.
	/// </summary>
	[TestFixture]
	public class Vector3Test {

		[Test]
		public void ComponentConstructorGetTest() {
			var a = new Vector3(2, 3, 4);
			Assert.AreEqual(2, a.X);
			Assert.AreEqual(3, a.Y);
			Assert.AreEqual(4, a.Z);
		}

		[Test]
		public void PointCastTest() {
			var a = new Vector3(2, 3, 4);
			Point3 b = a;
			Vector3 c = b;
			Assert.AreEqual(a.X, c.X);
			Assert.AreEqual(a.Y, c.Y);
			Assert.AreEqual(a.Z, c.Z);
		}

		[Test]
		public void CoordinatePairConstructorGetTest() {
			ICoordinateTriple<double> p = new Point3(3, 4, 5);
			var a = new Vector3(p);
			Assert.AreEqual(3, a.X);
			Assert.AreEqual(4, a.Y);
			Assert.AreEqual(5, a.Z);
		}

		[Test]
		public void MagnitudeTest() {
			var a = new Vector3(3, 4, 5);
			Assert.AreEqual(Math.Sqrt(50), a.GetMagnitude());
			a = new Vector3(-3, 4, -5);
			Assert.AreEqual(Math.Sqrt(50), a.GetMagnitude());
		}

		[Test]
		public void MagnitudeSquaredTest() {
			var a = new Vector3(3, 4, -5);
			Assert.AreEqual(50, a.GetMagnitudeSquared());
			a = new Vector3(-3, 4, 5);
			Assert.AreEqual(50, a.GetMagnitudeSquared());
		}

		[Test]
		public void CompareToTest() {
			var a = new Vector3(1, 1, 2);
			var b = new Vector3(3, 4, 5);
			Assert.IsTrue(a.CompareTo(b) < 0);
			Assert.IsTrue(b.CompareTo(a) > 0);
			Assert.AreNotEqual(a, b);
			b = new Vector3(1, 1, 1);
			Assert.IsTrue(b.CompareTo(a) < 0);
			Assert.IsTrue(a.CompareTo(b) > 0);
			Assert.AreNotEqual(a, b);
			b = new Vector3(1, 1, 2);
			Assert.AreEqual(0, a.CompareTo(b));
		}

		[Test]
		public void EqualsOpTest() {
			var a = new Vector3(1, 2, 3);
			var b = new Vector3(3, 4, 5);
			var c = new Vector3(3, 4, 5);
			Assert.IsFalse(a == b);
			Assert.IsFalse(a == c);
			Assert.IsFalse(b == a);
			Assert.IsTrue(b == c);
			Assert.IsFalse(c == a);
			Assert.IsTrue(c == b);
		}

		[Test]
		public void NotEqualsOpTest() {
			var a = new Vector3(1, 2, 3);
			var b = new Vector3(3, 4, 5);
			var c = new Vector3(3, 4, 5);
			Assert.IsTrue(a != b);
			Assert.IsTrue(a != c);
			Assert.IsTrue(b != a);
			Assert.IsFalse(b != c);
			Assert.IsTrue(c != a);
			Assert.IsFalse(c != b);
		}

		[Test]
		public void EqualsPoint2Test() {
			var a = new Vector3(1, 2, 3);
			var b = new Vector3(3, 4, 5);
			var c = new Vector3(3, 4, 5);
			Assert.IsFalse(a.Equals(b));
			Assert.IsFalse(a.Equals(c));
			Assert.IsFalse(b.Equals(a));
			Assert.IsTrue(b.Equals(c));
			Assert.IsFalse(c.Equals(a));
			Assert.IsTrue(c.Equals(b));
		}

		[Test]
		public void EqualsICoordinatePairTest() {
			var a = new Vector3(1, 2, 3);
			var b = new Vector3(3, 4, 5);
			var c = new Vector3(3, 4, 5);
			ICoordinateTriple<double> nil = null;
			Assert.IsFalse(a.Equals((ICoordinateTriple<double>)b));
			Assert.IsFalse(a.Equals((ICoordinateTriple<double>)c));
			Assert.IsFalse(b.Equals((ICoordinateTriple<double>)a));
			Assert.IsTrue(b.Equals((ICoordinateTriple<double>)c));
			Assert.IsFalse(c.Equals((ICoordinateTriple<double>)a));
			Assert.IsTrue(c.Equals((ICoordinateTriple<double>)b));
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			// ReSharper disable ExpressionIsAlwaysNull
			Assert.IsFalse(a.Equals(nil));
			// ReSharper restore ExpressionIsAlwaysNull
			// ReSharper restore ConditionIsAlwaysTrueOrFalse
		}

		[Test]
		public void EqualsObjectTest() {
			var a = new Vector3(1, 2, 3);
			var b = new Vector3(3, 4, 5);
			var c = new Vector3(3, 4, 5);
			Assert.IsFalse(a.Equals((object)(new Point3(b))));
			Assert.IsFalse(a.Equals((object)(new Point3(c))));
			Assert.IsFalse(((object)b).Equals(a));
			Assert.IsTrue(((object)b).Equals(new Point3(c)));
			Assert.IsFalse(c.Equals((object)a));
			Assert.IsTrue(c.Equals((object)b));
		}

		[Test]
		public void NegativeTest() {
			var v = new Vector3(1, 2, 3);
			v = v.GetNegative();
			Assert.AreEqual(-1, v.X);
			Assert.AreEqual(-2, v.Y);
			Assert.AreEqual(-3, v.Z);
		}

		[Test]
		public void DotTest() {
			var a = new Vector3(5, 2, -1);
			var b = new Vector3(3, -3, 0);
			Assert.AreEqual(9, a.Dot(b));
			Assert.AreEqual(9, b.Dot(a));
		}

		[Test]
		public void CrossTest() {
			var a = new Vector3(3, -3, 1);
			var b = new Vector3(4, 9, 2);
			Assert.AreEqual(
				new Vector3(-15, -2, 39),
				a.Cross(b)
			);
			Assert.AreEqual(
				new Vector3(15, 2, -39),
				b.Cross(a)
			);
		}

		[Test]
		public void NormalizedTest() {
			var v = new Vector3(3, 4, 5);
			v = v.GetNormalized();
			Assert.AreEqual(3.0 / Math.Sqrt(50), v.X);
			Assert.AreEqual(4.0 / Math.Sqrt(50), v.Y);
			Assert.AreEqual(5.0 / Math.Sqrt(50), v.Z);
		}

		[Test]
		public void ScaledTest() {
			var v = new Vector3(1.5, 2.9, 1);
			const double f = 1.23;
			Assert.AreEqual(v.X * f, v.GetScaled(f).X);
			Assert.AreEqual(v.Y * f, v.GetScaled(f).Y);
			Assert.AreEqual(f, v.GetScaled(f).Z);
		}

		[Test]
		public void AddTest() {
			var a = new Vector3(1, 3, 5);
			var b = new Vector3(5, 7, 9);
			Assert.AreEqual(new Vector3(6, 10, 14), a.Add(b));
		}

		[Test]
		public void SubtractTest() {
			var a = new Vector3(1, 3, 2);
			var b = new Vector3(5, 9, 3);
			Assert.AreEqual(new Vector3(4, 6, 1), b.Difference(a));
		}

		[Test]
		public void OpAddTest() {
			var a = new Vector3(1, 3, 1);
			var b = new Vector3(5, 7, -1);
			Assert.AreEqual(new Vector3(6, 10, 0), a + b);
		}

		[Test]
		public void OpSubtractTest() {
			var a = new Vector3(1, 3, 9);
			var b = new Vector3(5, 9, 6);
			Assert.AreEqual(new Vector3(4, 6, -3), b - a);
		}


	}
}

#pragma warning restore 1591