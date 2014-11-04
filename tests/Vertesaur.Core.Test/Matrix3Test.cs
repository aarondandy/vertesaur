using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.Core.Test
{

    [TestFixture]
    public class Matrix3Test
    {

        private Matrix3 _a;
        private Matrix3 _b;
        private Matrix3 _sum;
        private Matrix3 _product;
        private Matrix3 _identity;
        private Matrix3 _incremental;
        private Matrix3 _determinant99;
        private Matrix3 _priorInvert;
        private Matrix3 _postInvert;
        private Matrix3 _priorTrans;
        private Matrix3 _postTrans;
        private Matrix3 _upperTrig;
        private Matrix3 _lowerTrig;
        private Matrix3 _diagonal;
        private Matrix3 _scalar;

        [SetUp]
        public void SetUp() {
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

        [Test]
        public void DefaultConstructorTest() {
            var m = new Matrix3();
            Assert.AreEqual(_identity, m);
        }

        [Test]
        public void ElementConstructorTest() {
            var m = new Matrix3(0, 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.AreEqual(0, m.E00);
            Assert.AreEqual(1, m.E01);
            Assert.AreEqual(2, m.E02);
            Assert.AreEqual(3, m.E10);
            Assert.AreEqual(4, m.E11);
            Assert.AreEqual(5, m.E12);
            Assert.AreEqual(6, m.E20);
            Assert.AreEqual(7, m.E21);
            Assert.AreEqual(8, m.E22);
        }

        [Test]
        public void SetElementsTest() {
            var m = new Matrix3();
            m.SetElements(0, 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.AreEqual(_incremental, m);
        }

        [Test]
        public void SetIdentityTest() {
            var m = new Matrix3(_incremental);
            m.SetIdentity();
            Assert.AreEqual(_identity, m);
        }

        [Test]
        public void MatrixSizeTest() {
            IMatrix<double> m = _identity;
            Assert.AreEqual(3, m.RowCount);
            Assert.AreEqual(3, m.ColumnCount);
            Assert.AreEqual(3, ((IMatrixSquare<double>)m).Order);
            Assert.AreEqual(9, m.ElementCount);
        }

        [Test]
        public void ElementGetTest() {
            var m = _incremental;
            Assert.AreEqual(0, m.Get(0, 0));
            Assert.AreEqual(1, m.Get(0, 1));
            Assert.AreEqual(2, m.Get(0, 2));
            Assert.AreEqual(3, m.Get(1, 0));
            Assert.AreEqual(4, m.Get(1, 1));
            Assert.AreEqual(5, m.Get(1, 2));
            Assert.AreEqual(6, m.Get(2, 0));
            Assert.AreEqual(7, m.Get(2, 1));
            Assert.AreEqual(8, m.Get(2, 2));
        }

        [Test]
        public void ElementSetTest() {
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
            Assert.AreEqual(_incremental, m);
        }

        [Test]
        public void ElementFieldSetTest() {
            var m = new Matrix3 {
                E00 = 0,
                E01 = 1,
                E02 = 2,
                E10 = 3,
                E11 = 4,
                E12 = 5,
                E20 = 6,
                E21 = 7,
                E22 = 8
            };
            Assert.AreEqual(_incremental, m);
        }

        [Test]
        public void DeterminantTest() {
            Assert.AreEqual(99, _determinant99.CalculateDeterminant());
        }

        [Test]
        public void InvertTest() {
            var a = new Matrix3(_priorInvert);
            a.Invert();
            Assert.AreEqual(_postInvert, a);
        }

        [Test]
        public void GetInvertedTest() {
            var a = new Matrix3(_priorInvert);
            var b = a.GetInverse();
            Assert.AreEqual(_priorInvert, a, "Matrix was mutated.");
            Assert.AreEqual(_postInvert, b);
        }

        [Test]
        public void TransposeTest() {
            var a = new Matrix3(_priorTrans);
            a.Transpose();
            Assert.AreEqual(_postTrans, a);
        }

        [Test]
        public void GetTransposedTest() {
            var a = new Matrix3(_priorTrans);
            var b = a.GetTransposed();
            Assert.AreEqual(_priorTrans, a);
            Assert.AreEqual(_postTrans, b);
        }

        [Test]
        public void MultiplyTest() {
            var a = new Matrix3(_a);
            var b = new Matrix3(_b);
            Assert.AreEqual(_product, a.Multiply(b));
            Assert.AreEqual(_a, a);
            Assert.AreEqual(_b, b);
        }

        [Test]
        public void MultiplyOpTest() {
            var a = new Matrix3(_a);
            var b = new Matrix3(_b);
            Assert.AreEqual(_product, a * b);
            Assert.AreEqual(_a, a);
            Assert.AreEqual(_b, b);
        }

        [Test]
        public void MultiplyAssignmentTest() {
            var d = new Matrix3(_a);
            d.MultiplyAssign(_b);
            Assert.AreEqual(_product, d);
        }

        [Test]
        public void MultiplyAssignmentOpTest() {
            var d = new Matrix3(_a);
            d *= _b;
            Assert.AreEqual(_product, d);
        }

        [Test]
        public void AddTest() {
            var a = new Matrix3(_a);
            var b = new Matrix3(_b);
            Assert.AreEqual(_sum, a.Add(b));
            Assert.AreEqual(_a, a);
            Assert.AreEqual(_b, b);
        }

        [Test]
        public void AddOpTest() {
            var a = new Matrix3(_a);
            var b = new Matrix3(_b);
            Assert.AreEqual(_sum, a + b);
            Assert.AreEqual(_a, a);
            Assert.AreEqual(_b, b);
        }

        [Test]
        public void AddAssignmentTest() {
            var d = new Matrix3(_a);
            d.AddAssign(_b);
            Assert.AreEqual(_sum, d);
        }

        [Test]
        public void AddAssignmentOpTest() {
            var d = new Matrix3(_a);
            d += _b;
            Assert.AreEqual(_sum, d);
        }

        [Test]
        public void EqualsObjectTest() {
            object a = _a;
            object b = _b;
            object c = new Matrix3(_b);
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
            var a = _a;
            var b = _b;
            var c = new Matrix3(_b);
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
            var a = new Matrix3(_a);
            var a2 = a;
            var b = new Matrix3(_b);
            var b2 = b;
            var c = new Matrix3(_b);
            Matrix3 d = null;
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
            var a = new Matrix3(_a);
            var a2 = a;
            var b = new Matrix3(_b);
            var b2 = b;
            var c = new Matrix3(_b);
            Matrix3 d = null;
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
        public void IsDiagonalTest() {
            Assert.IsFalse(_incremental.IsDiagonal);
            Assert.IsFalse(_upperTrig.IsDiagonal);
            Assert.IsFalse(_lowerTrig.IsDiagonal);
            Assert.IsTrue(_diagonal.IsDiagonal);
        }

        [Test]
        public void IsScalarTest() {
            Assert.IsFalse(_incremental.IsScalar);
            Assert.IsFalse(_upperTrig.IsScalar);
            Assert.IsFalse(_lowerTrig.IsScalar);
            Assert.IsFalse(_diagonal.IsScalar);
            Assert.IsTrue(_scalar.IsScalar);
        }

        [Test]
        public void IsIdentityTest() {
            Assert.IsFalse(_incremental.IsIdentity);
            Assert.IsFalse(_upperTrig.IsIdentity);
            Assert.IsFalse(_lowerTrig.IsIdentity);
            Assert.IsFalse(_diagonal.IsIdentity);
            Assert.IsFalse(_scalar.IsIdentity);
            Assert.IsTrue(_identity.IsIdentity);
        }

        [Test]
        public void IsUpperTriangularTest() {
            Assert.IsFalse(_incremental.IsUpperTriangular);
            Assert.IsTrue(_upperTrig.IsUpperTriangular);
            Assert.IsFalse(_lowerTrig.IsUpperTriangular);
            Assert.IsTrue(_diagonal.IsUpperTriangular);
            Assert.IsTrue(_scalar.IsUpperTriangular);
            var m = new Matrix3(_identity) {
                E20 = 2
            };
            Assert.IsFalse(m.IsUpperTriangular);
        }

        [Test]
        public void IsLowerTriangularTest() {
            Assert.IsFalse(_incremental.IsLowerTriangular);
            Assert.IsFalse(_upperTrig.IsLowerTriangular);
            Assert.IsTrue(_lowerTrig.IsLowerTriangular);
            Assert.IsTrue(_diagonal.IsLowerTriangular);
            Assert.IsTrue(_scalar.IsLowerTriangular);
            var m = new Matrix3(_identity) {
                E02 = 2
            };
            Assert.IsFalse(m.IsLowerTriangular);
        }

    }
}

#pragma warning restore 1591