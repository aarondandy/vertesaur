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
	public class Point2Test
	{

		private Vector2 CastToDoubleVector(object o) {
			var cast = o.GetType()
				.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(x =>
					(x.Name == "op_Implicit" || x.Name == "op_Explicit")
					&& x.ReturnParameter.ParameterType == typeof(Vector2)
					&& x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == o.GetType());
			return (Vector2)cast.Invoke(o, new[] { o });
		}

		private object CastFromDoubleVector(Vector2 input, Type desiredCoordinateType) {
			var vectorType = GetGenericVectorType(desiredCoordinateType);
			var cast = vectorType
				.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(x =>
					(x.Name == "op_Implicit" || x.Name == "op_Explicit")
					&& x.ReturnParameter.ParameterType == vectorType
					&& x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == typeof(Vector2));
			return cast.Invoke(null, new object[] { input });
		}

		private Point2 CastToDoublePoint(object o) {
			var cast = o.GetType()
				.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(x =>
					(x.Name == "op_Implicit" || x.Name == "op_Explicit")
					&& x.ReturnParameter.ParameterType == typeof(Point2)
					&& x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == o.GetType());
			return (Point2)cast.Invoke(o, new[] { o });
		}

		private object CastFromDoublePoint(Point2 input, Type desiredCoordinateType) {
			var vectorType = GetGenericPointType(desiredCoordinateType);
			var cast = vectorType
				.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(x =>
					(x.Name == "op_Implicit" || x.Name == "op_Explicit")
					&& x.ReturnParameter.ParameterType == vectorType
					&& x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == typeof(Point2));
			return cast.Invoke(null, new object[] { input });
		}

		private Type GetGenericPointType(Type elementType) {
			return typeof(Point2<>).MakeGenericType(new[] { elementType });
		}

		private Type GetGenericVectorType(Type elementType) {
			return typeof(Vector2<>).MakeGenericType(new[] { elementType });
		}

		private ConstructorInfo GetConstructorPoint(Type elementType) {
			var vectorType = GetGenericPointType(elementType);
			return vectorType.GetConstructor(new[] { elementType, elementType });
		}

		private ConstructorInfo GetConstructorVector(Type elementType) {
			var vectorType = GetGenericVectorType(elementType);
			return vectorType.GetConstructor(new[] { elementType, elementType });
		}

		private object CreatePoint(Type elementType, object x, object y) {
			var constructor = GetConstructorPoint(elementType);
			return constructor.Invoke(new[] {
				Convert.ChangeType(x, elementType),
				Convert.ChangeType(y, elementType)
			});
		}

		private object CreateVector(Type elementType, object x, object y) {
			var constructor = GetConstructorVector(elementType);
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
			var a = CreatePoint(t, x, y);
			Assert.AreEqual(x, a.GetType().GetField("X").GetValue(a));
			Assert.AreEqual(y, a.GetType().GetField("Y").GetValue(a));
		}

		[TestCase(typeof(double), 1, 2, 3, 5, 3.6055512754639892931192212674705)]
		[TestCase(typeof(int), 1, 2, 3, 5, 3)]
		[TestCase(typeof(decimal), 1, 2, 3, 5, 3.6055512754639892931192212674705)]
		[TestCase(typeof(float), 1, 2, 3, 5, 3.6055512754639892931192212674705)]
		[Test]
		public void DistanceTest(Type t, double x0, double y0, double x1, double y1, double d) {
			var a = CreatePoint(t, x0, y0);
			var b = CreatePoint(t, x1, y1);
			var distanceResult = a.GetType().GetMethod("Distance").Invoke(a,new[]{b});
			Assert.AreEqual(
				d,
				(double)Convert.ChangeType(distanceResult,typeof(double)),
				0.000001
			);
		}

		[TestCase(typeof(double), 1, 2, 3, 5, 13)]
		[TestCase(typeof(int), 1, 2, 3, 5, 13)]
		[TestCase(typeof(decimal), 1, 2, 3, 5, 13)]
		[TestCase(typeof(float), 1, 2, 3, 5, 13)]
		[Test]
		public void DistanceSquaredTest(Type t, double x0, double y0, double x1, double y1, double d) {
			var a = CreatePoint(t, x0, y0);
			var b = CreatePoint(t, x1, y1);
			var distanceResult = a.GetType().GetMethod("DistanceSquared").Invoke(a,new[]{b});
			Assert.AreEqual(d, distanceResult);
		}

		[TestCase(typeof(double), 1, 3, 2, 4, 3, 7)]
		[TestCase(typeof(int), 1, 3, 2, 4, 3, 7)]
		[TestCase(typeof(decimal), 1, 3, 2, 4, 3, 7)]
		[TestCase(typeof(float), 1, 3, 2, 4, 3, 7)]
		[Test]
		public void AddTest(Type t, double x0, double y0, double x1, double y1, double xExpected, double yExpected) {
			var a = CastFromDoublePoint(new Point2(x0, y0), t);
			var b = CastFromDoubleVector(new Vector2(x1, y1), t);
			var c = a.GetType().GetMethod("Add", new[] {b.GetType()}).Invoke(a, new[] {b});
			var doubleC = CastToDoublePoint(c);
			Assert.AreEqual(xExpected, doubleC.X);
			Assert.AreEqual(yExpected, doubleC.Y);
		}

		[TestCase(typeof(double), 3, 7, 2, 4, 1, 3)]
		[TestCase(typeof(int), 3, 7, 2, 4, 1, 3)]
		[TestCase(typeof(decimal), 3, 7, 2, 4, 1, 3)]
		[TestCase(typeof(float), 3, 7, 2, 4, 1, 3)]
		[Test]
		public void DifferenceTest(Type t, double x0, double y0, double x1, double y1, double xExpected, double yExpected) {
			var a = CreatePoint(t, x0, y0);
			var b = CreatePoint(t, x1, y1);
			var c = a.GetType().GetMethod("Difference", new[] {b.GetType()}).Invoke(a, new[] {b});
			var doubleC = CastToDoubleVector(c);
			Assert.AreEqual(xExpected, doubleC.X);
			Assert.AreEqual(yExpected, doubleC.Y);
		}

	}
}
