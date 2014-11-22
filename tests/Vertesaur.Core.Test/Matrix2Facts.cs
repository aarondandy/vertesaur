using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public static class Matrix2Facts
    {

        private static readonly Matrix2 _a;
        private static readonly Matrix2 _b;
        private static readonly Matrix2 _product;
        private static readonly Matrix2 _sum;
        private static readonly Matrix2 _identity;
        private static readonly Matrix2 _incremented;
        private static readonly Matrix2 _determinant34;
        private static readonly Matrix2 _priorInvert;
        private static readonly Matrix2 _postInvert;
        private static readonly Matrix2 _trans;

        static Matrix2Facts() {
            _identity = new Matrix2(
                1, 0,
                0, 1);
            _incremented = new Matrix2(
                0, 1,
                2, 3);
            _determinant34 = new Matrix2(
                3, -8,
                5, -2);
            _priorInvert = new Matrix2(
                1, 3,
                2, 4);
            _postInvert = new Matrix2(
                -2, 1.5,
                1, -0.5);
            _trans = new Matrix2(
                0, 2,
                1, 3);
            _a = new Matrix2(
                3, 1,
                -10, -2);
            _b = new Matrix2(
                2, 3,
                -4, 5);
            _product = new Matrix2(
                2, 14,
                -12, -40);
            _sum = new Matrix2(
                5, 4,
                -14, 3);
        }

        [Fact]
        public static void constructor_default() {
            var m = new Matrix2();

            m.Should().Be(_identity);
        }

        [Fact]
        public static void constructor_element() {
            var m = new Matrix2(0, 1, 2, 3);

            m.Should().Be(_incremented);
        }

        [Fact]
        public static void elements_set_batch() {
            var m = new Matrix2();

            m.SetElements(0, 1, 2, 3);

            m.Should().Be(_incremented);
        }

        [Fact]
        public static void set_identity() {
            var m = new Matrix2(_incremented);

            m.SetIdentity();

            m.Should().Be(_identity);
        }

        [Fact]
        public static void matrix_size() {
            IMatrix<double> m = _incremented;

            m.RowCount.Should().Be(2);
            m.ColumnCount.Should().Be(2);
            ((IMatrixSquare<double>)m).Order.Should().Be(2);
            m.ElementCount.Should().Be(4);
        }

        [Fact]
        public static void elements_get() {
            var m = new Matrix2(_incremented);

            m.Get(0, 0).Should().Be(0);
            m.Get(0, 1).Should().Be(1);
            m.Get(1, 0).Should().Be(2);
            m.Get(1, 1).Should().Be(3);
        }

        [Fact]
        public static void elements_set() {
            var m = new Matrix2();

            m.Set(0, 0, 0);
            m.Set(0, 1, 1);
            m.Set(1, 0, 2);
            m.Set(1, 1, 3);

            m.Should().Be(_incremented);
        }

        [Fact]
        public static void element_field_set() {
            var m = new Matrix2();

            m.E00 = 0;
            m.E01 = 1;
            m.E10 = 2;
            m.E11 = 3;

            m.Should().Be(_incremented);
        }

        public static IEnumerable<object[]> DeterminantsData {
            get {
                yield return new object[] { 1.0, _identity };
                yield return new object[] { -2.0, _incremented };
                yield return new object[] { 34.0, _determinant34 };
                yield return new object[] { -2.0, _priorInvert };
                yield return new object[] { -0.5, _postInvert };
                yield return new object[] { -2.0, _trans };
                yield return new object[] { 4.0, _a };
                yield return new object[] { 22.0, _b };
                yield return new object[] { 88.0, _product };
                yield return new object[] { 71.0, _sum };
                yield return new object[] { 0.0, new Matrix2(3, 6, 1, 2) };
            }
        }

        [Theory, PropertyData("DeterminantsData")]
        public static void determinants(double expectedDeterminant, Matrix2 m) {
            var determinant = m.CalculateDeterminant();

            determinant.Should().Be(expectedDeterminant);
        }

        [Fact]
        public static void invert_basic() {
            var a = new Matrix2(_priorInvert);

            a.Invert();

            a.Should().Be(_postInvert);
        }

        [Fact]
        public static void inverse_safety() {
            var a = new Matrix2(_priorInvert);

            var b = a.GetInverse();

            a.Should().Be(_priorInvert, "Source matrix should not be mutated.");
            b.Should().Be(_postInvert);
        }

        private static void Equal(Matrix2 a, Matrix2 b, int precision) {
            Assert.Equal(a.E00, b.E00, precision);
            Assert.Equal(a.E01, b.E01, precision);
            Assert.Equal(a.E10, b.E10, precision);
            Assert.Equal(a.E11, b.E11, precision);
        }

        [Fact]
        public static void inverted_examples() {
            _priorInvert.GetInverse().Should().Be(_postInvert);
            _postInvert.GetInverse().Should().Be(_priorInvert);
            _identity.GetInverse().Should().Be(_identity);
            _incremented.GetInverse().Should().Be(new Matrix2(-3 / 2.0, 0.5, 1, 0));
            _trans.GetInverse().Should().Be(new Matrix2(-3 / 2.0, 1, 0.5, 0));
            Equal(new Matrix2(-0.5, -0.25, 5 / 2.0, 0.75), _a.GetInverse(), 10);
            Equal(new Matrix2(5 / 22.0, -3 / 22.0, 2 / 11.0, 1 / 11.0), _b.GetInverse(), 10);
            Equal(new Matrix2(-5 / 11.0, -7 / 44.0, 3 / 22.0, 1 / 44.0), _product.GetInverse(), 10);
            Equal(new Matrix2(3 / 71.0, -4 / 71.0, 14 / 71.0, 5 / 71.0), _sum.GetInverse(), 10);
            Equal(new Matrix2(-1 / 17.0, 4 / 17.0, -5 / 34.0, 3 / 34.0), _determinant34.GetInverse(), 10);
        }

        [Fact]
        public static void transpose() {
            var a = new Matrix2(_incremented);

            a.Transpose();

            a.Should().Be(_trans);
        }

        [Fact]
        public static void transposed_get() {
            var a = new Matrix2(_incremented);

            var b = a.GetTransposed();

            a.Should().Be(_incremented, "Source matrix should not be mutated.");
            b.Should().Be(_trans);
        }

        [Fact]
        public static void multiply() {
            var a = new Matrix2(_a);
            var b = new Matrix2(_b);

            var product = a.Multiply(b);

            product.Should().Be(_product);
            a.Should().Be(_a, "No argument mutation is allowed.");
            b.Should().Be(_b, "No argument mutation is allowed.");
        }

        [Fact]
        public static void multiply_op() {
            var a = new Matrix2(_a);
            var b = new Matrix2(_b);

            var product = a * b;

            product.Should().Be(_product);
            a.Should().Be(_a, "No argument mutation is allowed.");
            b.Should().Be(_b, "No argument mutation is allowed.");
        }

        [Fact]
        public static void multiply_assign() {
            var d = new Matrix2(_a);

            d.MultiplyAssign(_b);

            d.Should().Be(_product);
        }

        [Fact]
        public static void multiply_assign_op() {
            var d = new Matrix2(_a);

            d *= _b;

            d.Should().Be(_product);
        }

        [Fact]
        public static void equality_object() {
            var a = (object)new Matrix2(_a);
            var b = (object)new Matrix2(_b);
            var c = (object)new Matrix2(_b);

            a.Should().Be(a);
            b.Should().Be(b);
            a.Should().NotBe(b);
            b.Should().NotBe(a);
            a.Should().NotBe(c);
            b.Should().Be(c);
            a.Should().NotBe(null);
        }

        [Fact]
        public static void equality_typed() {
            var a = new Matrix2(_a);
            var b = new Matrix2(_b);
            var c = new Matrix2(_b);

            a.Equals(a).Should().BeTrue();
            b.Equals(b).Should().BeTrue();
            b.Equals(c).Should().BeTrue();
            a.Equals(b).Should().BeFalse();
            b.Equals(a).Should().BeFalse();
            a.Equals(c).Should().BeFalse();
            a.Equals(null).Should().BeFalse();
        }

        [Fact]
        public static void equality_op() {
            var a = new Matrix2(_a);
            var a2 = a;
            var b = new Matrix2(_b);
            var b2 = b;
            var c = new Matrix2(_b);
            var d = (Matrix2)null;

            Assert.False(a == b);
            Assert.False(b == a);
            Assert.True(a == a2);
            Assert.True(b == b2);
            Assert.False(a == c);
            Assert.True(b == c);
            Assert.False(a == null);
            Assert.False(null == a);
            Assert.False(b == d);
            Assert.False(d == b);
            Assert.True(null == d);
        }

        [Fact]
        public static void inequality_op() {
            var a = new Matrix2(_a);
            var a2 = a;
            var b = new Matrix2(_b);
            var b2 = b;
            var c = new Matrix2(_b);
            var d = (Matrix2)null;

            Assert.True(a != b);
            Assert.True(b != a);
            Assert.False(a != a2);
            Assert.False(b != b2);
            Assert.True(a != c);
            Assert.False(b != c);
            Assert.True(a != null);
            Assert.True(null != a);
            Assert.True(b != d);
            Assert.True(d != b);
            Assert.False(null != d);
        }

        [Fact]
        public static void add() {
            var variant1 = _a.Add(_b);
            var variant2 = _b.Add(_a);

            variant1.Should().Be(_sum);
            variant2.Should().Be(_sum);
        }

        [Fact]
        public static void add_op() {
            var variant1 = _a + _b;
            var variant2 = _b + _a;

            variant1.Should().Be(_sum);
            variant2.Should().Be(_sum);
        }

        [Fact]
        public static void add_assign() {
            var d = new Matrix2(_a);

            d.AddAssign(_b);

            d.Should().Be(_sum);
        }

        [Fact]
        public static void add_assign_op() {
            var d = new Matrix2(_a);

            d += _b;

            d.Should().Be(_sum);
        }

        [Fact]
        public static void diagonal_test() {
            Assert.False(_incremented.IsDiagonal);
            Assert.True(new Matrix2(1, 0, 0, 1).IsDiagonal);
            Assert.False(new Matrix2(1, 0, 1, 1).IsDiagonal);
            Assert.False(new Matrix2(1, 1, 0, 1).IsDiagonal);
        }

        [Fact]
        public static void scalar_test() {
            Assert.False(_incremented.IsScalar);
            Assert.True(_identity.IsScalar);
            Assert.True(new Matrix2(2, 0, 0, 2).IsScalar);
            Assert.False(new Matrix2(2, 0, 0, 1).IsScalar);
            Assert.False(new Matrix2(1, 0, 1, 1).IsScalar);
            Assert.False(new Matrix2(1, 1, 0, 1).IsScalar);
        }

        [Fact]
        public static void identity_test() {
            Assert.False(_incremented.IsIdentity);
            Assert.True(_identity.IsIdentity);
            Assert.False(new Matrix2(2, 0, 0, 2).IsIdentity);
            Assert.False(new Matrix2(1, 0, 1, 1).IsIdentity);
            Assert.False(new Matrix2(1, 1, 0, 1).IsIdentity);
        }

        [Fact]
        public static void upper_triangular_test() {
            Assert.False(_incremented.IsUpperTriangular);
            Assert.True(_identity.IsUpperTriangular);
            Assert.True(new Matrix2(2, 0, 0, 3).IsUpperTriangular);
            Assert.True(new Matrix2(2, 0, 0, 2).IsUpperTriangular);
            Assert.False(new Matrix2(4, 0, 5, 6).IsUpperTriangular);
            Assert.True(new Matrix2(7, 8, 0, 9).IsUpperTriangular);
        }

        [Fact]
        public static void lower_triangular_test() {
            Assert.False(_incremented.IsLowerTriangular);
            Assert.True(_identity.IsLowerTriangular);
            Assert.True(new Matrix2(2, 0, 0, 3).IsLowerTriangular);
            Assert.True(new Matrix2(2, 0, 0, 2).IsLowerTriangular);
            Assert.True(new Matrix2(4, 0, 5, 6).IsLowerTriangular);
            Assert.False(new Matrix2(7, 8, 0, 9).IsLowerTriangular);
        }
    }
}