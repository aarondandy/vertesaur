using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertesaur;

namespace Matrix4OperationMetrics
{
    public class MatrixInverseTest
    {

        private readonly Matrix4[] _data;

        public MatrixInverseTest(Matrix4[] data) {
            _data = data;
        }

        public SimpleResult[] ExecuteTests() {
            var results = new SimpleResult[_data.Length];
            for (int testIteration = 0; testIteration < _data.Length; testIteration++) {
                var iterationResult = new SimpleResult();
                results[testIteration] = iterationResult;
                var testMatrix = _data[testIteration];
                var determinant = testMatrix.CalculateDeterminant();
                if (determinant == 0.0) {
                    ;
                }
                else {
                    var inverseMatrix = testMatrix.GetInverse();
                    var restoredMatrix = inverseMatrix.GetInverse();
                    var errorValues = new List<double>();
                    for (int r = 0; r < testMatrix.RowCount; r++) {
                        for (int c = 0; c < testMatrix.ColumnCount; c++) {
                            var actual = testMatrix.Get(r, c);
                            if (actual == 0.0)
                                continue;

                            var observed = restoredMatrix.Get(r, c);
                            var error = Math.Abs(observed - actual) / Math.Abs(actual);
                            if (Double.IsNaN(error))
                                continue;

                            errorValues.Add(error);
                        }
                    }

                    if (errorValues.Count > 0) {
                        var min = errorValues[0];
                        var max = min;
                        var sum = min;
                        for (int i = 1; i < errorValues.Count; i++) {
                            var error = errorValues[i];
                            if (max < error)
                                max = error;
                            if (min > error)
                                min = error;
                            sum += error;
                        }
                        var avg = sum / (double)(errorValues.Count);
                        iterationResult.Min = min;
                        iterationResult.Max = max;
                        iterationResult.Avg = avg;
                    }

                }
            }
            return results;
        }


    }
}
