using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Vertesaur
{

    /// <summary>
    /// Various generalized operations that can be performed on square matrices.
    /// </summary>
    internal static class SquareMatrixOperations
    {

        internal static double CalculateDeterminantDestructive<TMatrix>(TMatrix matrix) where TMatrix : IMatrixSquare<double>, IMatrixMutable<double> {
            Contract.Requires(matrix != null);
            var negateValue = false;

            // NOTE: try to create an upper triangular matrix

            if (matrix.IsUpperTriangular || matrix.IsLowerTriangular)
                return matrix.CalculateDiagonalProduct();

            for (int i = 0; i < matrix.Order; i++) {

                var currentDiagonalValue = matrix.Get(i, i);
                if (currentDiagonalValue == 0.0) {
                    if (SwapForNonZeroRowBelow(matrix, i)) {
                        negateValue = !negateValue;
                        currentDiagonalValue = matrix.Get(i, i); // update this as it will be used to create zeros
                        Contract.Assume(currentDiagonalValue != 0.0);
                    }
                    else {
                        return 0.0; // could not find anything below to swap for
                    }
                }

                for (int zeroRow = i + 1; zeroRow < matrix.Order; zeroRow++) {
                    var targetElementValue = matrix.Get(zeroRow, i);
                    if (targetElementValue != 0.0) {
                        var factor = -targetElementValue / currentDiagonalValue;
                        matrix.AddSourceRowToTarget(i, zeroRow, factor);
                    }
                }

            }

            var determinantValue = matrix.CalculateDiagonalProduct();
            if (negateValue)
                determinantValue = -determinantValue;
            return determinantValue;
        }

        private static bool SwapForNonZeroRowBelow<TMatrix>(TMatrix matrix, int ordinal) where TMatrix : IMatrixSquare<double>, IMatrixMutable<double> {
            for (int rowSearch = ordinal + 1; rowSearch < matrix.Order; rowSearch++) {
                var searchValue = matrix.Get(rowSearch, ordinal);
                if (searchValue != 0.0) {
                    matrix.SwapRows(ordinal, rowSearch);
                    return true;
                }
            }
            return false;
        }

        internal static bool GaussJordanEliminationDestructive<TMatrix>(TMatrix sourceData, TMatrix result) where TMatrix : IMatrixSquare<double>, IMatrixMutable<double> {
            Contract.Requires(sourceData != null);
            Contract.Requires(result != null);
            Contract.Requires(sourceData.Order == result.Order);

            // TODO: Determine if this approach skips some possible solutions

            for (int i = 0; i < sourceData.Order; i++) {
                if (!DiagonalToOne(sourceData, result, i))
                    return false;

                // now the other values in the column must be made to be 0
                for (int r = 0; r < sourceData.Order; r++) {
                    if (r == i)
                        continue; // skip the row with the 1 value
                    if(!SetToGaussJordanZero(sourceData, result, r, i))
                        return false;
                }
            }
            return true;
        }

        private static bool DiagonalToOne<TMatrix>(TMatrix sourceData, TMatrix result, int ordinal) where TMatrix : IMatrixSquare<double>, IMatrixMutable<double> {
            var targetElementValue = sourceData.Get(ordinal, ordinal);
            if (targetElementValue == 1.0)
                return true;

            // attempt to find a row that already has one
            var rowIndexWithOneValue = SearchDownRowsForOneValue(sourceData, ordinal, ordinal + 1);
            if (rowIndexWithOneValue > ordinal) {
                // perform the swap to get the 1.0 value
                sourceData.SwapRows(ordinal, rowIndexWithOneValue);
                result.SwapRows(ordinal, rowIndexWithOneValue);
                return true;
            }
            
            // first attempt to find an additive value from the rows below to use with a multiple of one 1
            var rowIndexAdditiveForOne = SearchDownRowsForAdditiveForOneValue(sourceData, ordinal, ordinal + 1, targetElementValue);
            if (rowIndexAdditiveForOne > ordinal) {
                // when a match is found add those values to the row to get the 1 value
                sourceData.AddSourceRowToTarget(rowIndexAdditiveForOne, ordinal);
                result.AddSourceRowToTarget(rowIndexAdditiveForOne, ordinal);
                return true;
            }
            
            // next attempt to find a scalar value that can be applied to the row
            if (targetElementValue != 0.0) {
                sourceData.DivideRow(ordinal, targetElementValue);
                result.DivideRow(ordinal, targetElementValue);
                return true;
            }
            else {
                // if this value is a zero we can try to find one under it that is non-zero, swap them, then try again
                // this works out well because the zero is helpful down there later
                for (int r = ordinal + 1; r < sourceData.Order; r++) {
                    if (sourceData.Get(r, ordinal) != 0.0) {
                        sourceData.SwapRows(ordinal, r);
                        result.SwapRows(ordinal, r);
                        return DiagonalToOne(sourceData, result, ordinal);
                    }
                }
            }
            return false;
        }

        private static bool SetToGaussJordanZero<TMatrix>(TMatrix sourceData, TMatrix result, int targetRow, int targetColumn) where TMatrix : IMatrixSquare<double>, IMatrixMutable<double> {
            var targetElementValue = sourceData.Get(targetRow, targetColumn);
            if (targetElementValue == 0.0)
                return true;

            for (int searchRow = targetColumn; searchRow < sourceData.Order; searchRow++) {
                if (searchRow == targetRow)
                    continue; // can't use self

                var usedValue = sourceData.Get(searchRow, targetColumn);
                if (usedValue != 0.0) {
                    // find a value such that targetElementValue + (usedValue * factor) = 0
                    var factor = -targetElementValue / usedValue;
                    sourceData.AddSourceRowToTarget(searchRow, targetRow, factor);
                    result.AddSourceRowToTarget(searchRow, targetRow, factor);
                    return true;
                }
            }
            return false;
        }

        private static int SearchDownRowsForOneValue<TMatrix>(TMatrix m, int column, int rowSearchIndex) where TMatrix : IMatrix<double> {
            Contract.Requires(m != null);
            Contract.Requires(column >= 0);
            Contract.Requires(column < m.ColumnCount);
            Contract.Requires(rowSearchIndex >= 0);
            Contract.Ensures(Contract.Result<int>() < m.RowCount);

            for (; rowSearchIndex < m.RowCount; rowSearchIndex++) {
                if (m.Get(rowSearchIndex, column) == 1.0)
                    return rowSearchIndex;
            }
            return -1;
        }

        private static int SearchDownRowsForAdditiveForOneValue<TMatrix>(TMatrix m, int column, int rowSearchIndex, double additiveValue) where TMatrix : IMatrix<double> {
            Contract.Requires(m != null);
            Contract.Requires(column >= 0);
            Contract.Requires(column < m.ColumnCount);
            Contract.Requires(rowSearchIndex >= 0);
            Contract.Ensures(Contract.Result<int>() < m.RowCount);

            for (; rowSearchIndex < m.RowCount; rowSearchIndex++) {
                if (m.Get(rowSearchIndex, column) + additiveValue == 1.0)
                    return rowSearchIndex;
            }
            return -1;
        }

    }
}
