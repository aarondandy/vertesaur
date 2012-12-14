// ===============================================================================
//
// Copyright (c) 2012 Aaron Dandy 
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
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Vertesaur.Generation.Test
{
	[TestFixture]
	public class Vector2Test
	{

		private Vector2 CastToDoubleVector(object o) {
			var cast = o.GetType()
				.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(x =>
					(x.Name == "op_Implicit" || x.Name == "op_Explicit")
					&& x.ReturnParameter.ParameterType == typeof(Vector2)
					&& x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == o.GetType());
			return (Vector2)cast.Invoke(o, new[] {o});
		}

		private object CastFromDoubleVector(Vector2 input, Type desiredCoordinateType) {
			var vectorType = GetGenericVectorType(desiredCoordinateType);
			var cast = vectorType
				.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(x =>
					(x.Name == "op_Implicit" || x.Name == "op_Explicit")
					&& x.ReturnParameter.ParameterType == vectorType
					&& x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == typeof(Vector2));
			return cast.Invoke(null, new object[] {input});
		}

		private Type GetGenericVectorType(Type elementType) {
			return typeof(Vector2<>).MakeGenericType(new[] {elementType});
		}

		private ConstructorInfo GetConsutrctor(Type elementType) {
			var vectorType = GetGenericVectorType(elementType);
			return vectorType.GetConstructor(new[] { elementType, elementType });
		}

		private object CreateVector(Type elementType, object x, object y) {
			var constructor = GetConsutrctor(elementType);
			return constructor.Invoke(new[] {
				Convert.ChangeType(x, elementType),
				Convert.ChangeType(y, elementType)
			});
		}

		[TestCase(typeof(double), 3.1, 2.2)]
		[TestCase(typeof(int), 3, 2)]
		[TestCase(typeof(byte), 3, 2)]
		[TestCase(typeof(decimal), 3000.3, 2000.1)]
		[Test]
		public void ComponentConstructorGetTest(Type t, object x, object y) {
			var a = CreateVector(t, x, y);
			Assert.AreEqual(x, a.GetType().GetField("X").GetValue(a));
			Assert.AreEqual(y, a.GetType().GetField("Y").GetValue(a));
		}

		[TestCase(typeof(double), 3.1, 2.2, 3.8013155617496424838955952368432)]
		[TestCase(typeof(int), 3, 2, 3)]
		[TestCase(typeof(byte), 3, 2, 3)]
		[TestCase(typeof(decimal), 3000.3, 2000.1, 3605.8563615318899721304085880568)]
		[Test]
		public void MagnitudeTest(Type t, object x, object y, object m) {
			var a = CreateVector(t, x, y);
			var mResult = a.GetType().GetMethod("GetMagnitude").Invoke(a, new object[0]);
			Assert.AreEqual(
				(double)Convert.ChangeType(m, typeof(double)),
				(double)Convert.ChangeType(mResult, typeof(double)),
				0.00001
			);
		}

		[TestCase(typeof(double), 3.1, 2.2, 14.45)]
		[TestCase(typeof(int), 3, 2, 13)]
		[TestCase(typeof(byte), 3, 2, 13)]
		[TestCase(typeof(decimal), 3000.3, 2000.1, 13002200.1)]
		[Test]
		public void MagnitudeSquaredTest(Type t, object x, object y, double mExpected) {
			var a = CreateVector(t, x, y);
			var mResult = a.GetType().GetMethod("GetMagnitudeSquared").Invoke(a, new object[0]);
			Assert.AreEqual(
				mExpected,
				(double)Convert.ChangeType(mResult, typeof(double)),
				0.00001
			);
		}

		[TestCase(typeof(double), 3.1, 2.2)]
		[TestCase(typeof(int), 3, 2)]
		[TestCase(typeof(decimal), 3000.3, 2000.1)]
		[Test]
		public void NegativeTest(Type t, double x, double y) {
			var a = CastFromDoubleVector(new Vector2(x, y), t);
			var b = a.GetType().GetMethod("GetNegative").Invoke(a, new object[0]);
			Assert.AreEqual(-x, b.GetType().GetField("X").GetValue(b));
			Assert.AreEqual(-y, b.GetType().GetField("Y").GetValue(b));
		}

		[TestCase(typeof(double), 5, 2, 3, -3, 9)]
		[TestCase(typeof(int), 5, 2, 3, -3, 9)]
		[TestCase(typeof(decimal), 5, 2, 3, -3, 9)]
		[TestCase(typeof(float), 5, 2, 3, -3, 9)]
		[Test]
		public void DotTest(Type t, double x0, double y0, double x1, double y1, double dot) {
			var a = CreateVector(t, x0, y0);
			var b = CreateVector(t, x1, y1);
			var dotResult = a.GetType().GetMethod("Dot").Invoke(a, new []{b});
			Assert.AreEqual(dot, dotResult);
		}

		[TestCase(typeof(double), 5, 2, 3, -3, -21)]
		[TestCase(typeof(int), 5, 2, 3, -3, -21)]
		[TestCase(typeof(decimal), 5, 2, 3, -3, -21)]
		[TestCase(typeof(float), 5, 2, 3, -3, -21)]
		[Test]
		public void PerpendicularDotTest(Type t, double x0, double y0, double x1, double y1, double dot) {
			var a = CreateVector(t, x0, y0);
			var b = CreateVector(t, x1, y1);
			var dotResult = a.GetType().GetMethod("PerpendicularDot").Invoke(a, new [] { b });
			Assert.AreEqual(dot, dotResult);
		}

		[TestCase(typeof(double), 3, 4, 3.0/5.0, 4.0/5.0)]
		[TestCase(typeof(decimal), 3, 4, 3.0 / 5.0, 4.0 / 5.0)]
		[TestCase(typeof(float), 3, 4, 3.0 / 5.0, 4.0 / 5.0)]
		[Test]
		public void NormalizedTest(Type t, double x, double y, double expectedX, double expectedY) {
			var a = CastFromDoubleVector(new Vector2(x, y), t);
			var b = a.GetType().GetMethod("GetNormalized").Invoke(a, new object[0]);
			var doubleB = CastToDoubleVector(b);
			Assert.AreEqual(expectedX, doubleB.X, 0.0000001);
			Assert.AreEqual(expectedY, doubleB.Y, 0.0000001);
		}

		[TestCase(typeof(double), 1.5, 2.9, 1.23, 1.845, 3.567)]
		[TestCase(typeof(int), 1,2,3,3,6)]
		[TestCase(typeof(decimal), 1.5, 2.9, 1.23, 1.845, 3.567)]
		[TestCase(typeof(float), 1.5, 2.9, 1.23, 1.845, 3.567)]
		[Test]
		public void ScaledTest(Type t, double x, double y, double f, double expectedX, double expectedY) {
			var a = CreateVector(t, x, y);
			var b = a.GetType().GetMethod("GetScaled", new[] {t}).Invoke(a, new[] {
				Convert.ChangeType(f, t)
			});
			Assert.AreEqual(expectedX, (double)Convert.ChangeType(b.GetType().GetField("X").GetValue(b), typeof(double)), 0.00001);
			Assert.AreEqual(expectedY, (double)Convert.ChangeType(b.GetType().GetField("Y").GetValue(b), typeof(double)), 0.00001);
		}

		[TestCase(typeof(double), 1.5, 2.9, 1.23, 1.845, 2.73, 4.745)]
		[TestCase(typeof(int), 1, 2, 3, 4, 4, 6)]
		[TestCase(typeof(decimal), 1.5, 2.9, 1.23, 1.845, 2.73, 4.745)]
		[TestCase(typeof(float), 1.5, 2.9, 1.23, 1.845, 2.73, 4.745)]
		[Test]
		public void AddTest(Type t, double x0, double y0, double x1, double y1, double expectedX, double expectedY) {
			var a = CreateVector(t, x0, y0);
			var b = CreateVector(t, x1, y1);
			var c = a.GetType().GetMethod("Add",new[]{b.GetType()}).Invoke(a,new[]{b});
			Assert.AreEqual(expectedX, (double)Convert.ChangeType(c.GetType().GetField("X").GetValue(c), typeof(double)), 0.00001);
			Assert.AreEqual(expectedY, (double)Convert.ChangeType(c.GetType().GetField("Y").GetValue(c), typeof(double)), 0.00001);
		}

		[TestCase(typeof(double), 1.5, 2.9, 1.23, 1.845, 0.27, 1.055)]
		[TestCase(typeof(int), 1, 2, 3, 5, -2, -3)]
		[TestCase(typeof(decimal), 1.5, 2.9, 1.23, 1.845, 0.27, 1.055)]
		[TestCase(typeof(float), 1.5, 2.9, 1.23, 1.845, 0.27, 1.055)]
		[Test]
		public void SubtractTest(Type t, double x0, double y0, double x1, double y1, double expectedX, double expectedY) {
			var a = CreateVector(t, x0, y0);
			var b = CreateVector(t, x1, y1);
			var c = a.GetType().GetMethod("Difference", new[] { b.GetType() }).Invoke(a, new[] { b });
			Assert.AreEqual(expectedX, (double)Convert.ChangeType(c.GetType().GetField("X").GetValue(c), typeof(double)), 0.00001);
			Assert.AreEqual(expectedY, (double)Convert.ChangeType(c.GetType().GetField("Y").GetValue(c), typeof(double)), 0.00001);
		}

		[TestCase(typeof(double), 1.5, 2.9, 2.9, -1.5)]
		[TestCase(typeof(int), 1, 2, 2, -1)]
		[TestCase(typeof(decimal), 1.5, 2.9, 2.9, -1.5)]
		[TestCase(typeof(float), 1.5, 2.9, 2.9, -1.5)]
		[Test]
		public void PerpendicularRotateCwTest(Type t, double x, double y, double expectedX, double expectedY) {
			var a = CreateVector(t, x, y);
			var c = a.GetType().GetMethod("GetPerpendicularClockwise").Invoke(a, new object[0]);
			Assert.AreEqual(expectedX, (double)Convert.ChangeType(c.GetType().GetField("X").GetValue(c), typeof(double)), 0.00001);
			Assert.AreEqual(expectedY, (double)Convert.ChangeType(c.GetType().GetField("Y").GetValue(c), typeof(double)), 0.00001);
		}

		[TestCase(typeof(double), 1.5, 2.9, -2.9, 1.5)]
		[TestCase(typeof(int), 1, 2, -2, 1)]
		[TestCase(typeof(decimal), 1.5, 2.9, -2.9, 1.5)]
		[TestCase(typeof(float), 1.5, 2.9, -2.9, 1.5)]
		[Test]
		public void PerpendicularRotateCcwTest(Type t, double x, double y, double expectedX, double expectedY) {
			var a = CreateVector(t, x, y);
			var c = a.GetType().GetMethod("GetPerpendicularCounterClockwise").Invoke(a, new object[0]);
			Assert.AreEqual(expectedX, (double)Convert.ChangeType(c.GetType().GetField("X").GetValue(c), typeof(double)), 0.00001);
			Assert.AreEqual(expectedY, (double)Convert.ChangeType(c.GetType().GetField("Y").GetValue(c), typeof(double)), 0.00001);
		}

	}
}
