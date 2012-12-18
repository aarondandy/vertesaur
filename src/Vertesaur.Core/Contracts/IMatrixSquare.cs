// ===============================================================================
//
// Copyright (c) 2011,2012 Aaron Dandy 
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

using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur.Contracts {
	/// <summary>
	/// A square matrix which has the same number of rows as columns.
	/// </summary>
	/// <typeparam name="TValue">The element type of the matrix.</typeparam>
	/// <remarks>
	/// All matrix types which implement this interface must have the same number of rows as columns.
	/// </remarks>
	[ContractClass(typeof(CodeContractIMatrixSquare<>))]
	public interface IMatrixSquare<out TValue> :
		IMatrix<TValue>
	{
		/// <summary>
		/// Gets the order of the square matrix.
		/// </summary>
		/// <remarks>
		/// The order should be equal to the rows and columns of this matrix.
		/// </remarks>
		int Order { get; }
		/// <summary>
		/// Calculates the determinant value of this matrix.
		/// </summary>
		/// <returns>The determinant value.</returns>
		TValue CalculateDeterminant();
		/// <summary>
		/// Determines if this matrix is a diagonal matrix.
		/// </summary>
		bool IsDiagonal { get; }
		/// <summary>
		/// True if this matrix is a scalar matrix.
		/// </summary>
		bool IsScalar { get; }
		/// <summary>
		/// True if this matrix is equivalent to the identity matrix.
		/// </summary>
		bool IsIdentity { get; }
		/// <summary>
		/// True if this matrix is an upper triangular matrix where all elements below and left of the diagonal are 0.
		/// </summary>
		bool IsUpperTriangular { get; }
		/// <summary>
		/// True if this matrix is a lower triangular matrix where all elements above and right of the diagonal are 0.
		/// </summary>
		bool IsLowerTriangular { get; }

	}

	[ContractClassFor(typeof(IMatrixSquare<>))]
	internal abstract class CodeContractIMatrixSquare<TValue> : IMatrixSquare<TValue>
	{

		private CodeContractIMatrixSquare() { }


		[ContractInvariantMethod]
		[Conditional("CONTRACTS_FULL")]
		private void CodeContractInvariant() {
			Contract.Invariant(RowCount == ColumnCount);
			Contract.Invariant(Order == RowCount);
			Contract.Invariant(Order == ColumnCount);
		}

		public int Order {
			get {
				Contract.Ensures(Contract.Result<int>() == RowCount);
				Contract.Ensures(Contract.Result<int>() == ColumnCount);
				Contract.EndContractBlock();
				throw new System.NotImplementedException();
			}
		}

		public TValue CalculateDeterminant() {
			throw new System.NotImplementedException();
		}

		public bool IsDiagonal {
			get { throw new System.NotImplementedException(); }
		}

		public bool IsScalar {
			get { throw new System.NotImplementedException(); }
		}

		public bool IsIdentity {
			get { throw new System.NotImplementedException(); }
		}

		public bool IsUpperTriangular {
			get { throw new System.NotImplementedException(); }
		}

		public bool IsLowerTriangular {
			get { throw new System.NotImplementedException(); }
		}

		public int RowCount {
			get { throw new System.NotImplementedException(); }
		}

		public int ColumnCount {
			get { throw new System.NotImplementedException(); }
		}

		public int ElementCount {
			get { throw new System.NotImplementedException(); }
		}

		public TValue Get(int r, int c) {
			throw new System.NotImplementedException();
		}

	}

}
