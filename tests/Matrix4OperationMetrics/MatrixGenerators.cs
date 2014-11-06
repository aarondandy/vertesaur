using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertesaur;

namespace Matrix4OperationMetrics
{
    public static class MatrixGenerators
    {

        public static Matrix4[] GenerateMatrices(int n) {
            var results = new Matrix4[n];
            var rand = new Random(0);
            for (int i = 0; i < results.Length; i++) {
                results[i] = new Matrix4(
                    rand.NextDouble(), rand.NextDouble(), rand.NextDouble(), rand.NextDouble(),
                    rand.NextDouble(), rand.NextDouble(), rand.NextDouble(), rand.NextDouble(),
                    rand.NextDouble(), rand.NextDouble(), rand.NextDouble(), rand.NextDouble(),
                    rand.NextDouble(), rand.NextDouble(), rand.NextDouble(), rand.NextDouble()
                );
            }
            return results;
        }

    }
}
