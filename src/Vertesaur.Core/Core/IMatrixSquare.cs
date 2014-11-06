using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur
{
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
        /// The order should be equal to the number of rows and columns.
        /// </remarks>
        int Order { get; }
        /// <summary>
        /// Calculates the determinant value of this matrix.
        /// </summary>
        /// <returns>The determinant value.</returns>
        TValue CalculateDeterminant();
        /// <summary>
        /// Calculates the product of the diagonal values in this matrix.
        /// </summary>
        /// <returns>The product of the diagonal elements.</returns>
        TValue CalculateDiagonalProduct();
        /// <summary>
        /// Determines if this matrix is a diagonal matrix.
        /// </summary>
        bool IsDiagonal { get; }
        /// <summary>
        /// Determines if this matrix is a scalar matrix.
        /// </summary>
        bool IsScalar { get; }
        /// <summary>
        /// Determines if this matrix is equivalent to the identity matrix.
        /// </summary>
        bool IsIdentity { get; }
        /// <summary>
        /// Determines if this matrix is an upper triangular matrix where all elements below and left of the diagonal are zero.
        /// </summary>
        bool IsUpperTriangular { get; }
        /// <summary>
        /// Determines if this matrix is a lower triangular matrix where all elements above and right of the diagonal are zero.
        /// </summary>
        bool IsLowerTriangular { get; }

    }

    [ContractClassFor(typeof(IMatrixSquare<>))]
    internal abstract class CodeContractIMatrixSquare<TValue> : IMatrixSquare<TValue>
    {

        private CodeContractIMatrixSquare() { }

        public int Order {
            get {
                Contract.Ensures(Contract.Result<int>() == RowCount);
                Contract.Ensures(Contract.Result<int>() == ColumnCount);
                Contract.EndContractBlock();
                throw new System.NotImplementedException();
            }
        }

        public abstract TValue CalculateDeterminant();

        public abstract TValue CalculateDiagonalProduct();

        public abstract bool IsDiagonal { get; }

        public abstract bool IsScalar { get; }

        public abstract bool IsIdentity { get; }

        public abstract bool IsUpperTriangular { get; }

        public abstract bool IsLowerTriangular { get; }

        public abstract int RowCount { get; }

        public abstract int ColumnCount { get; }

        public abstract int ElementCount { get; }

        public abstract TValue Get(int r, int c);

    }

}
