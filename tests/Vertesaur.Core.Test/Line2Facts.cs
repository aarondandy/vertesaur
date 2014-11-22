using System;
using FluentAssertions;
using Xunit;

namespace Vertesaur.Test
{
    public static class Line2Facts
    {

        [Fact]
        public static void construct_point_to_point() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 5);
            
            var line = new Line2(a, b);

            line.P.Should().Be(a);
            line.Direction.Should().Be(new Vector2(2, 3));
        }

        [Fact]
        public static void construct_point_and_vector() {
            var a = new Point2(1, 2);
            var d = new Vector2(3, 4);

            var line = new Line2(a, d);

            line.P.Should().Be(a);
            line.Direction.Should().Be(d);
        }

        [Fact]
        public static void can_copy_with_constructor() {
            var first = new Line2(new Point2(1, 2), new Point2(4, 5));

            var clone = new Line2(first);

            clone.P.Should().Be(first.P);
            clone.Direction.Should().Be(first.Direction);
        }

        [Fact]
        public static void direction_calculation_from_constructor() {
            var a = new Line2(new Point2(2, 1), new Point2(-1, 3));
            var b = new Line2(new Point2(5, -1), new Point2(10, 1));

            a.Direction.Should().Be(new Vector2(-3, 2));
            b.Direction.Should().Be(new Vector2(5, 2));
        }

        [Fact]
        public static void basic_equality() {
            var a = new Line2(new Point2(1, 3), new Point2(2, 5));
            var b = new Line2(new Point2(1, 3), new Point2(2, 5));
            var c = new Line2(new Point2(0, 2), new Point2(5, 6));
            var d = new Line2(new Point2(2, 5), new Point2(1, 3));

            a.Should().Be(b);
            b.Should().Be(a);
            a.Should().NotBe(c);
            c.Should().NotBe(a);
            a.Should().NotBe(d);
            d.Should().NotBe(a);
        }

        [Fact]
        public static void interface_point_extraction() {
            var p = new Point2(1, 2);
            var line = new Line2(p, new Vector2(3, 4));

            var explicitP = ((ILine2<double>)line).P;

            explicitP.Should().Be(p);
        }

        [Fact]
        public static void infinite_magnitude() {
            var line = new Line2(new Point2(1, 2), new Vector2(3, 4));

            var magnitude = line.GetMagnitude();

            magnitude.Should().Be(Double.PositiveInfinity);
        }

        [Fact]
        public static void infinite_magnitude_squared() {
            var line = new Line2(new Point2(1, 2), new Vector2(3, 4));

            var squaredMagnitude = line.GetMagnitudeSquared();

            squaredMagnitude.Should().Be(Double.PositiveInfinity);
        }

        [Fact]
        public static void general_intersections() {
            var line = new Line2(new Point2(2, 1), new Vector2(2, 1));

            line.Intersects(new Point2(-2, -1)).Should().BeTrue();
            line.Intersects(new Point2(0, 0)).Should().BeTrue();
            line.Intersects(new Point2(1, 0.5)).Should().BeTrue();
            line.Intersects(new Point2(2, 1)).Should().BeTrue();
            line.Intersects(new Point2(4, 2)).Should().BeTrue();
            line.Intersects(new Point2(1, 2)).Should().BeFalse();
        }

        [Fact]
        public static void basic_distance_test() {
            var line = new Line2(new Point2(1, 1), new Vector2(2, 1));
            var targetA = new Point2(-1, 0);
            var targetB = new Point2(.5, 2);

            var distanceA = line.Distance(targetA);
            var distanceB = line.Distance(targetB);

            distanceA.Should().Be(0);
            distanceB.Should().Be(System.Math.Sqrt((0.5 * 0.5) + 1.0));
        }

        [Fact]
        public static void basic_distance_squared_test() {
            var line = new Line2(new Point2(1, 1), new Vector2(2, 1));
            var targetA = new Point2(-1, 0);
            var targetB = new Point2(.5, 2);

            var distanceA = line.DistanceSquared(targetA);
            var distanceB = line.DistanceSquared(targetB);

            distanceA.Should().Be(0);
            distanceB.Should().Be((0.5 * 0.5) + 1.0);
        }


        [Fact]
        public static void basic_mbr_test() {
            var lineA = new Line2(new Point2(1, 1), new Vector2(1, 1));
            var lineB = new Line2(new Point2(1, 1), new Vector2(0, 1));

            var mbrA = lineA.GetMbr();
            var mbrB = lineB.GetMbr();

            mbrA.XMin.Should().Be(Double.NegativeInfinity);
            mbrA.XMax.Should().Be(Double.PositiveInfinity);
            mbrA.YMin.Should().Be(Double.NegativeInfinity);
            mbrA.YMax.Should().Be(Double.PositiveInfinity);

            mbrB.XMin.Should().Be(1);
            mbrB.XMax.Should().Be(1);
            mbrB.YMin.Should().Be(Double.NegativeInfinity);
            mbrB.YMax.Should().Be(Double.PositiveInfinity);
        }

        [Fact]
        public static void line_line_basic_intersection_result() {
            var a = new Line2(new Point2(3, 3), new Vector2(1, 1));
            var b = new Line2(new Point2(0, 1), new Vector2(0, 1));
            var c = new Line2(new Point2(2, 2), new Vector2(-1, -1));
            var d = new Line2(new Point2(2, 3), new Vector2(-1, -1));

            a.Intersection(b).Should().Be(new Point2(0, 0));
            a.Intersection(c).Should().Be(c);
            c.Intersection(a).Should().Be(c);
            a.Intersection(d).Should().BeNull();
        }

        [Fact]
        public static void line_line_basic_intersection_testing() {
            var a = new Line2(new Point2(3, 3), new Vector2(1, 1));
            var b = new Line2(new Point2(0, 1), new Vector2(0, 1));
            var c = new Line2(new Point2(2, 2), new Vector2(-1, -1));
            var d = new Line2(new Point2(2, 3), new Vector2(-1, -1));
            var e = new Line2(new Point2(2, 3), new Vector2(-2, -2));
            var f = new Line2(new Point2(2, 3), new Vector2(0.5, 0.5));

            a.Intersects(a).Should().BeTrue();
            a.Intersects(b).Should().BeTrue();
            a.Intersects(c).Should().BeTrue();
            a.Intersects(d).Should().BeFalse();
            a.Intersects(e).Should().BeFalse();
            a.Intersects(f).Should().BeFalse();
        }

        [Fact]
        public static void line_ray_basic_intersection_result() {
            var a = new Line2(new Point2(3, 3), new Vector2(1, 1));
            var b = new Ray2(new Point2(0, 1), new Vector2(0, 1));
            var c = new Ray2(new Point2(2, 2), new Vector2(-1, -1));
            var d = new Ray2(new Point2(2, 3), new Vector2(-1, -1));

            a.Intersection(b.GetReverse()).Should().Be(Point2.Zero);
            a.Intersection(c).Should().Be(c);
            a.Intersection(c.GetReverse()).Should().Be(c.GetReverse());
            a.Intersection(b).Should().BeNull();
            a.Intersection(d).Should().BeNull();
            a.Intersection(d.GetReverse()).Should().BeNull();
        }

        [Fact]
        public static void line_ray_basic_intersection_testing() {
            var a = new Line2(new Point2(3, 3), new Vector2(1, 1));
            var b = new Ray2(new Point2(0, 1), new Vector2(0, 1));
            var c = new Ray2(new Point2(2, 2), new Vector2(-1, -1));
            var d = new Ray2(new Point2(2, 3), new Vector2(-1, -1));

            a.Intersects(b.GetReverse()).Should().BeTrue();
            a.Intersects(c).Should().BeTrue();
            a.Intersects(c.GetReverse()).Should().BeTrue();
            a.Intersects(b).Should().BeFalse();
            a.Intersects(d).Should().BeFalse();
            a.Intersects(d.GetReverse()).Should().BeFalse();
        }

    }
}