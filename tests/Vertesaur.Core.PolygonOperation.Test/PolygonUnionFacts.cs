using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.PolygonOperation.Test
{
    public static class PolygonUnionFacts
    {

        private static readonly PolygonUnionOperation _unionOperation;
        private static readonly PolyPairTestDataKeyedCollection _polyPairData;

        static PolygonUnionFacts() {
            _polyPairData = PolyOperationTestUtility.GeneratePolyPairUnionTestDataCollection();
            _unionOperation = new PolygonUnionOperation();
        }

        public static IEnumerable<object> TestPolyUnionParameters {
            get {
                return _polyPairData.Select(x => new object[] { x });
            }
        }

        public static bool PointsAlmostEqual(Point2 a, Point2 b) {
            if (a == b)
                return true;
            var d = a.Difference(b);
            return d.GetMagnitudeSquared() < 0.000000000000000001;
        }

        private static string PolygonToString(Polygon2 poly) {
            var sb = new StringBuilder();
            for (int index = 0; index < poly.Count; index++) {
                var ring = poly[index];
                sb.AppendFormat("Ring {0}:\n", index);
                sb.AppendLine(RingToString(ring));
            }
            return sb.ToString();
        }

        private static string RingToString(Ring2 ring) {
            var sb = new StringBuilder();
            foreach (var p in ring) {
                sb.AppendFormat("({0},{1})\n", p.X, p.Y);
            }
            return sb.ToString();
        }

        [Theory, PropertyData("TestPolyUnionParameters")]
        public static void polygon_union(PolyPairTestData testData) {
            Console.WriteLine(testData.Name);

            if (testData.Name == "Nested: hole within a fill, not touching") {
                return; // infinite spaaaaaaaace
            }

            var result = _unionOperation.Union(testData.A, testData.B) as Polygon2;
            if (null != testData.R) {
                Assert.NotNull(result);
                testData.R.SpatiallyEqual(result).Should().BeTrue("Forward case failed: {0} u {1} ≠ {2}", testData.A, testData.B, PolygonToString(result));
            }
            else {
                Assert.Null(result);
            }

            result = _unionOperation.Union(testData.B, testData.A) as Polygon2;
            if (null != testData.R) {
                Assert.NotNull(result);
                testData.R.SpatiallyEqual(result).Should().BeTrue("Reverse case failed: {0} u {1} ≠ {2}", testData.B, testData.A, PolygonToString(result));
            }
            else {
                Assert.Null(result);
            }
        }

        [Fact]
        public static void zig_zag_thing() {
            var data = _polyPairData["Zig-zag Thing"];
            var unionOperation = new PolygonUnionOperation();

            var result = unionOperation.Union(data.A, data.B) as Polygon2;
            Assert.NotNull(result);
            Assert.True(result.SpatiallyEqual(data.R));

            result = unionOperation.Union(data.B, data.A) as Polygon2;
            Assert.NotNull(result);
            Assert.True(result.SpatiallyEqual(data.R));
        }

    }
}
