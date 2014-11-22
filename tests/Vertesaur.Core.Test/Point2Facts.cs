using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class Point2Facts
    {

        [Fact]
        public void constructor_default() {
            var p = new Point2();

            Assert.Equal(0, p.X);
            Assert.Equal(0, p.Y);
        }

        [Fact]
        public void cast_vector() {
            var ptA = new Point2(2, 3);

            Vector2 vecA = ptA;
            Point2 ptB = vecA;

            ptB.Should().Be(ptA);
        }

        [Fact]
        public void constructor_coordinates() {
            var p = new Point2(1, 2);

            Assert.Equal(1, p.X);
            Assert.Equal(2, p.Y);
        }

        [Fact]
        public void distance_point() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 5);

            var d1 = a.Distance(b);
            var d2 = b.Distance(a);

            d1.Should().Be(d2);
            d1.Should().Be(System.Math.Sqrt(13));
        }

        [Fact]
        public void distance_squared_point() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 5);

            var d1 = a.DistanceSquared(b);
            var d2 = b.DistanceSquared(a);

            d1.Should().Be(d2);
            d1.Should().Be(13);
        }

        [Fact]
        public void compare_to() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(1, 1);
            var d = new Point2(1, 2);

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
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);

            Assert.False(a == b);
            Assert.False(a == c);
            Assert.False(b == a);
            Assert.True(b == c);
            Assert.False(c == a);
            Assert.True(c == b);
        }

        [Fact]
        public void inequal_op() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);

            Assert.True(a != b);
            Assert.True(a != c);
            Assert.True(b != a);
            Assert.False(b != c);
            Assert.True(c != a);
            Assert.False(c != b);
        }

        [Fact]
        public void equal_type() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);

            Assert.False(a.Equals(b));
            Assert.False(a.Equals(c));
            Assert.False(b.Equals(a));
            Assert.True(b.Equals(c));
            Assert.False(c.Equals(a));
            Assert.True(c.Equals(b));
        }

        [Fact]
        public void equal_coordinate_pair() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);
            ICoordinatePair<double> nil = null;

            Assert.False(a.Equals((ICoordinatePair<double>)b));
            Assert.False(a.Equals((ICoordinatePair<double>)c));
            Assert.False(b.Equals((ICoordinatePair<double>)a));
            Assert.True(b.Equals((ICoordinatePair<double>)c));
            Assert.False(c.Equals((ICoordinatePair<double>)a));
            Assert.True(c.Equals((ICoordinatePair<double>)b));
            Assert.False(a.Equals(nil));
        }

        [Fact]
        public void equal_object() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);

            Assert.False(a.Equals((object)(new Vector2(b))));
            Assert.False(a.Equals((object)(new Vector2(c))));
            Assert.False(((object)b).Equals(a));
            Assert.True(((object)b).Equals(new Vector2(c)));
            Assert.False(c.Equals((object)a));
            Assert.True(c.Equals((object)b));
        }

        [Fact]
        public void intersection_point() {
            IRelatableIntersects<Point2> a = new Point2(1, 2);

            Assert.False(a.Intersects(new Point2(3, 4)));
            Assert.True(a.Intersects(new Point2(1, 2)));
            Assert.False(a.Intersects(new Point2(1, 3)));
        }

        [Fact]
        public void disjoint_point() {
            IRelatableDisjoint<Point2> a = new Point2(1, 2);

            Assert.True(a.Disjoint(new Point2(3, 4)));
            Assert.False(a.Disjoint(new Point2(1, 2)));
            Assert.True(a.Disjoint(new Point2(1, 3)));
        }

        [Fact]
        public void touches_point() {
            IRelatableTouches<Point2> a = new Point2(1, 2);

            Assert.False(a.Touches(new Point2(3, 4)));
            Assert.False(a.Touches(new Point2(1, 2)));
            Assert.False(a.Touches(new Point2(1, 3)));
        }

        [Fact]
        public void crosses_point() {
            IRelatableCrosses<Point2> a = new Point2(1, 2);

            Assert.False(a.Crosses(new Point2(3, 4)));
            Assert.False(a.Crosses(new Point2(1, 2)));
            Assert.False(a.Crosses(new Point2(1, 3)));
        }

        [Fact]
        public void within_point() {
            IRelatableWithin<Point2> a = new Point2(1, 2);

            Assert.False(a.Within(new Point2(3, 4)));
            Assert.True(a.Within(new Point2(1, 2)));
            Assert.False(a.Within(new Point2(1, 3)));
        }

        [Fact]
        public void contains_point() {
            IRelatableContains<Point2> a = new Point2(1, 2);

            Assert.False(a.Contains(new Point2(3, 4)));
            Assert.True(a.Contains(new Point2(1, 2)));
            Assert.False(a.Contains(new Point2(1, 3)));
        }

        [Fact]
        public void overlaps_point() {
            IRelatableOverlaps<Point2> a = new Point2(1, 2);

            Assert.False(a.Overlaps(new Point2(3, 4)));
            Assert.False(a.Overlaps(new Point2(1, 2)));
            Assert.False(a.Overlaps(new Point2(1, 3)));
        }

        [Fact]
        public void add() {
            var a = new Point2(1, 3);
            var v = new Vector2(2, 4);

            var sum = a.Add(v);

            Assert.Equal(a.X + v.X, sum.X);
            Assert.Equal(a.Y + v.Y, sum.Y);
        }

        [Fact]
        public void difference() {
            var a = new Point2(1, 3);
            var b = new Point2(2, 4);

            var diff = a.Difference(b);

            Assert.Equal(a.X - b.X, diff.X);
            Assert.Equal(a.Y - b.Y, diff.Y);
        }
    }
}
