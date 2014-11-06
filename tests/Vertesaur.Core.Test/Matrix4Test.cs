using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.Core.Test
{
    [TestFixture]
    public class Matrix4Test
    {

        private Matrix4 _a;
        private Matrix4 _b;
        private Matrix4 _sum;
        private Matrix4 _product;
        private Matrix4 _forDeterminant660;
        private Matrix4 _forDeterminant0;
        private Matrix4 _priorInv;
        private Matrix4 _postInv;
        private Matrix4 _incremented;
        private Matrix4 _incrementedTransposed;
        private Matrix4 _lowerTrig;
        private Matrix4 _upperTrig;
        private Matrix4 _diagonal;
        private Matrix4 _identity;
        private Matrix4 _scalar;
        private Matrix4 _determinantIsNegOne;

        [SetUp]
        public void SetUp() {
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


        [Test]
        public void DefaultConstructorTest() {
            var m = new Matrix4();
            Assert.AreEqual(_identity, m);
        }

        [Test]
        public void ElementConstructorTest() {
            var m = new Matrix4(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.AreEqual(0, m.E00);
            Assert.AreEqual(1, m.E01);
            Assert.AreEqual(2, m.E02);
            Assert.AreEqual(3, m.E03);
            Assert.AreEqual(4, m.E10);
            Assert.AreEqual(5, m.E11);
            Assert.AreEqual(6, m.E12);
            Assert.AreEqual(7, m.E13);
            Assert.AreEqual(8, m.E20);
            Assert.AreEqual(9, m.E21);
            Assert.AreEqual(10, m.E22);
            Assert.AreEqual(11, m.E23);
            Assert.AreEqual(12, m.E30);
            Assert.AreEqual(13, m.E31);
            Assert.AreEqual(14, m.E32);
            Assert.AreEqual(15, m.E33);
        }

        [Test]
        public void SetElementsTest() {
            var m = new Matrix4();
            m.SetElements(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.AreEqual(0, m.E00);
            Assert.AreEqual(1, m.E01);
            Assert.AreEqual(2, m.E02);
            Assert.AreEqual(3, m.E03);
            Assert.AreEqual(4, m.E10);
            Assert.AreEqual(5, m.E11);
            Assert.AreEqual(6, m.E12);
            Assert.AreEqual(7, m.E13);
            Assert.AreEqual(8, m.E20);
            Assert.AreEqual(9, m.E21);
            Assert.AreEqual(10, m.E22);
            Assert.AreEqual(11, m.E23);
            Assert.AreEqual(12, m.E30);
            Assert.AreEqual(13, m.E31);
            Assert.AreEqual(14, m.E32);
            Assert.AreEqual(15, m.E33);
        }

        [Test]
        public void SetIdentityTest() {
            var m = new Matrix4(_incremented);
            m.SetIdentity();
            Assert.AreEqual(_identity, m);
        }

        [Test]
        public void MatrixSizeTest() {
            IMatrix<double> m = _a;
            Assert.AreEqual(4, m.RowCount);
            Assert.AreEqual(4, m.ColumnCount);
            Assert.AreEqual(4, ((IMatrixSquare<double>)m).Order);
            Assert.AreEqual(16, m.ElementCount);
        }

        [Test]
        public void ElementGetTest() {
            var m = _incremented;
            Assert.AreEqual(0, m.Get(0, 0));
            Assert.AreEqual(1, m.Get(0, 1));
            Assert.AreEqual(2, m.Get(0, 2));
            Assert.AreEqual(3, m.Get(0, 3));
            Assert.AreEqual(4, m.Get(1, 0));
            Assert.AreEqual(5, m.Get(1, 1));
            Assert.AreEqual(6, m.Get(1, 2));
            Assert.AreEqual(7, m.Get(1, 3));
            Assert.AreEqual(8, m.Get(2, 0));
            Assert.AreEqual(9, m.Get(2, 1));
            Assert.AreEqual(10, m.Get(2, 2));
            Assert.AreEqual(11, m.Get(2, 3));
            Assert.AreEqual(12, m.Get(3, 0));
            Assert.AreEqual(13, m.Get(3, 1));
            Assert.AreEqual(14, m.Get(3, 2));
            Assert.AreEqual(15, m.Get(3, 3));
        }

        [Test]
        public void ElementSetTest() {
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
            Assert.AreEqual(0, m.E00);
            Assert.AreEqual(1, m.E01);
            Assert.AreEqual(2, m.E02);
            Assert.AreEqual(3, m.E03);
            Assert.AreEqual(4, m.E10);
            Assert.AreEqual(5, m.E11);
            Assert.AreEqual(6, m.E12);
            Assert.AreEqual(7, m.E13);
            Assert.AreEqual(8, m.E20);
            Assert.AreEqual(9, m.E21);
            Assert.AreEqual(10, m.E22);
            Assert.AreEqual(11, m.E23);
            Assert.AreEqual(12, m.E30);
            Assert.AreEqual(13, m.E31);
            Assert.AreEqual(14, m.E32);
            Assert.AreEqual(15, m.E33);
        }

        [Test]
        public void ElementFieldSetTest() {
            var m = new Matrix4 {
                E00 = 0,
                E01 = 1,
                E02 = 2,
                E03 = 3,
                E10 = 4,
                E11 = 5,
                E12 = 6,
                E13 = 7,
                E20 = 8,
                E21 = 9,
                E22 = 10,
                E23 = 11,
                E30 = 12,
                E31 = 13,
                E32 = 14,
                E33 = 15
            };
            Assert.AreEqual(0, m.E00);
            Assert.AreEqual(1, m.E01);
            Assert.AreEqual(2, m.E02);
            Assert.AreEqual(3, m.E03);
            Assert.AreEqual(4, m.E10);
            Assert.AreEqual(5, m.E11);
            Assert.AreEqual(6, m.E12);
            Assert.AreEqual(7, m.E13);
            Assert.AreEqual(8, m.E20);
            Assert.AreEqual(9, m.E21);
            Assert.AreEqual(10, m.E22);
            Assert.AreEqual(11, m.E23);
            Assert.AreEqual(12, m.E30);
            Assert.AreEqual(13, m.E31);
            Assert.AreEqual(14, m.E32);
            Assert.AreEqual(15, m.E33);
        }

        [Test]
        public void DeterminantTest() {
            Assert.AreEqual(660, _forDeterminant660.CalculateDeterminant());
            Assert.AreEqual(0, _forDeterminant0.CalculateDeterminant());
            Assert.AreEqual(0, new Matrix4(1, -1, 0, 2, -1, 1, 2, 3, 2, -2, 3, 4, 6, -6, 6, 1).CalculateDeterminant());
            Assert.AreEqual(12, new Matrix4(1, 0, 2, 1, 2, -1, 1, 0, 1, 0, 0, 3, -1, 0, 2, 1).CalculateDeterminant());
        }

        public void AreEqual(Matrix4 a, Matrix4 b, double delta) {
            Assert.AreEqual(a.E00, b.E00, delta);
            Assert.AreEqual(a.E01, b.E01, delta);
            Assert.AreEqual(a.E02, b.E02, delta);
            Assert.AreEqual(a.E03, b.E03, delta);
            Assert.AreEqual(a.E10, b.E10, delta);
            Assert.AreEqual(a.E11, b.E11, delta);
            Assert.AreEqual(a.E12, b.E12, delta);
            Assert.AreEqual(a.E13, b.E13, delta);
            Assert.AreEqual(a.E20, b.E20, delta);
            Assert.AreEqual(a.E21, b.E21, delta);
            Assert.AreEqual(a.E22, b.E22, delta);
            Assert.AreEqual(a.E23, b.E23, delta);
            Assert.AreEqual(a.E30, b.E30, delta);
            Assert.AreEqual(a.E31, b.E31, delta);
            Assert.AreEqual(a.E32, b.E32, delta);
            Assert.AreEqual(a.E33, b.E33, delta);
        }

        [Test]
        public void InvertBasicTest() {
            var m = new Matrix4(_priorInv);
            m.Invert();
            AreEqual(_postInv, m, 0.0000001);
        }

        [Test]
        public void GetInvertedSafetyTest() {
            var m = new Matrix4(_priorInv);
            var n = m.GetInverse();
            Assert.AreEqual(_priorInv, m, "Matrix was mutated.");
            AreEqual(_postInv, n, 0.0000001);
        }

        [Test]
        public void TransposeTest() {
            var m = new Matrix4(_incremented);
            m.Transpose();
            Assert.AreEqual(_incrementedTransposed, m);
            m.Transpose();
            Assert.AreEqual(_incremented, m);
        }

        [Test]
        public void GetTransposedTest() {
            var m = new Matrix4(_incremented);
            var n = m.GetTransposed();
            Assert.AreEqual(_incremented, m, "Matrix was mutated.");
            Assert.AreEqual(_incrementedTransposed, n);
            m = new Matrix4(n);
            n = m.GetTransposed();
            Assert.AreEqual(_incrementedTransposed, m, "Matrix was mutated.");
            Assert.AreEqual(_incremented, n);
        }

        [Test]
        public void MultiplyTest() {
            Assert.AreEqual(_product, _a.Multiply(_b));
        }

        [Test]
        public void MultiplyOpTest() {
            Assert.AreEqual(_product, _a * _b);
        }

        [Test]
        public void MultiplyAssignmentTest() {
            var d = new Matrix4(_a);
            d.MultiplyAssign(_b);
            Assert.AreEqual(_product, d);
        }

        [Test]
        public void MultiplyAssignmentOpTest() {
            var d = new Matrix4(_a);
            d *= _b;
            Assert.AreEqual(_product, d);
        }

        [Test]
        public void AddTest() {
            Assert.AreEqual(_sum, _a.Add(_b));
        }

        [Test]
        public void AddOpTest() {
            Assert.AreEqual(_sum, _a + _b);
        }

        [Test]
        public void AddAssignTest() {
            var c = new Matrix4(_a);
            c.AddAssign(_b);
            Assert.AreEqual(_sum, c);

        }

        [Test]
        public void AddAssignOpTest() {
            var c = new Matrix4(_a);
            c += _b;
            Assert.AreEqual(_sum, c);
        }

        [Test]
        public void EqualsObjectTest() {
            var a = (object)_a;
            var b = (object)_b;
            var c = (object)new Matrix4(_b);
            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(b.Equals(a));
            // ReSharper disable EqualExpressionComparison
            Assert.IsTrue(a.Equals(a));
            Assert.IsTrue(b.Equals(b));
            // ReSharper restore EqualExpressionComparison
            Assert.IsFalse(a.Equals(c));
            Assert.IsTrue(b.Equals(c));
            Assert.IsFalse(a.Equals(null));
        }

        [Test]
        public void EqualsSelfTypeTest() {
            var a = new Matrix4(_a);
            var b = new Matrix4(_b);
            var c = new Matrix4(_b);
            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(b.Equals(a));
            Assert.IsTrue(a.Equals(a));
            Assert.IsTrue(b.Equals(b));
            Assert.IsFalse(a.Equals(c));
            Assert.IsTrue(b.Equals(c));
            Assert.IsFalse(a.Equals(null));
        }

        [Test]
        public void OpEqualTest() {
            var a = new Matrix4(_a);
            var a2 = a;
            var b = new Matrix4(_b);
            var b2 = b;
            var c = new Matrix4(_b);
            Matrix4 d = null;
            Assert.IsFalse(a == b);
            Assert.IsFalse(b == a);
            Assert.IsTrue(a == a2);
            Assert.IsTrue(b == b2);
            Assert.IsFalse(a == c);
            Assert.IsTrue(b == c);
            Assert.IsFalse(a == null);
            Assert.IsFalse(null == a);
            Assert.IsFalse(b == d);
            Assert.IsFalse(d == b);
            Assert.IsTrue(null == d);
        }

        [Test]
        public void OpNotEqualTest() {
            var a = new Matrix4(_a);
            var a2 = a;
            var b = new Matrix4(_b);
            var b2 = b;
            var c = new Matrix4(_b);
            Matrix4 d = null;
            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
            Assert.IsFalse(a != a2);
            Assert.IsFalse(b != b2);
            Assert.IsTrue(a != c);
            Assert.IsFalse(b != c);
            Assert.IsTrue(a != null);
            Assert.IsTrue(null != a);
            Assert.IsTrue(b != d);
            Assert.IsTrue(d != b);
            Assert.IsFalse(null != d);
        }

        [Test]
        public void IsUpperTriangularTest() {
            Assert.IsFalse(_a.IsUpperTriangular);
            Assert.IsFalse(_lowerTrig.IsUpperTriangular);
            Assert.IsTrue(_upperTrig.IsUpperTriangular);
            Assert.IsTrue(_diagonal.IsUpperTriangular);
            Assert.IsTrue(_identity.IsUpperTriangular);
        }

        [Test]
        public void IsLowerTriangularTest() {
            Assert.IsFalse(_a.IsLowerTriangular);
            Assert.IsTrue(_lowerTrig.IsLowerTriangular);
            Assert.IsFalse(_upperTrig.IsLowerTriangular);
            Assert.IsTrue(_diagonal.IsLowerTriangular);
            Assert.IsTrue(_identity.IsLowerTriangular);
        }

        [Test]
        public void IsIdentityTest() {
            Assert.IsFalse(_a.IsIdentity);
            Assert.IsFalse(_lowerTrig.IsIdentity);
            Assert.IsFalse(_upperTrig.IsIdentity);
            Assert.IsFalse(_diagonal.IsIdentity);
            Assert.IsTrue(_identity.IsIdentity);
            Assert.IsFalse(_scalar.IsIdentity);
        }

        [Test]
        public void IsScalarTest() {
            Assert.IsFalse(_a.IsScalar);
            Assert.IsFalse(_lowerTrig.IsScalar);
            Assert.IsFalse(_upperTrig.IsScalar);
            Assert.IsFalse(_diagonal.IsScalar);
            Assert.IsTrue(_identity.IsScalar);
            Assert.IsTrue(_scalar.IsScalar);
        }

        [Test]
        public void IsDiagonalTest() {
            Assert.IsFalse(_a.IsDiagonal);
            Assert.IsFalse(_lowerTrig.IsDiagonal);
            Assert.IsFalse(_upperTrig.IsDiagonal);
            Assert.IsTrue(_diagonal.IsDiagonal);
            Assert.IsTrue(_identity.IsDiagonal);
            Assert.IsTrue(_scalar.IsDiagonal);
        }

        [Test]
        public void DifficultDeterminant() {
            var determinant = _determinantIsNegOne.CalculateDeterminant();
            Assert.AreEqual(-1, determinant);
        }

    }
}

#pragma warning restore 1591
