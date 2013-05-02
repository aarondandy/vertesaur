using System;
using System.Collections.Generic;
using System.Linq;
using Vertesaur;
using Vertesaur.PolygonOperation;
using Vertesaur.PolygonOperation.Test;
using System.Diagnostics;

namespace IntersectionPerformanceTest
{
    class Program
    {
        static void Main(string[] args) {

            // setup
            //var data = PolyOperationTestUtility.GeneratePolyPairIntersectionTestDataCollection().ToArray();
            var data = GenerateComplexShapes(30).ToArray();
            var intersectionOp = new PolygonIntersectionOperation();

            // prep
            Prep(intersectionOp, data.First());

            // full nested test
            var spanSum = TimeSpan.Zero;
            const int numberTests = 3;
            for (int i = 1; i <= numberTests; i++) {
                Console.WriteLine("Test {0} ------------------", i);
                var localSpan = Test(intersectionOp, data);
                Console.WriteLine("Elapsed: {0}", localSpan.TotalMilliseconds);
                spanSum += localSpan;
            }

            // results
            Console.WriteLine("Test Total: {0} ms", spanSum.TotalMilliseconds);
            var averageTime = new TimeSpan(spanSum.Ticks / numberTests);
            Console.WriteLine("Test Average: {0} ms", averageTime.TotalMilliseconds);
            var totalIntersection = data.Length * data.Length;
            Console.WriteLine("Intersections: {0}", totalIntersection);
            var msPerIntersection = averageTime.TotalMilliseconds / totalIntersection;
            Console.WriteLine("Avg Intersection Time: {0} ms", msPerIntersection);
            var intersectionsPerMs = totalIntersection / averageTime.TotalMilliseconds;
            var intersectionPerSec = intersectionsPerMs * 1000;
            Console.WriteLine("Intersections per second: {0}", (int)intersectionPerSec);
            //EndPauseIfNeeded();
        }

        static void EndPauseIfNeeded() {
            if (Debugger.IsAttached) {
                Console.Write("Press the [Any] key to end...");
                Console.ReadKey();
            }
        }

        static void Prep(PolygonIntersectionOperation intersectionOperation, PolyPairTestData data) {
            var prepResult = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Console.WriteLine("Success: {0}", data.R == null ? null == prepResult : data.R.SpatiallyEqual(prepResult));
        }

        static TimeSpan Test(PolygonIntersectionOperation intersectionOperation, PolyPairTestData[] data) {
            var fullTimer = new Stopwatch();
            fullTimer.Start();
            int sum = 0;
            foreach (var aData in data) {
                var a = aData.A;
                for (int i = 0; i < data.Length; i++) {
                    var b = data[i].B;
                    var p = intersectionOperation.Intersect(a, b) as Polygon2;
                    if (null != p)
                        sum += p.Count;
                }
            }
            fullTimer.Stop();
            return fullTimer.Elapsed;
        }

        static IEnumerable<PolyPairTestData> GenerateComplexShapes(int n) {
            var testData = PolyOperationTestUtility.GeneratePolyPairIntersectionTestDataCollection();
            var rand = new Random(0);
            var cascadeData = testData["Cascade Boxes"];
            const double twoPi = Math.PI * 2.0;


            // make some boxes
            for (int i = 0; i < n; i++) {
                var dx = rand.NextDouble() - 0.5;
                var dy = rand.NextDouble() - 0.5;
                var d = new Vector2(dx, dy);
                yield return new PolyPairTestData(
                    cascadeData.Name + " #" + i,
                    new Polygon2(cascadeData.A.Select(r => new Ring2(r.Select(p => p.Add(d)), false))),
                    new Polygon2(cascadeData.B.Select(r => new Ring2(r.Select(p => p.Add(d)), false)))
                );
            }

            // make some circles
            for (int i = 0; i < n; i++) {
                var circleRingA = new Ring2(false);
                for (double theta = 0; theta <= twoPi; theta += 0.1 + (rand.NextDouble() / 10.0)) {
                    circleRingA.Add(new Point2(Math.Cos(theta), Math.Sin(theta)));
                }
                var circleRingB = new Ring2(false);
                for (double theta = 0; theta <= twoPi; theta += 0.1 + (rand.NextDouble() / 10.0)) {
                    circleRingB.Add(new Point2(Math.Cos(theta), Math.Sin(theta)));
                }
                yield return new PolyPairTestData(
                    "Odd circle #" + i,
                    new Polygon2(circleRingA),
                    new Polygon2(circleRingB)
                );
            }

            // make some stars
            for (int i = 0; i < n; i++) {
                var circleRingA = new Ring2(false);
                for (double theta = 0; theta <= twoPi; theta += 0.1 + (rand.NextDouble() / 10.0)) {
                    var rad = 0.5 + (rand.NextDouble() / 2.0);
                    circleRingA.Add(new Point2(Math.Cos(theta) * rad, Math.Sin(theta) * rad));
                }
                var circleRingB = new Ring2(false);
                for (double theta = 0; theta <= twoPi; theta += 0.1 + (rand.NextDouble() / 10.0)) {
                    var rad = 0.5 + (rand.NextDouble() / 2.0);
                    circleRingB.Add(new Point2(Math.Cos(theta) * rad, Math.Sin(theta) * rad));
                }
                yield return new PolyPairTestData(
                    "Start thing #" + i,
                    new Polygon2(circleRingA),
                    new Polygon2(circleRingB)
                );
            }

            // and add the other test items too
            foreach (var item in testData)
                yield return item;

        }

    }
}
