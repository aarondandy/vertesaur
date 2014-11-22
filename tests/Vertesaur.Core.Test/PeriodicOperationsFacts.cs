using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Vertesaur.Periodic;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public static class PeriodicOperationsFacts
    {

        [Fact]
        public static void zero_based_value_fix() {
            var op = new PeriodicOperations(0, 3);

            Assert.Equal(0, op.Fix(0));
            Assert.Equal(0, op.Fix(3));
            Assert.Equal(3, op.FixExcludingEnd(3));
            Assert.Equal(0, op.Fix(-3));
            Assert.Equal(0, op.Fix(6));
            Assert.Equal(0, op.Fix(-6));
            Assert.Equal(1, op.Fix(1));
            Assert.Equal(1, op.Fix(4));
            Assert.Equal(2, op.Fix(-1));
            Assert.Equal(2, op.Fix(-4));
        }

        [Fact]
        public static void longitude_value_fix() {
            var op = new PeriodicOperations(-180, 360);

            Assert.Equal(-180, op.Fix(180));
            Assert.Equal(180, op.FixExcludingEnd(180));
            Assert.Equal(-180, op.Fix(-540));
            Assert.Equal(-180, op.Fix(540));
            Assert.Equal(-180, op.Fix(-900));
            Assert.Equal(-100, op.Fix(-100));
            Assert.Equal(-80, op.Fix(280));
            Assert.Equal(80, op.Fix(-280));
            Assert.Equal(80, op.Fix(-640));
        }

        [Fact]
        public static void radian_value_fix() {
            var op = new PeriodicOperations(0, Math.PI + Math.PI);

            Assert.Equal(0, op.Fix(0));
            Assert.Equal(Math.PI, op.Fix(Math.PI));
            Assert.Equal(Math.PI, op.Fix(-Math.PI), 15);
            Assert.Equal(Math.PI, op.Fix(Math.PI * 3), 15);
        }

        [Fact]
        public static void intersects_regular_range_and_single_value() {
            var op = new PeriodicOperations(0, 5);

            Assert.True(op.Intersects(0, 2, 0));
            Assert.True(op.Intersects(0, 2, 1));
            Assert.True(op.Intersects(0, 2, 2));
            Assert.True(op.Intersects(0, 2, 5));
            Assert.True(op.Intersects(0, 1, 0));
            Assert.True(op.Intersects(0, 1, 1));
            Assert.False(op.Intersects(0, 1, 2));
            Assert.True(op.Intersects(0, 1, 5));
            Assert.False(op.Intersects(1, 3, 0));
            Assert.True(op.Intersects(1, 3, 2));
            Assert.False(op.Intersects(1, 3, 4));
        }

        [Fact]
        public static void intersects_reverse_range_and_single_value() {
            var op = new PeriodicOperations(0, 5);

            Assert.True(op.Intersects(2, 0, 0));
            Assert.False(op.Intersects(2, 0, 1));
            Assert.True(op.Intersects(2, 0, 2));
            Assert.True(op.Intersects(2, 0, 5));
            Assert.True(op.Intersects(1, 0, 0));
            Assert.True(op.Intersects(1, 0, 1));
            Assert.True(op.Intersects(1, 0, 2));
            Assert.True(op.Intersects(1, 0, 5));
            Assert.True(op.Intersects(3, 1, 0));
            Assert.False(op.Intersects(3, 1, 2));
            Assert.True(op.Intersects(3, 1, 4));
        }

        [Fact]
        public static void intersects_range_with_range() {
            var op = new PeriodicOperations(-180, 360);

            Assert.False(op.Intersects(0, 0, 10, 10));
            Assert.True(op.Intersects(0, 0, 0, 0));
            Assert.True(op.Intersects(10, 10, 10, 10));
            Assert.True(op.Intersects(-100, 30, 40, 0));
            Assert.False(op.Intersects(-100, 30, 40, -170));
            Assert.True(op.Intersects(-100, 30, 0, 0));
            Assert.True(op.Intersects(10, 10, -100, 30));
            Assert.False(op.Intersects(10, 10, 40, 0));
        }

        [Fact]
        public static void determine_range_value_distance() {
            var op = new PeriodicOperations(0, 5);

            Assert.Equal(0, op.Distance(0, 2, 0));
            Assert.Equal(0, op.Distance(0, 2, 1));
            Assert.Equal(0, op.Distance(0, 2, 2));
            Assert.Equal(0, op.Distance(0, 2, 5));
            Assert.Equal(0, op.Distance(0, 1, 0));
            Assert.Equal(0, op.Distance(0, 1, 1));
            Assert.Equal(1, op.Distance(0, 1, 2));
            Assert.Equal(0, op.Distance(0, 1, 5));
            Assert.Equal(1, op.Distance(1, 3, 0));
            Assert.Equal(0, op.Distance(1, 3, 2));
            Assert.Equal(1, op.Distance(1, 3, 4));
            Assert.Equal(0, op.Distance(2, 0, 0));
            Assert.Equal(1, op.Distance(2, 0, 1));
            Assert.Equal(0, op.Distance(2, 0, 2));
            Assert.Equal(0, op.Distance(2, 0, 5));
            Assert.Equal(0, op.Distance(1, 0, 0));
            Assert.Equal(0, op.Distance(1, 0, 1));
            Assert.Equal(0, op.Distance(1, 0, 2));
            Assert.Equal(0, op.Distance(1, 0, 5));
            Assert.Equal(0, op.Distance(3, 1, 0));
            Assert.Equal(1, op.Distance(3, 1, 2));
            Assert.Equal(0, op.Distance(3, 1, 4));
        }

        [Fact]
        public static void range_magnitude() {
            var op = new PeriodicOperations(0, 360);

            Assert.Equal(0, op.Magnitude(0, 0));
            Assert.Equal(2, op.Magnitude(-1, 1));
            Assert.Equal(345, op.Magnitude(5, -10));
        }

        [Fact]
        public static void value_value_distance() {
            var op = new PeriodicOperations(-180, 360);

            Assert.Equal(10, op.Distance(50, 60));
            Assert.Equal(10, op.Distance(60, 50));
            Assert.Equal(20, op.Distance(170, -170));
            Assert.Equal(20, op.Distance(-170, 170));
        }

        [Fact]
        public static void range_contains_range() {
            var op = new PeriodicOperations(-180, 360);

            Assert.False(op.Contains(0, 0, 10, 10));
            Assert.True(op.Contains(0, 0, 0, 0));
            Assert.True(op.Contains(10, 10, 10, 10));
            Assert.False(op.Contains(-100, 30, 40, 0));
            Assert.False(op.Contains(40, 0, -100, 30));
            Assert.True(op.Contains(-100, 30, 0, 0));
            Assert.False(op.Contains(0, 0, -100, 30));
            Assert.False(op.Contains(10, 10, -100, 30));
            Assert.True(op.Contains(-100, 30, 10, 10));
            Assert.False(op.Contains(10, 10, 40, 0));
            Assert.False(op.Contains(40, 0, 10, 10));
        }

        [Fact]
        public static void range_midpoint() {
            var op = new PeriodicOperations(-180, 360);

            Assert.Equal(10, op.CalculateMidpoint(10, 10));
            Assert.Equal(0, op.CalculateMidpoint(0, 0));
            Assert.Equal(-35, op.CalculateMidpoint(-100, 30));
            Assert.Equal(145, op.CalculateMidpoint(30, -100));
            Assert.Equal(20, op.CalculateMidpoint(0, 40));
            Assert.Equal(-160, op.CalculateMidpoint(40, 0));
        }

        [Fact]
        public static void small_area_within_full_longitude() {
            var op = new PeriodicOperations(-180, 360);

            Assert.True(op.Intersects(9, 23, -180, 180));
            Assert.True(op.Intersects(-180, 180, 9, 23));
        }

    }
}
