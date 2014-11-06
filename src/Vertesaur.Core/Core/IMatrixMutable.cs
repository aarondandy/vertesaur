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

        /// <summary>
        /// Swaps the rows specified by the given row indices <paramref name="ra"/> and <paramref name="rb"/>.
        /// </summary>
        /// <param name="ra">A row index to be swapped.</param>
        /// <param name="rb">The other row index to be swapped.</param>
        void SwapRows(int ra, int rb);

        /// <summary>
        /// Swaps the columns specified by the given column indices <paramref name="ca"/> and <paramref name="cb"/>.
        /// </summary>
        /// <param name="ca">A column index to be swapped.</param>
        /// <param name="cb">The other column index to be swapped.</param>
        void SwapColumns(int ca, int cb);

        /// <summary>
        /// Increments the element values in the <paramref name="targetRow"/> using corresponding values in the <paramref name="sourceRow"/>.
        /// </summary>
        /// <param name="sourceRow">The row to read values from.</param>
        /// <param name="targetRow">The row to increment.</param>
        void AddSourceRowToTarget(int sourceRow, int targetRow);

        /// <summary>
        /// Increments the element values in the <paramref name="targetRow"/> using corresponding values in the <paramref name="sourceRow"/>, scaled by a <paramref name="factor"/>.
        /// </summary>
        /// <param name="sourceRow">The row to read values from.</param>
        /// <param name="targetRow">The row to increment.</param>
        /// <param name="factor">The value to scale by before adding to the target.</param>
        void AddSourceRowToTarget(int sourceRow, int targetRow, TValue factor);

        /// <summary>
        /// Increments the element values in the <paramref name="targetColumn"/> using corresponding values in the <paramref name="sourceColumn"/>.
        /// </summary>
        /// <param name="sourceColumn">The column to read values from.</param>
        /// <param name="targetColumn">The column to increment.</param>
        void AddSourceColumnToTarget(int sourceColumn, int targetColumn);

        /// <summary>
        /// Increments the element values in the <paramref name="targetColumn"/> using corresponding values in the <paramref name="sourceColumn"/>, scaled by a <paramref name="factor"/>.
        /// </summary>
        /// <param name="sourceColumn">The column to read values from.</param>
        /// <param name="targetColumn">The column to increment.</param>
        /// <param name="factor">The value to scale by before adding to the target.</param>
        void AddSourceColumnToTarget(int sourceColumn, int targetColumn, TValue factor);

        /// <summary>
        /// Scales all elements in a row by a factor <paramref name="value"/>.
        /// </summary>
        /// <param name="r">The row to scale.</param>
        /// <param name="value">The value to scale the elements by.</param>
        void ScaleRow(int r, TValue value);

        /// <summary>
        /// Scales all elements in a column by a factor <paramref name="value"/>.
        /// </summary>
        /// <param name="c">The column to scale.</param>
        /// <param name="value">The value to scale the elements by.</param>
        void ScaleColumn(int c, TValue value);

        /// <summary>
        /// Divides all elements in a row by a <paramref name="denominator"/> value.
        /// </summary>
        /// <param name="r">The row to divide.</param>
        /// <param name="denominator">The value to divide the elements by.</param>
        void DivideRow(int r, TValue denominator);

        /// <summary>
        /// Divides all elements in a column by a <paramref name="denominator"/> value.
        /// </summary>
        /// <param name="c">The column to divide.</param>
        /// <param name="denominator">The value to divide the elements by.</param>
        void DivideColumn(int c, TValue denominator);

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

        public void DivideRow(int r, TValue denominator) {
            Contract.Requires(r >= 0);
            Contract.Requires(r < RowCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }

        public void DivideColumn(int c, TValue denominator) {
            Contract.Requires(c >= 0);
            Contract.Requires(c < ColumnCount);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }
    }

}
