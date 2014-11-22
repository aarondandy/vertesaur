using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public static class Multipoint2Facts
    {

        private static readonly Point2[] _points;

        static Multipoint2Facts() {
            _points = new[]{
			    new Point2(0,0),
			    new Point2(-1,-1),
			    new Point2(2,1),
			    new Point2(0,4) };
        }

        [Fact]
        public static void instersects_tests() {
            var mp = new MultiPoint2(_points);

            _points
                .All(mp.Intersects)
                .Should().BeTrue();
            _points
                .Where(p => p.X != p.Y)
                .Select(p => new Point2(p.Y, p.X))
                .Any(p => mp.Intersects(p))
                .Should()
                .BeFalse();
        }

        [Fact]
        public static void mbr() {
            var mp = new MultiPoint2(_points);
            var expected = mp.Aggregate(new Mbr(mp[0]), (mbr, p) => mbr.Encompass(p.GetMbr()));

            var actual = mp.GetMbr();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void centroid() {
            var target = new MultiPoint2(_points);

            var centroid = target.GetCentroid();

            Assert.Equal(new Point2(.25, 1), centroid);
        }

        [Fact]
        public static void distance() {
            var mp = new MultiPoint2(_points);

            _points.Select(mp.Distance).Should().OnlyContain(x => x == 0);
            Assert.Equal(System.Math.Sqrt(5), mp.Distance(new Point2(-1, 2)));
            Assert.Equal(System.Math.Sqrt(4), mp.Distance(new Point2(0, 2)));
            Assert.Equal(System.Math.Sqrt(2), mp.Distance(new Point2(1, 2)));
            Assert.Equal(1, mp.Distance(new Point2(2, 2)));
        }

        [Fact]
        public static void distance_squared() {
            var mp = new MultiPoint2(_points);

            _points.Select(mp.DistanceSquared).Should().OnlyContain(x => x == 0);
            Assert.Equal(5, mp.DistanceSquared(new Point2(-1, 2)));
            Assert.Equal(4, mp.DistanceSquared(new Point2(0, 2)));
            Assert.Equal(2, mp.DistanceSquared(new Point2(1, 2)));
            Assert.Equal(1, mp.DistanceSquared(new Point2(2, 2)));
        }

        [Fact]
        public static void constructor_points() {
            var mp = new MultiPoint2(_points);

            Assert.Equal(_points.Length, mp.Count);
        }

        [Fact]
        public static void constructor_expected_size() {
            var mp = new MultiPoint2(10);

            Assert.Equal(0, mp.Count);
        }

        [Fact]
        public static void constructor_default() {
            var mp = new MultiPoint2();

            Assert.Equal(0, mp.Count);
        }

    }
}
