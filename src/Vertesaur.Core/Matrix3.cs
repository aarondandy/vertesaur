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
using Vertesaur.Contracts;

namespace Vertesaur {

// ReSharper disable CompareOfFloatsByEqualityOperator

	/// <summary>
	/// A square matrix with three rows and three columns with an element type of double.
	/// </summary>
	public sealed class Matrix3 :
		IMatrixSquare<double>,
		IEquatable<Matrix3>,
		ICloneable
	{

		private const int OrderValue = 3;
		private const int ElementCountValue = OrderValue * OrderValue;

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left matrix.</param>
		/// <param name="right">The right matrix.</param>
		/// <returns>True if equal</returns>
		public static bool operator ==(Matrix3 left, Matrix3 right) {
			return (ReferenceEquals(null, left) ? ReferenceEquals(null, right) : left.Equals(right));
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left matrix.</param>
		/// <param name="right">The right matrix.</param>
		/// <returns>True if not equal</returns>
		public static bool operator !=(Matrix3 left, Matrix3 right) {
			return (ReferenceEquals(null, left) ? !ReferenceEquals(null, right) : !left.Equals(right));
		}

		/// <summary>
		/// Implements the operator *.
		/// </summary>
		/// <param name="left">The left matrix.</param>
		/// <param name="right">The right matrix.</param>
		/// <returns>The result of multiplying <paramref name="left"/> and <paramref name="right"/>.</returns>
		public static Matrix3 operator *(Matrix3 left, Matrix3 right) {
			if(null == left) throw new ArgumentNullException("left");
			if(null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix3>() != null);
			Contract.EndContractBlock();
			return left.Multiply(right);
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="left">The left matrix.</param>
		/// <param name="right">The right matrix.</param>
		/// <returns>The resulting matrix.</returns>
		public static Matrix3 operator +(Matrix3 left, Matrix3 right) {
			if (null == left) throw new ArgumentNullException("left");
			if (null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix3>() != null);
			Contract.EndContractBlock();
			return left.Add(right);
		}

		/// <summary>
		/// Creates a matrix with all elements set to 0.
		/// </summary>
		/// <returns>A matrix of zeros.</returns>
		public static Matrix3 CreateZero() {
			Contract.Ensures(Contract.Result<Matrix3>() != null);
			Contract.EndContractBlock();
			return new Matrix3(0, 0, 0, 0, 0, 0, 0, 0, 0);
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
		/// Constructs a new identity matrix.
		/// </summary>
		public Matrix3() {
			SetIdentity();
		}

		/// <summary>
		/// Constructs a new matrix with the given element values.
		/// </summary>
		/// <param name="e00">The value for the element at 0,0.</param>
		/// <param name="e01">The value for the element at 0,1.</param>
		/// <param name="e02">The value for the element at 0,2.</param>
		/// <param name="e10">The value for the element at 1,0.</param>
		/// <param name="e11">The value for the element at 1,1.</param>
		/// <param name="e12">The value for the element at 1,2.</param>
		/// <param name="e20">The value for the element at 2,0.</param>
		/// <param name="e21">The value for the element at 2,1.</param>
		/// <param name="e22">The value for the element at 2,2.</param>
		public Matrix3(
			double e00, double e01, double e02,
			double e10, double e11, double e12,
			double e20, double e21, double e22
		) {
			E00 = e00;
			E01 = e01;
			E02 = e02;
			E10 = e10;
			E11 = e11;
			E12 = e12;
			E20 = e20;
			E21 = e21;
			E22 = e22;
		}

		/// <summary>
		/// Copies the element values from the given matrix.
		/// </summary>
		/// <param name="m">A matrix to copy from.</param>
		public Matrix3(Matrix3 m) {
			if(null == m) throw new ArgumentNullException("m");
			Contract.EndContractBlock();
			E00 = m.E00;
			E01 = m.E01;
			E02 = m.E02;
			E10 = m.E10;
			E11 = m.E11;
			E12 = m.E12;
			E20 = m.E20;
			E21 = m.E21;
			E22 = m.E22;
		}

		/// <summary>
		/// Copies the element values from the given matrix.
		/// </summary>
		/// <param name="m">A matrix to copy from.</param>
		public Matrix3(IMatrix<double> m) {
			if (m == null) throw new ArgumentNullException("m");
			if (m.RowCount != OrderValue) throw new ArgumentException("Matrix must have 3 rows", "m");
			if (m.ColumnCount != OrderValue) throw new ArgumentException("Matrix must have 3 columns.", "m");
			Contract.Requires(OrderValue == m.RowCount);
			Contract.Requires(OrderValue == m.ColumnCount);
			Contract.EndContractBlock();
			E00 = m.Get(0, 0);
			E01 = m.Get(0, 1);
			E02 = m.Get(0, 2);
			E10 = m.Get(1, 0);
			E11 = m.Get(1, 1);
			E12 = m.Get(1, 2);
			E20 = m.Get(2, 0);
			E21 = m.Get(2, 1);
			E22 = m.Get(2, 2);
		}

		/// <summary>
		/// Sets all elements in the matrix.
		/// </summary>
		/// <param name="e00">The value for the element at 0,0.</param>
		/// <param name="e01">The value for the element at 0,1.</param>
		/// <param name="e02">The value for the element at 0,2.</param>
		/// <param name="e10">The value for the element at 1,0.</param>
		/// <param name="e11">The value for the element at 1,1.</param>
		/// <param name="e12">The value for the element at 1,2.</param>
		/// <param name="e20">The value for the element at 2,0.</param>
		/// <param name="e21">The value for the element at 2,1.</param>
		/// <param name="e22">The value for the element at 2,2.</param>
		public void SetElements(
			double e00, double e01, double e02,
			double e10, double e11, double e12,
			double e20, double e21, double e22
		) {
			E00 = e00;
			E01 = e01;
			E02 = e02;
			E10 = e10;
			E11 = e11;
			E12 = e12;
			E20 = e20;
			E21 = e21;
			E22 = e22;
		}

		/// <summary>
		/// Constructs a new diagonal matrix.
		/// </summary>
		/// <param name="e00">The value for the element at 0,0.</param>
		/// <param name="e11">The value for the element at 1,1.</param>
		/// <param name="e22">The value for the element at 2,2.</param>
		public Matrix3(double e00, double e11, double e22) {
			E00 = e00;
			E11 = e11;
			E22 = e22;
			E01 = E02 = E12 = 0;
			E10 = E20 = E21 = 0;
		}

		/// <summary>
		/// Sets the elements of this matrix to that of the identity matrix.
		/// </summary>
		public void SetIdentity() {
			E00 = E11 = E22 = 1.0;
			E01 = E02 = 0;
			E10 = E12 = 0;
			E20 = E21 = 0;
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
				return (
					c == 2
					? E02
					: (c == 0 ? E00 : E01)
				);
			}
			if (1 == r) {
				return (
					c == 2
					? E12
					: (c == 0 ? E10 : E11)
				);
			}
			return (
				c == 2
				? E22
				: (c == 0 ? E20 : E21)
			);
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
			if (r == 0) {
				if (c == 0)
					E00 = value;
				else if (c == 1)
					E01 = value;
				else
					E02 = value;
			}
			else if (r == 1) {
				if (c == 0)
					E10 = value;
				else if (c == 1)
					E11 = value;
				else
					E12 = value;
			}
			else {
				if (c == 0)
					E20 = value;
				else if (c == 1)
					E21 = value;
				else
					E22 = value;
			}
		}

		/// <summary>
		/// Indicates whether another matrix is equal to this instance.
		/// </summary>
		/// <param name="other">A matrix to compare.</param>
		/// <returns><see langword="true"/> when the given matrix is equal to this instance.</returns>
		public bool Equals(Matrix3 other) {
			return !ReferenceEquals(null, other)
				&& E00 == other.E00
				&& E01 == other.E01
				&& E02 == other.E02
				&& E10 == other.E10
				&& E11 == other.E11
				&& E12 == other.E12
				&& E20 == other.E20
				&& E21 == other.E21
				&& E22 == other.E22
			;
		}

		/// <summary>
		/// Indicates whether another matrix is equal to this instance.
		/// </summary>
		/// <param name="other">A matrix to compare.</param>
		/// <returns><see langword="true"/> when the given matrix is equal to this instance.</returns>
		public bool Equals(IMatrix<double> other) {
			return !ReferenceEquals(null, other)
				&& OrderValue == other.RowCount
				&& OrderValue == other.ColumnCount
				&& E00 == other.Get(0, 0)
				&& E01 == other.Get(0, 1)
				&& E02 == other.Get(0, 2)
				&& E10 == other.Get(1, 0)
				&& E11 == other.Get(1, 1)
				&& E12 == other.Get(1, 2)
				&& E20 == other.Get(2, 0)
				&& E21 == other.Get(2, 1)
				&& E22 == other.Get(2, 2)
			;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj) {
			return null != obj && (
				(obj is Matrix3 && Equals(obj as Matrix3))
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
				E00, ' ', E01, ' ', E02, "\n",
				E10, ' ', E11, ' ', E12, "\n",
				E20, ' ', E21, ' ', E22
			);
		}

		/// <inheritdoc/>
		public bool IsDiagonal {
			get { return IsUpperTriangular && IsLowerTriangular; }
		}

		/// <inheritdoc/>
		public bool IsScalar {
			get { return E00 == E11 && E11 == E22 && IsDiagonal; }
		}

		/// <inheritdoc/>
		public bool IsIdentity {
			get { return 1 == E00 && 1 == E11 && 1 == E22 && IsDiagonal; }
		}

		/// <inheritdoc/>
		public bool IsUpperTriangular {
			get {
				return 0 == E10
					&& 0 == E20 && 0 == E21;
			}
		}

		/// <inheritdoc/>
		public bool IsLowerTriangular {
			get {
				return 0 == E01 && 0 == E02
								&& 0 == E12;
			}
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
		/// Calculates the determinant of the matrix.
		/// </summary>
		/// <returns>The determinant.</returns>
		public double CalculateDeterminant() {
			if (1 == E00 && 1 == E11 && 1 == E22) {
				if (E01 == E10 && E02 == E20 && E12 == E21)
					return 1 + (2 * E01 * E02 * E12) - (E01 * E01) - (E02 * E02) - (E12 * E12);
				if (E01 == -E10 && E02 == -E20 && E12 == -E21)
					return 1 + (E01 * E01) + (E02 * E02) + (E12 * E12);
			}
			return (
				(
					(E00 * E11 * E22)
					+
					(E01 * E12 * E20)
					+
					(E02 * E10 * E21)
				)
				-
				(
					(E20 * E11 * E02)
					+
					(E21 * E12 * E00)
					+
					(E22 * E10 * E01)
				)
			);
		}

		/// <summary>
		/// Inverts this matrix.
		/// </summary>
		/// <exception cref="Vertesaur.NoInverseException">An inverse requires a valid non-zero finite determinant.</exception>
		public void Invert() {
			var determinant = CalculateDeterminant();
			if (0 == determinant || Double.IsNaN(determinant) || Double.IsInfinity(determinant))
				throw new NoInverseException();

			var t00 = E00;
			E00 = ((E11 * E22) - (E21 * E12)) / determinant;
			var t01 = E01;
			E01 = ((E21 * E02) - (E01 * E22)) / determinant;
			var t02 = E02;
			E02 = ((t01 * E12) - (E11 * E02)) / determinant;
			var t10 = E10;
			E10 = ((E20 * E12) - (E10 * E22)) / determinant;
			var t11 = E11;
			E11 = ((t00 * E22) - (E20 * t02)) / determinant;
			var t20 = E20;
			E20 = ((t10 * E21) - (E20 * t11)) / determinant;
			E12 = ((t10 * t02) - (t00 * E12)) / determinant;
			E21 = ((t20 * t01) - (t00 * E21)) / determinant;
			E22 = ((t00 * t11) - (t10 * t01)) / determinant;
		}

		/// <summary>
		/// Generates a matrix which is the inverse.
		/// </summary>
		/// <returns>The inverse of the matrix.</returns>
		/// <exception cref="Vertesaur.NoInverseException">An inverse requires a valid non-zero finite determinant.</exception>
		public Matrix3 GetInverse() {
			Contract.Ensures(Contract.Result<Matrix3>() != null);
			Contract.EndContractBlock();
			var determinant = CalculateDeterminant();
			if (0 == determinant || Double.IsNaN(determinant) || Double.IsInfinity(determinant))
				throw new NoInverseException();

			return new Matrix3(
				((E11 * E22) - (E21 * E12)) / determinant,
				((E21 * E02) - (E01 * E22)) / determinant,
				((E01 * E12) - (E11 * E02)) / determinant,
				((E20 * E12) - (E10 * E22)) / determinant,
				((E00 * E22) - (E20 * E02)) / determinant,
				((E10 * E02) - (E00 * E12)) / determinant,
				((E10 * E21) - (E20 * E11)) / determinant,
				((E20 * E01) - (E00 * E21)) / determinant,
				((E00 * E11) - (E10 * E01)) / determinant
			);
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
			temp = E12;
			E12 = E21;
			E21 = temp;
		}

		/// <summary>
		/// Generates a new matrix which is the transpose of this matrix.
		/// </summary>
		/// <returns>The transpose of this matrix.</returns>
		public Matrix3 GetTransposed() {
			Contract.Ensures(Contract.Result<Matrix3>() != null);
			Contract.EndContractBlock();
			return new Matrix3(
				E00, E10, E20,
				E01, E11, E21,
				E02, E12, E22
			);
		}

		/// <summary>
		/// Multiplies this left matrix by the given <paramref name="right"/> matrix and returns the product.
		/// </summary>
		/// <param name="right">The right matrix to multiply by.</param>
		/// <returns>A product of this matrix multiplied by the given <paramref name="right"/> matrix.</returns>
		public Matrix3 Multiply(Matrix3 right) {
			if(null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix3>() != null);
			Contract.EndContractBlock();
			return new Matrix3(
				((E00 * right.E00) + (E01 * right.E10) + (E02 * right.E20)),
				((E00 * right.E01) + (E01 * right.E11) + (E02 * right.E21)),
				((E00 * right.E02) + (E01 * right.E12) + (E02 * right.E22)),
				((E10 * right.E00) + (E11 * right.E10) + (E12 * right.E20)),
				((E10 * right.E01) + (E11 * right.E11) + (E12 * right.E21)),
				((E10 * right.E02) + (E11 * right.E12) + (E12 * right.E22)),
				((E20 * right.E00) + (E21 * right.E10) + (E22 * right.E20)),
				((E20 * right.E01) + (E21 * right.E11) + (E22 * right.E21)),
				((E20 * right.E02) + (E21 * right.E12) + (E22 * right.E22))
			);
		}

		/// <summary>
		/// Multiplies this left matrix by the given <paramref name="right"/> matrix and overwrites this matrix with the product.
		/// </summary>
		/// <param name="right">The right matrix to multiply by.</param>
		public void MultiplyAssignment(Matrix3 right) {
			if (null == right) throw new ArgumentNullException("right");
			Contract.EndContractBlock();
			SetElements(
				((E00 * right.E00) + (E01 * right.E10) + (E02 * right.E20)),
				((E00 * right.E01) + (E01 * right.E11) + (E02 * right.E21)),
				((E00 * right.E02) + (E01 * right.E12) + (E02 * right.E22)),
				((E10 * right.E00) + (E11 * right.E10) + (E12 * right.E20)),
				((E10 * right.E01) + (E11 * right.E11) + (E12 * right.E21)),
				((E10 * right.E02) + (E11 * right.E12) + (E12 * right.E22)),
				((E20 * right.E00) + (E21 * right.E10) + (E22 * right.E20)),
				((E20 * right.E01) + (E21 * right.E11) + (E22 * right.E21)),
				((E20 * right.E02) + (E21 * right.E12) + (E22 * right.E22))
			);
		}

		/// <summary>
		/// Adds this left matrix and the given <paramref name="right"/> matrix and returns the result.
		/// </summary>
		/// <param name="right">The right matrix to add.</param>
		/// <returns>The result.</returns>
		public Matrix3 Add(Matrix3 right) {
			if (null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix3>() != null);
			Contract.EndContractBlock();
			return new Matrix3(
				E00 + right.E00,
				E01 + right.E01,
				E02 + right.E02,
				E10 + right.E10,
				E11 + right.E11,
				E12 + right.E12,
				E20 + right.E20,
				E21 + right.E21,
				E22 + right.E22
			);
		}

		/// <summary>
		/// Adds this left matrix by the given <paramref name="right"/> matrix and overwrites this matrix with the sum.
		/// </summary>
		/// <param name="right">The right matrix to add.</param>
		public void AddAssignment(Matrix3 right) {
			if (null == right) throw new ArgumentNullException("right");
			Contract.EndContractBlock();
			E00 += right.E00;
			E01 += right.E01;
			E02 += right.E02;
			E10 += right.E10;
			E11 += right.E11;
			E12 += right.E12;
			E20 += right.E20;
			E21 += right.E21;
			E22 += right.E22;
		}

		/// <summary>
		/// Creates a new matrix composed of the same elements.
		/// </summary>
		/// <returns>A copy of this matrix.</returns>
		public Matrix3 Clone() {
			Contract.Ensures(Contract.Result<Matrix3>() != null);
			Contract.EndContractBlock();
			return new Matrix3(this);
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
