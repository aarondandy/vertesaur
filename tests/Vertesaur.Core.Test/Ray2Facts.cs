using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class Ray2Facts
    {
        [Fact]
        public void constructor_point_point() {
            var a = new Ray2(new Point2(0, 0), new Point2(2, 3));
            var b = new Ray2(new Point2(1, 2), new Point2(3, 4));

            Assert.Equal(new Point2(0, 0), a.P);
            Assert.Equal(new Vector2(2, 3), a.Direction);
            Assert.Equal(new Point2(1, 2), b.P);
            Assert.Equal(new Vector2(2, 2), b.Direction);
        }

        [Fact]
        public void constructor_point_direction() {
            var ray = new Ray2(new Point2(0, 0), new Vector2(2, 3));

            Assert.Equal(new Point2(0, 0), ray.P);
            Assert.Equal(new Vector2(2, 3), ray.Direction);
        }

        [Fact]
        public void constructor_copy() {
            var i = new Ray2(new Point2(1, 2), new Point2(4, 5));
            var j = new Ray2(i);

            Assert.Equal(new Point2(1, 2), j.P);
            Assert.Equal(new Vector2(3, 3), j.Direction);
        }

        [Fact]
        public void direction() {
            var a = new Ray2(new Point2(2, 1), new Point2(-1, 3));
            var b = new Ray2(new Point2(5, -1), new Point2(10, 1));

            Assert.Equal(new Vector2(-3, 2), a.Direction);
            Assert.Equal(new Vector2(5, 2), b.Direction);
        }

        [Fact]
        public void equal_op() {
            var a = new Ray2(new Point2(1, 3), new Point2(2, 5));
            var b = new Ray2(new Point2(1, 3), new Point2(2, 5));
            var c = new Ray2(new Point2(0, 2), new Point2(5, 6));
            var d = new Ray2(new Point2(2, 5), new Point2(1, 3));

            Assert.Equal(a, b);
            Assert.Equal(b, a);
            Assert.NotEqual(a, c);
            Assert.NotEqual(c, a);
            Assert.NotEqual(a, d);
            Assert.NotEqual(d, a);
        }

        [Fact]
        public void definition_point_get() {
            var ray = new Ray2(new Point2(1, 2), new Vector2(3, 4));

            Assert.Equal(new Point2(1, 2), ((IRay2<double>)ray).P);
        }

        [Fact]
        public void magnitude() {
            var ray = new Ray2(new Point2(1, 2), new Vector2(3, 4));

            Assert.Equal(double.PositiveInfinity, ray.GetMagnitude());
        }

        [Fact]
        public void magnitude_squared() {
            var ray = new Ray2(new Point2(1, 2), new Vector2(3, 4));

            Assert.Equal(double.PositiveInfinity, ray.GetMagnitudeSquared());
        }

        [Fact]
        public void intersects_point_test() {
            var ray = new Ray2(new Point2(2, 1), new Vector2(2, 1));
            Assert.False(ray.Intersects(new Point2(-2, -1)));
            Assert.False(ray.Intersects(new Point2(0, 0)));
            Assert.True(ray.Intersects(new Point2(6, 3)));
            Assert.True(ray.Intersects(new Point2(2, 1)));
            Assert.True(ray.Intersects(new Point2(4, 2)));
            Assert.False(ray.Intersects(new Point2(1, 2)));
        }

        [Fact]
        public void distance_point() {
            var ray = new Ray2(new Point2(1, 1), new Vector2(2, 1));

            Assert.Equal(System.Math.Sqrt(1 + (.5 * .5)), ray.Distance(new Point2(1.5, 2.5)));
            Assert.Equal(2, ray.Distance(new Point2(-1, 1)));
        }

        [Fact]
        public void distance_squared_test() {
            var ray = new Ray2(new Point2(1, 1), new Vector2(2, 1));

            Assert.Equal(1 + (.5 * .5), ray.DistanceSquared(new Point2(1.5, 2.5)));
            Assert.Equal(4, ray.DistanceSquared(new Point2(-1, 1)));
        }

        [Fact]
        public void constructor_point_vector() {
            var ray = new Ray2(new Point2(1, 2), new Vector2(3, 4));

            Assert.Equal(new Point2(1, 2), ray.P);
            Assert.Equal(new Vector2(3, 4), ray.Direction);
        }

        [Fact]
        public void mbr() {
            var a = new Ray2(new Point2(1, 1), new Vector2(1, 1));
            var b = new Ray2(new Point2(1, 1), new Vector2(0, 1));

            Assert.Equal(double.PositiveInfinity, a.GetMbr().YMax);
            Assert.Equal(double.PositiveInfinity, a.GetMbr().XMax);
            Assert.Equal(1, a.GetMbr().YMin);
            Assert.Equal(double.PositiveInfinity, b.GetMbr().YMax);
            Assert.Equal(1, b.GetMbr().XMax);
            Assert.Equal(1, b.GetMbr().YMin);
        }

        private static void AreSpatiallyEqual(object expected, object result) {
            if (result is ISpatiallyEquatable<Ray2> && expected is Ray2) {
                Assert.True(result is ISpatiallyEquatable<Ray2>);
                Assert.True(
                    ((ISpatiallyEquatable<Ray2>)result)
                    .SpatiallyEqual(expected as Ray2)
                );
            }
            else {
                Assert.Equal(expected, result);
            }
        }

        private static void IntersectionTest(Ray2 a, Ray2 b, object expected) {
            AreSpatiallyEqual(expected, a.Intersection(b));
            AreSpatiallyEqual(expected, b.Intersection(a));
        }

        [Fact]
        public void intersections_two_rays_crossing() {
            var a = new Ray2(Point2.Zero, new Point2(4, 4));
            var b = new Ray2(new Point2(0, 4), new Point2(4, 0));

            IntersectionTest(a, b, new Point2(2, 2));
        }

        [Fact]
        public void intersection_two_rays_on_edge_of_defining_point() {
            var a = new Ray2(Point2.Zero, new Point2(1, 1));
            var b = new Ray2(new Point2(0.5, 1.5), new Point2(1.5, .5));

            IntersectionTest(a, b, new Point2(1, 1));
        }

        [Fact]
        public void intersection_rays_at_ends() {
            var a = new Ray2(new Point2(0, 0), new Point2(1, 1));
            var b = new Ray2(new Point2(0.5, 1.5), new Point2(1, 1));

            IntersectionTest(a, b, new Point2(1, 1));
        }

        [Fact]
        public void intersection_rways_at_butts() {
            var a = new Ray2(new Point2(0, 0), new Point2(1, 1));
            var b = new Ray2(new Point2(0, 0), new Point2(1, -1));

            IntersectionTest(a, b, Point2.Zero);
        }

        [Fact]
        public void intersection_identical() {
            var a = new Ray2(new Point2(0, 0), new Point2(1, 1));
            var b = new Ray2(new Point2(0, 0), new Point2(1, 1));

            Assert.Equal(a, a.Intersection(a));
            Assert.Equal(b, a.Intersection(a));
            Assert.Equal(a, b.Intersection(b));
            Assert.Equal(b, b.Intersection(b));
            Assert.Equal(a, a.Intersection(b));
            Assert.Equal(b, a.Intersection(b));
            Assert.Equal(a, b.Intersection(a));
            Assert.Equal(b, b.Intersection(a));
        }

        [Fact]
        public void intersection_same_but_reversed() {
            var a = new Ray2(new Point2(0, 0), new Point2(1, 1));
            var b = new Ray2(new Point2(1, 1), new Point2(0, 0));

            IntersectionTest(a, b, new Segment2(Point2.Zero, new Point2(1, 1)));
        }

        [Fact]
        public void intersection_parallel_butts() {
            var a = new Ray2(new Point2(1, 1), new Point2(2, 2));
            var b = new Ray2(new Point2(1, 1), new Point2(0, 0));

            IntersectionTest(a, b, new Point2(1, 1));
        }

        [Fact]
        public void intersection_ray_with_grain() {
            var a = new Ray2(Point2.Zero, new Point2(4, 4));
            var d = new Vector2(2, 2);

            for (var v = -3.0; v <= 0; v++) {
                var b = new Ray2(new Point2(v, v), d);
                IntersectionTest(a, b, a);
            }
            for (var v = 0; v <= 5.0; v++) {
                var b = new Ray2(new Point2(v, v), d);
                IntersectionTest(a, b, b);
            }
        }

        [Fact]
        public void intersection_ray_against_grain() {
            var a = new Ray2(Point2.Zero, new Point2(4, 4));
            var direction = new Vector2(-2, -2);
            var b = new Ray2(new Point2(-1, -1), direction);
            var c = new Ray2(Point2.Zero, direction);

            IntersectionTest(a, b, null);
            IntersectionTest(a, c, Point2.Zero);
            for (var v = 1.0; v <= 4.0; v++) {
                var p = new Point2(v, v);
                IntersectionTest(a, new Ray2(p, direction), new Segment2(Point2.Zero, p));
            }
            for (var v = 5.0; v <= 7.0; v++) {
                var p = new Point2(v, v);
                IntersectionTest(a, new Ray2(p, direction), new Segment2(Point2.Zero, p));
            }
        }

        [Fact]
        public void intersection_point_above_ray() {
            var a = new Ray2(Point2.Zero, new Point2(4, 4));
            var direction = new Vector2(1, 0);

            for (var v = -3.0; v <= 1.0; v++) {
                var b = new Ray2(new Point2(v, 5.0), direction);
                IntersectionTest(a, b, new Point2(5, 5));
            }
        }

        [Fact]
        public void intersection_point_within_ray() {
            var a = new Ray2(Point2.Zero, new Point2(4, 4));
            var direction = new Vector2(1, 0);

            for (var v = -3.0; v <= 1.0; v++) {
                var b = new Ray2(new Point2(v, 3), direction);
                IntersectionTest(a, b, new Point2(3, 3));
            }
        }

        [Fact]
        public void intersection_point_below_ray() {
            var a = new Ray2(Point2.Zero, new Point2(4, 4));
            var direction = new Vector2(1, 0);

            for (var v = -3.0; v <= 1.0; v++) {
                var b = new Ray2(new Point2(v, -1), direction);
                IntersectionTest(a, b, null);
            }
        }
    }
}