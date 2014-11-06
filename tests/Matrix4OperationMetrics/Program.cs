using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix4OperationMetrics
{
    class Program
    {
        static void Main(string[] args) {

            var testData = MatrixGenerators.GenerateMatrices(100000);
            var test = new MatrixInverseTest(testData);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var results = test.ExecuteTests();
            stopwatch.Stop();
            
            Console.WriteLine("Elapsed: " + stopwatch.Elapsed);
            var avg = results.Average(x => x.Avg);
            var min = results.Min(x => x.Min);
            var max = results.Max(x => x.Max);

            Console.WriteLine("Max error: " + max);
            Console.WriteLine("Min error: " + min);
            Console.WriteLine("Avg error: " + avg);

            Console.ReadKey();
        }
    }
}
