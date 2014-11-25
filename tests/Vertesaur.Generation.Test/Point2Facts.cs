using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Generation.Test
{
    public static class Point2Facts
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

        private static Point2 CastToDoublePoint(object o) {
            var cast = o.GetType()
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(x =>
                    (x.Name == "op_Implicit" || x.Name == "op_Explicit")
                    && x.ReturnParameter.ParameterType == typeof(Point2)
                    && x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == o.GetType());
            return (Point2)cast.Invoke(o, new[] { o });
        }

        private static object CastFromDoublePoint(Point2 input, Type desiredCoordinateType) {
            var vectorType = GetGenericPointType(desiredCoordinateType);
            var cast = vectorType
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(x =>
                    (x.Name == "op_Implicit" || x.Name == "op_Explicit")
                    && x.ReturnParameter.ParameterType == vectorType
                    && x.GetParameters().Count() == 1 && x.GetParameters()[0].ParameterType == typeof(Point2));
            return cast.Invoke(null, new object[] { input });
        }

        private static Type GetGenericPointType(Type elementType) {
            return typeof(Point2<>).MakeGenericType(new[] { elementType });
        }

        private static Type GetGenericVectorType(Type elementType) {
            return typeof(Vector2<>).MakeGenericType(new[] { elementType });
        }

        private static ConstructorInfo GetConstructorPoint(Type elementType) {
            var vectorType = GetGenericPointType(elementType);
            return vectorType.GetConstructor(new[] { elementType, elementType });
        }

        private static ConstructorInfo GetConstructorVector(Type elementType) {
            var vectorType = GetGenericVectorType(elementType);
            return vectorType.GetConstructor(new[] { elementType, elementType });
        }

        private static object CreatePoint(Type elementType, object x, object y) {
            var constructor = GetConstructorPoint(elementType);
            return constructor.Invoke(new[] {
				Convert.ChangeType(x, elementType),
				Convert.ChangeType(y, elementType)
			});
        }

        private static object CreateVector(Type elementType, object x, object y) {
            var constructor = GetConstructorVector(elementType);
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
            var a = CreatePoint(t, x, y);
            Assert.Equal(Convert.ToDouble(x), Convert.ToDouble(a.GetType().GetField("X").GetValue(a)));
            Assert.Equal(Convert.ToDouble(y), Convert.ToDouble(a.GetType().GetField("Y").GetValue(a)));
        }

        [InlineData(typeof(double), 1, 2, 3, 5, 3.6055512754639892931192212674705)]
        [InlineData(typeof(int), 1, 2, 3, 5, 3)]
        [InlineData(typeof(decimal), 1, 2, 3, 5, 3.6055512754639892931192212674705)]
        [InlineData(typeof(float), 1, 2, 3, 5, 3.6055512754639892931192212674705)]
        [Theory]
        public static void DistanceTest(Type t, double x0, double y0, double x1, double y1, double d) {
            var a = CreatePoint(t, x0, y0);
            var b = CreatePoint(t, x1, y1);
            var distanceResult = a.GetType().GetMethod("Distance").Invoke(a, new[] { b });
            Assert.Equal(
                d,
                (double)Convert.ChangeType(distanceResult, typeof(double)),
                5
            );
        }

        [InlineData(typeof(double), 1, 2, 3, 5, 13)]
        [InlineData(typeof(int), 1, 2, 3, 5, 13)]
        [InlineData(typeof(decimal), 1, 2, 3, 5, 13)]
        [InlineData(typeof(float), 1, 2, 3, 5, 13)]
        [Theory]
        public static void DistanceSquaredTest(Type t, double x0, double y0, double x1, double y1, double d) {
            var a = CreatePoint(t, x0, y0);
            var b = CreatePoint(t, x1, y1);
            var distanceResult = a.GetType().GetMethod("DistanceSquared").Invoke(a, new[] { b });
            Assert.Equal(d, Convert.ToDouble(distanceResult));
        }

        [InlineData(typeof(double), 1, 3, 2, 4, 3, 7)]
        [InlineData(typeof(int), 1, 3, 2, 4, 3, 7)]
        [InlineData(typeof(decimal), 1, 3, 2, 4, 3, 7)]
        [InlineData(typeof(float), 1, 3, 2, 4, 3, 7)]
        [Theory]
        public static void AddTest(Type t, double x0, double y0, double x1, double y1, double xExpected, double yExpected) {
            var a = CastFromDoublePoint(new Point2(x0, y0), t);
            var b = CastFromDoubleVector(new Vector2(x1, y1), t);
            var c = a.GetType().GetMethod("Add", new[] { b.GetType() }).Invoke(a, new[] { b });
            var doubleC = CastToDoublePoint(c);
            Assert.Equal(xExpected, doubleC.X);
            Assert.Equal(yExpected, doubleC.Y);
        }

        [InlineData(typeof(double), 3, 7, 2, 4, 1, 3)]
        [InlineData(typeof(int), 3, 7, 2, 4, 1, 3)]
        [InlineData(typeof(decimal), 3, 7, 2, 4, 1, 3)]
        [InlineData(typeof(float), 3, 7, 2, 4, 1, 3)]
        [Theory]
        public static void DifferenceTest(Type t, double x0, double y0, double x1, double y1, double xExpected, double yExpected) {
            var a = CreatePoint(t, x0, y0);
            var b = CreatePoint(t, x1, y1);
            var c = a.GetType().GetMethod("Difference", new[] { b.GetType() }).Invoke(a, new[] { b });
            var doubleC = CastToDoubleVector(c);
            Assert.Equal(xExpected, doubleC.X);
            Assert.Equal(yExpected, doubleC.Y);
        }

    }
}
