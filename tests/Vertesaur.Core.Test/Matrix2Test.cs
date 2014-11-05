using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.Core.Test
{
    [TestFixture]
    public class Matrix2Test
    {

        private Matrix2 _a;
        private Matrix2 _b;
        private Matrix2 _product;
        private Matrix2 _sum;
        private Matrix2 _identity;
        private Matrix2 _incremented;
        private Matrix2 _determinant34;
        private Matrix2 _priorInvert;
        private Matrix2 _postInvert;
        private Matrix2 _trans;

        [SetUp]
        public void SetUp() {
            _identity = new Matrix2(
                1, 0,
                0, 1
            );
            _incremented = new Matrix2(0, 1, 2, 3);
            _determinant34 = new Matrix2(
                3, -8,
                5, -2
            );
            _priorInvert = new Matrix2(
                1, 3,
                2, 4
            );
            _postInvert = new Matrix2(
                -2, 1.5,
                1, -0.5
            );
            _trans = new Matrix2(
                0, 2,
                1, 3
            );
            _a = new Matrix2(
                3, 1,
                -10, -2
            );
            _b = new Matrix2(
                2, 3,
                -4, 5
            );
            _product = new Matrix2(
                2, 14,
                -12, -40
            );
            _sum = new Matrix2(
                5, 4,
                -14, 3
            );
        }

        [Test]
        public void DefaultConstructorTest() {
            var m = new Matrix2();
            Assert.AreEqual(_identity, m);
        }

        [Test]
        public void ElementConstructorTest() {
            var m = new Matrix2(0, 1, 2, 3);
            Assert.AreEqual(_incremented, m);
        }

        [Test]
        public void SetElementsTest() {
            var m = new Matrix2();
            m.SetElements(0, 1, 2, 3);
            Assert.AreEqual(_incremented, m);
        }

        [Test]
        public void SetIdentityTest() {
            var m = new Matrix2(_incremented);
            m.SetIdentity();
            Assert.AreEqual(_identity, m);
        }

        [Test]
        public void MatrixSizeTest() {
            IMatrix<double> m = _incremented;
            Assert.AreEqual(2, m.RowCount);
            Assert.AreEqual(2, m.ColumnCount);
            Assert.AreEqual(2, ((IMatrixSquare<double>)m).Order);
            Assert.AreEqual(4, m.ElementCount);
        }

        [Test]
        public void ElementGetTest() {
            var m = new Matrix2(_incremented);
            Assert.AreEqual(0, m.Get(0, 0));
            Assert.AreEqual(1, m.Get(0, 1));
            Assert.AreEqual(2, m.Get(1, 0));
            Assert.AreEqual(3, m.Get(1, 1));
        }

        [Test]
        public void ElementSetTest() {
            var m = new Matrix2();
            m.Set(0, 0, 0);
            m.Set(0, 1, 1);
            m.Set(1, 0, 2);
            m.Set(1, 1, 3);
            Assert.AreEqual(_incremented, m);
        }

        [Test]
        public void ElementFieldSetTest() {
            var m = new Matrix2 {
                E00 = 0,
                E01 = 1,
                E10 = 2,
                E11 = 3
            };
            Assert.AreEqual(_incremented, m);
        }

        [Test]
        public void DeterminantTest() {
            Assert.AreEqual(1, _identity.CalculateDeterminant());
            Assert.AreEqual(-2, _incremented.CalculateDeterminant());
            Assert.AreEqual(34, _determinant34.CalculateDeterminant());
            Assert.AreEqual(-2, _priorInvert.CalculateDeterminant());
            Assert.AreEqual(-0.5, _postInvert.CalculateDeterminant());
            Assert.AreEqual(-2, _trans.CalculateDeterminant());
            Assert.AreEqual(4, _a.CalculateDeterminant());
            Assert.AreEqual(22, _b.CalculateDeterminant());
            Assert.AreEqual(88, _product.CalculateDeterminant());
            Assert.AreEqual(71, _sum.CalculateDeterminant());
        }

        [Test]
        public void InvertTest() {
            var a = new Matrix2(_priorInvert);
            a.Invert();
            Assert.AreEqual(_postInvert, a);
        }

        [Test]
        public void GetInvertedSafetyTest() {
            var a = new Matrix2(_priorInvert);
            var b = a.GetInverse();
            Assert.AreEqual(_priorInvert, a, "Matrix mutated.");
            Assert.AreEqual(_postInvert, b);
        }

        [Test]
        public void GetInvertedTests() {
            Assert.AreEqual(_postInvert, _priorInvert.GetInverse());
            Assert.AreEqual(_priorInvert, _postInvert.GetInverse());
            Assert.AreEqual(_identity, _identity.GetInverse());
            Assert.AreEqual(new Matrix2(-3 / 2.0, 0.5, 1, 0), _incremented.GetInverse());
            Assert.AreEqual(new Matrix2(-1 / 17.0, 4/17.0, -5/34.0, 3/34.0), _determinant34.GetInverse());
            Assert.AreEqual(new Matrix2(-3 / 2.0, 1, 0.5, 0), _trans.GetInverse());
            Assert.AreEqual(new Matrix2(-0.5, -0.25, 5 / 2.0, 0.75), _a.GetInverse());
            Assert.AreEqual(new Matrix2(5 / 22.0, -3 / 22.0, 2 / 11.0, 1 / 11.0), _b.GetInverse());
            Assert.AreEqual(new Matrix2(-5 / 11.0, -7 / 44.0, 3 / 22.0, 1 / 44.0), _product.GetInverse());
            Assert.AreEqual(new Matrix2(3 / 71.0, -4 / 71.0, 14 / 71.0, 5 / 71.0), _sum.GetInverse());
        }

        [Test]
        public void TransposeTest() {
            var a = new Matrix2(_incremented);
            a.Transpose();
            Assert.AreEqual(_trans, a);
        }

        [Test]
        public void GetTransposedTest() {
            var a = new Matrix2(_incremented);
            var b = a.GetTransposed();
            Assert.AreEqual(_incremented, a, "Matrix mutated.");
            Assert.AreEqual(_trans, b);
        }

        [Test]
        public void MultiplyTest() {
            var a = new Matrix2(_a);
            var b = new Matrix2(_b);
            Assert.AreEqual(_product, a.Multiply(b));
            Assert.AreEqual(_a, a);
            Assert.AreEqual(_b, b);
        }

        [Test]
        public void MultiplyOpTest() {
            var a = new Matrix2(_a);
            var b = new Matrix2(_b);
            Assert.AreEqual(_product, a * b);
            Assert.AreEqual(_a, a);
            Assert.AreEqual(_b, b);
        }

        [Test]
        public void MultiplyAssignmentTest() {
            var d = new Matrix2(_a);
            d.MultiplyAssign(_b);
            Assert.AreEqual(_product, d);
        }

        [Test]
        public void MultiplyAssignmentOpTest() {
            var d = new Matrix2(_a);
            d *= _b;
            Assert.AreEqual(_product, d);
        }

        [Test]
        public void EqualsObjectTest() {
            var a = (object)new Matrix2(_a);
            var b = (object)new Matrix2(_b);
            var c = (object)new Matrix2(_b);
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
            var a = new Matrix2(_a);
            var b = new Matrix2(_b);
            var c = new Matrix2(_b);
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
            var a = new Matrix2(_a);
            var a2 = a;
            var b = new Matrix2(_b);
            var b2 = b;
            var c = new Matrix2(_b);
            var d = (Matrix2)null;
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
            var a = new Matrix2(_a);
            var a2 = a;
            var b = new Matrix2(_b);
            var b2 = b;
            var c = new Matrix2(_b);
            var d = (Matrix2)null;
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
        public void AddTest() {
            Assert.AreEqual(_sum, _a.Add(_b));
            Assert.AreEqual(_sum, _b.Add(_a));
        }

        [Test]
        public void OpAddTest() {
            Assert.AreEqual(_sum, _a + _b);
            Assert.AreEqual(_sum, _b + _a);
        }

        [Test]
        public void AddAssignmentTest() {
            var d = new Matrix2(_a);
            d.AddAssign(_b);
            Assert.AreEqual(_sum, d);
        }

        [Test]
        public void AddAssignmentOpTest() {
            var d = new Matrix2(_a);
            d += _b;
            Assert.AreEqual(_sum, d);
        }

        [Test]
        public void IsDiagonalTest() {
            Assert.IsFalse(_incremented.IsDiagonal);
            Assert.IsTrue(new Matrix2(1, 0, 0, 1).IsDiagonal);
            Assert.IsFalse(new Matrix2(1, 0, 1, 1).IsDiagonal);
            Assert.IsFalse(new Matrix2(1, 1, 0, 1).IsDiagonal);
        }

        [Test]
        public void IsScalarTest() {
            Assert.IsFalse(_incremented.IsScalar);
            Assert.IsTrue(_identity.IsScalar);
            Assert.IsTrue(new Matrix2(2, 0, 0, 2).IsScalar);
            Assert.IsFalse(new Matrix2(2, 0, 0, 1).IsScalar);
            Assert.IsFalse(new Matrix2(1, 0, 1, 1).IsScalar);
            Assert.IsFalse(new Matrix2(1, 1, 0, 1).IsScalar);
        }

        [Test]
        public void IsIdentityTest() {
            Assert.IsFalse(_incremented.IsIdentity);
            Assert.IsTrue(_identity.IsIdentity);
            Assert.IsFalse(new Matrix2(2, 0, 0, 2).IsIdentity);
            Assert.IsFalse(new Matrix2(1, 0, 1, 1).IsIdentity);
            Assert.IsFalse(new Matrix2(1, 1, 0, 1).IsIdentity);
        }

        [Test]
        public void IsUpperTriangularTest() {
            Assert.IsFalse(_incremented.IsUpperTriangular);
            Assert.IsTrue(_identity.IsUpperTriangular);
            Assert.IsTrue(new Matrix2(2, 0, 0, 3).IsUpperTriangular);
            Assert.IsTrue(new Matrix2(2, 0, 0, 2).IsUpperTriangular);
            Assert.IsFalse(new Matrix2(4, 0, 5, 6).IsUpperTriangular);
            Assert.IsTrue(new Matrix2(7, 8, 0, 9).IsUpperTriangular);
        }

        [Test]
        public void IsLowerTriangularTest() {
            Assert.IsFalse(_incremented.IsLowerTriangular);
            Assert.IsTrue(_identity.IsLowerTriangular);
            Assert.IsTrue(new Matrix2(2, 0, 0, 3).IsLowerTriangular);
            Assert.IsTrue(new Matrix2(2, 0, 0, 2).IsLowerTriangular);
            Assert.IsTrue(new Matrix2(4, 0, 5, 6).IsLowerTriangular);
            Assert.IsFalse(new Matrix2(7, 8, 0, 9).IsLowerTriangular);
        }

    }
}

#pragma warning restore 1591