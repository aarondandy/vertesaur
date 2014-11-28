using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.PolygonOperation.Test
{

    /// <summary>
    /// Various tests to verify that the points of intersection for two rings are correct.
    /// </summary>
    public static class RingFindPointCrossingsTest
    {

        private static readonly PolygonIntersectionOperation _intersectionOperation;
        private static readonly RingPairTestDataKeyedCollection _ringPairData;

        static RingFindPointCrossingsTest() {
            _ringPairData = RingOperationTestUtility.GenerateRingPairTestDataCollection();
            _intersectionOperation = new PolygonIntersectionOperation();
        }

        public static IEnumerable<object[]> TestRingCrossingsParameters {
            get {
                return _ringPairData.Where(rp => null != rp.CrossingPoints).Select(x => new object[]{x});
            }
        }

        private static bool PointsAlmostEqual(Point2 a, Point2 b) {
            if (a == b)
                return true;
            var d = a.Difference(b);
            return d.GetMagnitudeSquared() < 0.000000000000000001;
        }

        [Theory, PropertyData("TestRingCrossingsParameters")]
        public static void ring_point_crossings(RingPairTestData testData) {
            if (testData.Name == "Fuzzed: 3")
                return; // must test this a different way

            Console.WriteLine(testData.Name);

            var result = _intersectionOperation.FindPointCrossings(new Polygon2(testData.A), new Polygon2(testData.B));
            Assert.NotNull(result);
            Console.WriteLine("{0} crossing points", result.Count);

            PolyOperationTestUtility.AssertEqual(
                testData.CrossingPoints.OrderBy(x => x),
                result.Select(r => r.Point).OrderBy(x => x),
                (x, y) => Assert.True(PointsAlmostEqual(x, y), "Points not equal."));

            result = _intersectionOperation.FindPointCrossings(new Polygon2(testData.B), new Polygon2(testData.A));
            Assert.NotNull(result);

            PolyOperationTestUtility.AssertEqual(
                testData.CrossingPoints.OrderBy(x => x),
                result.Select(r => r.Point).OrderBy(x => x),
                (x, y) => Assert.True(PointsAlmostEqual(x, y), "Points not equal."));
        }

        [Fact]
        public static void cascade_boxes() {
            var testData = _ringPairData[RingOperationTestUtility.RingPairNameCascadeBoxes];
            ring_point_crossings(testData);
        }
    }
}
