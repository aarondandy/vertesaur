using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.PolygonOperation.Test
{

    public static class PolygonIntersectionFacts
    {

        private static readonly PolygonIntersectionOperation _intersectionOperation;
        private static readonly PolyPairTestDataKeyedCollection _polyPairData;

        static PolygonIntersectionFacts() {
            _polyPairData = PolyOperationTestUtility.GeneratePolyPairIntersectionTestDataCollection();
            _intersectionOperation = new PolygonIntersectionOperation();
        }

        public static IEnumerable<object[]> TestPolyIntersectionParameters {
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

        [Theory, MemberData("TestPolyIntersectionParameters")]
        public static void polygon_intersection(PolyPairTestData testData) {
            Console.WriteLine(testData.Name);

            if (testData.Name == "Fuzzed: 3") {
                Console.WriteLine("Skipping " + testData.Name + " ...need to test this one another way.");
                return;
            }

            var result = _intersectionOperation.Intersect(testData.A, testData.B) as Polygon2;
            if (null != testData.R) {
                Assert.NotNull(result);
                testData.R.SpatiallyEqual(result).Should().BeTrue("Forward case failed: {0} ∩ {1} ≠ {2}", testData.A, testData.B, PolygonToString(result));
            }
            else {
                Assert.Null(result);
            }

            result = _intersectionOperation.Intersect(testData.B, testData.A) as Polygon2;
            if (null != testData.R) {
                Assert.NotNull(result);
                if (testData.Name == "Chess 4 (2 Fills and 2 Holes)")
                    Assert.Equal(testData.R.GetArea(), result.GetArea()); // TODO: pinch self intersecting polygons
                else if (testData.Name == "Chess 9 (5 Fills and 4 Holes)")
                    Assert.Equal(testData.R.GetArea(), result.GetArea()); // TODO: pinch self intersecting polygons
                else
                    testData.R.SpatiallyEqual(result).Should().BeTrue("Reverse case failed: {0} ∩ {1} ≠ {2}", testData.B, testData.A, PolygonToString(result));
            }
            else {
                Assert.Null(result);
            }
        }

        private static Polygon2 ReverseWinding(Polygon2 p) {
            if (null == p)
                return null;
            return new Polygon2(p.Select(ReverseWinding));
        }

        private static Ring2 ReverseWinding(Ring2 r) {
            if (null == r)
                return null;
            return r.Hole.HasValue
                ? new Ring2(r.Reverse(), r.Hole.Value)
                : new Ring2(r.Reverse());
        }

        [Theory, MemberData("TestPolyIntersectionParameters")]
        public static void polygon_intersection_reverse_winding(PolyPairTestData testData) {
            Console.WriteLine(testData.Name);

            var a = ReverseWinding(testData.A);
            var b = ReverseWinding(testData.B);
            var r = ReverseWinding(testData.R);

            if (testData.Name == "Fuzzed: 3") {
                Console.WriteLine("Skipping " + testData.Name + " ...need to test this one another way.");
                return;
            }

            var result = _intersectionOperation.Intersect(a, b) as Polygon2;
            if (null != r) {
                Assert.NotNull(result);
                r.SpatiallyEqual(result).Should().BeTrue("Forward case failed: {0} ∩ {1} ≠ {2}", a, b, PolygonToString(result));
            }
            else {
                Assert.Null(result);
            }

            result = _intersectionOperation.Intersect(b, a) as Polygon2;
            if (null != r) {
                Assert.NotNull(result);

                if (testData.Name == "Chess 4 (2 Fills and 2 Holes)")
                    Assert.Equal(r.GetArea(), result.GetArea()); // TODO: pinch self intersecting polygons
                else if (testData.Name == "Chess 9 (5 Fills and 4 Holes)")
                    Assert.Equal(r.GetArea(), result.GetArea()); // TODO: pinch self intersecting polygons
                else
                    r.SpatiallyEqual(result).Should().BeTrue("Reverse case failed: {0} ∩ {1} ≠ {2}", b, a, PolygonToString(result));
            }
            else {
                Assert.Null(result);
            }
        }

        [Fact]
        public static void can_perform_box_and_donut_intersection_without_exception() {
            var donut = new Polygon2 {
				// outer boundary
				new Ring2(
					Enumerable.Range(0,8)
					.Select(i => i * 45 / 180.0 * Math.PI)
					.Select(t => new Point2(Math.Cos(t),Math.Sin(t)))
				){Hole = false},
				// inner hole
				new Ring2(
					Enumerable.Range(0,8)
					.Reverse()
					.Select(i => i * 45/ 180.0 * Math.PI)
					.Select(t => new Point2(Math.Cos(t)*0.5,Math.Sin(t)*0.5))
				) {Hole = true}
			};

            var box = new Polygon2(new Ring2(new[]{
				new Point2(0,0),
				new Point2(1,0),
				new Point2(1,1),
				new Point2(0,1)
			}) { Hole = false });

            var intersectionOperation = new PolygonIntersectionOperation();
            var result = intersectionOperation.Intersect(box, donut);
        }

        [Fact]
        public static void cascade_boxes() {
            var data = _polyPairData["Cascade Boxes"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var a = data.A;
            var b = data.B;
            var r = data.R;

            var result = intersectionOperation.Intersect(a, b) as Polygon2;
            Assert.True(result.SpatiallyEqual(r));

            result = intersectionOperation.Intersect(b, a) as Polygon2;
            Assert.True(result.SpatiallyEqual(r));
        }

        [Fact]
        public static void cascade_boxes_reverse() {
            var data = _polyPairData["Cascade Boxes"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var a = ReverseWinding(data.A);
            var b = ReverseWinding(data.B);
            var r = ReverseWinding(data.R);

            var result = intersectionOperation.Intersect(a, b) as Polygon2;
            Assert.True(result.SpatiallyEqual(r));

            result = intersectionOperation.Intersect(b, a) as Polygon2;
            Assert.True(result.SpatiallyEqual(r));

        }

        [Fact]
        public static void under_ledge() {
            var data = _polyPairData["Under Ledge"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var a = data.A;
            var b = data.B;
            var r = data.R;

            Assert.Null(r);

            var result = intersectionOperation.Intersect(a, b) as Polygon2;
            Assert.Null(result);

            result = intersectionOperation.Intersect(b, a) as Polygon2;
            Assert.Null(result);
        }

        [Fact]
        public static void six_triangle_holes_reverse() {
            var data = _polyPairData["Six Triangle Holes"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var a = ReverseWinding(data.A);
            var b = ReverseWinding(data.B);
            var r = ReverseWinding(data.R);

            var result = intersectionOperation.Intersect(a, b) as Polygon2;
            Assert.True(result.SpatiallyEqual(r));

            result = intersectionOperation.Intersect(b, a) as Polygon2;
            Assert.True(result.SpatiallyEqual(r));

        }

        [Fact]
        public static void two_stacked_boxes_reverse() {
            var data = _polyPairData["Two Stacked Boxes"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var a = ReverseWinding(data.A);
            var b = ReverseWinding(data.B);
            var r = ReverseWinding(data.R);

            var result = intersectionOperation.Intersect(a, b) as Polygon2;
            Assert.Null(result);

            result = intersectionOperation.Intersect(b, a) as Polygon2;
            Assert.Null(result);

        }

        [Fact]
        public static void reverse_zThing_in_box() {
            var data = _polyPairData["Z-Thing in a Box"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var a = ReverseWinding(data.A);
            var b = ReverseWinding(data.B);
            var r = ReverseWinding(data.R);

            var result = intersectionOperation.Intersect(a, b) as Polygon2;
            Assert.True(result.SpatiallyEqual(r));

            result = intersectionOperation.Intersect(b, a) as Polygon2;
            Assert.True(result.SpatiallyEqual(r));
        }

        [Fact]
        public static void triangle_in_box_side_touch() {
            var data = _polyPairData["Triangle In Box: side touch"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

        }

        [Fact]
        public static void two_stacked_boxes() {
            var data = _polyPairData["Two Stacked Boxes"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.Null(result);

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.Null(result);

        }

        [Fact]
        public static void same_boxes() {
            var data = _polyPairData["Same Boxes"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

        }

        [Fact]
        public static void nested_fill_within_another() {
            var data = _polyPairData["Nested: fill within another, not touching"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

        }

        [Fact]
        public static void hole_in_hole_no_intersection() {
            var data = _polyPairData["Nested: hole within a hole, not touching"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

        }

        [Fact]
        public static void diamond_in_double_diamond_touching_sides() {
            var data = _polyPairData["Diamond in Double Diamond: touching sides"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

        }

        [Fact]
        public static void zig_zag_thing_holes() {
            var data = _polyPairData["Zig-zag Thing: holes"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

        }

        [Fact]
        public static void cascade_fill_hole() {
            var data = _polyPairData["Cascade Boxes: fill and a hole"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

        }

        [Fact]
        public static void fuzzed_3() {
            var data = _polyPairData["Fuzzed: 3"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.NotNull(result);
            Assert.True(result
                .SelectMany(r => r)
                .Select(p => data.R.DistanceSquared(p))
                .All(d => d < 0.000000000000000001)
            );

            var result2 = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.NotNull(result2);
            Assert.True(result2
                .SelectMany(r => r)
                .Select(p => data.R.DistanceSquared(p))
                .All(d => d < 0.000000000000000001)
            );
        }

        [Fact]
        public static void zThing_in_box() {
            var data = _polyPairData["Z-Thing in a Box"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));
        }

        [Fact]
        public static void boxes_overlapping_top_half_holes() {
            var sourceData = _polyPairData["Boxes Overlapping: top half"];
            var inverseA = new Polygon2(sourceData.A[0].Reverse(), true);
            var inverseB = new Polygon2(sourceData.B[0].Reverse(), true);
            var expectedResult = inverseA;
            var result = _intersectionOperation.Intersect(inverseA, inverseB) as Polygon2;
            Assert.True(result.SpatiallyEqual(expectedResult));

            result = _intersectionOperation.Intersect(inverseB, inverseA) as Polygon2;
            Assert.True(result.SpatiallyEqual(expectedResult));
        }

        [Fact]
        public static void chess_four_hole() {
            var data = _polyPairData["Chess 4 Holes"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));
        }

        [Fact]
        public static void chess_nine_hole() {
            var data = _polyPairData["Chess 9 Holes"];
            var intersectionOperation = new PolygonIntersectionOperation();

            var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            Assert.True(result.SpatiallyEqual(data.R));
        }

        [Fact]
        public static void three_part_trig_holes() {
            var data = _polyPairData["Three Part Triangle Holes"];
            var intersectionOperation = new PolygonIntersectionOperation();
            Polygon2 result;

            result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            result.SpatiallyEqual(data.R).Should().BeTrue("Forward Result:\n {0}", PolygonToString(result));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            result.SpatiallyEqual(data.R).Should().BeTrue("Reverse Result:\n {0}", PolygonToString(result));
        }

        [Fact]
        public static void six_triangle_holes() {
            var data = _polyPairData["Six Triangle Holes"];
            var intersectionOperation = new PolygonIntersectionOperation();
            Polygon2 result;

            result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            result.SpatiallyEqual(data.R).Should().BeTrue("Forward Result:\n {0}", PolygonToString(result));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            result.SpatiallyEqual(data.R).Should().BeTrue("Reverse Result:\n {0}", PolygonToString(result));
        }

        [Fact(Skip="requires pinching self intersecting rings gh#1")]
        public static void chess_four_fill_holes() {
            var data = _polyPairData["Chess 4 (2 Fills and 2 Holes)"];
            var intersectionOperation = new PolygonIntersectionOperation();
            Polygon2 result;

            result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            result.SpatiallyEqual(data.R).Should().BeTrue("Forward Result:\n {0}", PolygonToString(result));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            result.SpatiallyEqual(data.R).Should().BeTrue("Reverse Result:\n {0}", PolygonToString(result));
        }

        [Fact(Skip = "requires pinching self intersecting rings gh#1")]
        public static void chess_nine_fill_holes() {
            var data = _polyPairData["Chess 9 (5 Fills and 4 Holes)"];
            var intersectionOperation = new PolygonIntersectionOperation();
            Polygon2 result;

            result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
            result.SpatiallyEqual(data.R).Should().BeTrue("Forward Result:\n {0}", PolygonToString(result));

            result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
            result.SpatiallyEqual(data.R).Should().BeTrue("Reverse Result:\n {0}", PolygonToString(result));
        }

    }
}
