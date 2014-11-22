using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public static class MbrFacts
    {
        [Fact]
        public static void range_y() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(4, 3, 2, 1);

            a.Y.Low.Should().Be(2);
            a.Y.High.Should().Be(4);
            b.Y.Low.Should().Be(1);
            b.Y.High.Should().Be(3);
        }

        [Fact]
        public static void ymin() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(4, 3, 2, 1);

            Assert.Equal(2, a.YMin);
            Assert.Equal(1, b.YMin);
        }

        [Fact]
        public static void ymax() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(4, 3, 2, 1);

            Assert.Equal(4, a.YMax);
            Assert.Equal(3, b.YMax);
        }

        [Fact]
        public static void range_x() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(4, 3, 2, 1);

            a.X.Low.Should().Be(1);
            a.X.High.Should().Be(3);
            b.X.Low.Should().Be(2);
            b.X.High.Should().Be(4);
        }

        [Fact]
        public static void xmin() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(4, 3, 2, 1);

            Assert.Equal(1, a.XMin);
            Assert.Equal(2, b.XMin);
        }

        [Fact]
        public static void xmax() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(4, 3, 2, 1);

            Assert.Equal(3, a.XMax);
            Assert.Equal(4, b.XMax);
        }

        [Fact]
        public static void width() {
            var a = new Mbr(1, 6, 3, 5);
            var b = new Mbr(3, 6, -1, 5);

            Assert.Equal(2, a.Width);
            Assert.Equal(4, b.Width);
        }

        [Fact]
        public static void height() {
            var a = new Mbr(1, 2, 6, 5);
            var b = new Mbr(1, 5, 6, -2);

            Assert.Equal(3, a.Height);
            Assert.Equal(7, b.Height);
        }

        [Fact]
        public static void intersects_point_test() {
            var mbr = new Mbr(new Point2(1, 2), new Point2(3, 4));

            Assert.False(mbr.Intersects(new Point2(0, 0)));
            Assert.False(mbr.Intersects(new Point2(2, 1)));
            Assert.False(mbr.Intersects(new Point2(0, 3)));
            Assert.False(mbr.Intersects(new Point2(-1, -1)));
            Assert.False(mbr.Intersects(new Point2(10, 10)));
            Assert.True(mbr.Intersects(new Point2(1, 2)));
            Assert.True(mbr.Intersects(new Point2(3, 4)));
            Assert.True(mbr.Intersects(new Point2(3, 2)));
            Assert.True(mbr.Intersects(new Point2(1, 4)));
            Assert.True(mbr.Intersects(new Point2(2, 3)));
        }

        [Fact]
        public static void intersects_mbr_test() {
            var a = new Mbr(1, 1, 5, 6);
            var b = new Mbr(0, 0, 3, 2);
            var c = new Mbr(2, 3, 4, 5);

            Assert.True(a.Intersects(b));
            Assert.True(a.Intersects(c));
            Assert.False(b.Intersects(c));
        }

        [Fact]
        public static void area() {
            var mbr = new Mbr(1, 2, 3, 5);

            Assert.Equal(6, mbr.GetArea());
        }

        [Fact]
        public static void constructor_ranges() {
            var mbr = new Mbr(new Range(1, 3), new Range(4, 2));

            Assert.Equal(1, mbr.XMin);
            Assert.Equal(2, mbr.YMin);
            Assert.Equal(3, mbr.XMax);
            Assert.Equal(4, mbr.YMax);
        }

        [Fact]
        public static void constructor_values() {
            var mbr = new Mbr(1, 2, 3, 4);

            Assert.Equal(1, mbr.XMin);
            Assert.Equal(2, mbr.YMin);
            Assert.Equal(3, mbr.XMax);
            Assert.Equal(4, mbr.YMax);
        }

        [Fact]
        public static void constructor_point() {
            var mbr = new Mbr(new Point2(1, 2));

            Assert.Equal(1, mbr.XMin);
            Assert.Equal(2, mbr.YMin);
            Assert.Equal(1, mbr.XMax);
            Assert.Equal(2, mbr.YMax);
        }

        [Fact]
        public static void constructor_points() {
            var mbr = new Mbr(new Point2(1, 2), new Point2(3, 4));

            Assert.Equal(1, mbr.XMin);
            Assert.Equal(2, mbr.YMin);
            Assert.Equal(3, mbr.XMax);
            Assert.Equal(4, mbr.YMax);
        }

        [Fact]
        public static void distance_point() {
            var mbr = new Mbr(new Point2(1, 2), new Point2(3, 4));

            Assert.Equal(System.Math.Sqrt(1 + 4), mbr.Distance(new Point2(0, 0)));
            Assert.Equal(1, mbr.Distance(new Point2(2, 1)));
            Assert.Equal(1, mbr.Distance(new Point2(0, 3)));
            Assert.Equal(System.Math.Sqrt(4 + 9), mbr.Distance(new Point2(-1, -1)));
            Assert.Equal(System.Math.Sqrt(36 + (7 * 7)), mbr.Distance(new Point2(10, 10)));
            Assert.Equal(0, mbr.Distance(new Point2(1, 2)));
            Assert.Equal(0, mbr.Distance(new Point2(3, 4)));
            Assert.Equal(0, mbr.Distance(new Point2(3, 2)));
            Assert.Equal(0, mbr.Distance(new Point2(1, 4)));
            Assert.Equal(0, mbr.Distance(new Point2(2, 3)));
        }

        [Fact]
        public static void distance_squared_point() {
            var mbr = new Mbr(new Point2(1, 2), new Point2(3, 4));

            Assert.Equal((1 + 4), mbr.DistanceSquared(new Point2(0, 0)));
            Assert.Equal(1, mbr.DistanceSquared(new Point2(2, 1)));
            Assert.Equal(1, mbr.DistanceSquared(new Point2(0, 3)));
            Assert.Equal((4 + 9), mbr.DistanceSquared(new Point2(-1, -1)));
            Assert.Equal((36 + (7 * 7)), mbr.DistanceSquared(new Point2(10, 10)));
            Assert.Equal(0, mbr.DistanceSquared(new Point2(1, 2)));
            Assert.Equal(0, mbr.DistanceSquared(new Point2(3, 4)));
            Assert.Equal(0, mbr.DistanceSquared(new Point2(3, 2)));
            Assert.Equal(0, mbr.DistanceSquared(new Point2(1, 4)));
            Assert.Equal(0, mbr.DistanceSquared(new Point2(2, 3)));
        }

        [Fact]
        public static void distance_mbr() {
            var a = new Mbr(0, 0, 2, 2);
            var b = new Mbr(1, 1, 3, 3);
            var c = new Mbr(3, 4, 5, 5);
            var d = new Mbr(8, 8, 10, 10);

            Assert.Equal(0, a.Distance(b));
            Assert.Equal(System.Math.Sqrt(1 + 4), a.Distance(c));
            Assert.Equal(System.Math.Sqrt(36 + 36), a.Distance(d));
            Assert.Equal(1, b.Distance(c));
            Assert.Equal(System.Math.Sqrt(25 + 25), b.Distance(d));
            Assert.Equal(System.Math.Sqrt(9 + 9), c.Distance(d));
        }

        [Fact]
        public static void distange_squared_mbr() {
            var a = new Mbr(0, 0, 2, 2);
            var b = new Mbr(1, 1, 3, 3);
            var c = new Mbr(3, 4, 5, 5);
            var d = new Mbr(8, 8, 10, 10);

            Assert.Equal(0, a.DistanceSquared(b));
            Assert.Equal((1 + 4), a.DistanceSquared(c));
            Assert.Equal((36 + 36), a.DistanceSquared(d));
            Assert.Equal(1, b.Distance(c));
            Assert.Equal((25 + 25), b.DistanceSquared(d));
            Assert.Equal((9 + 9), c.DistanceSquared(d));
        }

        [Fact]
        public static void centroid() {
            var mbr = new Mbr(1, 2, 3, 4);

            Assert.Equal(new Point2(2, 3), mbr.GetCentroid());
        }

        [Fact]
        public static void equals_type() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 4, 5);
            var c = new Mbr(b.X, b.Y);

            Assert.False(a.Equals(b));
            Assert.False(a.Equals(c));
            Assert.False(b.Equals(a));
            Assert.True(b.Equals(c));
            Assert.False(c.Equals(a));
            Assert.True(c.Equals(b));
        }

        [Fact]
        public static void equals_object() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 4, 5);
            var c = new Mbr(b.X, b.Y);

            Assert.False(a.Equals((object)b));
            Assert.False(a.Equals((object)c));
            Assert.False(b.Equals((object)a));
            Assert.True(b.Equals((object)c));
            Assert.False(c.Equals((object)a));
            Assert.True(c.Equals((object)b));
        }

        [Fact]
        public static void equals_op() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 4, 5);
            var c = new Mbr(b.X, b.Y);

            Assert.False(a == b);
            Assert.False(a == c);
            Assert.False(b == a);
            Assert.True(b == c);
            Assert.False(c == a);
            Assert.True(c == b);
        }

        [Fact]
        public static void inequal_op() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 4, 5);
            var c = new Mbr(b.X, b.Y);

            Assert.True(a != b);
            Assert.True(a != c);
            Assert.True(b != a);
            Assert.False(b != c);
            Assert.True(c != a);
            Assert.False(c != b);
        }

        [Fact]
        public static void touches_mbr() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 4, 4);
            var c = new Mbr(3, 4, 5, 6);
            var d = new Mbr(3, 0, 5, 6);
            var e = new Mbr(0, 4, 4, 6);

            Assert.False(a.Touches(a));
            Assert.False(a.Touches(b));
            Assert.True(a.Touches(c));
            Assert.True(a.Touches(d));
            Assert.True(a.Touches(e));
        }

        [Fact]
        public static void touches_point() {
            var a = new Mbr(1, 2, 3, 4);

            Assert.False(a.Touches(new Point2(2, 3)));
            Assert.False(a.Touches(Point2.Zero));
            Assert.False(a.Touches(Point2.Invalid));
            Assert.True(a.Touches(new Point2(1, 2)));
            Assert.True(a.Touches(new Point2(3, 4)));
            Assert.True(a.Touches(new Point2(1, 4)));
            Assert.True(a.Touches(new Point2(3, 2)));
            Assert.True(a.Touches(new Point2(2, 4)));
            Assert.True(a.Touches(new Point2(3, 3)));
        }

        [Fact]
        public static void crosses_mbr() {
            var a = new Mbr(1, 2, 3, 4); // 2x2
            var b = new Mbr(2, 3, 2, 5); // 0x2
            var c = new Mbr(2, 3, 4, 3); // 2x0
            var d = new Mbr(2, 3, 2, 3); // 0x0

            Assert.False(a.Crosses(a));
            Assert.True(a.Crosses(b));
            Assert.True(a.Crosses(c));
            Assert.True(b.Crosses(a));
            Assert.False(b.Crosses(b));
            Assert.False(b.Crosses(c));
            Assert.True(c.Crosses(a));
            Assert.False(c.Crosses(b));
            Assert.False(c.Crosses(c));
            Assert.False(d.Crosses(a));
            Assert.False(d.Crosses(b));
            Assert.False(d.Crosses(c));
            Assert.False(d.Crosses(d));
        }

        [Fact]
        public static void within_mbr() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 4, 5);
            var c = new Mbr(1.5, 2.5, 2.5, 3.5);
            var d = new Mbr(1.5, 2, 2.5, 4);

            Assert.True(a.Within(a));
            Assert.False(a.Within(b));
            Assert.False(a.Within(c));
            Assert.False(a.Within(d));
            Assert.False(b.Within(a));
            Assert.True(b.Within(b));
            Assert.False(b.Within(c));
            Assert.False(b.Within(d));
            Assert.True(c.Within(a));
            Assert.False(c.Within(b));
            Assert.True(c.Within(c));
            Assert.True(c.Within(d));
            Assert.True(d.Within(a));
            Assert.False(d.Within(b));
            Assert.False(d.Within(c));
            Assert.True(d.Within(d));
        }

        [Fact]
        public static void contains_mbr() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 4, 5);
            var c = new Mbr(1.5, 2.5, 2.5, 3.5);
            var d = new Mbr(1.5, 2, 2.5, 4);

            Assert.True(a.Contains(a));
            Assert.False(b.Contains(a));
            Assert.False(c.Contains(a));
            Assert.False(d.Contains(a));
            Assert.False(a.Contains(b));
            Assert.True(b.Contains(b));
            Assert.False(c.Contains(b));
            Assert.False(d.Contains(b));
            Assert.True(a.Contains(c));
            Assert.False(b.Contains(c));
            Assert.True(c.Contains(c));
            Assert.True(d.Contains(c));
            Assert.True(a.Contains(d));
            Assert.False(b.Contains(d));
            Assert.False(c.Contains(d));
            Assert.True(d.Contains(d));
        }

        [Fact]
        public static void disjoint_mbr() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 4, 5);
            var c = new Mbr(3, 2, 5, 4);
            var d = new Mbr(3, 4, 5, 6);
            var e = new Mbr(5, 6, 7, 8);

            Assert.False(a.Disjoint(a));
            Assert.False(a.Disjoint(b));
            Assert.False(b.Disjoint(a));
            Assert.False(a.Disjoint(c));
            Assert.False(c.Disjoint(a));
            Assert.False(a.Disjoint(d));
            Assert.False(d.Disjoint(a));
            Assert.True(a.Disjoint(e));
            Assert.True(e.Disjoint(a));
        }

        [Fact]
        public static void spatially_equal() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 4, 5);
            var c = new Mbr(b.X, b.Y);

            Assert.False(((ISpatiallyEquatable<Mbr>)a).SpatiallyEqual(b));
            Assert.False(((ISpatiallyEquatable<Mbr>)a).SpatiallyEqual(c));
            Assert.False(((ISpatiallyEquatable<Mbr>)b).SpatiallyEqual(a));
            Assert.True(((ISpatiallyEquatable<Mbr>)b).SpatiallyEqual(c));
            Assert.False(((ISpatiallyEquatable<Mbr>)c).SpatiallyEqual(a));
            Assert.True(((ISpatiallyEquatable<Mbr>)c).SpatiallyEqual(b));
        }

        [Fact]
        public static void within_point() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 2, 3);
            var c = new Mbr(3, 3, 3, 3);
            var p = new Point2(2, 3);

            Assert.False(a.Within(p));
            Assert.True(b.Within(p));
            Assert.False(c.Within(p));
        }

        [Fact]
        public static void contains_point() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 2, 3);
            var c = new Mbr(5, 6, 7, 8);
            var p = new Point2(2, 3);

            Assert.True(a.Contains(p));
            Assert.True(b.Contains(p));
            Assert.False(c.Contains(p));
        }

        [Fact]
        public static void disjoint_point() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 2, 3);
            var c = new Mbr(5, 6, 7, 8);
            var p = new Point2(2, 3);

            Assert.False(a.Disjoint(p));
            Assert.False(b.Disjoint(p));
            Assert.True(c.Disjoint(p));
        }

        [Fact]
        public static void spatially_equal_point() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 2, 3);
            var c = new Mbr(5, 6, 7, 8);
            var p = new Point2(2, 3);

            Assert.False(a.SpatiallyEqual(p));
            Assert.True(b.SpatiallyEqual(p));
            Assert.False(c.SpatiallyEqual(p));
        }

        [Fact]
        public static void crosses_point() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 2, 3);
            var c = new Mbr(5, 6, 7, 8);
            var p = new Point2(2, 3);

            Assert.False(((IRelatableCrosses<Point2>)a).Crosses(p));
            Assert.False(((IRelatableCrosses<Point2>)b).Crosses(p));
            Assert.False(((IRelatableCrosses<Point2>)c).Crosses(p));
        }

        [Fact]
        public static void overlaps_point() {
            var a = new Mbr(1, 2, 3, 4);
            var b = new Mbr(2, 3, 2, 3);
            var c = new Mbr(5, 6, 7, 8);
            var p = new Point2(2, 3);

            Assert.False(((IRelatableOverlaps<Point2>)a).Overlaps(p));
            Assert.False(((IRelatableOverlaps<Point2>)b).Overlaps(p));
            Assert.False(((IRelatableOverlaps<Point2>)c).Overlaps(p));
        }
    }
}