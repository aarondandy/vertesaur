using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Vertesaur
{

    /// <summary>
    /// A matrix with mutable elements.
    /// </summary>
    /// <typeparam name="TValue">The element type of the matrix.</typeparam>
    /// <remarks>
    /// All element locations are in a zero based row major format.
    /// </remarks>
    [ContractClass(typeof(CodeContractIMatrixMutable<>))]
    public interface IMatrixMutable<TValue> : IMatrix<TValue>
    {

        /// <summary>
        /// Sets the element at the given row and column to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="r">The row.</param>
        /// <param name="c">The column.</param>
        /// <param name="value">The value to store in the matrix element.</param>
        void Set(int r, int c, TValue value);

        void SwapRows(int ra, int rb);

        void SwapColumns(int ca, int cb);

        void AddSourceRowToTarget(int sourceRow, int targetRow);

        void AddSourceRowToTarget(int sourceRow, int targetRow, TValue factor);

        void AddSourceColumnToTarget(int sourceColumn, int targetColumn);

        void AddSourceColumnToTarget(int sourceColumn, int targetColumn, TValue factor);

        void ScaleRow(int r, TValue value);

        void ScaleColumn(int c, TValue value);

    }

    [ContractClassFor(typeof(IMatrixMutable<>))]
    internal abstract class CodeContractIMatrixMutable<TValue> : IMatrixMutable<TValue>
    {

        public abstract int RowCount { get; }

        public abstract int ColumnCount { get; }

        public abstract int ElementCount { get; }

        public abstract TValue Get(int r, int c);

        public void Set(int r, int c, TValue value) {
            Contract.Requires(r >= 0);
            Contract.Requires(r < RowCount);
            Contract.Requires(c >= 0);
            Contract.Requires(c < ColumnCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        public void SwapRows(int ra, int rb) {
            Contract.Requires(ra >= 0);
            Contract.Requires(ra < RowCount);
            Contract.Requires(rb >= 0);
            Contract.Requires(rb < RowCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        public void SwapColumns(int ca, int cb) {
            Contract.Requires(ca >= 0);
            Contract.Requires(ca < ColumnCount);
            Contract.Requires(cb >= 0);
            Contract.Requires(cb < ColumnCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        public void AddSourceRowToTarget(int sourceRow, int targetRow) {
            Contract.Requires(sourceRow >= 0);
            Contract.Requires(sourceRow < RowCount);
            Contract.Requires(targetRow >= 0);
            Contract.Requires(targetRow < RowCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        public void AddSourceRowToTarget(int sourceRow, int targetRow, TValue factor) {
            Contract.Requires(sourceRow >= 0);
            Contract.Requires(sourceRow < RowCount);
            Contract.Requires(targetRow >= 0);
            Contract.Requires(targetRow < RowCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        public void AddSourceColumnToTarget(int sourceColumn, int targetColumn) {
            Contract.Requires(sourceColumn >= 0);
            Contract.Requires(sourceColumn < ColumnCount);
            Contract.Requires(targetColumn >= 0);
            Contract.Requires(targetColumn < ColumnCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        public void AddSourceColumnToTarget(int sourceColumn, int targetColumn, TValue factor) {
            Contract.Requires(sourceColumn >= 0);
            Contract.Requires(sourceColumn < ColumnCount);
            Contract.Requires(targetColumn >= 0);
            Contract.Requires(targetColumn < ColumnCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }
        
        public void ScaleRow(int r, TValue value) {
            Contract.Requires(r >= 0);
            Contract.Requires(r < RowCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        public void ScaleColumn(int c, TValue value) {
            Contract.Requires(c >= 0);
            Contract.Requires(c < ColumnCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }
    }

}
