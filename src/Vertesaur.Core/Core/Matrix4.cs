using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Vertesaur.Utility;

namespace Vertesaur
{

    // ReSharper disable CompareOfFloatsByEqualityOperator

    /// <summary>
    /// A square matrix with four rows and four columns with an element type of double.
    /// </summary>
    public sealed class Matrix4 :
        IMatrixSquare<double>,
        IMatrixMutable<double>,
        IEquatable<Matrix4>,
        ICloneable
    {

        private const int OrderValue = 4;
        private const int ElementCountValue = OrderValue * OrderValue;

        /// <summary>
        /// Determines if two values are equal.
        /// </summary>
        /// <param name="left">A matrix.</param>
        /// <param name="right">A matrix.</param>
        /// <returns>True when both matrices are equal.</returns>
        public static bool operator ==(Matrix4 left, Matrix4 right) {
            return (ReferenceEquals(null, left) ? ReferenceEquals(null, right) : left.Equals(right));
        }

        /// <summary>
        /// Determines if two values are not equal.
        /// </summary>
        /// <param name="left">A matrix.</param>
        /// <param name="right">A matrix.</param>
        /// <returns>True when both matrices are not equal.</returns>
        public static bool operator !=(Matrix4 left, Matrix4 right) {
            return (ReferenceEquals(null, left) ? !ReferenceEquals(null, right) : !left.Equals(right));
        }

        /// <summary>
        /// Multiplies two matrices and returns a result.
        /// </summary>
        /// <param name="left">Left matrix.</param>
        /// <param name="right">Right matrix.</param>
        /// <returns>Returns the product of two matrices.</returns>
        public static Matrix4 operator *(Matrix4 left, Matrix4 right) {
            if (null == left) throw new ArgumentNullException("left");
            if (null == right) throw new ArgumentNullException("right");
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return left.Multiply(right);
        }

        /// <summary>
        /// Multiplies a matrix by a factor value.
        /// </summary>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <param name="factor">The factor value to multiply the matrix by.</param>
        /// <returns>A matrix with all elements scaled.</returns>
        public static Matrix4 operator *(Matrix4 matrix, double factor) {
            if (null == matrix) throw new ArgumentNullException("matrix");
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return matrix.Multiply(factor);
        }

        /// <summary>
        /// Multiplies a matrix by a factor value.
        /// </summary>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <param name="factor">The factor value to multiply the matrix by.</param>
        /// <returns>A matrix with all elements scaled.</returns>
        public static Matrix4 operator *(double factor, Matrix4 matrix) {
            if (null == matrix) throw new ArgumentNullException("matrix");
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return matrix.Multiply(factor);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left matrix.</param>
        /// <param name="right">The right matrix.</param>
        /// <returns>The resulting matrix.</returns>
        public static Matrix4 operator +(Matrix4 left, Matrix4 right) {
            if (null == left) throw new ArgumentNullException("left");
            if (null == right) throw new ArgumentNullException("right");
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return left.Add(right);
        }

        /// <summary>
        /// Creates a matrix with all elements set to 0.
        /// </summary>
        /// <returns>A matrix of zeros.</returns>
        public static Matrix4 CreateZero() {
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return new Matrix4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// The element at row 0 and column 0.
        /// </summary>
        public double E00 { get { return _e[0]; } }
        /// <summary>
        /// The element at row 0 and column 1.
        /// </summary>
        public double E01 { get { return _e[1]; } }
        /// <summary>
        /// The element at row 0 and column 2.
        /// </summary>
        public double E02 { get { return _e[2]; } }
        /// <summary>
        /// The element at row 0 and column 3.
        /// </summary>
        public double E03 { get { return _e[3]; } }
        /// <summary>
        /// The element at row 1 and column 0.
        /// </summary>
        public double E10 { get { return _e[4]; } }
        /// <summary>
        /// The element at row 1 and column 1.
        /// </summary>
        public double E11 { get { return _e[5]; } }
        /// <summary>
        /// The element at row 1 and column 2.
        /// </summary>
        public double E12 { get { return _e[6]; } }
        /// <summary>
        /// The element at row 1 and column 3.
        /// </summary>
        public double E13 { get { return _e[7]; } }
        /// <summary>
        /// The element at row 2 and column 0.
        /// </summary>
        public double E20 { get { return _e[8]; } }
        /// <summary>
        /// The element at row 2 and column 1.
        /// </summary>
        public double E21 { get { return _e[9]; } }
        /// <summary>
        /// The element at row 2 and column 2.
        /// </summary>
        public double E22 { get { return _e[10]; } }
        /// <summary>
        /// The element at row 2 and column 3.
        /// </summary>
        public double E23 { get { return _e[11]; } }
        /// <summary>
        /// The element at row 3 and column 0.
        /// </summary>
        public double E30 { get { return _e[12]; } }
        /// <summary>
        /// The element at row 3 and column 1.
        /// </summary>
        public double E31 { get { return _e[13]; } }
        /// <summary>
        /// The element at row 3 and column 2.
        /// </summary>
        public double E32 { get { return _e[14]; } }
        /// <summary>
        /// The element at row 3 and column 3.
        /// </summary>
        public double E33 { get { return _e[15]; } }

        private double[] _e;

        /// <summary>
        /// Constructs a new identity matrix.
        /// </summary>
        public Matrix4() {
            _e = new double[ElementCountValue];
            SetIdentity();
        }

        /// <summary>
        /// Constructs a new matrix with the given element values.
        /// </summary>
        /// <param name="e00">The value for the element at 0,0.</param>
        /// <param name="e01">The value for the element at 0,1.</param>
        /// <param name="e02">The value for the element at 0,2.</param>
        /// <param name="e03">The value for the element at 0,3.</param>
        /// <param name="e10">The value for the element at 1,0.</param>
        /// <param name="e11">The value for the element at 1,1.</param>
        /// <param name="e12">The value for the element at 1,2.</param>
        /// <param name="e13">The value for the element at 1,3.</param>
        /// <param name="e20">The value for the element at 2,0.</param>
        /// <param name="e21">The value for the element at 2,1.</param>
        /// <param name="e22">The value for the element at 2,2.</param>
        /// <param name="e23">The value for the element at 2,3.</param>
        /// <param name="e30">The value for the element at 3,0.</param>
        /// <param name="e31">The value for the element at 3,1.</param>
        /// <param name="e32">The value for the element at 3,2.</param>
        /// <param name="e33">The value for the element at 3,3.</param>
        public Matrix4(
            double e00, double e01, double e02, double e03,
            double e10, double e11, double e12, double e13,
            double e20, double e21, double e22, double e23,
            double e30, double e31, double e32, double e33
        ) {
            _e = new double[ElementCountValue];
            _e[ 0] = e00;
            _e[ 1] = e01;
            _e[ 2] = e02;
            _e[ 3] = e03;
            _e[ 4] = e10;
            _e[ 5] = e11;
            _e[ 6] = e12;
            _e[ 7] = e13;
            _e[ 8] = e20;
            _e[ 9] = e21;
            _e[10] = e22;
            _e[11] = e23;
            _e[12] = e30;
            _e[13] = e31;
            _e[14] = e32;
            _e[15] = e33;
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        /// <summary>
        /// Copies the element values from the given matrix.
        /// </summary>
        /// <param name="m">A matrix to copy from.</param>
        public Matrix4(Matrix4 m) {
            if (m == null) throw new ArgumentNullException("m");
            Contract.EndContractBlock();
            _e = new double[ElementCountValue];
            CopyFrom(m);
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        /// <summary>
        /// Copies the element values from the given matrix.
        /// </summary>
        /// <param name="m">A matrix to copy from.</param>
        public Matrix4(IMatrix<double> m) {
            if (m == null) throw new ArgumentNullException("m");
            if (m.RowCount != OrderValue) throw new ArgumentException("Matrix must have 4 rows", "m");
            if (m.ColumnCount != OrderValue) throw new ArgumentException("Matrix must have 4 columns.", "m");
            Contract.Requires(m.RowCount == OrderValue);
            Contract.Requires(m.ColumnCount == OrderValue);
            _e = new double[ElementCountValue];
            for (int r = 0; r < RowCount; r++) {
                var rowOffset = r * OrderValue;
                for (int c = 0; c < ColumnCount; c++)
                    _e[rowOffset + c] = m.Get(r, c);
            }
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        /// <summary>
        /// Constructs a new diagonal matrix.
        /// </summary>
        /// <param name="e00">The value for the element at 0,0.</param>
        /// <param name="e11">The value for the element at 1,1.</param>
        /// <param name="e22">The value for the element at 2,2.</param>
        /// <param name="e33">The value for the element at 3,3.</param>
        public Matrix4(double e00, double e11, double e22, double e33) {
            _e = new double[ElementCountValue];
            _e[ 0] = e00;
            _e[ 5] = e11;
            _e[10] = e22;
            _e[15] = e33;
            _e[ 1] = _e[ 2] = _e[ 3] = 0.0;
            _e[ 4] = _e[ 6] = _e[ 7] = 0.0;
            _e[ 8] = _e[ 9] = _e[11] = 0.0;
            _e[12] = _e[13] = _e[14] = 0.0;
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(4 == OrderValue);
            Contract.Invariant(OrderValue == RowCount);
            Contract.Invariant(OrderValue == ColumnCount);
        }

        /// <inheritdoc/>
        public bool Equals(Matrix4 other) {
            return !ReferenceEquals(null, other)
                && _e[ 0] == other._e[ 0]
                && _e[ 1] == other._e[ 1]
                && _e[ 2] == other._e[ 2]
                && _e[ 3] == other._e[ 3]
                && _e[ 4] == other._e[ 4]
                && _e[ 5] == other._e[ 5]
                && _e[ 6] == other._e[ 6]
                && _e[ 7] == other._e[ 7]
                && _e[ 8] == other._e[ 8]
                && _e[ 9] == other._e[ 9]
                && _e[10] == other._e[10]
                && _e[11] == other._e[11]
                && _e[12] == other._e[12]
                && _e[13] == other._e[13]
                && _e[14] == other._e[14]
                && _e[15] == other._e[15]
            ;
        }

        /// <inheritdoc/>
        public bool Equals(IMatrix<double> other) {
            return !ReferenceEquals(null, other)
                && OrderValue == other.RowCount
                && OrderValue == other.ColumnCount
                && _e[ 0] == other.Get(0, 0)
                && _e[ 1] == other.Get(0, 1)
                && _e[ 2] == other.Get(0, 2)
                && _e[ 3] == other.Get(0, 3)
                && _e[ 4] == other.Get(1, 0)
                && _e[ 5] == other.Get(1, 1)
                && _e[ 6] == other.Get(1, 2)
                && _e[ 7] == other.Get(1, 3)
                && _e[ 8] == other.Get(2, 0)
                && _e[ 9] == other.Get(2, 1)
                && _e[10] == other.Get(2, 2)
                && _e[11] == other.Get(2, 3)
                && _e[12] == other.Get(3, 0)
                && _e[13] == other.Get(3, 1)
                && _e[14] == other.Get(3, 2)
                && _e[15] == other.Get(3, 3)
            ;
        }

        private void CopyFrom(Matrix4 m) {
            Contract.Requires(m != null);
            _e = (double[])(m._e.Clone());
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            // ReSharper disable NonReadonlyFieldInGetHashCode
            return E00.GetHashCode() ^ OrderValue;
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return String.Concat(
                E00, ' ', E01, ' ', E02, ' ', E03, "\n",
                E10, ' ', E11, ' ', E12, ' ', E13, "\n",
                E20, ' ', E21, ' ', E22, ' ', E23, "\n",
                E30, ' ', E31, ' ', E32, ' ', E33
            );
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            return null != obj && (
                (obj is Matrix4 && Equals(obj as Matrix4))
                || (obj is IMatrix<double> && Equals(obj as IMatrix<double>))
            );
        }

        /// <summary>
        /// Sets all elements in the matrix.
        /// </summary>
        /// <param name="e00">The value for the element at 0,0.</param>
        /// <param name="e01">The value for the element at 0,1.</param>
        /// <param name="e02">The value for the element at 0,2.</param>
        /// <param name="e03">The value for the element at 0,3.</param>
        /// <param name="e10">The value for the element at 1,0.</param>
        /// <param name="e11">The value for the element at 1,1.</param>
        /// <param name="e12">The value for the element at 1,2.</param>
        /// <param name="e13">The value for the element at 1,3.</param>
        /// <param name="e20">The value for the element at 2,0.</param>
        /// <param name="e21">The value for the element at 2,1.</param>
        /// <param name="e22">The value for the element at 2,2.</param>
        /// <param name="e23">The value for the element at 2,3.</param>
        /// <param name="e30">The value for the element at 3,0.</param>
        /// <param name="e31">The value for the element at 3,1.</param>
        /// <param name="e32">The value for the element at 3,2.</param>
        /// <param name="e33">The value for the element at 3,3.</param>
        public void SetElements(
            double e00, double e01, double e02, double e03,
            double e10, double e11, double e12, double e13,
            double e20, double e21, double e22, double e23,
            double e30, double e31, double e32, double e33
        ) {
            Contract.Ensures(RowCount == 4);
            Contract.Ensures(ColumnCount == 4);
            _e[ 0] = e00;
            _e[ 1] = e01;
            _e[ 2] = e02;
            _e[ 3] = e03;
            _e[ 4] = e10;
            _e[ 5] = e11;
            _e[ 6] = e12;
            _e[ 7] = e13;
            _e[ 8] = e20;
            _e[ 9] = e21;
            _e[10] = e22;
            _e[11] = e23;
            _e[12] = e30;
            _e[13] = e31;
            _e[14] = e32;
            _e[15] = e33;
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        /// <summary>
        /// Sets the elements of this matrix to that of the identity matrix.
        /// </summary>
        public void SetIdentity() {
            _e[ 0] = _e[ 5] = _e[10] = _e[15] = 1.0;
            _e[ 1] = _e[ 2] = _e[ 3] = 0.0;
            _e[ 4] = _e[ 6] = _e[ 7] = 0.0;
            _e[ 8] = _e[ 9] = _e[11] = 0.0;
            _e[12] = _e[13] = _e[14] = 0.0;
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        /// <summary>
        /// Retrieves the element at the given row and column location.
        /// </summary>
        /// <param name="r">The row.</param>
        /// <param name="c">The column.</param>
        /// <returns>The element at the given location.</returns>
        public double Get(int r, int c) {
            if (c < 0 || c >= OrderValue) throw new ArgumentOutOfRangeException("c", "Invalid column.");
            if (r < 0 || r >= OrderValue) throw new ArgumentOutOfRangeException("r", "Invalid row.");
            Contract.EndContractBlock();
            return _e[(r * OrderValue) + c];
        }

        /// <summary>
        /// Sets the element at the given row and column to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="r">The row.</param>
        /// <param name="c">The column.</param>
        /// <param name="value">The value to store in the matrix element.</param>
        public void Set(int r, int c, double value) {
            if (c < 0 || c >= OrderValue) throw new ArgumentOutOfRangeException("c", "Invalid column.");
            if (r < 0 || r >= OrderValue) throw new ArgumentOutOfRangeException("r", "Invalid row.");
            Contract.EndContractBlock();
            _e[(r * OrderValue) + c] = value;
        }

        /// <inheritdoc/>
        public double CalculateDiagonalProduct() {
            return _e[0] * _e[5] * _e[10] * _e[15];
        }

        /// <summary>
        /// Calculates the determinant of the matrix.
        /// </summary>
        /// <returns>The determinant.</returns>
        public double CalculateDeterminant() {
            return SquareMatrixOperations.CalculateDeterminantDestructive(Clone());
        }

        /// <summary>
        /// Inverts this matrix.
        /// </summary>
        /// <exception cref="Vertesaur.NoInverseException">An inverse requires a valid non-zero finite determinant.</exception>
        public void Invert() {
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            var result = new Matrix4();
            Contract.Assume(result.IsIdentity);
            if (SquareMatrixOperations.GaussJordanEliminationDestructive(Clone(), result))
                CopyFrom(result);
            else
                throw new NoInverseException();
        }

        /// <summary>
        /// Generates a matrix which is the inverse.
        /// </summary>
        /// <returns>The inverse of the matrix.</returns>
        /// <exception cref="Vertesaur.NoInverseException">An inverse requires a valid non-zero finite determinant.</exception>
        public Matrix4 GetInverse() {
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            var result = new Matrix4();
            Contract.Assume(result.IsIdentity);
            if (SquareMatrixOperations.GaussJordanEliminationDestructive(Clone(), result))
                return result;
            else
                throw new NoInverseException();
        }

        /// <summary>
        /// Transposes this matrix.
        /// </summary>
        public void Transpose() {
            NumberUtility.Swap(ref _e[ 1],ref _e[ 4]);
            NumberUtility.Swap(ref _e[ 2],ref _e[ 8]);
            NumberUtility.Swap(ref _e[ 3],ref _e[12]);
            NumberUtility.Swap(ref _e[ 6],ref _e[ 9]);
            NumberUtility.Swap(ref _e[ 7],ref _e[13]);
            NumberUtility.Swap(ref _e[11],ref _e[14]);
        }

        /// <summary>
        /// Generates a new matrix which is the transpose of this matrix.
        /// </summary>
        /// <returns>The transpose of this matrix.</returns>
        public Matrix4 GetTransposed() {
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return new Matrix4(
                E00, E10, E20, E30,
                E01, E11, E21, E31,
                E02, E12, E22, E32,
                E03, E13, E23, E33
            );
        }

        /// <summary>
        /// Multiplies all elements of this matrix by a single factor, returning the result as a new matrix.
        /// </summary>
        /// <param name="factor">The factor to multiply by.</param>
        /// <returns>The scaled matrix.</returns>
        public Matrix4 Multiply(double factor) {
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return new Matrix4(
                _e[ 0] * factor,
                _e[ 1] * factor,
                _e[ 2] * factor,
                _e[ 3] * factor,
                _e[ 4] * factor,
                _e[ 5] * factor,
                _e[ 6] * factor,
                _e[ 7] * factor,
                _e[ 8] * factor,
                _e[ 9] * factor,
                _e[10] * factor,
                _e[11] * factor,
                _e[12] * factor,
                _e[13] * factor,
                _e[14] * factor,
                _e[15] * factor
            );
        }

        /// <summary>
        /// Multiplies all elements of this matrix by a single factor, assigning the result to this matrix.
        /// </summary>
        /// <param name="factor">The factor to multiply by.</param>
        public void MultiplyAssign(double factor) {
            _e[ 0] *= factor;
            _e[ 1] *= factor;
            _e[ 2] *= factor;
            _e[ 3] *= factor;
            _e[ 4] *= factor;
            _e[ 5] *= factor;
            _e[ 6] *= factor;
            _e[ 7] *= factor;
            _e[ 8] *= factor;
            _e[ 9] *= factor;
            _e[10] *= factor;
            _e[11] *= factor;
            _e[12] *= factor;
            _e[13] *= factor;
            _e[14] *= factor;
            _e[15] *= factor;
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        /// <summary>
        /// Multiplies this left matrix by the given <paramref name="right"/> matrix and returns the product.
        /// </summary>
        /// <param name="right">The right matrix to multiply by.</param>
        /// <returns>A product of this matrix multiplied by the given <paramref name="right"/> matrix.</returns>
        public Matrix4 Multiply(Matrix4 right) {
            if (null == right) throw new ArgumentNullException("right");
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return new Matrix4(
                ((_e[ 0] * right._e[ 0]) + (_e[ 1] * right._e[ 4]) + (_e[ 2] * right._e[ 8]) + (_e[ 3] * right._e[12])),
                ((_e[ 0] * right._e[ 1]) + (_e[ 1] * right._e[ 5]) + (_e[ 2] * right._e[ 9]) + (_e[ 3] * right._e[13])),
                ((_e[ 0] * right._e[ 2]) + (_e[ 1] * right._e[ 6]) + (_e[ 2] * right._e[10]) + (_e[ 3] * right._e[14])),
                ((_e[ 0] * right._e[ 3]) + (_e[ 1] * right._e[ 7]) + (_e[ 2] * right._e[11]) + (_e[ 3] * right._e[15])),
                ((_e[ 4] * right._e[ 0]) + (_e[ 5] * right._e[ 4]) + (_e[ 6] * right._e[ 8]) + (_e[ 7] * right._e[12])),
                ((_e[ 4] * right._e[ 1]) + (_e[ 5] * right._e[ 5]) + (_e[ 6] * right._e[ 9]) + (_e[ 7] * right._e[13])),
                ((_e[ 4] * right._e[ 2]) + (_e[ 5] * right._e[ 6]) + (_e[ 6] * right._e[10]) + (_e[ 7] * right._e[14])),
                ((_e[ 4] * right._e[ 3]) + (_e[ 5] * right._e[ 7]) + (_e[ 6] * right._e[11]) + (_e[ 7] * right._e[15])),
                ((_e[ 8] * right._e[ 0]) + (_e[ 9] * right._e[ 4]) + (_e[10] * right._e[ 8]) + (_e[11] * right._e[12])),
                ((_e[ 8] * right._e[ 1]) + (_e[ 9] * right._e[ 5]) + (_e[10] * right._e[ 9]) + (_e[11] * right._e[13])),
                ((_e[ 8] * right._e[ 2]) + (_e[ 9] * right._e[ 6]) + (_e[10] * right._e[10]) + (_e[11] * right._e[14])),
                ((_e[ 8] * right._e[ 3]) + (_e[ 9] * right._e[ 7]) + (_e[10] * right._e[11]) + (_e[11] * right._e[15])),
                ((_e[12] * right._e[ 0]) + (_e[13] * right._e[ 4]) + (_e[14] * right._e[ 8]) + (_e[15] * right._e[12])),
                ((_e[12] * right._e[ 1]) + (_e[13] * right._e[ 5]) + (_e[14] * right._e[ 9]) + (_e[15] * right._e[13])),
                ((_e[12] * right._e[ 2]) + (_e[13] * right._e[ 6]) + (_e[14] * right._e[10]) + (_e[15] * right._e[14])),
                ((_e[12] * right._e[ 3]) + (_e[13] * right._e[ 7]) + (_e[14] * right._e[11]) + (_e[15] * right._e[15]))
            );
        }

        /// <summary>
        /// Multiplies this left matrix by the given <paramref name="right"/> matrix and overwrites this matrix with the product.
        /// </summary>
        /// <param name="right">The right matrix to multiply by.</param>
        public void MultiplyAssign(Matrix4 right) {
            if (null == right) throw new ArgumentNullException("right");
            Contract.EndContractBlock();
            SetElements(
                ((_e[ 0] * right._e[ 0]) + (_e[ 1] * right._e[ 4]) + (_e[ 2] * right._e[ 8]) + (_e[ 3] * right._e[12])),
                ((_e[ 0] * right._e[ 1]) + (_e[ 1] * right._e[ 5]) + (_e[ 2] * right._e[ 9]) + (_e[ 3] * right._e[13])),
                ((_e[ 0] * right._e[ 2]) + (_e[ 1] * right._e[ 6]) + (_e[ 2] * right._e[10]) + (_e[ 3] * right._e[14])),
                ((_e[ 0] * right._e[ 3]) + (_e[ 1] * right._e[ 7]) + (_e[ 2] * right._e[11]) + (_e[ 3] * right._e[15])),
                ((_e[ 4] * right._e[ 0]) + (_e[ 5] * right._e[ 4]) + (_e[ 6] * right._e[ 8]) + (_e[ 7] * right._e[12])),
                ((_e[ 4] * right._e[ 1]) + (_e[ 5] * right._e[ 5]) + (_e[ 6] * right._e[ 9]) + (_e[ 7] * right._e[13])),
                ((_e[ 4] * right._e[ 2]) + (_e[ 5] * right._e[ 6]) + (_e[ 6] * right._e[10]) + (_e[ 7] * right._e[14])),
                ((_e[ 4] * right._e[ 3]) + (_e[ 5] * right._e[ 7]) + (_e[ 6] * right._e[11]) + (_e[ 7] * right._e[15])),
                ((_e[ 8] * right._e[ 0]) + (_e[ 9] * right._e[ 4]) + (_e[10] * right._e[ 8]) + (_e[11] * right._e[12])),
                ((_e[ 8] * right._e[ 1]) + (_e[ 9] * right._e[ 5]) + (_e[10] * right._e[ 9]) + (_e[11] * right._e[13])),
                ((_e[ 8] * right._e[ 2]) + (_e[ 9] * right._e[ 6]) + (_e[10] * right._e[10]) + (_e[11] * right._e[14])),
                ((_e[ 8] * right._e[ 3]) + (_e[ 9] * right._e[ 7]) + (_e[10] * right._e[11]) + (_e[11] * right._e[15])),
                ((_e[12] * right._e[ 0]) + (_e[13] * right._e[ 4]) + (_e[14] * right._e[ 8]) + (_e[15] * right._e[12])),
                ((_e[12] * right._e[ 1]) + (_e[13] * right._e[ 5]) + (_e[14] * right._e[ 9]) + (_e[15] * right._e[13])),
                ((_e[12] * right._e[ 2]) + (_e[13] * right._e[ 6]) + (_e[14] * right._e[10]) + (_e[15] * right._e[14])),
                ((_e[12] * right._e[ 3]) + (_e[13] * right._e[ 7]) + (_e[14] * right._e[11]) + (_e[15] * right._e[15]))
            );
        }

        /// <summary>
        /// Adds this left matrix and the given <paramref name="right"/> matrix and returns the result.
        /// </summary>
        /// <param name="right">The right matrix to add.</param>
        /// <returns>The result.</returns>
        public Matrix4 Add(Matrix4 right) {
            if (null == right) throw new ArgumentNullException("right");
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return new Matrix4(
                _e[ 0] + right._e[ 0],
                _e[ 1] + right._e[ 1],
                _e[ 2] + right._e[ 2],
                _e[ 3] + right._e[ 3],
                _e[ 4] + right._e[ 4],
                _e[ 5] + right._e[ 5],
                _e[ 6] + right._e[ 6],
                _e[ 7] + right._e[ 7],
                _e[ 8] + right._e[ 8],
                _e[ 9] + right._e[ 9],
                _e[10] + right._e[10],
                _e[11] + right._e[11],
                _e[12] + right._e[12],
                _e[13] + right._e[13],
                _e[14] + right._e[14],
                _e[15] + right._e[15]
            );
        }

        /// <summary>
        /// Adds this left matrix by the given <paramref name="right"/> matrix and overwrites this matrix with the sum.
        /// </summary>
        /// <param name="right">The right matrix to add.</param>
        public void AddAssign(Matrix4 right) {
            if (null == right) throw new ArgumentNullException("right");
            Contract.EndContractBlock();
            _e[ 0] += right._e[ 0];
            _e[ 1] += right._e[ 1];
            _e[ 2] += right._e[ 2];
            _e[ 3] += right._e[ 3];
            _e[ 4] += right._e[ 4];
            _e[ 5] += right._e[ 5];
            _e[ 6] += right._e[ 6];
            _e[ 7] += right._e[ 7];
            _e[ 8] += right._e[ 8];
            _e[ 9] += right._e[ 9];
            _e[10] += right._e[10];
            _e[11] += right._e[11];
            _e[12] += right._e[12];
            _e[13] += right._e[13];
            _e[14] += right._e[14];
            _e[15] += right._e[15];
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        /// <summary>
        /// Subtracts the given <paramref name="right"/> matrix from this left matrix and returns the result.
        /// </summary>
        /// <param name="right">The right matrix to subtract.</param>
        /// <returns>The result.</returns>
        public Matrix4 Subtract(Matrix4 right) {
            if (null == right) throw new ArgumentNullException("right");
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return new Matrix4(
                _e[ 0] - right._e[ 0],
                _e[ 1] - right._e[ 1],
                _e[ 2] - right._e[ 2],
                _e[ 3] - right._e[ 3],
                _e[ 4] - right._e[ 4],
                _e[ 5] - right._e[ 5],
                _e[ 6] - right._e[ 6],
                _e[ 7] - right._e[ 7],
                _e[ 8] - right._e[ 8],
                _e[ 9] - right._e[ 9],
                _e[10] - right._e[10],
                _e[11] - right._e[11],
                _e[12] - right._e[12],
                _e[13] - right._e[13],
                _e[14] - right._e[14],
                _e[15] - right._e[15]
            );
        }

        /// <inheritdoc/>
        public int Order {
            get { return OrderValue; }
        }

        /// <inheritdoc/>
        public int RowCount {
            get { return OrderValue; }
        }

        /// <inheritdoc/>
        public int ColumnCount {
            get { return OrderValue; }
        }

        /// <inheritdoc/>
        int IMatrix<double>.ElementCount {
            get { return ElementCountValue; }
        }

        /// <inheritdoc/>
        public bool IsDiagonal {
            get { return IsUpperTriangular && IsLowerTriangular; }
        }

        /// <inheritdoc/>
        public bool IsScalar {
            get { return _e[0] == _e[5] && _e[5] == _e[10] && _e[10] == _e[15] && IsDiagonal; }
        }

        /// <inheritdoc/>
        public bool IsIdentity {
            get { return _e[0] == 1.0 && _e[5] == 1.0 && _e[10] == 1.0 && _e[15] == 1.0 && IsDiagonal; }
        }

        /// <inheritdoc/>
        public bool IsUpperTriangular {
            get {
                return _e[ 4] == 0.0
                    && _e[ 8] == 0.0 && _e[ 9] == 0.0
                    && _e[12] == 0.0 && _e[13] == 0.0 && _e[14] == 0.0;
            }
        }

        /// <inheritdoc/>
        public bool IsLowerTriangular {
            get {
                return _e[ 1] == 0.0 && _e[ 2] == 0.0 && _e[ 3] == 0.0
                                     && _e[ 6] == 0.0 && _e[ 7] == 0.0
                                                      && _e[11] == 0.0;
            }
        }

        /// <summary>
        /// Creates a new 4x4 matrix with the same elements.
        /// </summary>
        /// <returns>A clone of this matrix.</returns>
        public Matrix4 Clone() {
            Contract.Ensures(Contract.Result<Matrix4>() != null);
            return new Matrix4(this);
        }

        object ICloneable.Clone() {
            return Clone();
        }

        /// <inheritdoc/>
        public void SwapRows(int ra, int rb) {
            if (ra < 0 || ra >= OrderValue) throw new ArgumentOutOfRangeException("ra", "Invalid row.");
            if (rb < 0 || rb >= OrderValue) throw new ArgumentOutOfRangeException("rb", "Invalid row.");
            Contract.EndContractBlock();
            if (ra == rb)
                return;
            var raOffset = ra * OrderValue;
            var rbOffset = rb * OrderValue;
            for (int c = 0; c < OrderValue; c++)
                NumberUtility.Swap(ref _e[raOffset + c], ref _e[rbOffset + c]);
        }

        /// <inheritdoc/>
        public void SwapColumns(int ca, int cb) {
            if (ca < 0 || ca >= OrderValue) throw new ArgumentOutOfRangeException("ca", "Invalid column.");
            if (cb < 0 || cb >= OrderValue) throw new ArgumentOutOfRangeException("cb", "Invalid column.");
            Contract.EndContractBlock();
            if (ca == cb)
                return;
            for (int r = 0; r < OrderValue; r++)
                NumberUtility.Swap(ref _e[(r * OrderValue) + ca], ref _e[(r * OrderValue) + cb]);
        }

        /// <inheritdoc/>
        public void AddSourceRowToTarget(int sourceRow, int targetRow) {
            if (sourceRow < 0 || sourceRow >= OrderValue) throw new ArgumentOutOfRangeException("sourceRow", "Invalid row.");
            if (targetRow < 0 || targetRow >= OrderValue) throw new ArgumentOutOfRangeException("targetRow", "Invalid row.");
            Contract.EndContractBlock();
            var targetRowOffset = targetRow * OrderValue;
            var sourceRowOffset = sourceRow * OrderValue;
            for (int c = 0; c < OrderValue; c++)
                _e[targetRowOffset + c] += _e[sourceRowOffset + c];
        }

        /// <inheritdoc/>
        public void AddSourceRowToTarget(int sourceRow, int targetRow, double factor) {
            if (sourceRow < 0 || sourceRow >= OrderValue) throw new ArgumentOutOfRangeException("sourceRow", "Invalid row.");
            if (targetRow < 0 || targetRow >= OrderValue) throw new ArgumentOutOfRangeException("targetRow", "Invalid row.");
            Contract.EndContractBlock();
            var targetRowOffset = targetRow * OrderValue;
            var sourceRowOffset = sourceRow * OrderValue;
            for (int c = 0; c < OrderValue; c++)
                _e[targetRowOffset + c] += _e[sourceRowOffset + c] * factor;
        }

        /// <inheritdoc/>
        public void AddSourceColumnToTarget(int sourceColumn, int targetColumn) {
            if (sourceColumn < 0 || sourceColumn >= OrderValue) throw new ArgumentOutOfRangeException("sourceColumn", "Invalid column.");
            if (targetColumn < 0 || targetColumn >= OrderValue) throw new ArgumentOutOfRangeException("targetColumn", "Invalid column.");
            Contract.EndContractBlock();
            for (int r = 0; r < OrderValue; r++) {
                var rowOffset = r * OrderValue;
                _e[rowOffset + targetColumn] += _e[rowOffset + sourceColumn];
            }
        }

        /// <inheritdoc/>
        public void AddSourceColumnToTarget(int sourceColumn, int targetColumn, double factor) {
            if (sourceColumn < 0 || sourceColumn >= OrderValue) throw new ArgumentOutOfRangeException("sourceColumn", "Invalid column.");
            if (targetColumn < 0 || targetColumn >= OrderValue) throw new ArgumentOutOfRangeException("targetColumn", "Invalid column.");
            Contract.EndContractBlock();
            for (int r = 0; r < OrderValue; r++) {
                var rowOffset = r * OrderValue;
                _e[rowOffset + targetColumn] += _e[rowOffset + sourceColumn] * factor;
            }
        }

        /// <inheritdoc/>
        public void ScaleRow(int r, double value) {
            Contract.Requires(r >= 0 && r < OrderValue);
            switch (r) {
                case 0: {
                    _e[0] *= value;
                    _e[1] *= value;
                    _e[2] *= value;
                    _e[3] *= value;
                    break;
                }
                case 1: {
                    _e[4] *= value;
                    _e[5] *= value;
                    _e[6] *= value;
                    _e[7] *= value;
                    break;
                }
                case 2: {
                    _e[8] *= value;
                    _e[9] *= value;
                    _e[10] *= value;
                    _e[11] *= value;
                    break;
                }
                case 3: {
                    _e[12] *= value;
                    _e[13] *= value;
                    _e[14] *= value;
                    _e[15] *= value;
                    break;
                }
                default: throw new ArgumentOutOfRangeException("r", "Invalid row.");
            }
        }

        /// <inheritdoc/>
        public void ScaleColumn(int c, double value) {
            if (c < 0 || c >= OrderValue) throw new ArgumentOutOfRangeException("c", "Invalid column.");
            Contract.EndContractBlock();
            for (int r = 0; r < OrderValue; r++)
                _e[(r * OrderValue) + c] *= value;
        }

        /// <inheritdoc/>
        public void DivideRow(int r, double denominator) {
            Contract.Requires(r >= 0 && r < OrderValue);
            switch (r) {
                case 0: {
                    _e[0] /= denominator;
                    _e[1] /= denominator;
                    _e[2] /= denominator;
                    _e[3] /= denominator;
                    break;
                }
                case 1: {
                    _e[4] /= denominator;
                    _e[5] /= denominator;
                    _e[6] /= denominator;
                    _e[7] /= denominator;
                    break;
                }
                case 2: {
                    _e[8] /= denominator;
                    _e[9] /= denominator;
                    _e[10] /= denominator;
                    _e[11] /= denominator;
                    break;
                }
                case 3: {
                    _e[12] /= denominator;
                    _e[13] /= denominator;
                    _e[14] /= denominator;
                    _e[15] /= denominator;
                    break;
                }
                default: throw new ArgumentOutOfRangeException("r", "Invalid row.");
            }
        }

        /// <inheritdoc/>
        public void DivideColumn(int c, double denominator) {
            if (c < 0 || c >= OrderValue) throw new ArgumentOutOfRangeException("c", "Invalid column.");
            Contract.EndContractBlock();
            for (int r = 0; r < OrderValue; r++)
                _e[(r * OrderValue) + c] /= denominator;
        }

    }

    // ReSharper restore CompareOfFloatsByEqualityOperator

}
