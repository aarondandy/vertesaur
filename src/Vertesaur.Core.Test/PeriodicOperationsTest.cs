using System;
using NUnit.Framework;
using Vertesaur.Periodic;

namespace Vertesaur.Core.Test
{
    [TestFixture]
    public class PeriodicOperationsTest
    {

        [Test]
        public void zero_based_value_fix() {
            var op = new PeriodicOperations(0, 3);

            Assert.AreEqual(0, op.Fix(0));

            Assert.AreEqual(0, op.Fix(3));
            Assert.AreEqual(3, op.FixExcludingEnd(3));
            Assert.AreEqual(0, op.Fix(-3));
            Assert.AreEqual(0, op.Fix(6));
            Assert.AreEqual(0, op.Fix(-6));

            Assert.AreEqual(1, op.Fix(1));
            Assert.AreEqual(1, op.Fix(4));
            Assert.AreEqual(2, op.Fix(-1));
            Assert.AreEqual(2, op.Fix(-4));
        }

        [Test]
        public void longitude_value_fix() {
            var op = new PeriodicOperations(-180, 360);

            Assert.AreEqual(-180, op.Fix(180));
            Assert.AreEqual(180, op.FixExcludingEnd(180));
            Assert.AreEqual(-180, op.Fix(-540));
            Assert.AreEqual(-180, op.Fix(540));
            Assert.AreEqual(-180, op.Fix(-900));

            Assert.AreEqual(-100, op.Fix(-100));
            Assert.AreEqual(-80, op.Fix(280));
            Assert.AreEqual(80, op.Fix(-280));
            Assert.AreEqual(80, op.Fix(-640));
        }

        [Test]
        public void radian_value_fix() {
            var op = new PeriodicOperations(0, Math.PI + Math.PI);

            Assert.AreEqual(0, op.Fix(0));
            Assert.AreEqual(Math.PI, op.Fix(Math.PI));
            Assert.AreEqual(Math.PI, op.Fix(-Math.PI), 0.001);
            Assert.AreEqual(Math.PI, op.Fix(Math.PI * 3), 0.001);
        }

        [Test]
        public void intersects_regular_range_and_single_value() {
            var op = new PeriodicOperations(0, 5);

            Assert.IsTrue(op.Intersects(0, 2, 0));
            Assert.IsTrue(op.Intersects(0, 2, 1));
            Assert.IsTrue(op.Intersects(0, 2, 2));
            Assert.IsTrue(op.Intersects(0, 2, 5));

            Assert.IsTrue(op.Intersects(0, 1, 0));
            Assert.IsTrue(op.Intersects(0, 1, 1));
            Assert.IsFalse(op.Intersects(0, 1, 2));
            Assert.IsTrue(op.Intersects(0, 1, 5));

            Assert.IsFalse(op.Intersects(1, 3, 0));
            Assert.IsTrue(op.Intersects(1, 3, 2));
            Assert.IsFalse(op.Intersects(1, 3, 4));
        }

        [Test]
        public void intersects_reverse_range_and_single_value() {
            var op = new PeriodicOperations(0, 5);

            Assert.IsTrue(op.Intersects(2, 0, 0));
            Assert.IsFalse(op.Intersects(2, 0, 1));
            Assert.IsTrue(op.Intersects(2, 0, 2));
            Assert.IsTrue(op.Intersects(2, 0, 5));

            Assert.IsTrue(op.Intersects(1, 0, 0));
            Assert.IsTrue(op.Intersects(1, 0, 1));
            Assert.IsTrue(op.Intersects(1, 0, 2));
            Assert.IsTrue(op.Intersects(1, 0, 5));

            Assert.IsTrue(op.Intersects(3, 1, 0));
            Assert.IsFalse(op.Intersects(3, 1, 2));
            Assert.IsTrue(op.Intersects(3, 1, 4));
        }

        [Test]
        public void intersects_range_with_range() {
            var op = new PeriodicOperations(-180, 360);

            Assert.IsFalse(op.Intersects(0, 0, 10, 10));
            Assert.IsTrue(op.Intersects(0, 0, 0, 0));
            Assert.IsTrue(op.Intersects(10, 10, 10, 10));
            Assert.IsTrue(op.Intersects(-100, 30, 40, 0));
            Assert.IsFalse(op.Intersects(-100, 30, 40, -170));
            Assert.IsTrue(op.Intersects(-100, 30, 0, 0));
            Assert.IsTrue(op.Intersects(10, 10, -100, 30));
            Assert.IsFalse(op.Intersects(10, 10, 40, 0));
        }

        [Test]
        public void determine_range_value_distance() {
            var op = new PeriodicOperations(0, 5);

            Assert.AreEqual(0, op.Distance(0, 2, 0));
            Assert.AreEqual(0, op.Distance(0, 2, 1));
            Assert.AreEqual(0, op.Distance(0, 2, 2));
            Assert.AreEqual(0, op.Distance(0, 2, 5));

            Assert.AreEqual(0, op.Distance(0, 1, 0));
            Assert.AreEqual(0, op.Distance(0, 1, 1));
            Assert.AreEqual(1, op.Distance(0, 1, 2));
            Assert.AreEqual(0, op.Distance(0, 1, 5));

            Assert.AreEqual(1, op.Distance(1, 3, 0));
            Assert.AreEqual(0, op.Distance(1, 3, 2));
            Assert.AreEqual(1, op.Distance(1, 3, 4));

            Assert.AreEqual(0, op.Distance(2, 0, 0));
            Assert.AreEqual(1, op.Distance(2, 0, 1));
            Assert.AreEqual(0, op.Distance(2, 0, 2));
            Assert.AreEqual(0, op.Distance(2, 0, 5));

            Assert.AreEqual(0, op.Distance(1, 0, 0));
            Assert.AreEqual(0, op.Distance(1, 0, 1));
            Assert.AreEqual(0, op.Distance(1, 0, 2));
            Assert.AreEqual(0, op.Distance(1, 0, 5));

            Assert.AreEqual(0, op.Distance(3, 1, 0));
            Assert.AreEqual(1, op.Distance(3, 1, 2));
            Assert.AreEqual(0, op.Distance(3, 1, 4));
        }

        [Test]
        public void range_magnitude() {
            var op = new PeriodicOperations(0, 360);
            Assert.AreEqual(0, op.Magnitude(0, 0));
            Assert.AreEqual(2, op.Magnitude(-1, 1));
            Assert.AreEqual(345, op.Magnitude(5, -10));
        }

        [Test]
        public void value_value_distance() {
            var op = new PeriodicOperations(-180, 360);
            Assert.AreEqual(10, op.Distance(50, 60));
            Assert.AreEqual(10, op.Distance(60, 50));
            Assert.AreEqual(20, op.Distance(170, -170));
            Assert.AreEqual(20, op.Distance(-170, 170));
        }

        [Test]
        public void range_contains_range() {
            var op = new PeriodicOperations(-180, 360);
            Assert.IsFalse(op.Contains(0, 0, 10, 10));
            Assert.IsTrue(op.Contains(0, 0, 0, 0));
            Assert.IsTrue(op.Contains(10, 10, 10, 10));
            Assert.IsFalse(op.Contains(-100, 30, 40, 0));
            Assert.IsFalse(op.Contains(40, 0, -100, 30));
            Assert.IsTrue(op.Contains(-100, 30, 0, 0));
            Assert.IsFalse(op.Contains(0, 0, -100, 30));
            Assert.IsFalse(op.Contains(10, 10, -100, 30));
            Assert.IsTrue(op.Contains(-100, 30, 10, 10));
            Assert.IsFalse(op.Contains(10, 10, 40, 0));
            Assert.IsFalse(op.Contains(40, 0, 10, 10));
        }

        [Test]
        public void range_midpoint() {
            var op = new PeriodicOperations(-180, 360);
            Assert.AreEqual(10, op.CalculateMidpoint(10, 10));
            Assert.AreEqual(0, op.CalculateMidpoint(0, 0));
            Assert.AreEqual(-35, op.CalculateMidpoint(-100, 30));
            Assert.AreEqual(145, op.CalculateMidpoint(30, -100));
            Assert.AreEqual(20, op.CalculateMidpoint(0, 40));
            Assert.AreEqual(-160, op.CalculateMidpoint(40, 0));
        }

        [Test]
        public void small_area_within_full_longitude() {
            var op = new PeriodicOperations(-180, 360);
            Assert.IsTrue(op.Intersects(9, 23, -180, 180));
            Assert.IsTrue(op.Intersects(-180, 180, 9, 23));
        }

    }
}
