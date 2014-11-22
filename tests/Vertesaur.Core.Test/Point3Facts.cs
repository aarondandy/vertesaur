using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class Point3Facts
    {

        [Fact]
        public void constructor_default() {
            var p = new Point3();

            Assert.Equal(0, p.X);
            Assert.Equal(0, p.Y);
            Assert.Equal(0, p.Z);
        }

        [Fact]
        public void cast_vector() {
            var a = new Point3(2, 3, 4);

            Vector3 b = a;
            Point3 c = b;

            Assert.Equal(a.X, c.X);
            Assert.Equal(a.Y, c.Y);
        }

        [Fact]
        public void constructor_coordinates() {
            var p = new Point3(1, 2, 3);

            Assert.Equal(1, p.X);
            Assert.Equal(2, p.Y);
            Assert.Equal(3, p.Z);
        }

        [Fact]
        public void distance_point() {
            var a = new Point3(1, 2, 5);
            var b = new Point3(3, 5, 1);

            var d1 = a.Distance(b);
            var d2 = b.Distance(a);

            d1.Should().Be(d2);
            d1.Should().Be(System.Math.Sqrt(29));
        }

        [Fact]
        public void distance_squared_point() {
            var a = new Point3(1, 2, 5);
            var b = new Point3(3, 5, 1);

            var d1 = a.DistanceSquared(b);
            var d2 = b.DistanceSquared(a);

            d1.Should().Be(d2);
            d1.Should().Be(29);
        }

        [Fact]
        public void compare_to() {
            var a = new Point3(1, 2, 0);
            var b = new Point3(3, 4, 10);
            var d = new Point3(1, 2, 0);
            var c = new Point3(1, 1, 1);

            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(a) > 0);
            Assert.NotEqual(a, b);
            Assert.True(c.CompareTo(a) < 0);
            Assert.True(a.CompareTo(c) > 0);
            Assert.NotEqual(a, c);
            Assert.Equal(0, a.CompareTo(d));
        }

        [Fact]
        public void equal_op() {
            var a = new Point3(1, 2, 1);
            var b = new Point3(3, 4, 2);
            var c = new Point3(3, 4, 2);

            Assert.False(a == b);
            Assert.False(a == c);
            Assert.False(b == a);
            Assert.True(b == c);
            Assert.False(c == a);
            Assert.True(c == b);
        }

        [Fact]
        public void inequal_op() {
            var a = new Point3(1, 2, 4);
            var b = new Point3(3, 4, 9);
            var c = new Point3(3, 4, 9);

            Assert.True(a != b);
            Assert.True(a != c);
            Assert.True(b != a);
            Assert.False(b != c);
            Assert.True(c != a);
            Assert.False(c != b);
        }

        [Fact]
        public void equal_typed() {
            var a = new Point3(1, 2, 9);
            var b = new Point3(3, 4, 6);
            var c = new Point3(3, 4, 6);

            Assert.False(a.Equals(b));
            Assert.False(a.Equals(c));
            Assert.False(b.Equals(a));
            Assert.True(b.Equals(c));
            Assert.False(c.Equals(a));
            Assert.True(c.Equals(b));
        }

        [Fact]
        public void equal_coordtriple() {
            var a = new Point3(1, 2, 0);
            var b = new Point3(3, 4, 1);
            var c = new Point3(3, 4, 1);
            ICoordinateTriple<double> nil = null;

            Assert.False(a.Equals((ICoordinateTriple<double>)b));
            Assert.False(a.Equals((ICoordinateTriple<double>)c));
            Assert.False(b.Equals((ICoordinateTriple<double>)a));
            Assert.True(b.Equals((ICoordinateTriple<double>)c));
            Assert.False(c.Equals((ICoordinateTriple<double>)a));
            Assert.True(c.Equals((ICoordinateTriple<double>)b));
            Assert.False(a.Equals(nil));
        }

        [Fact]
        public void equal_object() {
            var a = new Point3(1, 2, 0);
            var b = new Point3(3, 4, 9);
            var c = new Point3(3, 4, 9);

            Assert.False(a.Equals((object)(new Vector3(b))));
            Assert.False(a.Equals((object)(new Vector3(c))));
            Assert.False(((object)b).Equals(a));
            Assert.True(((object)b).Equals(new Vector3(c)));
            Assert.False(c.Equals((object)a));
            Assert.True(c.Equals((object)b));
        }

        [Fact]
        public void intersects_point_test() {
            IRelatableIntersects<Point3> a = new Point3(1, 2, 3);

            Assert.False(a.Intersects(new Point3(3, 4, 5)));
            Assert.True(a.Intersects(new Point3(1, 2, 3)));
            Assert.False(a.Intersects(new Point3(1, 3, 5)));
        }

        [Fact]
        public void disjoin_point() {
            IRelatableDisjoint<Point3> a = new Point3(1, 2, 3);

            Assert.True(a.Disjoint(new Point3(3, 4, 5)));
            Assert.False(a.Disjoint(new Point3(1, 2, 3)));
            Assert.True(a.Disjoint(new Point3(1, 3, 5)));
        }

        [Fact]
        public void touches_point() {
            IRelatableTouches<Point3> a = new Point3(1, 2, 3);

            Assert.False(a.Touches(new Point3(3, 4, 5)));
            Assert.False(a.Touches(new Point3(1, 2, 3)));
            Assert.False(a.Touches(new Point3(1, 3, 5)));
        }

        [Fact]
        public void crosses_point() {
            IRelatableCrosses<Point3> a = new Point3(1, 2, 3);

            Assert.False(a.Crosses(new Point3(3, 4, 5)));
            Assert.False(a.Crosses(new Point3(1, 2, 3)));
            Assert.False(a.Crosses(new Point3(1, 3, 5)));
        }

        [Fact]
        public void within_point() {
            IRelatableWithin<Point3> a = new Point3(1, 2, 3);

            Assert.False(a.Within(new Point3(3, 4, 5)));
            Assert.True(a.Within(new Point3(1, 2, 3)));
            Assert.False(a.Within(new Point3(1, 3, 5)));
        }

        [Fact]
        public void contains_point() {
            IRelatableContains<Point3> a = new Point3(1, 2, 3);

            Assert.False(a.Contains(new Point3(3, 4, 5)));
            Assert.True(a.Contains(new Point3(1, 2, 3)));
            Assert.False(a.Contains(new Point3(1, 3, 5)));
        }

        [Fact]
        public void overlaps_point() {
            IRelatableOverlaps<Point3> a = new Point3(1, 2, 3);

            Assert.False(a.Overlaps(new Point3(3, 4, 5)));
            Assert.False(a.Overlaps(new Point3(1, 2, 3)));
            Assert.False(a.Overlaps(new Point3(1, 3, 5)));
        }

        [Fact]
        public void add() {
            var a = new Point3(1, 3, 5);
            var v = new Vector3(2, 4, 6);

            var b = a.Add(v);

            Assert.Equal(a.X + v.X, b.X);
            Assert.Equal(a.Y + v.Y, b.Y);
            Assert.Equal(a.Z + v.Z, b.Z);
        }

        [Fact]
        public void difference() {
            var a = new Point3(1, 3, 5);
            var b = new Point3(2, 4, 6);

            var d = a.Difference(b);

            Assert.Equal(a.X - b.X, d.X);
            Assert.Equal(a.Y - b.Y, d.Y);
            Assert.Equal(a.Z - b.Z, d.Z);
        }
    }
}