using System;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class Vector3Facts
    {

        [Fact]
        public void constructor_values() {
            var a = new Vector3(2, 3, 4);
            Assert.Equal(2, a.X);
            Assert.Equal(3, a.Y);
            Assert.Equal(4, a.Z);
        }

        [Fact]
        public void cast_point() {
            var a = new Vector3(2, 3, 4);
            Point3 b = a;
            Vector3 c = b;
            Assert.Equal(a.X, c.X);
            Assert.Equal(a.Y, c.Y);
            Assert.Equal(a.Z, c.Z);
        }

        [Fact]
        public void coordinate_triple_coordinate_get_test() {
            ICoordinateTriple<double> p = new Point3(3, 4, 5);
            var a = new Vector3(p);
            Assert.Equal(3, a.X);
            Assert.Equal(4, a.Y);
            Assert.Equal(5, a.Z);
        }

        [Fact]
        public void magnitude() {
            var a = new Vector3(3, 4, 5);
            Assert.Equal(Math.Sqrt(50), a.GetMagnitude());
            a = new Vector3(-3, 4, -5);
            Assert.Equal(Math.Sqrt(50), a.GetMagnitude());
        }

        [Fact]
        public void magnitude_squared() {
            var a = new Vector3(3, 4, -5);
            Assert.Equal(50, a.GetMagnitudeSquared());
            a = new Vector3(-3, 4, 5);
            Assert.Equal(50, a.GetMagnitudeSquared());
        }

        [Fact]
        public void compare_to() {
            var a = new Vector3(1, 1, 2);
            var b = new Vector3(3, 4, 5);
            Assert.True(a.CompareTo(b) < 0);
            Assert.True(b.CompareTo(a) > 0);
            Assert.NotEqual(a, b);
            b = new Vector3(1, 1, 1);
            Assert.True(b.CompareTo(a) < 0);
            Assert.True(a.CompareTo(b) > 0);
            Assert.NotEqual(a, b);
            b = new Vector3(1, 1, 2);
            Assert.Equal(0, a.CompareTo(b));
        }

        [Fact]
        public void equal_op() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(3, 4, 5);
            var c = new Vector3(3, 4, 5);

            Assert.False(a == b);
            Assert.False(a == c);
            Assert.False(b == a);
            Assert.True(b == c);
            Assert.False(c == a);
            Assert.True(c == b);
        }

        [Fact]
        public void inequal_op() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(3, 4, 5);
            var c = new Vector3(3, 4, 5);

            Assert.True(a != b);
            Assert.True(a != c);
            Assert.True(b != a);
            Assert.False(b != c);
            Assert.True(c != a);
            Assert.False(c != b);
        }

        [Fact]
        public void equal_point() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(3, 4, 5);
            var c = new Vector3(3, 4, 5);

            Assert.False(a.Equals(b));
            Assert.False(a.Equals(c));
            Assert.False(b.Equals(a));
            Assert.True(b.Equals(c));
            Assert.False(c.Equals(a));
            Assert.True(c.Equals(b));
        }

        [Fact]
        public void equal_coordinate_triple() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(3, 4, 5);
            var c = new Vector3(3, 4, 5);
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
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(3, 4, 5);
            var c = new Vector3(3, 4, 5);

            Assert.False(a.Equals((object)(new Point3(b))));
            Assert.False(a.Equals((object)(new Point3(c))));
            Assert.False(((object)b).Equals(a));
            Assert.True(((object)b).Equals(new Point3(c)));
            Assert.False(c.Equals((object)a));
            Assert.True(c.Equals((object)b));
        }

        [Fact]
        public void negative() {
            var v = new Vector3(1, 2, 3);

            v = v.GetNegative();

            Assert.Equal(-1, v.X);
            Assert.Equal(-2, v.Y);
            Assert.Equal(-3, v.Z);
        }

        [Fact]
        public void dot() {
            var a = new Vector3(5, 2, -1);
            var b = new Vector3(3, -3, 0);

            Assert.Equal(9, a.Dot(b));
            Assert.Equal(9, b.Dot(a));
        }

        [Fact]
        public void cross() {
            var a = new Vector3(3, -3, 1);
            var b = new Vector3(4, 9, 2);

            Assert.Equal(
                new Vector3(-15, -2, 39),
                a.Cross(b)
            );
            Assert.Equal(
                new Vector3(15, 2, -39),
                b.Cross(a)
            );
        }

        [Fact]
        public void normalized() {
            var v = new Vector3(3, 4, 5);
            v = v.GetNormalized();

            Assert.Equal(3.0 / Math.Sqrt(50), v.X);
            Assert.Equal(4.0 / Math.Sqrt(50), v.Y);
            Assert.Equal(5.0 / Math.Sqrt(50), v.Z);
        }

        [Fact]
        public void scaled() {
            var v = new Vector3(1.5, 2.9, 1);
            const double f = 1.23;

            Assert.Equal(v.X * f, v.GetScaled(f).X);
            Assert.Equal(v.Y * f, v.GetScaled(f).Y);
            Assert.Equal(v.Z * f, v.GetScaled(f).Z);
        }

        [Fact]
        public void divided() {
            var v = new Vector3(1.5, 2.9, -2.1);
            const double f = 1.23;

            Assert.Equal(v.X / f, v.GetDivided(f).X);
            Assert.Equal(v.Y / f, v.GetDivided(f).Y);
            Assert.Equal(v.Z / f, v.GetDivided(f).Z);
        }

        [Fact]
        public void add() {
            var a = new Vector3(1, 3, 5);
            var b = new Vector3(5, 7, 9);

            Assert.Equal(new Vector3(6, 10, 14), a.Add(b));
        }

        [Fact]
        public void subtract() {
            var a = new Vector3(1, 3, 2);
            var b = new Vector3(5, 9, 3);

            Assert.Equal(new Vector3(4, 6, 1), b.Difference(a));
        }

        [Fact]
        public void op_add() {
            var a = new Vector3(1, 3, 1);
            var b = new Vector3(5, 7, -1);

            Assert.Equal(new Vector3(6, 10, 0), a + b);
        }

        [Fact]
        public void op_subtract() {
            var a = new Vector3(1, 3, 9);
            var b = new Vector3(5, 9, 6);

            Assert.Equal(new Vector3(4, 6, -3), b - a);
        }
    }
}