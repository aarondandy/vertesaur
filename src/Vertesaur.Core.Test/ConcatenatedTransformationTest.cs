using System;
using System.Collections.Generic;
using NUnit.Framework;
using Vertesaur.Transformation;

namespace Vertesaur.Core.Test
{
    [TestFixture]
    public class ConcatenatedTransformationTest
    {

        private class A :
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
        }

        private class B :
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
        }

        [Test]
        public void Simple_typed_validation_test() {
            var txList = new ITransformation[] { new A(), new B() };
            var cat = new ConcatenatedTransformation<double, Point2>(txList);
            Assert.AreEqual(new Point2(1, 2), cat.TransformValue(1));
        }

        [Test]
        public void Simple_single_validation_test() {
            var txList = new ITransformation[] { new A() };
            var cat = new ConcatenatedTransformation<double, Vector2>(txList);
            Assert.AreEqual(new Vector2(2, 4), cat.TransformValue(2));
        }

        [Test]
        public void Complex_validation_test() {
            var txList = new ITransformation[] { new A(), new B(), new A() };
            var cat = new ConcatenatedTransformation<double, Vector2>(txList);
            Assert.AreEqual(new Vector2(3, 6), cat.TransformValue(2));
        }

    }
}
