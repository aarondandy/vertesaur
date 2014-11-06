using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

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
        public double E00;
        /// <summary>
        /// The element at row 0 and column 1.
        /// </summary>
        public double E01;
        /// <summary>
        /// The element at row 0 and column 2.
        /// </summary>
        public double E02;
        /// <summary>
        /// The element at row 0 and column 3.
        /// </summary>
        public double E03;
        /// <summary>
        /// The element at row 1 and column 0.
        /// </summary>
        public double E10;
        /// <summary>
        /// The element at row 1 and column 1.
        /// </summary>
        public double E11;
        /// <summary>
        /// The element at row 1 and column 2.
        /// </summary>
        public double E12;
        /// <summary>
        /// The element at row 1 and column 3.
        /// </summary>
        public double E13;
        /// <summary>
        /// The element at row 2 and column 0.
        /// </summary>
        public double E20;
        /// <summary>
        /// The element at row 2 and column 1.
        /// </summary>
        public double E21;
        /// <summary>
        /// The element at row 2 and column 2.
        /// </summary>
        public double E22;
        /// <summary>
        /// The element at row 2 and column 3.
        /// </summary>
        public double E23;
        /// <summary>
        /// The element at row 3 and column 0.
        /// </summary>
        public double E30;
        /// <summary>
        /// The element at row 3 and column 1.
        /// </summary>
        public double E31;
        /// <summary>
        /// The element at row 3 and column 2.
        /// </summary>
        public double E32;
        /// <summary>
        /// The element at row 3 and column 3.
        /// </summary>
        public double E33;

        /// <summary>
        /// Constructs a new identity matrix.
        /// </summary>
        public Matrix4() {
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
            E00 = e00;
            E01 = e01;
            E02 = e02;
            E03 = e03;
            E10 = e10;
            E11 = e11;
            E12 = e12;
            E13 = e13;
            E20 = e20;
            E21 = e21;
            E22 = e22;
            E23 = e23;
            E30 = e30;
            E31 = e31;
            E32 = e32;
            E33 = e33;
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
            E00 = m.Get(0, 0);
            E01 = m.Get(0, 1);
            E02 = m.Get(0, 2);
            E03 = m.Get(0, 3);
            E10 = m.Get(1, 0);
            E11 = m.Get(1, 1);
            E12 = m.Get(1, 2);
            E13 = m.Get(1, 3);
            E20 = m.Get(2, 0);
            E21 = m.Get(2, 1);
            E22 = m.Get(2, 2);
            E23 = m.Get(2, 3);
            E30 = m.Get(3, 0);
            E31 = m.Get(3, 1);
            E32 = m.Get(3, 2);
            E33 = m.Get(3, 3);
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
            E00 = e00;
            E11 = e11;
            E22 = e22;
            E33 = e33;
            E01 = E02 = E03 = 0;
            E10 = E12 = E13 = 0;
            E20 = E21 = E23 = 0;
            E30 = E31 = E32 = 0;
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
                && E00 == other.E00
                && E01 == other.E01
                && E02 == other.E02
                && E03 == other.E03
                && E10 == other.E10
                && E11 == other.E11
                && E12 == other.E12
                && E13 == other.E13
                && E20 == other.E20
                && E21 == other.E21
                && E22 == other.E22
                && E23 == other.E23
                && E30 == other.E30
                && E31 == other.E31
                && E32 == other.E32
                && E33 == other.E33
            ;
        }

        /// <inheritdoc/>
        public bool Equals(IMatrix<double> other) {
            return !ReferenceEquals(null, other)
                && OrderValue == other.RowCount
                && OrderValue == other.ColumnCount
                && E00 == other.Get(0, 0)
                && E01 == other.Get(0, 1)
                && E02 == other.Get(0, 2)
                && E03 == other.Get(0, 3)
                && E10 == other.Get(1, 0)
                && E11 == other.Get(1, 1)
                && E12 == other.Get(1, 2)
                && E13 == other.Get(1, 3)
                && E20 == other.Get(2, 0)
                && E21 == other.Get(2, 1)
                && E22 == other.Get(2, 2)
                && E23 == other.Get(2, 3)
                && E30 == other.Get(3, 0)
                && E31 == other.Get(3, 1)
                && E32 == other.Get(3, 2)
                && E33 == other.Get(3, 3)
            ;
        }

        private void CopyFrom(Matrix4 m) {
            Contract.Requires(m != null);
            E00 = m.E00;
            E01 = m.E01;
            E02 = m.E02;
            E03 = m.E03;
            E10 = m.E10;
            E11 = m.E11;
            E12 = m.E12;
            E13 = m.E13;
            E20 = m.E20;
            E21 = m.E21;
            E22 = m.E22;
            E23 = m.E23;
            E30 = m.E30;
            E31 = m.E31;
            E32 = m.E32;
            E33 = m.E33;
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
            E00 = e00;
            E01 = e01;
            E02 = e02;
            E03 = e03;
            E10 = e10;
            E11 = e11;
            E12 = e12;
            E13 = e13;
            E20 = e20;
            E21 = e21;
            E22 = e22;
            E23 = e23;
            E30 = e30;
            E31 = e31;
            E32 = e32;
            E33 = e33;
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        /// <summary>
        /// Sets the elements of this matrix to that of the identity matrix.
        /// </summary>
        public void SetIdentity() {
            E00 = E11 = E22 = E33 = 1.0;
            E01 = E02 = E03 = 0;
            E10 = E12 = E13 = 0;
            E20 = E21 = E23 = 0;
            E30 = E31 = E32 = 0;
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
            if (0 == r) {
                return c < 2
                    ? (c == 0 ? E00 : E01)
                    : (c == 2 ? E02 : E03);
            }
            if (1 == r) {
                return c < 2
                    ? (c == 0 ? E10 : E11)
                    : (c == 2 ? E12 : E13);
            }
            if (2 == r) {
                return c < 2
                    ? (c == 0 ? E20 : E21)
                    : (c == 2 ? E22 : E23);
            }
            return c < 2
                ? (c == 0 ? E30 : E31)
                : (c == 2 ? E32 : E33);
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

            if (0 == r) {
                if (0 == c)
                    E00 = value;
                else if (1 == c)
                    E01 = value;
                else if (2 == c)
                    E02 = value;
                else
                    E03 = value;
            }
            else if (1 == r) {
                if (0 == c)
                    E10 = value;
                else if (1 == c)
                    E11 = value;
                else if (2 == c)
                    E12 = value;
                else
                    E13 = value;
            }
            else if (2 == r) {
                if (0 == c)
                    E20 = value;
                else if (1 == c)
                    E21 = value;
                else if (2 == c)
                    E22 = value;
                else
                    E23 = value;
            }
            else {
                if (0 == c)
                    E30 = value;
                else if (1 == c)
                    E31 = value;
                else if (2 == c)
                    E32 = value;
                else
                    E33 = value;
            }
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
        }

        /// <inheritdoc/>
        public double CalculateDiagonalProduct() {
            return E00 * E11 * E22 * E33;
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
            var copy = Clone();
            var result = new Matrix4();
            Contract.Assume(result.IsIdentity);
            if (SquareMatrixOperations.GaussJordanEliminationDestructive(copy, result))
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
            var copy = Clone();
            var result = new Matrix4();
            Contract.Assume(result.IsIdentity);
            if (SquareMatrixOperations.GaussJordanEliminationDestructive(copy, result))
                return result;
            else
                throw new NoInverseException();
        }

        /// <summary>
        /// Transposes this matrix.
        /// </summary>
        public void Transpose() {
            var temp = E01;
            E01 = E10;
            E10 = temp;
            temp = E02;
            E02 = E20;
            E20 = temp;
            temp = E03;
            E03 = E30;
            E30 = temp;
            temp = E12;
            E12 = E21;
            E21 = temp;
            temp = E13;
            E13 = E31;
            E31 = temp;
            temp = E23;
            E23 = E32;
            E32 = temp;
            Contract.Assume(OrderValue == RowCount);
            Contract.Assume(OrderValue == ColumnCount);
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
                E00 * factor,
                E01 * factor,
                E02 * factor,
                E03 * factor,
                E10 * factor,
                E11 * factor,
                E12 * factor,
                E13 * factor,
                E20 * factor,
                E21 * factor,
                E22 * factor,
                E23 * factor,
                E30 * factor,
                E31 * factor,
                E32 * factor,
                E33 * factor
            );
        }

        /// <summary>
        /// Multiplies all elements of this matrix by a single factor, assigning the result to this matrix.
        /// </summary>
        /// <param name="factor">The factor to multiply by.</param>
        public void MultiplyAssign(double factor) {
            E00 *= factor;
            E01 *= factor;
            E02 *= factor;
            E03 *= factor;
            E10 *= factor;
            E11 *= factor;
            E12 *= factor;
            E13 *= factor;
            E20 *= factor;
            E21 *= factor;
            E22 *= factor;
            E23 *= factor;
            E30 *= factor;
            E31 *= factor;
            E32 *= factor;
            E33 *= factor;
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
                ((E00 * right.E00) + (E01 * right.E10) + (E02 * right.E20) + (E03 * right.E30)),
                ((E00 * right.E01) + (E01 * right.E11) + (E02 * right.E21) + (E03 * right.E31)),
                ((E00 * right.E02) + (E01 * right.E12) + (E02 * right.E22) + (E03 * right.E32)),
                ((E00 * right.E03) + (E01 * right.E13) + (E02 * right.E23) + (E03 * right.E33)),
                ((E10 * right.E00) + (E11 * right.E10) + (E12 * right.E20) + (E13 * right.E30)),
                ((E10 * right.E01) + (E11 * right.E11) + (E12 * right.E21) + (E13 * right.E31)),
                ((E10 * right.E02) + (E11 * right.E12) + (E12 * right.E22) + (E13 * right.E32)),
                ((E10 * right.E03) + (E11 * right.E13) + (E12 * right.E23) + (E13 * right.E33)),
                ((E20 * right.E00) + (E21 * right.E10) + (E22 * right.E20) + (E23 * right.E30)),
                ((E20 * right.E01) + (E21 * right.E11) + (E22 * right.E21) + (E23 * right.E31)),
                ((E20 * right.E02) + (E21 * right.E12) + (E22 * right.E22) + (E23 * right.E32)),
                ((E20 * right.E03) + (E21 * right.E13) + (E22 * right.E23) + (E23 * right.E33)),
                ((E30 * right.E00) + (E31 * right.E10) + (E32 * right.E20) + (E33 * right.E30)),
                ((E30 * right.E01) + (E31 * right.E11) + (E32 * right.E21) + (E33 * right.E31)),
                ((E30 * right.E02) + (E31 * right.E12) + (E32 * right.E22) + (E33 * right.E32)),
                ((E30 * right.E03) + (E31 * right.E13) + (E32 * right.E23) + (E33 * right.E33))
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
                ((E00 * right.E00) + (E01 * right.E10) + (E02 * right.E20) + (E03 * right.E30)),
                ((E00 * right.E01) + (E01 * right.E11) + (E02 * right.E21) + (E03 * right.E31)),
                ((E00 * right.E02) + (E01 * right.E12) + (E02 * right.E22) + (E03 * right.E32)),
                ((E00 * right.E03) + (E01 * right.E13) + (E02 * right.E23) + (E03 * right.E33)),
                ((E10 * right.E00) + (E11 * right.E10) + (E12 * right.E20) + (E13 * right.E30)),
                ((E10 * right.E01) + (E11 * right.E11) + (E12 * right.E21) + (E13 * right.E31)),
                ((E10 * right.E02) + (E11 * right.E12) + (E12 * right.E22) + (E13 * right.E32)),
                ((E10 * right.E03) + (E11 * right.E13) + (E12 * right.E23) + (E13 * right.E33)),
                ((E20 * right.E00) + (E21 * right.E10) + (E22 * right.E20) + (E23 * right.E30)),
                ((E20 * right.E01) + (E21 * right.E11) + (E22 * right.E21) + (E23 * right.E31)),
                ((E20 * right.E02) + (E21 * right.E12) + (E22 * right.E22) + (E23 * right.E32)),
                ((E20 * right.E03) + (E21 * right.E13) + (E22 * right.E23) + (E23 * right.E33)),
                ((E30 * right.E00) + (E31 * right.E10) + (E32 * right.E20) + (E33 * right.E30)),
                ((E30 * right.E01) + (E31 * right.E11) + (E32 * right.E21) + (E33 * right.E31)),
                ((E30 * right.E02) + (E31 * right.E12) + (E32 * right.E22) + (E33 * right.E32)),
                ((E30 * right.E03) + (E31 * right.E13) + (E32 * right.E23) + (E33 * right.E33))
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
                E00 + right.E00,
                E01 + right.E01,
                E02 + right.E02,
                E03 + right.E03,
                E10 + right.E10,
                E11 + right.E11,
                E12 + right.E12,
                E13 + right.E13,
                E20 + right.E20,
                E21 + right.E21,
                E22 + right.E22,
                E23 + right.E23,
                E30 + right.E30,
                E31 + right.E31,
                E32 + right.E32,
                E33 + right.E33
            );
        }

        /// <summary>
        /// Adds this left matrix by the given <paramref name="right"/> matrix and overwrites this matrix with the sum.
        /// </summary>
        /// <param name="right">The right matrix to add.</param>
        public void AddAssign(Matrix4 right) {
            if (null == right) throw new ArgumentNullException("right");
            Contract.EndContractBlock();
            E00 += right.E00;
            E01 += right.E01;
            E02 += right.E02;
            E03 += right.E03;
            E10 += right.E10;
            E11 += right.E11;
            E12 += right.E12;
            E13 += right.E13;
            E20 += right.E20;
            E21 += right.E21;
            E22 += right.E22;
            E23 += right.E23;
            E30 += right.E30;
            E31 += right.E31;
            E32 += right.E32;
            E33 += right.E33;
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
                E00 - right.E00,
                E01 - right.E01,
                E02 - right.E02,
                E03 - right.E03,
                E10 - right.E10,
                E11 - right.E11,
                E12 - right.E12,
                E13 - right.E13,
                E20 - right.E20,
                E21 - right.E21,
                E22 - right.E22,
                E23 - right.E23,
                E30 - right.E30,
                E31 - right.E31,
                E32 - right.E32,
                E33 - right.E33
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
            get { return E00 == E11 && E11 == E22 && E22 == E33 && IsDiagonal; }
        }

        /// <inheritdoc/>
        public bool IsIdentity {
            get { return 1 == E00 && 1 == E11 && 1 == E22 && 1 == E33 && IsDiagonal; }
        }

        /// <inheritdoc/>
        public bool IsUpperTriangular {
            get {
                return 0 == E10
                    && 0 == E20 && 0 == E21
                    && 0 == E30 && 0 == E31 && 0 == E32;
            }
        }

        /// <inheritdoc/>
        public bool IsLowerTriangular {
            get {
                return 0 == E01 && 0 == E02 && 0 == E03
                                && 0 == E12 && 0 == E13
                                            && 0 == E23;
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
            for (int c = 0; c < OrderValue; c++) {
                var tmp = Get(ra, c);
                Set(ra, c, Get(rb, c));
                Set(rb, c, tmp);
            }
        }

        /// <inheritdoc/>
        public void SwapColumns(int ca, int cb) {
            if (ca < 0 || ca >= OrderValue) throw new ArgumentOutOfRangeException("ca", "Invalid column.");
            if (cb < 0 || cb >= OrderValue) throw new ArgumentOutOfRangeException("cb", "Invalid column.");
            Contract.EndContractBlock();
            if (ca == cb)
                return;
            for (int r = 0; r < OrderValue; r++) {
                var tmp = Get(r, ca);
                Set(r, ca, Get(r, cb));
                Set(r, cb, tmp);
            }
        }

        /// <inheritdoc/>
        public void AddSourceRowToTarget(int sourceRow, int targetRow) {
            if (sourceRow < 0 || sourceRow >= OrderValue) throw new ArgumentOutOfRangeException("sourceRow", "Invalid row.");
            if (targetRow < 0 || targetRow >= OrderValue) throw new ArgumentOutOfRangeException("targetRow", "Invalid row.");
            Contract.EndContractBlock();
            for (int c = 0; c < OrderValue; c++)
                Set(targetRow, c, Get(sourceRow, c) + Get(targetRow, c));
        }

        /// <inheritdoc/>
        public void AddSourceRowToTarget(int sourceRow, int targetRow, double factor) {
            if (sourceRow < 0 || sourceRow >= OrderValue) throw new ArgumentOutOfRangeException("sourceRow", "Invalid row.");
            if (targetRow < 0 || targetRow >= OrderValue) throw new ArgumentOutOfRangeException("targetRow", "Invalid row.");
            Contract.EndContractBlock();
            for (int c = 0; c < OrderValue; c++)
                Set(targetRow, c, (Get(sourceRow, c) * factor) + Get(targetRow, c));
        }

        /// <inheritdoc/>
        public void AddSourceColumnToTarget(int sourceColumn, int targetColumn) {
            if (sourceColumn < 0 || sourceColumn >= OrderValue) throw new ArgumentOutOfRangeException("sourceColumn", "Invalid column.");
            if (targetColumn < 0 || targetColumn >= OrderValue) throw new ArgumentOutOfRangeException("targetColumn", "Invalid column.");
            Contract.EndContractBlock();
            for (int r = 0; r < OrderValue; r++)
                Set(r, targetColumn, Get(r, sourceColumn) + Get(r, targetColumn));
        }

        /// <inheritdoc/>
        public void AddSourceColumnToTarget(int sourceColumn, int targetColumn, double factor) {
            if (sourceColumn < 0 || sourceColumn >= OrderValue) throw new ArgumentOutOfRangeException("sourceColumn", "Invalid column.");
            if (targetColumn < 0 || targetColumn >= OrderValue) throw new ArgumentOutOfRangeException("targetColumn", "Invalid column.");
            Contract.EndContractBlock();
            for (int r = 0; r < OrderValue; r++)
                Set(r, targetColumn, (Get(r, sourceColumn) * factor) + Get(r, targetColumn));
        }

        /// <inheritdoc/>
        public void ScaleRow(int r, double value) {
            if (r < 0 || r >= OrderValue) throw new ArgumentOutOfRangeException("r", "Invalid row.");
            Contract.EndContractBlock();
            for (int c = 0; c < OrderValue; c++)
                Set(r, c, Get(r, c) * value);
        }

        /// <inheritdoc/>
        public void ScaleColumn(int c, double value) {
            if (c < 0 || c >= OrderValue) throw new ArgumentOutOfRangeException("c", "Invalid column.");
            Contract.EndContractBlock();
            for (int r = 0; r < OrderValue; r++)
                Set(r, c, Get(r, c) * value);
        }

        /// <inheritdoc/>
        public void DivideRow(int r, double denominator) {
            if (r < 0 || r >= OrderValue) throw new ArgumentOutOfRangeException("r", "Invalid row.");
            Contract.EndContractBlock();
            for (int c = 0; c < OrderValue; c++)
                Set(r, c, Get(r, c) / denominator);
        }

        /// <inheritdoc/>
        public void DivideColumn(int c, double denominator) {
            if (c < 0 || c >= OrderValue) throw new ArgumentOutOfRangeException("c", "Invalid column.");
            Contract.EndContractBlock();
            for (int r = 0; r < OrderValue; r++)
                Set(r, c, Get(r, c) / denominator);
        }

    }

    // ReSharper restore CompareOfFloatsByEqualityOperator

}
