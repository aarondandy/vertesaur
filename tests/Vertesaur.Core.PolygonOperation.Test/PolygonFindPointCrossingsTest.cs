using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.PolygonOperation.Test
{

    /// <summary>
    /// Various tests to verify that the points of intersection for two polygons are correct.
    /// </summary>
    [TestFixture]
    public class PolygonFindPointCrossingsTest
    {

        private PolygonIntersectionOperation _intersectionOperation;
        private PolyPairTestDataKeyedCollection _polyPairData;

        public PolygonFindPointCrossingsTest() {
            _polyPairData = PolyOperationTestUtility.GeneratePolyPairTestDataCollection();
        }

        protected IEnumerable<object> GenerateTestPolyCrossingsParameters() {
            return _polyPairData.Where(pp => null != pp.CrossingPoints);
        }

        [SetUp]
        public void SetUp() {
            _intersectionOperation = new PolygonIntersectionOperation();
        }

        public static bool PointsAlmostEqual(Point2 a, Point2 b) {
            if (a == b)
                return true;
            var d = a.Difference(b);
            return d.GetMagnitudeSquared() < 0.000000000000000001;
        }


        [Test]
        public void TestPolyPointCrossings([ValueSource("GenerateTestPolyCrossingsParameters")] PolyPairTestData testData) {
            if (testData.Name == "Fuzzed: 3")
                Assert.Ignore("We must test this one a different way.");

            Console.WriteLine(testData.Name);

            var result = _intersectionOperation.FindPointCrossings(testData.A, testData.B);

            Assert.IsNotNull(result);
            Console.WriteLine("{0} crossing points", result.Count);

            PolyOperationTestUtility.AssertSame(
                testData.CrossingPoints.OrderBy(p => p),
                result.Select(r => r.Point).OrderBy(p => p),
                (x, y) => Assert.That(PointsAlmostEqual(x, y), "Points not equal."));

            result = _intersectionOperation.FindPointCrossings(testData.B, testData.A);
            Assert.IsNotNull(result);

            PolyOperationTestUtility.AssertSame(
                testData.CrossingPoints.OrderBy(p => p),
                result.Select(r => r.Point).OrderBy(p => p),
                (x, y) => Assert.That(PointsAlmostEqual(x, y), "Points not equal."));
        }

        [Test, Explicit("for debug")]
        public void CascadeBoxesTest() {
            var testData = _polyPairData[RingOperationTestUtility.RingPairNameCascadeBoxes];
            TestPolyPointCrossings(testData);
        }

    }
}