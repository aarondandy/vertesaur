using System;
using System.Collections.Generic;
using System.Linq;
using Vertesaur.Transformation;
using FluentAssertions;
using Xunit;

namespace Vertesaur.Test
{
    public static class ConcatenatedTransformationFacts
    {

        [Fact]
        public static void simple_typed_validation_test() {
            var txList = new ITransformation[] { new DummyUpConversion(), new DummyDownConversion() };

            var cat = new ConcatenatedTransformation<double, Point2>(txList);

            cat.TransformValue(1).Should().Be(new Point2(1, 2));
        }

        [Fact]
        public static void simple_single_validation_test() {
            var txList = new ITransformation[] { new DummyUpConversion() };

            var cat = new ConcatenatedTransformation<double, Vector2>(txList);

            cat.TransformValue(2).Should().Be(new Vector2(2, 4));
        }

        [Fact]
        public static void complex_validation_test() {
            var txList = new ITransformation[] { new DummyUpConversion(), new DummyDownConversion(), new DummyUpConversion() };

            var cat = new ConcatenatedTransformation<double, Vector2>(txList);

            cat.TransformValue(2).Should().Be(new Vector2(3, 6));
        }

        [Fact]
        public static void generic_enumerator_test() {
            var txList = new ITransformation[] { new DummyUpConversion(), new DummyDownConversion(), new DummyUpConversion() };
            var cat = new ConcatenatedTransformation<double, Vector2>(txList);
            
            var enumerator = cat.GetEnumerator();

            cat.Should().NotContainNulls();
            enumerator.Should().NotBeNull();
            while (enumerator.MoveNext()) {
                enumerator.Current.Should().NotBeNull();
            }
        }

        private class DummyUpConversion :
            ITransformation<double, Vector2>,
            ITransformation<double, Vector3>,
            ITransformation<Vector2, Vector3>
        {

            public Vector2 TransformValue(double value) {
                return new Vector2(value, value * 2);
            }

            public IEnumerable<Vector2> TransformValues(IEnumerable<double> values) {
                throw new NotImplementedException();
            }

            public ITransformation<Vector2, double> GetInverse() {
                throw new NotImplementedException();
            }

            public bool HasInverse {
                get { return false; }
            }

            ITransformation ITransformation.GetInverse() {
                throw new NotImplementedException();
            }

            Vector3 ITransformation<double, Vector3>.TransformValue(double value) {
                return new Vector3(value, value * 2, value + 1);
            }

            IEnumerable<Vector3> ITransformation<double, Vector3>.TransformValues(IEnumerable<double> values) {
                throw new NotImplementedException();
            }

            ITransformation<Vector3, double> ITransformation<double, Vector3>.GetInverse() {
                throw new NotImplementedException();
            }

            public Vector3 TransformValue(Vector2 value) {
                throw new NotImplementedException();
            }

            public IEnumerable<Vector3> TransformValues(IEnumerable<Vector2> values) {
                throw new NotImplementedException();
            }

            ITransformation<Vector3, Vector2> ITransformation<Vector2, Vector3>.GetInverse() {
                throw new NotImplementedException();
            }

            public object TransformValue(object value) {
                throw new NotImplementedException();
            }

            public IEnumerable<object> TransformValues(IEnumerable<object> values) {
                throw new NotImplementedException();
            }

            public Type[] GetInputTypes() {
                return new Type[] {
                    typeof(double),
                    typeof(Vector2)
                };
            }

            public Type[] GetOutputTypes(Type inputType) {
                if (inputType == typeof(double))
                    return new[] { typeof(Vector2), typeof(Vector3) };
                if (inputType == typeof(Vector2))
                    return new[] { typeof(Vector3) };
                return new Type[0];
            }
        }

        private class DummyDownConversion :
            ITransformation<Vector2, Point2>,
            ITransformation<Vector2, Point3>,
            ITransformation<Vector3, double>
        {
            public Point2 TransformValue(Vector2 value) {
                return new Point2(value);
            }

            public IEnumerable<Point2> TransformValues(IEnumerable<Vector2> values) {
                throw new NotImplementedException();
            }

            public ITransformation<Point2, Vector2> GetInverse() {
                throw new NotImplementedException();
            }

            public bool HasInverse {
                get { return false; }
            }

            ITransformation ITransformation.GetInverse() {
                throw new NotImplementedException();
            }

            Point3 ITransformation<Vector2, Point3>.TransformValue(Vector2 value) {
                throw new NotImplementedException();
            }

            IEnumerable<Point3> ITransformation<Vector2, Point3>.TransformValues(IEnumerable<Vector2> values) {
                throw new NotImplementedException();
            }

            ITransformation<Point3, Vector2> ITransformation<Vector2, Point3>.GetInverse() {
                throw new NotImplementedException();
            }

            public double TransformValue(Vector3 value) {
                return (value.X + value.Y + value.Z) / 3;
            }

            public IEnumerable<double> TransformValues(IEnumerable<Vector3> values) {
                throw new NotImplementedException();
            }

            ITransformation<double, Vector3> ITransformation<Vector3, double>.GetInverse() {
                throw new NotImplementedException();
            }

            public object TransformValue(object value) {
                throw new NotImplementedException();
            }

            public IEnumerable<object> TransformValues(IEnumerable<object> values) {
                throw new NotImplementedException();
            }

            public Type[] GetInputTypes() {
                return new[] { typeof(Vector2), typeof(Vector3) };
            }

            public Type[] GetOutputTypes(Type inputType) {
                if (inputType == typeof(Vector2))
                    return new[] { typeof(Point2), typeof(Point3) };
                if (inputType == typeof(Vector3))
                    return new[] { typeof(double) };
                return new Type[0];
            }
        }

    }
}
