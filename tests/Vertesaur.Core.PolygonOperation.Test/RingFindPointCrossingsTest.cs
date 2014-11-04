using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.PolygonOperation.Test
{

    /// <summary>
    /// Various tests to verify that the points of intersection for two rings are correct.
    /// </summary>
    [TestFixture]
    public class RingFindPointCrossingsTest
    {

        private PolygonIntersectionOperation _intersectionOperation;
        private RingPairTestDataKeyedCollection _ringPairData;

        public RingFindPointCrossingsTest() {
            _ringPairData = RingOperationTestUtility.GenerateRingPairTestDataCollection();
        }

        protected IEnumerable<object> GenerateTestRingCrossingsParameters() {
            return _ringPairData.Where(rp => null != rp.CrossingPoints);
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
        public void TestRingPointCrossings([ValueSource("GenerateTestRingCrossingsParameters")] RingPairTestData testData) {
            if (testData.Name == "Fuzzed: 3")
                Assert.Ignore("Must test this a different way.");

            Console.WriteLine(testData.Name);

            var result = _intersectionOperation.FindPointCrossings(new Polygon2(testData.A), new Polygon2(testData.B));
            Assert.IsNotNull(result);
            Console.WriteLine("{0} crossing points", result.Count);

            PolyOperationTestUtility.AssertSame(
                testData.CrossingPoints.OrderBy(x => x),
                result.Select(r => r.Point).OrderBy(x => x),
                (x, y) => Assert.That(PointsAlmostEqual(x, y), "Points not equal."));

            result = _intersectionOperation.FindPointCrossings(new Polygon2(testData.B), new Polygon2(testData.A));
            Assert.IsNotNull(result);

            PolyOperationTestUtility.AssertSame(
                testData.CrossingPoints.OrderBy(x => x),
                result.Select(r => r.Point).OrderBy(x => x),
                (x, y) => Assert.That(PointsAlmostEqual(x, y), "Points not equal."));
        }

        [Test, Explicit("for debug")]
        public void CascadeBoxesTest() {
            var testData = _ringPairData[RingOperationTestUtility.RingPairNameCascadeBoxes];
            TestRingPointCrossings(testData);
        }


    }
}

#pragma warning restore 1591
