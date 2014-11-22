using System;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class SpheroidEquatorialPolarFacts
    {
        [Fact]
        public void constructor() {
            var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
            Assert.Equal(3.0, obj.A);
            Assert.Equal(9.0 / 4.0, obj.B);

            obj = new SpheroidEquatorialPolar(4.0, 5.0);
            Assert.Equal(4.0, obj.A);
            Assert.Equal(5.0, obj.B);
        }

        [Fact]
        public void property_a() {
            var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);

            Assert.Equal(3.0, obj.A);
        }

        [Fact]
        public void property_b() {
            var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);

            Assert.Equal(9.0 / 4.0, obj.B);
        }

        [Fact]
        public void property_f() {
            var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);

            Assert.Equal(1.0 / 4.0, obj.F);
        }

        [Fact]
        public void property_invf() {
            var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);

            Assert.Equal(4.0, obj.InvF);
        }

        [Fact]
        public void property_e() {
            var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);

            Assert.Equal(Math.Sqrt(7.0 / 16.0), obj.E);
        }

        [Fact]
        public void property_esq() {
            var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);

            Assert.Equal(obj.E * obj.E, obj.ESquared, 10);
        }

        [Fact]
        public void property_e2() {
            var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);

            Assert.Equal(Math.Sqrt((0.4375 / (1 - 0.4375))), obj.ESecond);
        }

        [Fact]
        public void property_e2sq() {
            var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
            Assert.Equal(obj.ESecond * obj.ESecond, obj.ESecondSquared, 15);
        }
    }
}
