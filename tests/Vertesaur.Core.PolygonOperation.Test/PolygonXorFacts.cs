using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.PolygonOperation.Test
{
    public static class PolygonXorFacts
    {

        private static readonly PolygonXorOperation _xorOperation;
        private static readonly PolyPairTestDataKeyedCollection _polyPairData;

        static PolygonXorFacts() {
            _polyPairData = PolyOperationTestUtility.GeneratePolyPairXorTestDataCollection();
            _xorOperation = new PolygonXorOperation();
        }

        public static IEnumerable<object[]> TestPolyXorParameters {
            get {
                return _polyPairData.Select(x => new object[] { x });
            }
        }

        private static bool PointsAlmostEqual(Point2 a, Point2 b) {
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

        [Theory, MemberData("TestPolyXorParameters")]
        public static void polygon_xor(PolyPairTestData testData) {
            Console.WriteLine(testData.Name);

            if (testData.Name == "Fuzzed: 3") {
                Console.WriteLine("Skipping " + testData.Name + " ...need to test this one another way.");
                return;
            }

            var result = _xorOperation.Xor(testData.A, testData.B) as Polygon2;
            if (null != testData.R) {
                Assert.NotNull(result);
                testData.R.SpatiallyEqual(result).Should().BeTrue("Forward case failed: {0} ∩ {1} ≠ {2}", testData.A, testData.B, PolygonToString(result));
            }
            else {
                Assert.Null(result);
            }

            result = _xorOperation.Xor(testData.B, testData.A) as Polygon2;
            if (null != testData.R) {
                Assert.NotNull(result);
                testData.R.SpatiallyEqual(result).Should().BeTrue("Reverse case failed: {0} ∩ {1} ≠ {2}", testData.B, testData.A, PolygonToString(result));
            }
            else {
                Assert.Null(result);
            }
        }

    }
}
