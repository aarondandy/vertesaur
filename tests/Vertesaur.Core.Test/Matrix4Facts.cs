using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public static class Matrix4Facts
    {

        private static readonly Matrix4 _a;
        private static readonly Matrix4 _b;
        private static readonly Matrix4 _sum;
        private static readonly Matrix4 _product;
        private static readonly Matrix4 _forDeterminant660;
        private static readonly Matrix4 _forDeterminant0;
        private static readonly Matrix4 _priorInv;
        private static readonly Matrix4 _postInv;
        private static readonly Matrix4 _incremented;
        private static readonly Matrix4 _incrementedTransposed;
        private static readonly Matrix4 _lowerTrig;
        private static readonly Matrix4 _upperTrig;
        private static readonly Matrix4 _diagonal;
        private static readonly Matrix4 _identity;
        private static readonly Matrix4 _scalar;
        private static readonly Matrix4 _determinantIsNegOne;

        static Matrix4Facts() {
            _a = new Matrix4(
                3, 1, 2, -2,
                -10, -2, 4, 0,
                -9, 5, -3, 500,
                1, 0, 10, -2
            );
            _b = new Matrix4(
                2, 3, 90, 31,
                -4, 5, 7, 0,
                13, -2, -1, 0,
                2, 0, 1, -10
            );
            _sum = new Matrix4(
                5, 4, 92, 29,
                -14, 3, 11, 0,
                4, 3, -4, 500,
                3, 0, 11, -12
            );
            _product = new Matrix4(
                24, 10, 273, 113,
                40, -48, -918, -310,
                923, 4, -272, -5279,
                128, -17, 78, 51
            );
            _forDeterminant660 = new Matrix4(
                1, 2, 3, 4,
                12, 13, 14, 5,
                11, 16, 15, 6,
                10, 9, 8, 7
            );
            _forDeterminant0 = new Matrix4(
                1, 5, 9, 13,
                2, 6, 10, 14,
                3, 7, 11, 15,
                4, 8, 12, 16
            );
            _priorInv = new Matrix4(
                1, 2, 3, 1,
                0, 1, 4, 4,
                7, 10, 5, 1,
                6, 7, 0, 7
            );
            _postInv = new Matrix4(
                17.85, -7.7, -4.55, 2.5,
                -14.6, 6.2, 3.8, -2,
                4.35, -1.7, -1.05, 0.5,
                -0.7, 0.4, 0.1, 0
            );
            _incremented = new Matrix4(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            _incrementedTransposed = new Matrix4(
                0, 4, 8, 12,
                1, 5, 9, 13,
                2, 6, 10, 14,
                3, 7, 11, 15
            );
            _lowerTrig = new Matrix4(
                3, 0, 0, 0,
                -1, 2, 0, 0,
                -9, 5, 2, 0,
                1, 0, 10, -2
            );
            _upperTrig = new Matrix4(
                3, 1, 2, -2,
                0, -2, 4, 0,
                0, 0, -3, 500,
                0, 0, 0, -2
            );
            _diagonal = new Matrix4(
                3, 0, 0, 0,
                0, 2, 0, 0,
                0, 0, 2, 0,
                0, 0, 0, 4
            );
            _identity = new Matrix4(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );
            _scalar = new Matrix4(
                2, 0, 0, 0,
                0, 2, 0, 0,
                0, 0, 2, 0,
                0, 0, 0, 2
            );
            _determinantIsNegOne = new Matrix4(
                1, 1, 0, 0,
                1, 1, 1, 0,
                0, 1, 1, 0,
                0, 0, 0, 1
            );
        }


        [Fact]
        public static void constructor_default() {
            var m = new Matrix4();

            Assert.Equal(_identity, m);
        }

        [Fact]
        public static void constructor_elements() {
            var m = new Matrix4(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            m.Should().Be(_incremented);
        }

        [Fact]
        public static void element_field_get() {
            var m = new Matrix4(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.Equal(0, m.E00);
            Assert.Equal(1, m.E01);
            Assert.Equal(2, m.E02);
            Assert.Equal(3, m.E03);
            Assert.Equal(4, m.E10);
            Assert.Equal(5, m.E11);
            Assert.Equal(6, m.E12);
            Assert.Equal(7, m.E13);
            Assert.Equal(8, m.E20);
            Assert.Equal(9, m.E21);
            Assert.Equal(10, m.E22);
            Assert.Equal(11, m.E23);
            Assert.Equal(12, m.E30);
            Assert.Equal(13, m.E31);
            Assert.Equal(14, m.E32);
            Assert.Equal(15, m.E33);
        }

        [Fact]
        public static void element_set_bulk() {
            var m = new Matrix4();

            m.SetElements(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.Equal(0, m.E00);
            Assert.Equal(1, m.E01);
            Assert.Equal(2, m.E02);
            Assert.Equal(3, m.E03);
            Assert.Equal(4, m.E10);
            Assert.Equal(5, m.E11);
            Assert.Equal(6, m.E12);
            Assert.Equal(7, m.E13);
            Assert.Equal(8, m.E20);
            Assert.Equal(9, m.E21);
            Assert.Equal(10, m.E22);
            Assert.Equal(11, m.E23);
            Assert.Equal(12, m.E30);
            Assert.Equal(13, m.E31);
            Assert.Equal(14, m.E32);
            Assert.Equal(15, m.E33);
        }

        [Fact]
        public static void identity_set() {
            var m = new Matrix4(_incremented);

            m.SetIdentity();

            Assert.Equal(_identity, m);
        }

        [Fact]
        public static void matrix_size() {
            IMatrix<double> m = _a;

            Assert.Equal(4, m.RowCount);
            Assert.Equal(4, m.ColumnCount);
            Assert.Equal(4, ((IMatrixSquare<double>)m).Order);
            Assert.Equal(16, m.ElementCount);
        }

        [Fact]
        public static void element_get() {
            var m = _incremented;

            Assert.Equal(0, m.Get(0, 0));
            Assert.Equal(1, m.Get(0, 1));
            Assert.Equal(2, m.Get(0, 2));
            Assert.Equal(3, m.Get(0, 3));
            Assert.Equal(4, m.Get(1, 0));
            Assert.Equal(5, m.Get(1, 1));
            Assert.Equal(6, m.Get(1, 2));
            Assert.Equal(7, m.Get(1, 3));
            Assert.Equal(8, m.Get(2, 0));
            Assert.Equal(9, m.Get(2, 1));
            Assert.Equal(10, m.Get(2, 2));
            Assert.Equal(11, m.Get(2, 3));
            Assert.Equal(12, m.Get(3, 0));
            Assert.Equal(13, m.Get(3, 1));
            Assert.Equal(14, m.Get(3, 2));
            Assert.Equal(15, m.Get(3, 3));
        }

        [Fact]
        public static void element_set() {
            var m = new Matrix4();

            m.Set(0, 0, 0);
            m.Set(0, 1, 1);
            m.Set(0, 2, 2);
            m.Set(0, 3, 3);
            m.Set(1, 0, 4);
            m.Set(1, 1, 5);
            m.Set(1, 2, 6);
            m.Set(1, 3, 7);
            m.Set(2, 0, 8);
            m.Set(2, 1, 9);
            m.Set(2, 2, 10);
            m.Set(2, 3, 11);
            m.Set(3, 0, 12);
            m.Set(3, 1, 13);
            m.Set(3, 2, 14);
            m.Set(3, 3, 15);

            Assert.Equal(0, m.E00);
            Assert.Equal(1, m.E01);
            Assert.Equal(2, m.E02);
            Assert.Equal(3, m.E03);
            Assert.Equal(4, m.E10);
            Assert.Equal(5, m.E11);
            Assert.Equal(6, m.E12);
            Assert.Equal(7, m.E13);
            Assert.Equal(8, m.E20);
            Assert.Equal(9, m.E21);
            Assert.Equal(10, m.E22);
            Assert.Equal(11, m.E23);
            Assert.Equal(12, m.E30);
            Assert.Equal(13, m.E31);
            Assert.Equal(14, m.E32);
            Assert.Equal(15, m.E33);
        }

        [Fact]
        public static void determinant() {
            Assert.Equal(660, _forDeterminant660.CalculateDeterminant());
            Assert.Equal(0, _forDeterminant0.CalculateDeterminant());
            Assert.Equal(0, new Matrix4(1, -1, 0, 2, -1, 1, 2, 3, 2, -2, 3, 4, 6, -6, 6, 1).CalculateDeterminant());
            Assert.Equal(12, new Matrix4(1, 0, 2, 1, 2, -1, 1, 0, 1, 0, 0, 3, -1, 0, 2, 1).CalculateDeterminant());
        }

        public static void AreEqual(Matrix4 a, Matrix4 b, int preciscion) {
            Assert.Equal(a.E00, b.E00, preciscion);
            Assert.Equal(a.E01, b.E01, preciscion);
            Assert.Equal(a.E02, b.E02, preciscion);
            Assert.Equal(a.E03, b.E03, preciscion);
            Assert.Equal(a.E10, b.E10, preciscion);
            Assert.Equal(a.E11, b.E11, preciscion);
            Assert.Equal(a.E12, b.E12, preciscion);
            Assert.Equal(a.E13, b.E13, preciscion);
            Assert.Equal(a.E20, b.E20, preciscion);
            Assert.Equal(a.E21, b.E21, preciscion);
            Assert.Equal(a.E22, b.E22, preciscion);
            Assert.Equal(a.E23, b.E23, preciscion);
            Assert.Equal(a.E30, b.E30, preciscion);
            Assert.Equal(a.E31, b.E31, preciscion);
            Assert.Equal(a.E32, b.E32, preciscion);
            Assert.Equal(a.E33, b.E33, preciscion);
        }

        [Fact]
        public static void invert() {
            var m = new Matrix4(_priorInv);

            m.Invert();

            AreEqual(_postInv, m, 10);
        }

        [Fact]
        public static void inverse_safety() {
            var m = new Matrix4(_priorInv);

            var n = m.GetInverse();

            Assert.Equal(_priorInv, m);
            AreEqual(_postInv, n, 10);
        }

        [Fact]
        public static void transpose() {
            var m = new Matrix4(_incremented);

            m.Transpose();

            Assert.Equal(_incrementedTransposed, m);
        }

        [Fact]
        public static void double_traspose() {
            var m = new Matrix4(_incremented);

            m.Transpose();
            m.Transpose();

            Assert.Equal(_incremented, m);
        }

        [Fact]
        public static void transposed() {
            var m = new Matrix4(_incremented);

            var n = m.GetTransposed();

            Assert.Equal(_incremented, m);
            Assert.Equal(_incrementedTransposed, n);
        }

        [Fact]
        public static void multiply() {
            var product = _a.Multiply(_b);

            Assert.Equal(_product, product);
        }

        [Fact]
        public static void multiply_op() {
            var product = _a * _b;

            Assert.Equal(_product, product);
        }

        [Fact]
        public static void multiply_assign() {
            var d = new Matrix4(_a);

            d.MultiplyAssign(_b);

            Assert.Equal(_product, d);
        }

        [Fact]
        public static void multiply_assign_op() {
            var d = new Matrix4(_a);

            d *= _b;

            Assert.Equal(_product, d);
        }

        [Fact]
        public static void add() {
            var sum = _a.Add(_b);

            Assert.Equal(_sum, sum);
        }

        [Fact]
        public static void add_op() {
            var sum = _a + _b;

            Assert.Equal(_sum, sum);
        }

        [Fact]
        public static void add_assign() {
            var c = new Matrix4(_a);

            c.AddAssign(_b);

            Assert.Equal(_sum, c);
        }

        [Fact]
        public static void add_assign_op() {
            var c = new Matrix4(_a);

            c += _b;

            Assert.Equal(_sum, c);
        }

        [Fact]
        public static void equals_object() {
            var a = (object)_a;
            var b = (object)_b;
            var c = (object)new Matrix4(_b);

            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.False(a.Equals(c));
            Assert.True(b.Equals(c));
            Assert.False(a.Equals(null));
        }

        [Fact]
        public static void equals_typed() {
            var a = new Matrix4(_a);
            var b = new Matrix4(_b);
            var c = new Matrix4(_b);

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
            var a = new Matrix4(_a);
            var a2 = a;
            var b = new Matrix4(_b);
            var b2 = b;
            var c = new Matrix4(_b);
            Matrix4 d = null;

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
            var a = new Matrix4(_a);
            var a2 = a;
            var b = new Matrix4(_b);
            var b2 = b;
            var c = new Matrix4(_b);
            Matrix4 d = null;

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
        public static void upper_triangular_test() {
            Assert.False(_a.IsUpperTriangular);
            Assert.False(_lowerTrig.IsUpperTriangular);
            Assert.True(_upperTrig.IsUpperTriangular);
            Assert.True(_diagonal.IsUpperTriangular);
            Assert.True(_identity.IsUpperTriangular);
        }

        [Fact]
        public static void lower_triangular_test() {
            Assert.False(_a.IsLowerTriangular);
            Assert.True(_lowerTrig.IsLowerTriangular);
            Assert.False(_upperTrig.IsLowerTriangular);
            Assert.True(_diagonal.IsLowerTriangular);
            Assert.True(_identity.IsLowerTriangular);
        }

        [Fact]
        public static void identity_test() {
            Assert.False(_a.IsIdentity);
            Assert.False(_lowerTrig.IsIdentity);
            Assert.False(_upperTrig.IsIdentity);
            Assert.False(_diagonal.IsIdentity);
            Assert.True(_identity.IsIdentity);
            Assert.False(_scalar.IsIdentity);
        }

        [Fact]
        public static void scalar_test() {
            Assert.False(_a.IsScalar);
            Assert.False(_lowerTrig.IsScalar);
            Assert.False(_upperTrig.IsScalar);
            Assert.False(_diagonal.IsScalar);
            Assert.True(_identity.IsScalar);
            Assert.True(_scalar.IsScalar);
        }

        [Fact]
        public static void diagonal_test() {
            Assert.False(_a.IsDiagonal);
            Assert.False(_lowerTrig.IsDiagonal);
            Assert.False(_upperTrig.IsDiagonal);
            Assert.True(_diagonal.IsDiagonal);
            Assert.True(_identity.IsDiagonal);
            Assert.True(_scalar.IsDiagonal);
        }

        [Fact]
        public static void determinant_tricky() {
            var determinant = _determinantIsNegOne.CalculateDeterminant();

            Assert.Equal(-1, determinant);
        }

    }
}
