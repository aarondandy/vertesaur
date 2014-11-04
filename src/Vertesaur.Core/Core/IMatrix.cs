using System;
using System.Diagnostics.Contracts;

namespace Vertesaur
{
    /// <summary>
    /// A matrix.
    /// </summary>
    /// <typeparam name="TValue">The element type of the matrix.</typeparam>
    /// <remarks>
    /// All element locations are in a zero based row major format.
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
        /// <param name="r">The row location of the element.</param>
        /// <param name="c">The column location of the element.</param>
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

    }
}
