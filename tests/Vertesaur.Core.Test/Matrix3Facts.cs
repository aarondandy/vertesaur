using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{

    public static class Matrix3Facts
    {

        private static readonly Matrix3 _a;
        private static readonly Matrix3 _b;
        private static readonly Matrix3 _sum;
        private static readonly Matrix3 _product;
        private static readonly Matrix3 _identity;
        private static readonly Matrix3 _incremental;
        private static readonly Matrix3 _determinant99;
        private static readonly Matrix3 _priorInvert;
        private static readonly Matrix3 _postInvert;
        private static readonly Matrix3 _priorTrans;
        private static readonly Matrix3 _postTrans;
        private static readonly Matrix3 _upperTrig;
        private static readonly Matrix3 _lowerTrig;
        private static readonly Matrix3 _diagonal;
        private static readonly Matrix3 _scalar;

        static Matrix3Facts() {
            _a = new Matrix3(
                3, 1, 2,
                -10, -2, 4,
                -9, 5, -3
            );
            _b = new Matrix3(
                2, 3, 90,
                -4, 5, 7,
                13, -2, -1
            );
            _sum = new Matrix3(
                5, 4, 92,
                -14, 3, 11,
                4, 3, -4
            );
            _product = new Matrix3(
                28, 10, 275,
                40, -48, -918,
                -77, 4, -772
            );
            _identity = new Matrix3(
                1, 0, 0,
                0, 1, 0,
                0, 0, 1
            );
            _incremental = new Matrix3(0, 1, 2, 3, 4, 5, 6, 7, 8);
            _determinant99 = new Matrix3(
                1, 5, 7,
                4, 2, 9,
                6, 3, 8
            );
            _priorInvert = new Matrix3(
                1, 1, 2,
                1, 2, 4,
                2, -1, 0
            );
            _postInvert = new Matrix3(
                2, -1, 0,
                4, -2, -1,
                -2.5, 1.5, 0.5
            );
            _priorTrans = new Matrix3(
                1, 2, 5,
                3, 4, 6,
                7, 8, 9
            );
            _postTrans = new Matrix3(
                1, 3, 7,
                2, 4, 8,
                5, 6, 9
            );
            _upperTrig = new Matrix3(
                1, 2, 3,
                0, 5, 6,
                0, 0, 9
            );
            _lowerTrig = new Matrix3(
                1, 0, 0,
                4, 5, 0,
                7, 8, 9
            );
            _diagonal = new Matrix3(
                1, 0, 0,
                0, 5, 0,
                0, 0, 9
            );
            _scalar = new Matrix3(
                5, 0, 0,
                0, 5, 0,
                0, 0, 5
            );
        }

        [Fact]
        public static void constructor_default() {
            var m = new Matrix3();

            Assert.Equal(_identity, m);
        }

        [Fact]
        public static void constrctor_elements() {
            var m = new Matrix3(0, 1, 2, 3, 4, 5, 6, 7, 8);

            m.Should().Be(_incremental);
        }

        [Fact]
        public static void element_field_get() {
            var m = new Matrix3(0, 1, 2, 3, 4, 5, 6, 7, 8);

            m.E00.Should().Be(0);
            m.E01.Should().Be(1);
            m.E02.Should().Be(2);
            m.E10.Should().Be(3);
            m.E11.Should().Be(4);
            m.E12.Should().Be(5);
            m.E20.Should().Be(6);
            m.E21.Should().Be(7);
            m.E22.Should().Be(8);
        }

        [Fact]
        public static void element_set_bulk() {
            var m = new Matrix3();

            m.SetElements(0, 1, 2, 3, 4, 5, 6, 7, 8);

            m.Should().Be(_incremental);
        }
                                                                                             
        [Fact]
        public static void set_identity() {
            var m = new Matrix3(_incremental);

            m.SetIdentity();

            m.Should().Be(_identity);
        }

        [Fact]
        public static void matrix_size() {
            IMatrix<double> m = _identity;

            Assert.Equal(3, m.RowCount);
            Assert.Equal(3, m.ColumnCount);
            Assert.Equal(3, ((IMatrixSquare<double>)m).Order);
            Assert.Equal(9, m.ElementCount);
        }

        [Fact]
        public static void element_get() {
            var m = _incremental;

            Assert.Equal(0, m.Get(0, 0));
            Assert.Equal(1, m.Get(0, 1));
            Assert.Equal(2, m.Get(0, 2));
            Assert.Equal(3, m.Get(1, 0));
            Assert.Equal(4, m.Get(1, 1));
            Assert.Equal(5, m.Get(1, 2));
            Assert.Equal(6, m.Get(2, 0));
            Assert.Equal(7, m.Get(2, 1));
            Assert.Equal(8, m.Get(2, 2));
        }

        [Fact]
        public static void element_set() {
            var m = new Matrix3();

            m.Set(0, 0, 0);
            m.Set(0, 1, 1);
            m.Set(0, 2, 2);
            m.Set(1, 0, 3);
            m.Set(1, 1, 4);
            m.Set(1, 2, 5);
            m.Set(2, 0, 6);
            m.Set(2, 1, 7);
            m.Set(2, 2, 8);

            Assert.Equal(_incremental, m);
        }

        public static IEnumerable<object[]> DeterminantsData {
            get {
                yield return new object[] { 99, _determinant99 };
                yield return new object[] { -244, _a };
                yield return new object[] { -4851, _b };
                yield return new object[] { 1, _identity };
                yield return new object[] { 2, _priorInvert };
                yield return new object[] { 0.5, _postInvert };
                yield return new object[] { 45, _upperTrig };
                yield return new object[] { 45, _lowerTrig };
                yield return new object[] { 45, _diagonal };
                yield return new object[] { 2, new Matrix3(1, -2, 4, -1, 2, -5, 2, -2, 11) };
                yield return new object[] { 0, new Matrix3(1, 0, 1, 1, 1, 0, 0, 0, 0) };
                yield return new object[] { 0, new Matrix3(2, 0, -7, 3, 0, 1, -4, 0, 9) };
                yield return new object[] { 0, new Matrix3(2, -1, 3, 1, 2, 4, 2, 4, 8) };
                yield return new object[] { -30, new Matrix3(2, -1, 1, 0, 3, -4, 0, 0, -5) };
                yield return new object[] { -26, new Matrix3(2, 4, 1, -2, -5, 4, 4, 9, 10) };
                yield return new object[] { 2, new Matrix3(1, -2, 4, -1, 2, -5, 2, -2, 11) };
            }
        }

        [Theory, PropertyData("DeterminantsData")]
        public static void determinant(double expectedDeterminant, Matrix3 m) {
            var determinant = m.CalculateDeterminant();

            determinant.Should().Be(expectedDeterminant);
        }

        [Fact]
        public static void invert() {
            var a = new Matrix3(_priorInvert);

            a.Invert();

            a.Should().Be(_postInvert);
        }

        [Fact]
        public static void inverse_safety() {
            var a = new Matrix3(_priorInvert);

            var b = a.GetInverse();

            a.Should().Be(_priorInvert);
            b.Should().Be(_postInvert);
        }

        [Fact]
        public static void inverse() {
            Assert.Equal(_postInvert, _priorInvert.GetInverse());
            Assert.Equal(_priorInvert, _postInvert.GetInverse());
        }

        [Fact]
        public static void transpose() {
            var a = new Matrix3(_priorTrans);

            a.Transpose();

            a.Should().Be(_postTrans);
        }

        [Fact]
        public static void transposed() {
            var a = new Matrix3(_priorTrans);

            var b = a.GetTransposed();

            Assert.Equal(_priorTrans, a);
            Assert.Equal(_postTrans, b);
        }

        [Fact]
        public static void multiply() {
            var a = new Matrix3(_a);
            var b = new Matrix3(_b);

            var product = a.Multiply(b);

            Assert.Equal(_product, product);
            Assert.Equal(_a, a);
            Assert.Equal(_b, b);
        }

        [Fact]
        public static void multiply_op() {
            var a = new Matrix3(_a);
            var b = new Matrix3(_b);

            var product = a * b;

            Assert.Equal(_product, product);
            Assert.Equal(_a, a);
            Assert.Equal(_b, b);
        }

        [Fact]
        public static void multiply_assign() {
            var d = new Matrix3(_a);

            d.MultiplyAssign(_b);

            Assert.Equal(_product, d);
        }

        [Fact]
        public static void multiply_assign_op() {
            var d = new Matrix3(_a);

            d *= _b;

            Assert.Equal(_product, d);
        }

        [Fact]
        public static void add() {
            var a = new Matrix3(_a);
            var b = new Matrix3(_b);

            var sum = a.Add(b);

            Assert.Equal(_sum, sum);
            Assert.Equal(_a, a);
            Assert.Equal(_b, b);
        }

        [Fact]
        public static void add_op() {
            var a = new Matrix3(_a);
            var b = new Matrix3(_b);

            var sum = a + b;

            Assert.Equal(_sum, sum);
            Assert.Equal(_a, a);
            Assert.Equal(_b, b);
        }

        [Fact]
        public static void add_assign() {
            var d = new Matrix3(_a);

            d.AddAssign(_b);

            Assert.Equal(_sum, d);
        }

        [Fact]
        public static void add_assign_op() {
            var d = new Matrix3(_a);

            d += _b;

            Assert.Equal(_sum, d);
        }

        [Fact]
        public static void equals_object() {
            object a = _a;
            object b = _b;
            object c = new Matrix3(_b);

            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
            // ReSharper disable EqualExpressionComparison
            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            // ReSharper restore EqualExpressionComparison
            Assert.False(a.Equals(c));
            Assert.True(b.Equals(c));
            Assert.False(a.Equals(null));
        }

        [Fact]
        public static void equals_typed() {
            var a = _a;
            var b = _b;
            var c = new Matrix3(_b);

            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.False(a.Equals(c));
            Assert.True(b.Equals(c));
            Assert.False(a.Equals(null));
        }

        [Fact]
        public static void equal_op() {
            var a = new Matrix3(_a);
            var a2 = a;
            var b = new Matrix3(_b);
            var b2 = b;
            var c = new Matrix3(_b);
            Matrix3 d = null;

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
        public static void inequal_op() {
            var a = new Matrix3(_a);
            var a2 = a;
            var b = new Matrix3(_b);
            var b2 = b;
            var c = new Matrix3(_b);
            Matrix3 d = null;

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
        public static void diagonal_test() {
            Assert.False(_incremental.IsDiagonal);
            Assert.False(_upperTrig.IsDiagonal);
            Assert.False(_lowerTrig.IsDiagonal);
            Assert.True(_diagonal.IsDiagonal);
        }

        [Fact]
        public static void scalar_test() {
            Assert.False(_incremental.IsScalar);
            Assert.False(_upperTrig.IsScalar);
            Assert.False(_lowerTrig.IsScalar);
            Assert.False(_diagonal.IsScalar);
            Assert.True(_scalar.IsScalar);
        }

        [Fact]
        public static void identity_test() {
            Assert.False(_incremental.IsIdentity);
            Assert.False(_upperTrig.IsIdentity);
            Assert.False(_lowerTrig.IsIdentity);
            Assert.False(_diagonal.IsIdentity);
            Assert.False(_scalar.IsIdentity);
            Assert.True(_identity.IsIdentity);
        }

        [Fact]
        public static void upper_triangular_test() {
            var m = new Matrix3(_identity);

            m.Set(2, 0, 2);

            Assert.False(m.IsUpperTriangular);

            Assert.False(_incremental.IsUpperTriangular);
            Assert.True(_upperTrig.IsUpperTriangular);
            Assert.False(_lowerTrig.IsUpperTriangular);
            Assert.True(_diagonal.IsUpperTriangular);
            Assert.True(_scalar.IsUpperTriangular);
        }

        [Fact]
        public static void lower_triangular_test() {
            var m = new Matrix3(_identity);

            m.Set(0, 2, 2);

            Assert.False(m.IsLowerTriangular);

            Assert.False(_incremental.IsLowerTriangular);
            Assert.False(_upperTrig.IsLowerTriangular);
            Assert.True(_lowerTrig.IsLowerTriangular);
            Assert.True(_diagonal.IsLowerTriangular);
            Assert.True(_scalar.IsLowerTriangular);
        }

    }
}