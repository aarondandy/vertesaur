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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using Vertesaur.Contracts;

namespace Vertesaur {

// ReSharper disable CompareOfFloatsByEqualityOperator

	/// <summary>
	/// A square matrix with four rows and four columns with an element type of double.
	/// </summary>
	public sealed class Matrix4 :
		IMatrixSquare<double>,
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
		[ContractAnnotation("left:null,right:null=>true; left:notnull,right:null=>false; left:null,right:notnull=>false")]
		public static bool operator ==(Matrix4 left, Matrix4 right) {
			return (ReferenceEquals(null, left) ? ReferenceEquals(null, right) : left.Equals(right));
		}

		/// <summary>
		/// Determines if two values are not equal.
		/// </summary>
		/// <param name="left">A matrix.</param>
		/// <param name="right">A matrix.</param>
		/// <returns>True when both matrices are not equal.</returns>
		[ContractAnnotation("left:null,right:null=>false; left:notnull,right:null=>true; left:null,right:notnull=>true")]
		public static bool operator !=(Matrix4 left, Matrix4 right) {
			return (ReferenceEquals(null, left) ? !ReferenceEquals(null, right) : !left.Equals(right));
		}

		/// <summary>
		/// Multiplies two matrices and returns a result.
		/// </summary>
		/// <param name="left">Left matrix.</param>
		/// <param name="right">Right matrix.</param>
		/// <returns>Returns the product of two matrices.</returns>
		[NotNull]
		public static Matrix4 operator *([NotNull] Matrix4 left, [NotNull] Matrix4 right) {
			if(null == left) throw new ArgumentNullException("left");
			if(null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix4>() != null);
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
		public static Matrix4 operator +([NotNull] Matrix4 left, [NotNull] Matrix4 right) {
			if (null == left) throw new ArgumentNullException("left");
			if (null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix4>() != null);
			Contract.EndContractBlock();
			return left.Add(right);
		}

		/// <summary>
		/// Creates a matrix with all elements set to 0.
		/// </summary>
		/// <returns>A matrix of zeros.</returns>
		[NotNull]
		public static Matrix4 CreateZero() {
			Contract.Ensures(Contract.Result<Matrix4>() != null);
			Contract.EndContractBlock();
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
		}

		/// <summary>
		/// Copies the element values from the given matrix.
		/// </summary>
		/// <param name="m">A matrix to copy from.</param>
		public Matrix4([NotNull] Matrix4 m) {
			if(m == null) throw new ArgumentNullException("m");
			Contract.EndContractBlock();
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
		/// Copies the element values from the given matrix.
		/// </summary>
		/// <param name="m">A matrix to copy from.</param>
		public Matrix4([NotNull] IMatrix<double> m) {
			if (m == null) throw new ArgumentNullException("m");
			if (m.RowCount != OrderValue) throw new ArgumentException("Matrix must have 4 rows", "m");
			if (m.ColumnCount != OrderValue) throw new ArgumentException("Matrix must have 4 columns.", "m");
			Contract.EndContractBlock();
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
		}

		/// <inheritdoc/>
		[ContractAnnotation("null=>false")]
		public bool Equals([CanBeNull] Matrix4 other) {
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
		[ContractAnnotation("null=>false")]
		public bool Equals([CanBeNull] IMatrix<double> other) {
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
		[ContractAnnotation("null=>false")]
		public override bool Equals([CanBeNull] object obj) {
			return null != obj && (
				(obj is Matrix4 && Equals(obj as Matrix4))
				||
				(obj is IMatrix<double> && Equals(obj as IMatrix<double>))
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
		}

		/// <summary>
		/// Retrieves the element at the given row and column location.
		/// </summary>
		/// <param name="r">The row.</param>
		/// <param name="c">The column.</param>
		/// <returns>The element at the given location.</returns>
		public double Get(int r, int c) {
			if (c < 0 || c >= 4) throw new ArgumentOutOfRangeException("c", "Invalid column.");
			if (r < 0 || r >= 4) throw new ArgumentOutOfRangeException("r", "Invalid row.");
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
			if (c < 0 || c > 3) {
				throw new ArgumentOutOfRangeException("c");
			}
			switch (r) {
			case 0:
				switch (c) {
				case 0:
					E00 = value;
					break;
				case 1:
					E01 = value;
					break;
				case 2:
					E02 = value;
					break;
				default:
					E03 = value;
					break;
				}
				break;
			case 1:
				switch (c) {
				case 0:
					E10 = value;
					break;
				case 1:
					E11 = value;
					break;
				case 2:
					E12 = value;
					break;
				default:
					E13 = value;
					break;
				}
				break;
			case 2:
				switch (c) {
				case 0:
					E20 = value;
					break;
				case 1:
					E21 = value;
					break;
				case 2:
					E22 = value;
					break;
				default:
					E23 = value;
					break;
				}
				break;
			case 3:
				switch (c) {
				case 0:
					E30 = value;
					break;
				case 1:
					E31 = value;
					break;
				case 2:
					E32 = value;
					break;
				default:
					E33 = value;
					break;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("r");
			}
		}

		/// <summary>
		/// Calculates the determinant of the matrix.
		/// </summary>
		/// <returns>The determinant.</returns>
		public double CalculateDeterminant() {
			return CalculateDeterminantSlow();
		}
		/// <summary>
		/// This method is slow but will produce the correct result.
		/// </summary>
		private double CalculateDeterminantSlow() {
			var v = 0.0;
			if(0 != E31)
				v = E31 * SubDeterminantA();
			if (0 != E30)
				v -= E30 * SubDeterminantB();
			if (0 != E32)
				v -= E32 * SubDeterminantC();
			if (0 != E33)
				v += E33 * SubDeterminantD();
			return v;
		}

		private double SubDeterminantA() {
			// TODO: inline
			return ((E00 * E12 * E23) + (E02 * E13 * E20) + (E03 * E10 * E22))
				- ((E03 * E12 * E20) + (E00 * E13 * E22) + (E02 * E10 * E23));
		}

		private double SubDeterminantB() {
			// TODO: inline
			return ((E01 * E12 * E23) + (E02 * E13 * E21) + (E03 * E11 * E22))
				- ((E03 * E12 * E21) + (E01 * E13 * E22) + (E02 * E11 * E23));
		}

		private double SubDeterminantC() {
			// TODO: inline
			return ((E00 * E11 * E23) + (E01 * E13 * E20) + (E03 * E10 * E21))
				- ((E03 * E11 * E20) + (E00 * E13 * E21) + (E01 * E10 * E23));
		}

		private double SubDeterminantD() {
			// TODO: inline
			return ((E00 * E11 * E22) + (E01 * E12 * E20) + (E02 * E10 * E21))
				- ((E02 * E11 * E20) + (E00 * E12 * E21) + (E01 * E10 * E22));
		}

		private double SubDeterminant(int r0, int r1, int r2, int c0, int c1, int c2) {
			// TODO: inline
			CodeContractCheckRowColumnIndex(r0, c0);
			CodeContractCheckRowColumnIndex(r1, c1);
			CodeContractCheckRowColumnIndex(r2, c2);
			Contract.EndContractBlock();

			return (
				(
					(Get(r0, c0) * Get(r1, c1) * Get(r2, c2))
					+
					(Get(r0, c1) * Get(r1, c2) * Get(r2, c0))
					+
					(Get(r0, c2) * Get(r1, c0) * Get(r2, c1))
				)
				-
				(
					(Get(r2, c0) * Get(r1, c1) * Get(r0, c2))
					+
					(Get(r2, c1) * Get(r1, c2) * Get(r0, c0))
					+
					(Get(r2, c2) * Get(r1, c0) * Get(r0, c1))
				)
			);
		}

		private double SubDeterminantShort(int ir, int ic) {
			CodeContractCheckRowColumnIndex(ir, ic);
			// TODO: inline
			return SubDeterminant(
				(ir == 0) ? 1 : 0,
				(ir < 2) ? 2 : 1,
				(ir < 3) ? 3 : 2,
				(ic == 0) ? 1 : 0,
				(ic < 2) ? 2 : 1,
				(ic < 3) ? 3 : 2
			);
		}

		/// <summary>
		/// Inverts this matrix.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">An inverse requires a valid non-zero determinant.</exception>
		public void Invert() {
			var determinant = CalculateDeterminant();
			if (0 == determinant || Double.IsNaN(determinant)) {
				throw new InvalidOperationException();
			}
			var negativeDeterminant = -determinant;
			SetElements(
				(SubDeterminantShort(0, 0) / determinant),
				(SubDeterminantShort(1, 0) / negativeDeterminant),
				(SubDeterminantShort(2, 0) / determinant),
				(SubDeterminantShort(3, 0) / negativeDeterminant),

				(SubDeterminantShort(0, 1) / negativeDeterminant),
				(SubDeterminantShort(1, 1) / determinant),
				(SubDeterminantShort(2, 1) / negativeDeterminant),
				(SubDeterminantShort(3, 1) / determinant),

				(SubDeterminantShort(0, 2) / determinant),
				(SubDeterminantShort(1, 2) / negativeDeterminant),
				(SubDeterminantShort(2, 2) / determinant),
				(SubDeterminantShort(3, 2) / negativeDeterminant),

				(SubDeterminantShort(0, 3) / negativeDeterminant),
				(SubDeterminantShort(1, 3) / determinant),
				(SubDeterminantShort(2, 3) / negativeDeterminant),
				(SubDeterminantShort(3, 3) / determinant)
			);
		}

		/// <summary>
		/// Generates a matrix which is the inverse.
		/// </summary>
		/// <returns>The inverse of the matrix.</returns>
		/// <exception cref="System.InvalidOperationException">An inverse requires a valid non-zero determinant.</exception>
		[NotNull]
		public Matrix4 GetInverse() {
			Contract.Ensures(Contract.Result<Matrix4>() != null);
			Contract.EndContractBlock();

			var determinant = CalculateDeterminant();
			if (0 == determinant || Double.IsNaN(determinant)) {
				throw new InvalidOperationException();
			}
			var negativeDeterminant = -determinant;
			return new Matrix4(
				(SubDeterminantShort(0, 0) / determinant),
				(SubDeterminantShort(1, 0) / negativeDeterminant),
				(SubDeterminantShort(2, 0) / determinant),
				(SubDeterminantShort(3, 0) / negativeDeterminant),

				(SubDeterminantShort(0, 1) / negativeDeterminant),
				(SubDeterminantShort(1, 1) / determinant),
				(SubDeterminantShort(2, 1) / negativeDeterminant),
				(SubDeterminantShort(3, 1) / determinant),

				(SubDeterminantShort(0, 2) / determinant),
				(SubDeterminantShort(1, 2) / negativeDeterminant),
				(SubDeterminantShort(2, 2) / determinant),
				(SubDeterminantShort(3, 2) / negativeDeterminant),

				(SubDeterminantShort(0, 3) / negativeDeterminant),
				(SubDeterminantShort(1, 3) / determinant),
				(SubDeterminantShort(2, 3) / negativeDeterminant),
				(SubDeterminantShort(3, 3) / determinant)
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
		}

		/// <summary>
		/// Generates a new matrix which is the transpose of this matrix.
		/// </summary>
		/// <returns>The transpose of this matrix.</returns>
		[NotNull] public Matrix4 GetTransposed() {
			Contract.Ensures(Contract.Result<Matrix4>() != null);
			Contract.EndContractBlock();
			return new Matrix4(
				E00, E10, E20, E30,
				E01, E11, E21, E31,
				E02, E12, E22, E32,
				E03, E13, E23, E33
			);
		}

		/// <summary>
		/// Multiplies this left matrix by the given <paramref name="right"/> matrix and returns the product.
		/// </summary>
		/// <param name="right">The right matrix to multiply by.</param>
		/// <returns>A product of this matrix multiplied by the given <paramref name="right"/> matrix.</returns>
		[NotNull] public Matrix4 Multiply([NotNull] Matrix4 right) {
			if(null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix4>() != null);
			Contract.EndContractBlock();
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
		public void MultiplyAssignment([NotNull] Matrix4 right) {
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
		[NotNull] public Matrix4 Add([NotNull] Matrix4 right) {
			if (null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix4>() != null);
			Contract.EndContractBlock();
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
		public void AddAssignment([NotNull] Matrix4 right) {
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
		}

		/// <summary>
		/// Subtracts the given <paramref name="right"/> matrix from this left matrix and returns the result.
		/// </summary>
		/// <param name="right">The right matrix to subtract.</param>
		/// <returns>The result.</returns>
		[NotNull]
		public Matrix4 Subtract([NotNull] Matrix4 right) {
			if (null == right) throw new ArgumentNullException("right");
			Contract.Ensures(Contract.Result<Matrix4>() != null);
			Contract.EndContractBlock();
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
		public int ElementCount {
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
			[JetBrains.Annotations.Pure] get {
				return 0 == E01 && 0 == E02 && 0 == E03
								&& 0 == E12 && 0 == E13
											&& 0 == E23;
			}
		}

		/// <summary>
		/// Creates a new 4x4 matrix with the same elements.
		/// </summary>
		/// <returns>A clone of this matrix.</returns>
		[NotNull]
		public Matrix4 Clone() {
			Contract.Ensures(Contract.Result<Matrix4>() != null);
			Contract.EndContractBlock();
			return new Matrix4(this);
		}

		object ICloneable.Clone() {
			return Clone();
		}

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(RowCount == 4);
			Contract.Invariant(ColumnCount == 4);
			Contract.Invariant(OrderValue == 4);
		}

		// ReSharper disable UnusedParameter.Local
		[ContractAbbreviator]
		[Conditional("CONTRACTS_FULL")]
		private void CodeContractCheckRowColumnIndex(int r, int c) {
			Contract.Requires(r >= 0);
			Contract.Requires(r < 4);
			Contract.Requires(r < RowCount);
			Contract.Requires(c >= 0);
			Contract.Requires(c < 4);
			Contract.Requires(c < RowCount);
		}
		// ReSharper restore UnusedParameter.Local

	}

// ReSharper restore CompareOfFloatsByEqualityOperator

}
