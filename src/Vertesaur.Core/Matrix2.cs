// ===============================================================================
//
// Copyright (c) 2011 Aaron Dandy 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// ===============================================================================

using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using Vertesaur.Contracts;

namespace Vertesaur {

// ReSharper disable CompareOfFloatsByEqualityOperator

	/// <summary>
	/// A square matrix with two rows and two columns with an element type of double.
	/// </summary>
	public sealed class Matrix2 :
		IMatrixSquare<double>,
		IEquatable<Matrix2>,
		ICloneable
	{

		private const int OrderValue = 2;
		private const int ElementCountValue = OrderValue * OrderValue;

		/// <summary>
		/// Determines if two matrices are equal.
		/// </summary>
		/// <param name="left">A matrix.</param>
		/// <param name="right">A matrix.</param>
		/// <returns>True when equal.</returns>
		[ContractAnnotation("left:null,right:null=>true; left:notnull,right:null=>false; left:null,right:notnull=>false")]
		public static bool operator ==(Matrix2 left, Matrix2 right) {
			return ReferenceEquals(null, left) ? ReferenceEquals(null, right) : left.Equals(right);
		}

		/// <summary>
		/// Determines if two matrices are not equal.
		/// </summary>
		/// <param name="left">A matrix.</param>
		/// <param name="right">A matrix.</param>
		/// <returns>True when not equal.</returns>
		[ContractAnnotation("left:null,right:null=>false; left:notnull,right:null=>true; left:null,right:notnull=>true")]
		public static bool operator !=(Matrix2 left, Matrix2 right) {
			return !(ReferenceEquals(null, left) ? ReferenceEquals(null, right) : left.Equals(right));
		}

		/// <summary>
		/// Implements the operator *.
		/// </summary>
		/// <param name="left">The left matrix.</param>
		/// <param name="right">The right matrix.</param>
		/// <returns>The result of multiplying the <paramref name="left"/> matrix by the <paramref name="right"/> matrix.</returns>
		[NotNull]
		public static Matrix2 operator *([NotNull] Matrix2 left, [NotNull] Matrix2 right) {
			if(null == left) throw new ArgumentNullException("left");
			if(null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix2>() != null);
			Contract.EndContractBlock();
			return left.Multiply(right);
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="left">The left matrix.</param>
		/// <param name="right">The right matrix.</param>
		/// <returns>The resulting matrix.</returns>
		[NotNull]
		public static Matrix2 operator +([NotNull] Matrix2 left, [NotNull] Matrix2 right) {
			if (null == left) throw new ArgumentNullException("left");
			if (null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix2>() != null);
			Contract.EndContractBlock();
			return left.Add(right);
		}

		/// <summary>
		/// Creates a matrix with all elements set to 0.
		/// </summary>
		/// <returns>A matrix of zeros.</returns>
		[NotNull]
		public static Matrix2 CreateZero() {
			Contract.Ensures(Contract.Result<Matrix2>() != null);
			Contract.EndContractBlock();
			return new Matrix2(0,0,0,0);
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
		/// The element at row 1 and column 0.
		/// </summary>
		public double E10;
		/// <summary>
		/// The element at row 1 and column 1.
		/// </summary>
		public double E11;

		/// <summary>
		/// Constructs a new identity matrix.
		/// </summary>
		public Matrix2() {
			SetIdentity();
		}

		/// <summary>
		/// Constructs a new matrix with the given element values.
		/// </summary>
		/// <param name="e00">The value for the element at 0,0.</param>
		/// <param name="e01">The value for the element at 0,1.</param>
		/// <param name="e10">The value for the element at 1,0.</param>
		/// <param name="e11">The value for the element at 1,1.</param>
		public Matrix2(
			double e00, double e01,
			double e10, double e11
		) {
			E00 = e00;
			E01 = e01;
			E10 = e10;
			E11 = e11;
		}

		/// <summary>
		/// Copies the element values from the given matrix.
		/// </summary>
		/// <param name="m">A matrix to copy from.</param>
		public Matrix2([NotNull] Matrix2 m) {
			if(m == null) throw new ArgumentNullException("m");
			Contract.EndContractBlock();

			E00 = m.E00;
			E01 = m.E01;
			E10 = m.E10;
			E11 = m.E11;
		}

		/// <summary>
		/// Copies the element values from the given matrix.
		/// </summary>
		/// <param name="m">A matrix to copy from.</param>
		public Matrix2([NotNull] IMatrix<double> m) {
			if(m == null) throw new ArgumentNullException("m");
			if(m.RowCount != OrderValue) throw new ArgumentException("Matrix must have 2 rows","m");
			if(m.ColumnCount != OrderValue) throw new ArgumentException("Matrix must have 2 columns.", "m");
			Contract.EndContractBlock();

			E00 = m.Get(0, 0);
			E01 = m.Get(0, 1);
			E10 = m.Get(1, 0);
			E11 = m.Get(1, 1);
		}

		/// <summary>
		/// Sets all elements in the matrix.
		/// </summary>
		/// <param name="e00">The value for the element at 0,0.</param>
		/// <param name="e01">The value for the element at 0,1.</param>
		/// <param name="e10">The value for the element at 1,0.</param>
		/// <param name="e11">The value for the element at 1,1.</param>
		public void SetElements(
			double e00, double e01,
			double e10, double e11
		) {
			E00 = e00;
			E01 = e01;
			E10 = e10;
			E11 = e11;
		}

		/// <summary>
		/// Sets the elements of the matrix to that of the identity matrix.
		/// </summary>
		public void SetIdentity() {
			E00 = E11 = 1.0;
			E01 = E10 = 0;
		}

		/// <summary>
		/// Sets the element at the given row and column to the given <paramref name="value"/>.
		/// </summary>
		/// <param name="r">The row.</param>
		/// <param name="c">The column.</param>
		/// <param name="value">The value to store in the matrix element.</param>
		public void Set(int r, int c, double value) {
			if(c < 0 || c > 1) throw new ArgumentOutOfRangeException("c", "Invalid column.");
			if(r < 0 || r > 1) throw new ArgumentOutOfRangeException("r", "Invalid row.");
			Contract.EndContractBlock();

			if (r == 0) {
				if (c == 0)
					E00 = value;
				else 
					E01 = value;
			}
			else {
				if (c == 0)
					E10 = value;
				else
					E11 = value;
			}
		}

		/// <summary>
		/// Retrieves the element at the given row and column location.
		/// </summary>
		/// <param name="r">The row.</param>
		/// <param name="c">The column.</param>
		/// <returns>The element at the given location.</returns>
		public double Get(int r, int c) {
			if (c < 0 || c > 1) throw new ArgumentOutOfRangeException("c", "Invalid column.");
			if (r < 0 || r > 1) throw new ArgumentOutOfRangeException("r", "Invalid row.");
			Contract.EndContractBlock();

			return c == 0
				? (0 == r ? E00 : E10)
				: (0 == r ? E01 : E11);
		}

		/// <summary>
		/// Indicates whether another matrix is equal to this instance.
		/// </summary>
		/// <param name="other">A matrix to compare.</param>
		/// <returns><see langword="true"/> when the given matrix is equal to this instance.</returns>
		[ContractAnnotation("null=>false")]
		public bool Equals([CanBeNull] Matrix2 other) {
			return !ReferenceEquals(null, other)
				&& E00 == other.E00
				&& E01 == other.E01
				&& E10 == other.E10
				&& E11 == other.E11
			;
		}

		/// <summary>
		/// Indicates whether another matrix is equal to this instance.
		/// </summary>
		/// <param name="other">A matrix to compare.</param>
		/// <returns><see langword="true"/> when the given matrix is equal to this instance.</returns>
		[ContractAnnotation("null=>false")]
		public bool Equals([CanBeNull] IMatrix<double> other) {
			return !ReferenceEquals(null, other)
				&& other.RowCount == OrderValue
				&& other.ColumnCount == OrderValue
				&& other.Get(0, 0) == E00
				&& other.Get(0, 1) == E01
				&& other.Get(1, 0) == E10
				&& other.Get(1, 1) == E11;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		[ContractAnnotation("null=>false")]
		public override bool Equals([CanBeNull] object obj) {
			return null != obj && (
				(obj is Matrix2 && Equals(obj as Matrix2))
				||
				(obj is IMatrix<double> && Equals(obj as IMatrix<double>))
			);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode() {
// ReSharper disable NonReadonlyFieldInGetHashCode
			return E00.GetHashCode() ^ E11.GetHashCode() ^ -OrderValue;
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
				E00, ' ', E01, "\n",
				E10, ' ', E11
			);
		}

		/// <summary>
		/// Calculates the determinant of the matrix.
		/// </summary>
		/// <returns>The determinant.</returns>
		public double CalculateDeterminant() {
			return (E00 * E11) - (E10 * E01);
		}

		/// <inheritdoc/>
		public bool IsDiagonal {
			get { return 0 == E10 && 0 == E01; }
		}

		/// <inheritdoc/>
		public bool IsScalar {
			get { return E00 == E11 && IsDiagonal; }
		}

		/// <inheritdoc/>
		public bool IsIdentity {
			get { return 1 == E00 && 1 == E11 && IsDiagonal; }
		}

		/// <inheritdoc/>
		public bool IsUpperTriangular {
			get { return 0 == E10; }
		}

		/// <inheritdoc/>
		public bool IsLowerTriangular {
			get { return 0 == E01; }
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

		/// <summary>
		/// Adds this left matrix and the given <paramref name="right"/> matrix and returns the result.
		/// </summary>
		/// <param name="right">The right matrix to add.</param>
		/// <returns>The result.</returns>
		[NotNull]
		public Matrix2 Add([NotNull] Matrix2 right) {
			Contract.Requires(right != null);
			Contract.Ensures(Contract.Result<Matrix2>() != null);
			Contract.EndContractBlock();
			return new Matrix2(
				E00 + right.E00,
				E01 + right.E01,
				E10 + right.E10,
				E11 + right.E11
			);
		}

		/// <summary>
		/// Adds this left matrix by the given <paramref name="right"/> matrix and overwrites this matrix with the sum.
		/// </summary>
		/// <param name="right">The right matrix to add.</param>
		public void AddAssignment([NotNull] Matrix2 right) {
			Contract.Requires(right != null);
			Contract.EndContractBlock();
			E00 += right.E00;
			E01 += right.E01;
			E10 += right.E10;
			E11 += right.E11;
		}

		/// <summary>
		/// Subtracts the given <paramref name="right"/> matrix from this left matrix and returns the result.
		/// </summary>
		/// <param name="right">The right matrix to subtract.</param>
		/// <returns>The result.</returns>
		[NotNull]
		public Matrix2 Subtract([NotNull]Matrix2 right) {
			Contract.Requires(right != null);
			Contract.Ensures(Contract.Result<Matrix2>() != null);
			Contract.EndContractBlock();
			return new Matrix2(
				E00 - right.E00,
				E01 - right.E01,
				E10 - right.E10,
				E11 - right.E11
			);
		}

		/// <summary>
		/// Subtracts the elements of the <paramref name="right"/> matrix from this matrix and assigns the results back to this matrix.
		/// </summary>
		/// <param name="right">The right matrix use to subtract from this matrix.</param>
		public void SubtractAssignment([NotNull] Matrix2 right) {
			Contract.Requires(right != null);
			Contract.EndContractBlock();
			E00 -= right.E00;
			E01 -= right.E01;
			E10 -= right.E10;
			E11 -= right.E11;
		}

		/// <summary>
		/// Multiplies this matrix by another.
		/// </summary>
		/// <param name="right">The right matrix.</param>
		/// <returns>The result of multiplying this matrix by the right matrix (<c>this * <paramref name="right"/></c>).</returns>
		[NotNull]
		public Matrix2 Multiply([NotNull] Matrix2 right) {
			Contract.Requires(right != null);
			Contract.Ensures(Contract.Result<Matrix2>() != null);
			Contract.EndContractBlock();
			return new Matrix2(
				(E00 * right.E00) + (E01 * right.E10),
				(E00 * right.E01) + (E01 * right.E11),
				(E10 * right.E00) + (E11 * right.E10),
				(E10 * right.E01) + (E11 * right.E11)
			);
		}

		/// <summary>
		/// Multiplies this matrix by another and stores the result in this matrix (<c>this*=<paramref name="right"/></c>).
		/// </summary>
		/// <param name="right">The right matrix.</param>
		public void MultiplyAssignment([NotNull] Matrix2 right) {
			Contract.Requires(right != null);
			Contract.EndContractBlock();
			var t00 = E00;
			E00 = (t00 * right.E00) + (E01 * right.E10);
			E01 = (t00 * right.E01) + (E01 * right.E11);
			var t10 = E10;
			E10 = (t10 * right.E00) + (E11 * right.E10);
			E11 = (t10 * right.E01) + (E11 * right.E11);
		}

		/// <summary>
		/// Inverts the matrix.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">An inverse requires a valid non-zero determinant.</exception>
		public void Invert() {
			var determinant = CalculateDeterminant();
			if (0 == determinant || Double.IsNaN(determinant))
				throw new InvalidOperationException();

			var temp = E00;
			E00 = E11 / determinant;
			E11 = temp / determinant;
			temp = -determinant;
			E01 = E01 / temp;
			E10 = E10 / temp;
		}

		/// <summary>
		/// Generates a matrix which is the inverse.
		/// </summary>
		/// <returns>The inverse of the matrix.</returns>
		/// <exception cref="System.InvalidOperationException">An inverse requires a valid non-zero determinant.</exception>
		[NotNull]
		public Matrix2 GetInverse() {
			Contract.Ensures(Contract.Result<Matrix2>() != null);
			Contract.EndContractBlock();
			var determinant = CalculateDeterminant();
			if (0 == determinant || Double.IsNaN(determinant))
				throw new InvalidOperationException();

			return new Matrix2(
				E11 / determinant,
				E01 / -determinant,
				E10 / -determinant,
				E00 / determinant
			);
		}

		/// <summary>
		/// Transposes this matrix.
		/// </summary>
		public void Transpose() {
			var temp = E01;
			E01 = E10;
			E10 = temp;
		}

		/// <summary>
		/// Generates a new matrix which is the transpose of this matrix.
		/// </summary>
		/// <returns>The transpose of this matrix.</returns>
		[NotNull]
		public Matrix2 GetTransposed() {
			Contract.Ensures(Contract.Result<Matrix2>() != null);
			Contract.EndContractBlock();
			return new Matrix2(
				E00, E10,
				E01, E11
			);
		}

		/// <summary>
		/// Interchanges two rows.
		/// </summary>
		/// <param name="rowA">One of the row indices to interchange.</param>
		/// <param name="rowB">The other row index to interchange with.</param>
		[Obsolete("needs testing")]
		public void InterchangeRows(int rowA, int rowB) {
			if (rowA < 0 || rowA > 1)
				throw new ArgumentOutOfRangeException("rowA");
			if (rowB < 0 || rowB > 1)
				throw new ArgumentOutOfRangeException("rowB");
			Contract.EndContractBlock();
			
			if (rowA != rowB) {
				var temp0 = E00;
				var temp1 = E01;
				E00 = E10;
				E01 = E11;
				E10 = temp0;
				E11 = temp1;
			}
		}

		/// <summary>
		/// Multiplies all elements in a row by a scalar.
		/// </summary>
		/// <param name="scalar">The value to multiply the row by.</param>
		/// <param name="row">The row to apply multiplication to.</param>
		[Obsolete("needs testing")]
		public void MultiplyRow(double scalar, int row) {
			if (row < 0 || row > 1)
				throw new ArgumentOutOfRangeException("row");
			Contract.EndContractBlock();

			if(row == 0) {
				E00 *= scalar;
				E01 *= scalar;
			}
			else {
				E10 *= scalar;
				E11 *= scalar;
			}
		}

		/// <summary>
		/// Creates a new matrix composed of the same elements.
		/// </summary>
		/// <returns>A copy of this matrix.</returns>
		[NotNull] public Matrix2 Clone() {
			Contract.Ensures(Contract.Result<Matrix2>() != null);
			Contract.EndContractBlock();
			return new Matrix2(this);
		}

		object ICloneable.Clone() {
			return Clone();
		}

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
		}

	}

// ReSharper restore CompareOfFloatsByEqualityOperator

}
