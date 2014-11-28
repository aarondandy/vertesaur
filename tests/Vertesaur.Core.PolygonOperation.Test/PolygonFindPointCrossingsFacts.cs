using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.PolygonOperation.Test
{

    /// <summary>
    /// Various tests to verify that the points of intersection for two polygons are correct.
    /// </summary>
    public static class PolygonFindPointCrossingsFacts
    {

        private static readonly PolygonIntersectionOperation _intersectionOperation;
        private static readonly PolyPairTestDataKeyedCollection _polyPairData;

        static PolygonFindPointCrossingsFacts() {
            _polyPairData = PolyOperationTestUtility.GeneratePolyPairTestDataCollection();
            _intersectionOperation = new PolygonIntersectionOperation();
        }

        public static IEnumerable<object[]> TestPolyCrossingsParameters {
            get {
                return _polyPairData.Where(pp => null != pp.CrossingPoints).Select(x => new object[]{x});
            }
        }

        public static bool PointsAlmostEqual(Point2 a, Point2 b) {
            if (a == b)
                return true;
            var d = a.Difference(b);
            return d.GetMagnitudeSquared() < 0.000000000000000001;
        }

        [Theory, PropertyData("TestPolyCrossingsParameters")]
        public static void polygon_intersection_point_crossings(PolyPairTestData testData) {
            if (testData.Name == "Fuzzed: 3")
                return; // NOTE: we must test this one a different way

            Console.WriteLine(testData.Name);

            var result = _intersectionOperation.FindPointCrossings(testData.A, testData.B);

            Assert.NotNull(result);
            Console.WriteLine("{0} crossing points", result.Count);

            PolyOperationTestUtility.AssertEqual(
                testData.CrossingPoints.OrderBy(p => p),
                result.Select(r => r.Point).OrderBy(p => p),
                (x, y) => Assert.True(PointsAlmostEqual(x, y), "Points not equal."));

            result = _intersectionOperation.FindPointCrossings(testData.B, testData.A);
            Assert.NotNull(result);

            PolyOperationTestUtility.AssertEqual(
                testData.CrossingPoints.OrderBy(p => p),
                result.Select(r => r.Point).OrderBy(p => p),
                (x, y) => Assert.True(PointsAlmostEqual(x, y), "Points not equal."));
        }

        [Fact]
        public static void cascade_boxes_example() {
            var testData = _polyPairData[RingOperationTestUtility.RingPairNameCascadeBoxes];
            polygon_intersection_point_crossings(testData);
        }

    }
}