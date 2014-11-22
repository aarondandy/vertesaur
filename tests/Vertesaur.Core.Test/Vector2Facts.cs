using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class Vector2Facts
    {

        [Fact]
        public void constructor_coordinates() {
            var a = new Vector2(2, 3);

            Assert.Equal(2, a.X);
            Assert.Equal(3, a.Y);
        }

        [Fact]
        public void cast() {
            var a = new Vector2(2, 3);

            Point2 b = a;
            Vector2 c = b;

            Assert.Equal(a.X, c.X);
            Assert.Equal(a.Y, c.Y);
        }

        [Fact]
        public void coordinate_pair_get_elements() {
            ICoordinatePair<double> p = new Point2(3, 4);

            var a = new Vector2(p);

            Assert.Equal(3, a.X);
            Assert.Equal(4, a.Y);
        }

        [Fact]
        public void magnitude() {
            var a = new Vector2(3, 4);
            Assert.Equal(5, a.GetMagnitude());
            a = new Vector2(-3, 4);
            Assert.Equal(5, a.GetMagnitude());
        }

        [Fact]
        public void magnitude_squared() {
            var a = new Vector2(3, 4);
            Assert.Equal(25, a.GetMagnitudeSquared());
            a = new Vector2(-3, 4);
            Assert.Equal(25, a.GetMagnitudeSquared());
        }

        [Fact]
        public void compare_to() {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);
            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(a) > 0);
            Assert.NotEqual(a, b);
            b = new Vector2(1, 1);
            Assert.True(b.CompareTo(a) < 0);
            Assert.True(a.CompareTo(b) > 0);
            Assert.NotEqual(a, b);
            b = new Vector2(1, 2);
            Assert.Equal(0, a.CompareTo(b));
        }

        [Fact]
        public void equal_op() {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);
            var c = new Vector2(3, 4);
            Assert.False(a == b);
            Assert.False(a == c);
            Assert.False(b == a);
            Assert.True(b == c);
            Assert.False(c == a);
            Assert.True(c == b);
        }

        [Fact]
        public void inequal_op() {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);
            var c = new Vector2(3, 4);
            Assert.True(a != b);
            Assert.True(a != c);
            Assert.True(b != a);
            Assert.False(b != c);
            Assert.True(c != a);
            Assert.False(c != b);
        }

        [Fact]
        public void equal_point() {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);
            var c = new Vector2(3, 4);
            Assert.False(a.Equals(b));
            Assert.False(a.Equals(c));
            Assert.False(b.Equals(a));
            Assert.True(b.Equals(c));
            Assert.False(c.Equals(a));
            Assert.True(c.Equals(b));
        }

        [Fact]
        public void equal_coordinate_pair() {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);
            var c = new Vector2(3, 4);
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
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);
            var c = new Vector2(3, 4);
            Assert.False(a.Equals((object)(new Point2(b))));
            Assert.False(a.Equals((object)(new Point2(c))));
            Assert.False(((object)b).Equals(a));
            Assert.True(((object)b).Equals(new Point2(c)));
            Assert.False(c.Equals((object)a));
            Assert.True(c.Equals((object)b));
        }

        [Fact]
        public void negative() {
            var v = new Vector2(1, 2);
            v = v.GetNegative();
            Assert.Equal(-1, v.X);
            Assert.Equal(-2, v.Y);
        }

        [Fact]
        public void dot() {
            var a = new Vector2(5, 2);
            var b = new Vector2(3, -3);
            Assert.Equal(9, a.Dot(b));
            Assert.Equal(9, b.Dot(a));
        }

        [Fact]
        public void perpendicular_dot() {
            var a = new Vector2(5, 2);
            var b = new Vector2(3, -3);

            Assert.Equal(-21, a.PerpendicularDot(b));
            Assert.Equal(21, b.PerpendicularDot(a));
        }

        [Fact]
        public void normalized() {
            var v = new Vector2(3, 4);
            v = v.GetNormalized();

            Assert.Equal(3.0 / 5.0, v.X);
            Assert.Equal(4.0 / 5.0, v.Y);
        }

        [Fact]
        public void scaled() {
            var v = new Vector2(1.5, 2.9);
            const double f = 1.23;

            Assert.Equal(v.X * f, v.GetScaled(f).X);
            Assert.Equal(v.Y * f, v.GetScaled(f).Y);
        }

        [Fact]
        public void divided() {
            var v = new Vector2(1.5, 2.9);
            const double f = 1.23;

            Assert.Equal(v.X / f, v.GetDivided(f).X);
            Assert.Equal(v.Y / f, v.GetDivided(f).Y);
        }

        [Fact]
        public void add() {
            var a = new Vector2(1, 3);
            var b = new Vector2(5, 7);

            Assert.Equal(new Vector2(6, 10), a.Add(b));
        }

        [Fact]
        public void subtracted() {
            var a = new Vector2(1, 3);
            var b = new Vector2(5, 9);

            Assert.Equal(new Vector2(4, 6), b.Difference(a));
        }

        [Fact]
        public void add_op() {
            var a = new Vector2(1, 3);
            var b = new Vector2(5, 7);
            Assert.Equal(new Vector2(6, 10), a + b);
        }

        [Fact]
        public void subtract_op() {
            var a = new Vector2(1, 3);
            var b = new Vector2(5, 9);
            Assert.Equal(new Vector2(4, 6), b - a);
        }

        [Fact]
        public void perpendicular_clockwise() {
            var a = new Vector2(1, 2);
            a = a.GetPerpendicularClockwise();
            Assert.Equal(new Vector2(2, -1), a);
            a = a.GetPerpendicularClockwise();
            Assert.Equal(new Vector2(-1, -2), a);
            a = a.GetPerpendicularClockwise();
            Assert.Equal(new Vector2(-2, 1), a);
            a = a.GetPerpendicularClockwise();
            Assert.Equal(new Vector2(1, 2), a);
        }

        [Fact]
        public void perpendicular_counter_clockwise() {
            var a = new Vector2(1, 2);
            a = a.GetPerpendicularCounterClockwise();
            Assert.Equal(new Vector2(-2, 1), a);
            a = a.GetPerpendicularCounterClockwise();
            Assert.Equal(new Vector2(-1, -2), a);
            a = a.GetPerpendicularCounterClockwise();
            Assert.Equal(new Vector2(2, -1), a);
            a = a.GetPerpendicularCounterClockwise();
            Assert.Equal(new Vector2(1, 2), a);
        }

    }
}