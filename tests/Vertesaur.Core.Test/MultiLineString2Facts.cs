using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public static class MultilineString2Facts
    {
        private static readonly List<LineString2> _lineStrings;

        static MultilineString2Facts() {
            _lineStrings = new[] {
				new[]{new Point2(0,2),new Point2(0,1),new Point2(1,0),new Point2(6,0)},
				new[]{new Point2(2,5),new Point2(2,2),new Point2(4,2),new Point2(4,-1)},
				new[]{new Point2(4,2),new Point2(6,2),new Point2(3,-1)}
			}
            .Select(points => new LineString2(points))
            .ToList();
        }

        [Fact]
        public static void magnitude() {
            var mls = new MultiLineString2(_lineStrings);
            var expected = mls.Sum(ls => ls.GetMagnitude());

            var actual = mls.GetMagnitude();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void magnitude_squared() {
            var mls = new MultiLineString2(_lineStrings);
            var expected = mls.Sum(ls => ls.GetMagnitude());
            expected = expected * expected;

            var actual = mls.GetMagnitudeSquared();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void intersects_point() {
            var mls = new MultiLineString2(_lineStrings);

            Assert.False(mls.Intersects(new Point2(1, 2)));
            Assert.False(mls.Intersects(new Point2(3, 3)));
            Assert.True(mls.Intersects(new Point2(1, 0)));
            Assert.True(mls.Intersects(new Point2(5, 1)));
            _lineStrings.SelectMany(ls => ls).All(p => mls.Intersects(p)).Should().BeTrue();
        }

        [Fact]
        public static void mbr() {
            var mls = new MultiLineString2(_lineStrings);
            var expected = mls.Skip(1).Aggregate(
                new Mbr(mls.First().GetMbr()),
                (mbr, l) => mbr.Encompass(l.GetMbr()));

            var actual = mls.GetMbr();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void centroid() {
            var mls = new MultiLineString2(_lineStrings);
            var magnitudeSum = 0.0;
            var expected = (Point2)mls.Aggregate(new Vector2(), (p, ls) => {
                var magnitude = ls.GetMagnitude();
                magnitudeSum += magnitude;
                var centroid = ls.GetCentroid();
                return p + (new Vector2(centroid.X, centroid.Y).GetScaled(magnitude));
            }).GetDivided(magnitudeSum);

            var actual = mls.GetCentroid();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void distance_point() {
            var mls = new MultiLineString2(_lineStrings);

            Assert.Equal(1, mls.Distance(new Point2(1, 2)));
            Assert.Equal(1, mls.Distance(new Point2(3, 3)));
            Assert.Equal(0, mls.Distance(new Point2(1, 0)));
            Assert.Equal(0, mls.Distance(new Point2(5, 1)));
            _lineStrings.SelectMany(ls => ls).Select(p => mls.Distance(p)).Should().OnlyContain(x => x == 0);
        }

        [Fact]
        public static void distance_squared_point() {
            var mls = new MultiLineString2(_lineStrings);

            Assert.Equal(1, mls.DistanceSquared(new Point2(1, 2)));
            Assert.Equal(1, mls.DistanceSquared(new Point2(3, 3)));
            Assert.Equal(0, mls.DistanceSquared(new Point2(1, 0)));
            Assert.Equal(0, mls.DistanceSquared(new Point2(5, 1)));
            _lineStrings.SelectMany(ls => ls).Select(p => mls.DistanceSquared(p)).Should().OnlyContain(x => x == 0);
        }

        [Fact]
        public static void constructor_default() {
            var mls = new MultiLineString2();

            Assert.Equal(0, mls.Count);
            mls.GetMagnitude().Should().Be(Double.NaN);
        }

        [Fact]
        public static void constructor_expected_capacity() {
            var mls = new MultiLineString2(10);

            Assert.Equal(0, mls.Count);
            mls.GetMagnitude().Should().Be(Double.NaN);
        }

        [Fact]
        public static void constructor_copy() {
            var mls = new MultiLineString2(_lineStrings);

            Assert.Equal(3, mls.Count);
            for (int i = 0; i < _lineStrings.Count; i++) {
                Assert.Equal(_lineStrings[i].Count, mls[i].Count);
            }
        }
    }
}