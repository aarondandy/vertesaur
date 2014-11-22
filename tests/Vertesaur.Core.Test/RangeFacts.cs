using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class RangeFacts
    {

        [Fact]
        public void constructor_two_values() {
            var a = new Range(0, 0);
            var b = new Range(1, 2);
            var c = new Range(4, 3);

            Assert.Equal(0, a.Low);
            Assert.Equal(0, a.High);
            Assert.Equal(1, b.Low);
            Assert.Equal(2, b.High);
            Assert.Equal(3, c.Low);
            Assert.Equal(4, c.High);
        }

        [Fact]
        public void midpoint() {
            var a = new Range(1, 3);
            var b = new Range(-1, 1);
            var c = new Range(2, -4);
            Assert.Equal(2, a.Mid);
            Assert.Equal(0, b.Mid);
            Assert.Equal(-1, c.Mid);
        }

        [Fact]
        public void magnitude() {
            var a = new Range(0, 0);
            var b = new Range(-1, 1);
            var c = new Range(5, -10);
            Assert.Equal(0, a.GetMagnitude());
            Assert.Equal(0, a.GetMagnitudeSquared());
            Assert.Equal(2, b.GetMagnitude());
            Assert.Equal(4, b.GetMagnitudeSquared());
            Assert.Equal(15, c.GetMagnitude());
            Assert.Equal(15 * 15, c.GetMagnitudeSquared());
        }

        [Fact]
        public void distance_value() {
            var a = new Range(0, 0);
            var b = new Range(-1, 1);
            var c = new Range(5, -10);

            Assert.Equal(0, a.Distance(0));
            Assert.Equal(2, a.Distance(2));
            Assert.Equal(0, b.Distance(1));
            Assert.Equal(3, b.Distance(-4));
            Assert.Equal(1, b.Distance(-2));
            Assert.Equal(1, b.Distance(2));
            Assert.Equal(0, c.Distance(1));
            Assert.Equal(0, c.Distance(3));
            Assert.Equal(3, c.Distance(8));
            Assert.Equal(10, c.Distance(-20));
        }

        [Fact]
        public void distance_squared_value() {
            var a = new Range(0, 0);
            var b = new Range(-1, 1);
            var c = new Range(5, -10);

            Assert.Equal(0, a.DistanceSquared(0));
            Assert.Equal(4, a.DistanceSquared(2));
            Assert.Equal(0, b.DistanceSquared(1));
            Assert.Equal(9, b.DistanceSquared(-4));
            Assert.Equal(1, b.DistanceSquared(-2));
            Assert.Equal(1, b.DistanceSquared(2));
            Assert.Equal(0, c.DistanceSquared(1));
            Assert.Equal(0, c.DistanceSquared(3));
            Assert.Equal(9, c.DistanceSquared(8));
            Assert.Equal(100, c.DistanceSquared(-20));
        }

        [Fact]
        public void distance_range() {
            var a = new Range(0, 0);
            var b = new Range(1, 1);
            var c = new Range(1, 2);
            var d = new Range(3, 2);
            var e = new Range(-2, -1);

            Assert.Equal(0, a.Distance(a));
            Assert.Equal(0, b.Distance(b));
            Assert.Equal(1, a.Distance(b));
            Assert.Equal(1, b.Distance(a));
            Assert.Equal(0, c.Distance(d));
            Assert.Equal(0, d.Distance(c));
            Assert.Equal(2, c.Distance(e));
            Assert.Equal(2, e.Distance(c));
        }

        [Fact]
        public void distance_squared_range() {
            var a = new Range(0, 0);
            var b = new Range(1, 1);
            var c = new Range(1, 2);
            var d = new Range(3, 2);
            var e = new Range(-2, -1);

            Assert.Equal(0, a.DistanceSquared(a));
            Assert.Equal(0, b.DistanceSquared(b));
            Assert.Equal(1, a.DistanceSquared(b));
            Assert.Equal(1, b.DistanceSquared(a));
            Assert.Equal(0, c.DistanceSquared(d));
            Assert.Equal(0, d.DistanceSquared(c));
            Assert.Equal(4, c.DistanceSquared(e));
            Assert.Equal(4, e.DistanceSquared(c));
        }

        [Fact]
        public void equal_range() {
            var a = new Range(0, 0);
            var b = new Range(1, 1);
            var c = new Range(1, 2);
            var d = new Range(3, 2);
            var e = new Range(2, 1);

            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
            Assert.True(c.Equals(c));
            Assert.True(d.Equals(d));
            Assert.False(c.Equals(d));
            Assert.False(d.Equals(c));
            Assert.True(c.Equals(c));
            Assert.True(e.Equals(e));
            Assert.True(c.Equals(e));
            Assert.True(e.Equals(c));
        }

        [Fact]
        public void equal_irange() {
            IRange<double> a = new Range(0, 0);
            IRange<double> b = new Range(1, 1);
            var c = new Range(1, 2);
            var d = new Range(3, 2);
            var e = new Range(2, 1);
            
            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
            Assert.True(c.Equals(c));
            Assert.True(d.Equals(d));
            Assert.False(c.Equals(d));
            Assert.False(d.Equals(c));
            Assert.True(c.Equals(c));
            Assert.True(e.Equals(e));
            Assert.True(c.Equals(e));
            Assert.True(e.Equals(c));
        }

        [Fact]
        public void equal_op() {
            var a = new Range(0, 0);
            var b = new Range(1, 1);
            var c = new Range(1, 2);
            var d = new Range(3, 2);
            var e = new Range(2, 1);

            Assert.False(a == b);
            Assert.False(b == a);
            Assert.False(c == d);
            Assert.False(d == c);
            Assert.True(c == e);
            Assert.True(e == c);
        }

        [Fact]
        public void inequal_op() {
            var a = new Range(0, 0);
            var b = new Range(1, 1);
            var c = new Range(1, 2);
            var d = new Range(3, 2);
            var e = new Range(2, 1);

            Assert.True(a != b);
            Assert.True(b != a);
            Assert.True(c != d);
            Assert.True(d != c);
            Assert.False(c != e);
            Assert.False(e != c);
        }

        [Fact]
        public void intersects_value() {
            var a = new Range(0, 0);
            var b = new Range(1, 1);
            var c = new Range(3, 1);

            Assert.True(a.Intersects(0));
            Assert.False(a.Intersects(1));
            Assert.False(b.Intersects(0));
            Assert.True(b.Intersects(1));
            Assert.False(c.Intersects(-1));
            Assert.False(c.Intersects(0));
            Assert.True(c.Intersects(1));
            Assert.True(c.Intersects(2));
            Assert.True(c.Intersects(3));
            Assert.False(c.Intersects(4));
            Assert.False(c.Intersects(-3));
        }

        [Fact]
        public void intersects_range() {
            var a = new Range(0, 0);
            var b = new Range(1, 1);
            var c = new Range(-1, 3);
            var d = new Range(4, 2);

            Assert.False(a.Intersects(b));
            Assert.True(a.Intersects(a));
            Assert.True(b.Intersects(b));
            Assert.True(c.Intersects(d));
            Assert.True(c.Intersects(a));
            Assert.True(b.Intersects(c));
            Assert.False(b.Intersects(d));
        }

        [Fact]
        public void within_value() {
            var a = new Range(0, 0);
            var b = new Range(1, 1);
            var c = new Range(3, 1);

            Assert.True(a.Within(0));
            Assert.False(a.Within(1));
            Assert.False(b.Within(0));
            Assert.True(b.Within(1));
            Assert.False(c.Within(-1));
            Assert.False(c.Within(0));
            Assert.False(c.Within(1));
            Assert.False(c.Within(2));
            Assert.False(c.Within(3));
            Assert.False(c.Within(4));
            Assert.False(c.Within(-3));
        }

        [Fact]
        public void within_range() {
            var a = new Range(0, 0);
            var b = new Range(1, 1);
            var c = new Range(-1, 3);
            var d = new Range(4, 2);

            Assert.False(a.Within(b));
            Assert.True(a.Within(a));
            Assert.True(b.Within(b));
            Assert.False(c.Within(d));
            Assert.False(c.Within(a));
            Assert.True(b.Within(c));
            Assert.False(b.Within(d));
            Assert.True(a.Within(c));
            Assert.False(a.Within(d));
            Assert.True(new Range(2, 3).Within(d));
        }

        [Fact]
        public void touches_value() {
            var a = new Range(1, 1);
            var b = new Range(1, 2);

            Assert.False(a.Touches(0));
            Assert.False(a.Touches(1));
            Assert.False(a.Touches(2));
            Assert.False(a.Touches(3));
            Assert.False(b.Touches(0));
            Assert.True(b.Touches(1));
            Assert.False(b.Touches(1.5));
            Assert.True(b.Touches(2));
            Assert.False(b.Touches(3));
        }

        [Fact]
        public void touches_range() {
            var a = new Range(1, 1);
            var b = new Range(1, 2);
            var c = new Range(2, 3);
            var d = new Range(2.1, 2.9);
            var e = new Range(1, 2.5);

            Assert.True(a.Touches(b));
            Assert.False(a.Touches(c));
            Assert.True(b.Touches(a));
            Assert.True(b.Touches(c));
            Assert.False(c.Touches(a));
            Assert.True(c.Touches(b));
            Assert.False(c.Touches(d));
            Assert.False(d.Touches(c));
            Assert.False(c.Touches(e));
            Assert.False(e.Touches(c));
        }

        [Fact]
        public void overlaps_value() {
            var a = new Range(1, 2);
            var b = new Range(3, 3);

            Assert.False(a.Overlaps(0));
            Assert.False(a.Overlaps(1));
            Assert.True(a.Overlaps(1.5));
            Assert.False(a.Overlaps(2));
            Assert.False(a.Overlaps(3));
            Assert.False(b.Overlaps(2));
            Assert.True(b.Overlaps(3));
            Assert.False(b.Overlaps(4));
        }

        [Fact]
        public void overlaps_range() {
            var a = new Range(1, 2);
            var b = new Range(3, 3);
            var c = new Range(0, 1.5);
            var d = new Range(1.5, 3);

            Assert.True(a.Overlaps(a));
            Assert.False(a.Overlaps(b));
            Assert.True(a.Overlaps(c));
            Assert.True(a.Overlaps(d));
            Assert.False(b.Overlaps(a));
            Assert.True(b.Overlaps(b));
            Assert.False(b.Overlaps(c));
            Assert.False(b.Overlaps(d));
            Assert.True(c.Overlaps(a));
            Assert.False(c.Overlaps(b));
            Assert.True(c.Overlaps(c));
            Assert.False(c.Overlaps(d));
            Assert.True(d.Overlaps(a));
            Assert.False(d.Overlaps(b));
            Assert.False(d.Overlaps(c));
            Assert.True(d.Overlaps(d));
        }

        [Fact]
        public void disjoint_value() {
            var a = new Range(1, 2);
            var b = new Range(3, 3);

            Assert.True(a.Disjoint(0));
            Assert.False(a.Disjoint(1));
            Assert.False(a.Disjoint(1.5));
            Assert.False(a.Disjoint(2));
            Assert.True(a.Disjoint(3));
            Assert.True(b.Disjoint(2));
            Assert.False(b.Disjoint(3));
            Assert.True(b.Disjoint(4));
        }

        [Fact]
        public void disjoint_range() {
            var a = new Range(1, 3);
            var b = new Range(1, 2);
            var c = new Range(2, 4);
            var d = new Range(3, 4);

            Assert.False(a.Disjoint(a));
            Assert.False(a.Disjoint(b));
            Assert.False(a.Disjoint(c));
            Assert.False(a.Disjoint(d));
            Assert.False(b.Disjoint(a));
            Assert.False(b.Disjoint(b));
            Assert.False(b.Disjoint(c));
            Assert.True(b.Disjoint(d));
            Assert.False(c.Disjoint(a));
            Assert.False(c.Disjoint(b));
            Assert.False(c.Disjoint(c));
            Assert.False(c.Disjoint(d));
            Assert.False(d.Disjoint(a));
            Assert.True(d.Disjoint(b));
            Assert.False(d.Disjoint(c));
            Assert.False(d.Disjoint(d));
        }

        [Fact]
        public void contains_value() {
            var a = new Range(1, 2);
            var b = new Range(3);

            Assert.False(a.Contains(0));
            Assert.True(a.Contains(1));
            Assert.True(a.Contains(1.5));
            Assert.True(a.Contains(2));
            Assert.False(a.Contains(3));
            Assert.False(b.Contains(2));
            Assert.True(b.Contains(3));
            Assert.False(b.Contains(4));
        }

        [Fact]
        public void contains_range() {
            var a = new Range(1, 5);
            var b = new Range(1, 3);
            var c = new Range(2, 5);
            var d = new Range(2, 4);
            var e = new Range(2, 6);
            var f = new Range(7, 9);

            Assert.True(a.Contains(a));
            Assert.True(a.Contains(b));
            Assert.False(b.Contains(a));
            Assert.True(a.Contains(c));
            Assert.False(c.Contains(a));
            Assert.True(a.Contains(d));
            Assert.False(d.Contains(a));
            Assert.False(a.Contains(e));
            Assert.False(e.Contains(a));
            Assert.False(a.Contains(f));
            Assert.False(f.Contains(a));
        }

        [Fact]
        public void crosses_value() {
            var a = new Range(1, 2);
            var b = new Range(3);

            Assert.False(a.Crosses(0));
            Assert.False(a.Crosses(1));
            Assert.True(a.Crosses(1.5));
            Assert.False(a.Crosses(2));
            Assert.False(a.Crosses(3));
            Assert.False(b.Crosses(2));
            Assert.False(b.Crosses(3));
            Assert.False(b.Crosses(4));
        }

        [Fact]
        public void crosses_range() {
            var a = new Range(1, 2);
            var b = new Range(2, 6);
            var c = new Range(3, 5);
            var d = new Range(5, 7);
            var e = new Range(3, 3);

            Assert.False(a.Crosses(a));
            Assert.False(a.Crosses(b));
            Assert.False(a.Crosses(c));
            Assert.False(a.Crosses(d));
            Assert.False(a.Crosses(e));
            Assert.False(b.Crosses(a));
            Assert.False(b.Crosses(b));
            Assert.False(b.Crosses(c));
            Assert.False(b.Crosses(d));
            Assert.True(b.Crosses(e));
            Assert.False(c.Crosses(a));
            Assert.False(c.Crosses(b));
            Assert.False(c.Crosses(c));
            Assert.False(c.Crosses(d));
            Assert.False(c.Crosses(e));
            Assert.False(d.Crosses(a));
            Assert.False(d.Crosses(b));
            Assert.False(d.Crosses(c));
            Assert.False(d.Crosses(d));
            Assert.False(d.Crosses(e));
            Assert.False(e.Crosses(a));
            Assert.True(e.Crosses(b));
            Assert.False(e.Crosses(c));
            Assert.False(e.Crosses(d));
            Assert.False(e.Crosses(e));
        }

        [Fact]
        public void centered() {
            Assert.Equal(new Range(1, 2), new Range(2, 3).GetCentered(1.5));
            Assert.Equal(new Range(-0.5, 0.5), new Range(2, 3).GetCentered(0));
            Assert.Equal(new Range(-3.5, -1.5), new Range(4, 6).GetCentered(-2.5));
        }

        [Fact]
        public void resized() {
            Assert.Equal(new Range(1.75, 3.25), new Range(2, 3).GetResized(1.5));
            Assert.Equal(new Range(2.5, 2.5), new Range(2, 3).GetResized(0));
            Assert.Equal(new Range(3.75, 6.25), new Range(4, 6).GetResized(-2.5));
            Assert.Equal(new Range(4.5, 5.5), new Range(4, 6).GetResized(1));
        }

        [Fact]
        public void equals_object() {
            var a = (object)new Range(0, 0);
            var b = (object)new Range(-1, 3);
            var c = (object)new Range(-1, 3);
            var d = b;
            var e = a;

            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
            Assert.True(a.Equals(e));
            Assert.True(b.Equals(d));
            Assert.False(a.Equals(c));
            Assert.True(b.Equals(c));
            Assert.False(a.Equals(null));
        }

        [Fact]
        public void emcompass_value_for_value_range() {
            var a = new Range(1);
            var b = new Range(2);
            var c = new Range(2);

            Assert.Equal(new Range(1, 2), a.Encompass(b));
            Assert.Equal(new Range(1, 2), b.Encompass(a));
            Assert.Equal(new Range(2), b.Encompass(c));
        }

        [Fact]
        public void encompass_value() {
            var a = new Range(1, 2);
            var b = new Range(1.5);
            var c = new Range(3);

            Assert.Equal(a, a.Encompass(b));
            Assert.Equal(a, b.Encompass(a));
            Assert.Equal(new Range(1.5, 3), b.Encompass(c));
            Assert.Equal(new Range(1.5, 3), c.Encompass(b));
            Assert.Equal(new Range(1, 3), a.Encompass(c));
            Assert.Equal(new Range(1, 3), c.Encompass(a));
        }

        [Fact]
        public void encompass_range() {
            var a = new Range(1, 2);
            var b = new Range(1.5, 2.5);
            var c = new Range(1.25, 1.75);
            var d = new Range(3, 4);

            Assert.Equal(new Range(1, 2.5), a.Encompass(b));
            Assert.Equal(new Range(1, 2.5), b.Encompass(a));
            Assert.Equal(a, a.Encompass(c));
            Assert.Equal(a, c.Encompass(a));
            Assert.Equal(new Range(1, 4), a.Encompass(d));
            Assert.Equal(new Range(1, 4), d.Encompass(a));
            Assert.Equal(new Range(1.25, 2.5), b.Encompass(c));
            Assert.Equal(new Range(1.25, 2.5), c.Encompass(b));
            Assert.Equal(new Range(1.5, 4), b.Encompass(d));
            Assert.Equal(new Range(1.5, 4), d.Encompass(b));
            Assert.Equal(new Range(1.25, 4), c.Encompass(d));
            Assert.Equal(new Range(1.25, 4), d.Encompass(c));
        }
    }
}
