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

using System;
using System.Diagnostics.Contracts;

namespace Vertesaur.Contracts
{
    /// <summary>
    /// A matrix.
    /// </summary>
    /// <typeparam name="TValue">The element type of the matrix.</typeparam>
    /// <remarks>
    /// All element locations are in a zero based row major format!
    /// </remarks>
    [ContractClass(typeof(CodeContractIMatrix<>))]
    public interface IMatrix<out TValue>
    {

        /// <summary>
        /// The number of rows in the matrix.
        /// </summary>
        int RowCount { get; }
        /// <summary>
        /// The number of columns in the matrix.
        /// </summary>
        int ColumnCount { get; }
        /// <summary>
        /// The total number of elements in the matrix.
        /// </summary>
        int ElementCount { get; }
        /// <summary>
        /// Retrieves the element at the given row and column location.
        /// </summary>
        /// <param name="r">The row.</param>
        /// <param name="c">The column.</param>
        /// <returns>The element at the given location.</returns>
        /// <remarks>
        /// All element locations are in a zero based row major format.
        /// </remarks>
        TValue Get(int r, int c);

    }

    [ContractClassFor(typeof(IMatrix<>))]
    internal abstract class CodeContractIMatrix<TValue> : IMatrix<TValue>
    {

        public int RowCount {
            get {
                Contract.Ensures(Contract.Result<int>() >= 0);
                throw new NotImplementedException();
            }
        }

        public int ColumnCount {
            get {
                Contract.Ensures(Contract.Result<int>() >= 0);
                throw new NotImplementedException();
            }
        }

        public int ElementCount {
            get {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() == RowCount * ColumnCount);
                throw new NotImplementedException();
            }
        }

        public TValue Get(int r, int c) {
            Contract.Requires(r >= 0);
            Contract.Requires(r < RowCount);
            Contract.Requires(c >= 0);
            Contract.Requires(c < ColumnCount);
            Contract.Ensures(Contract.OldValue(RowCount) == RowCount);
            Contract.Ensures(Contract.OldValue(ColumnCount) == ColumnCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        [ContractInvariantMethod]
        private void CodeContractInvariant() {
            Contract.Invariant(RowCount > 0);
            Contract.Invariant(ColumnCount > 0);
            Contract.Invariant(ElementCount == RowCount * ColumnCount);
        }
    }
}
