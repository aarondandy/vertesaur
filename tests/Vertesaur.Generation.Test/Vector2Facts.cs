using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Generation.Test
{
    public static class Vector2Facts
    {

        private static Vector2 CastToDoubleVector(object o) {
            var cast = o.GetType()
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(x =>
                    (x.Name == "op_Implicit" || x.Name == "op_Explicit")
                    && x.ReturnParameter.ParameterType == typeof(Vector2)
                    && x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == o.GetType());
            return (Vector2)cast.Invoke(o, new[] { o });
        }

        private static object CastFromDoubleVector(Vector2 input, Type desiredCoordinateType) {
            var vectorType = GetGenericVectorType(desiredCoordinateType);
            var cast = vectorType
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(x =>
                    (x.Name == "op_Implicit" || x.Name == "op_Explicit")
                    && x.ReturnParameter.ParameterType == vectorType
                    && x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == typeof(Vector2));
            return cast.Invoke(null, new object[] { input });
        }

        private static Type GetGenericVectorType(Type elementType) {
            return typeof(Vector2<>).MakeGenericType(new[] { elementType });
        }

        private static ConstructorInfo GetConstructor(Type elementType) {
            var vectorType = GetGenericVectorType(elementType);
            return vectorType.GetConstructor(new[] { elementType, elementType });
        }

        private static object CreateVector(Type elementType, object x, object y) {
            var constructor = GetConstructor(elementType);
            return constructor.Invoke(new[] {
				Convert.ChangeType(x, elementType),
				Convert.ChangeType(y, elementType)
			});
        }

        [InlineData(typeof(double), 3.1, 2.2)]
        [InlineData(typeof(int), 3, 2)]
        [InlineData(typeof(byte), 3, 2)]
        [InlineData(typeof(decimal), 3000.3, 2000.1)]
        [Theory]
        public static void ComponentConstructorGetTest(Type t, object x, object y) {
            var a = CreateVector(t, x, y);
            Assert.Equal(Convert.ToDouble(x), Convert.ToDouble(a.GetType().GetField("X").GetValue(a)));
            Assert.Equal(Convert.ToDouble(y), Convert.ToDouble(a.GetType().GetField("Y").GetValue(a)));
        }

        [InlineData(typeof(double), 3.1, 2.2, 3.8013155617496424838955952368432)]
        [InlineData(typeof(int), 3, 2, 3)]
        [InlineData(typeof(byte), 3, 2, 3)]
        [InlineData(typeof(decimal), 3000.3, 2000.1, 3605.8563615318899721304085880568)]
        [Theory]
        public static void MagnitudeTest(Type t, object x, object y, object m) {
            var a = CreateVector(t, x, y);
            var mResult = a.GetType().GetMethod("GetMagnitude").Invoke(a, new object[0]);
            Assert.Equal(
                (double)Convert.ChangeType(m, typeof(double)),
                (double)Convert.ChangeType(mResult, typeof(double)),
                10
            );
        }

        [InlineData(typeof(double), 3.1, 2.2, 14.45)]
        [InlineData(typeof(int), 3, 2, 13)]
        [InlineData(typeof(byte), 3, 2, 13)]
        [InlineData(typeof(decimal), 3000.3, 2000.1, 13002200.1)]
        [Theory]
        public static void MagnitudeSquaredTest(Type t, object x, object y, double mExpected) {
            var a = CreateVector(t, x, y);
            var mResult = a.GetType().GetMethod("GetMagnitudeSquared").Invoke(a, new object[0]);
            Assert.Equal(
                mExpected,
                (double)Convert.ChangeType(mResult, typeof(double)),
                10
            );
        }

        [InlineData(typeof(double), 3.1, 2.2)]
        [InlineData(typeof(int), 3, 2)]
        [InlineData(typeof(decimal), 3000.3, 2000.1)]
        [Theory]
        public static void NegativeTest(Type t, double x, double y) {
            var a = CastFromDoubleVector(new Vector2(x, y), t);
            var b = a.GetType().GetMethod("GetNegative").Invoke(a, new object[0]);
            Assert.Equal(-x, Convert.ToDouble(b.GetType().GetField("X").GetValue(b)));
            Assert.Equal(-y, Convert.ToDouble(b.GetType().GetField("Y").GetValue(b)));
        }

        [InlineData(typeof(double), 5, 2, 3, -3, 9)]
        [InlineData(typeof(int), 5, 2, 3, -3, 9)]
        [InlineData(typeof(decimal), 5, 2, 3, -3, 9)]
        [InlineData(typeof(float), 5, 2, 3, -3, 9)]
        [Theory]
        public static void DotTest(Type t, double x0, double y0, double x1, double y1, double dot) {
            var a = CreateVector(t, x0, y0);
            var b = CreateVector(t, x1, y1);
            var dotResult = a.GetType().GetMethod("Dot").Invoke(a, new[] { b });
            Assert.Equal(dot, Convert.ToDouble(dotResult));
        }

        [InlineData(typeof(double), 5, 2, 3, -3, -21)]
        [InlineData(typeof(int), 5, 2, 3, -3, -21)]
        [InlineData(typeof(decimal), 5, 2, 3, -3, -21)]
        [InlineData(typeof(float), 5, 2, 3, -3, -21)]
        [Theory]
        public static void PerpendicularDotTest(Type t, double x0, double y0, double x1, double y1, double dot) {
            var a = CreateVector(t, x0, y0);
            var b = CreateVector(t, x1, y1);
            var dotResult = a.GetType().GetMethod("PerpendicularDot").Invoke(a, new[] { b });
            Assert.Equal(dot, Convert.ToDouble(dotResult));
        }

        [InlineData(typeof(double), 3, 4, 3.0 / 5.0, 4.0 / 5.0)]
        [InlineData(typeof(decimal), 3, 4, 3.0 / 5.0, 4.0 / 5.0)]
        [InlineData(typeof(float), 3, 4, 3.0 / 5.0, 4.0 / 5.0)]
        [Theory]
        public static void NormalizedTest(Type t, double x, double y, double expectedX, double expectedY) {
            var a = CastFromDoubleVector(new Vector2(x, y), t);
            var b = a.GetType().GetMethod("GetNormalized").Invoke(a, new object[0]);
            var doubleB = CastToDoubleVector(b);
            Assert.Equal(expectedX, doubleB.X, 5);
            Assert.Equal(expectedY, doubleB.Y, 5);
        }

        [InlineData(typeof(double), 1.5, 2.9, 1.23, 1.845, 3.567)]
        [InlineData(typeof(int), 1, 2, 3, 3, 6)]
        [InlineData(typeof(decimal), 1.5, 2.9, 1.23, 1.845, 3.567)]
        [InlineData(typeof(float), 1.5, 2.9, 1.23, 1.845, 3.567)]
        [Theory]
        public static void ScaledTest(Type t, double x, double y, double f, double expectedX, double expectedY) {
            var a = CreateVector(t, x, y);
            var b = a.GetType().GetMethod("GetScaled", new[] { t }).Invoke(a, new[] {
				Convert.ChangeType(f, t)
			});
            Assert.Equal(expectedX, (double)Convert.ChangeType(b.GetType().GetField("X").GetValue(b), typeof(double)), 5);
            Assert.Equal(expectedY, (double)Convert.ChangeType(b.GetType().GetField("Y").GetValue(b), typeof(double)), 5);
        }

        [InlineData(typeof(double), 1.5, 2.9, 1.23, 1.845, 2.73, 4.745)]
        [InlineData(typeof(int), 1, 2, 3, 4, 4, 6)]
        [InlineData(typeof(decimal), 1.5, 2.9, 1.23, 1.845, 2.73, 4.745)]
        [InlineData(typeof(float), 1.5, 2.9, 1.23, 1.845, 2.73, 4.745)]
        [Theory]
        public static void AddTest(Type t, double x0, double y0, double x1, double y1, double expectedX, double expectedY) {
            var a = CreateVector(t, x0, y0);
            var b = CreateVector(t, x1, y1);
            var c = a.GetType().GetMethod("Add", new[] { b.GetType() }).Invoke(a, new[] { b });
            Assert.Equal(expectedX, (double)Convert.ChangeType(c.GetType().GetField("X").GetValue(c), typeof(double)), 5);
            Assert.Equal(expectedY, (double)Convert.ChangeType(c.GetType().GetField("Y").GetValue(c), typeof(double)), 5);
        }

        [InlineData(typeof(double), 1.5, 2.9, 1.23, 1.845, 0.27, 1.055)]
        [InlineData(typeof(int), 1, 2, 3, 5, -2, -3)]
        [InlineData(typeof(decimal), 1.5, 2.9, 1.23, 1.845, 0.27, 1.055)]
        [InlineData(typeof(float), 1.5, 2.9, 1.23, 1.845, 0.27, 1.055)]
        [Theory]
        public static void SubtractTest(Type t, double x0, double y0, double x1, double y1, double expectedX, double expectedY) {
            var a = CreateVector(t, x0, y0);
            var b = CreateVector(t, x1, y1);
            var c = a.GetType().GetMethod("Difference", new[] { b.GetType() }).Invoke(a, new[] { b });
            Assert.Equal(expectedX, (double)Convert.ChangeType(c.GetType().GetField("X").GetValue(c), typeof(double)), 5);
            Assert.Equal(expectedY, (double)Convert.ChangeType(c.GetType().GetField("Y").GetValue(c), typeof(double)), 5);
        }

        [InlineData(typeof(double), 1.5, 2.9, 2.9, -1.5)]
        [InlineData(typeof(int), 1, 2, 2, -1)]
        [InlineData(typeof(decimal), 1.5, 2.9, 2.9, -1.5)]
        [InlineData(typeof(float), 1.5, 2.9, 2.9, -1.5)]
        [Theory]
        public static void PerpendicularRotateCwTest(Type t, double x, double y, double expectedX, double expectedY) {
            var a = CreateVector(t, x, y);
            var c = a.GetType().GetMethod("GetPerpendicularClockwise").Invoke(a, new object[0]);
            Assert.Equal(expectedX, (double)Convert.ChangeType(c.GetType().GetField("X").GetValue(c), typeof(double)), 5);
            Assert.Equal(expectedY, (double)Convert.ChangeType(c.GetType().GetField("Y").GetValue(c), typeof(double)), 5);
        }

        [InlineData(typeof(double), 1.5, 2.9, -2.9, 1.5)]
        [InlineData(typeof(int), 1, 2, -2, 1)]
        [InlineData(typeof(decimal), 1.5, 2.9, -2.9, 1.5)]
        [InlineData(typeof(float), 1.5, 2.9, -2.9, 1.5)]
        [Theory]
        public static void PerpendicularRotateCcwTest(Type t, double x, double y, double expectedX, double expectedY) {
            var a = CreateVector(t, x, y);
            var c = a.GetType().GetMethod("GetPerpendicularCounterClockwise").Invoke(a, new object[0]);
            Assert.Equal(expectedX, (double)Convert.ChangeType(c.GetType().GetField("X").GetValue(c), typeof(double)), 5);
            Assert.Equal(expectedY, (double)Convert.ChangeType(c.GetType().GetField("Y").GetValue(c), typeof(double)), 5);
        }

    }
}
