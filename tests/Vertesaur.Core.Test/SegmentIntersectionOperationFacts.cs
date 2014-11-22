using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Vertesaur.SegmentOperation;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class SegmentIntersectionOperationFacts
    {

        [Fact]
        public void intersection_cross() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(0, 1), new Point2(1, 0));

            var res = (SegmentIntersectionOperation.PointResult)SegmentIntersectionOperation.IntersectionDetails(a, b);
            Assert.Equal(new Point2(.5, .5), res.P);
            Assert.Equal(new Vector2(0.5, 0.5), res.Ratios);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeA);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeB);

            res = (SegmentIntersectionOperation.PointResult)SegmentIntersectionOperation.IntersectionDetails(b, a);
            Assert.Equal(new Point2(.5, .5), res.P);
            Assert.Equal(new Vector2(0.5, 0.5), res.Ratios);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeA);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeB);
        }

        [Fact]
        public void intersection_cross_edge() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(0.5, 1.5), new Point2(1.5, .5));

            var res = (SegmentIntersectionOperation.PointResult)SegmentIntersectionOperation.IntersectionDetails(a, b);
            Assert.Equal(new Point2(1, 1), res.P);
            Assert.Equal(new Vector2(1, 0.5), res.Ratios);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeA);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeB);

            res = (SegmentIntersectionOperation.PointResult)SegmentIntersectionOperation.IntersectionDetails(b, a);
            Assert.Equal(new Point2(1, 1), res.P);
            Assert.Equal(new Vector2(0.5, 1), res.Ratios);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeA);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeB);
        }

        [Fact]
        public void intersection_cross_ends() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(0.5, 1.5), new Point2(1, 1));

            var res = (SegmentIntersectionOperation.PointResult)SegmentIntersectionOperation.IntersectionDetails(a, b);
            Assert.Equal(new Point2(1, 1), res.P);
            Assert.Equal(new Vector2(1, 1), res.Ratios);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeA);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeB);

            res = (SegmentIntersectionOperation.PointResult)SegmentIntersectionOperation.IntersectionDetails(b, a);
            Assert.Equal(new Point2(1, 1), res.P);
            Assert.Equal(new Vector2(1, 1), res.Ratios);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeA);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeB);
        }

        [Fact]
        public void intersection_same() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(1, 1), new Point2(0, 0));

            var res = SegmentIntersectionOperation.IntersectionDetails(a, a) as SegmentIntersectionOperation.SegmentResult;
            AreSpatiallyEqual(a, res.S);
            AreSpatiallyEqual(b, res.S);
            Assert.Equal(a.A, res.S.A);
            Assert.Equal(a.B, res.S.B);
            Assert.Equal(new Vector2(0, 0), res.A.Ratios);
            Assert.Equal(new Vector2(1, 1), res.B.Ratios);

            res = SegmentIntersectionOperation.IntersectionDetails(b, b) as SegmentIntersectionOperation.SegmentResult;
            AreSpatiallyEqual(a, res.S);
            AreSpatiallyEqual(b, res.S);
            Assert.Equal(b.A, res.S.A);
            Assert.Equal(b.B, res.S.B);
            Assert.Equal(new Vector2(0, 0), res.A.Ratios);
            Assert.Equal(new Vector2(1, 1), res.B.Ratios);

            res = SegmentIntersectionOperation.IntersectionDetails(a, b) as SegmentIntersectionOperation.SegmentResult;
            AreSpatiallyEqual(a, res.S);
            AreSpatiallyEqual(b, res.S);
            Assert.Equal(a.A, res.S.A);
            Assert.Equal(a.B, res.S.B);
            Assert.Equal(new Vector2(0, 1), res.A.Ratios);
            Assert.Equal(new Vector2(1, 0), res.B.Ratios);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Butt, res.A.TypeA);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.A.TypeB);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.B.TypeA);
            Assert.Equal(SegmentIntersectionOperation.SegmentIntersectionType.Butt, res.B.TypeB);

            res = SegmentIntersectionOperation.IntersectionDetails(b, a) as SegmentIntersectionOperation.SegmentResult;
            AreSpatiallyEqual(a, res.S);
            AreSpatiallyEqual(b, res.S);
            Assert.Equal(b.A, res.S.A);
            Assert.Equal(b.B, res.S.B);
            Assert.Equal(new Vector2(0, 1), res.A.Ratios);
            Assert.Equal(new Vector2(1, 0), res.B.Ratios);

        }

        [Fact]
        public void intersection_overlap() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(.5, .5), new Point2(2, 2));
            var exp = new Segment2(new Point2(.5, .5), new Point2(1, 1));

            var res = SegmentIntersectionOperation.IntersectionDetails(a, b) as SegmentIntersectionOperation.SegmentResult;
            Assert.Equal(exp, res.S);
            Assert.Equal(new Vector2(.5, 0), res.A.Ratios);
            Assert.Equal(new Vector2(1, 1 / 3.0), res.B.Ratios);

            res = SegmentIntersectionOperation.IntersectionDetails(b, a) as SegmentIntersectionOperation.SegmentResult;
            Assert.Equal(exp, res.S);
            Assert.Equal(new Vector2(0, .5), res.A.Ratios);
            Assert.Equal(new Vector2(1 / 3.0, 1), res.B.Ratios);
        }

        [Fact]
        public void intersection_within() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(-.5, -.5), new Point2(2, 2));

            var res = SegmentIntersectionOperation.IntersectionDetails(a, b) as SegmentIntersectionOperation.SegmentResult;
            Assert.Equal(a, res.S);
            Assert.Equal(new Vector2(0, .2), res.A.Ratios);
            Assert.Equal(1, res.B.S);
            Assert.Equal(.6, res.B.T, 10);

            res = SegmentIntersectionOperation.IntersectionDetails(b, a) as SegmentIntersectionOperation.SegmentResult;
            Assert.Equal(a, res.S);
            Assert.Equal(new Vector2(.2, 0), res.A.Ratios);
            Assert.Equal(.6, res.B.S, 10);
            Assert.Equal(1, res.B.T);
        }

        [Fact]
        public void no_intersection_paralell() {
            var a = new Segment2(new Point2(0, 1), new Point2(1, 0));
            var b = new Segment2(new Point2(1, 1), new Point2(2, 0));

            var res = SegmentIntersectionOperation.IntersectionDetails(a, b);
            Assert.Null(res.Geometry);

            res = SegmentIntersectionOperation.IntersectionDetails(b, a);
            Assert.Null(res.Geometry);
        }

        [Fact]
        public void no_intersection_perpendicular() {
            var a = new Segment2(new Point2(0, 1), new Point2(1, 0));
            var b = new Segment2(new Point2(2, 2), new Point2(1, 1));

            var res = SegmentIntersectionOperation.IntersectionDetails(a, b);
            Assert.Null(res.Geometry);

            res = SegmentIntersectionOperation.IntersectionDetails(b, a);
            Assert.Null(res.Geometry);
        }

        private static void AreSpatiallyEqual(object expected, object result) {
            if (result is ISpatiallyEquatable<Segment2> && expected is Segment2) {
                Assert.True(result is ISpatiallyEquatable<Segment2>);
                Assert.True(
                    ((ISpatiallyEquatable<Segment2>)result)
                    .SpatiallyEqual(expected as Segment2)
                );
            }
            else {
                Assert.Equal(expected, result);
            }
        }
    }
}
