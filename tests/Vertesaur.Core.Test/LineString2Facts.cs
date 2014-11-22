using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Vertesaur.Test
{
    public class LineString2Facts
    {
        private readonly Point2[] _points;

        public LineString2Facts() {
            _points = new[]{
			    new Point2(0,0),
			    new Point2(3,3),
			    new Point2(2,6),
			    new Point2(0,5),
			    new Point2(2,1),
			    new Point2(6,6)
		    };
        }

        [Fact]
        public void segment_count() {
            var a = new LineString2();
            var b = new LineString2(new[] { Point2.Zero });
            var c = new LineString2(new[] { Point2.Zero, Point2.Zero.Add(Vector2.XUnit) });
            var d = new LineString2(_points);

            a.SegmentCount.Should().Be(0);
            b.SegmentCount.Should().Be(0);
            c.SegmentCount.Should().Be(1);
            d.SegmentCount.Should().Be(_points.Length-1);
        }

        [Fact]
        public void magnitude() {
            var lineString = new LineString2(_points);

            var actual = lineString.GetMagnitude();

            actual.Should().Be(
                Enumerable.Range(0, lineString.SegmentCount)
                .Select(i => lineString.GetSegment(i))
                .Select(s => s.GetMagnitude())
                .Sum());
        }

        [Fact]
        public void magnitude_squared() {
            var lineString = new LineString2(_points);
            var expectedMagnitude = Enumerable.Range(0, lineString.SegmentCount)
                .Select(i => lineString.GetSegment(i))
                .Select(s => s.GetMagnitude())
                .Sum();

            var actual = lineString.GetMagnitudeSquared();

            actual.Should().Be(expectedMagnitude * expectedMagnitude);
        }

        [Fact]
        public void intersection_testing() {
            var lineString = new LineString2(_points);

            lineString.Intersects(new Point2(1, 3)).Should().BeTrue();
            lineString.Intersects(new Point2(6, 6)).Should().BeTrue();
            lineString.Intersects(new Point2(1, 5)).Should().BeFalse();
        }

        [Fact]
        public void get_segment() {
            var lineString = new LineString2(_points);

            lineString.GetSegment(1).B.Should().Be(_points[2]);
            lineString.GetSegment(2).A.Should().Be(_points[2]);
        }

        [Fact]
        public void get_mbr_basic() {
            var lineString = new LineString2(_points);

            var mbr = lineString.GetMbr();

            mbr.Should().Be(new Mbr(0, 0, 6, 6));
        }

        [Fact]
        public void centroid() {
            var lineString = new LineString2(_points);

            var centroid = lineString.GetCentroid();

            Assert.Equal(
                ((1.5 * 18.0) + (2.5 * 10.0) + (1 * 5.0) + (1 * 20.0) + (4 * 41.0)) / 94.0,
                centroid.X,
                10);
            Assert.Equal(
                ((1.5 * 18.0) + (4.5 * 10.0) + (5.5 * 5.0) + (3 * 20.0) + (3.5 * 41.0)) / 94.0,
                centroid.Y,
                10);
        }

        [Fact]
        public void distance() {
            var lineString = new LineString2(_points);

            _points.Select(lineString.Distance).Should().OnlyContain(x => x == 0);
            lineString.Distance(new Point2(1, 3)).Should().Be(0);
            lineString.Distance(new Point2(1, 5)).Should().Be(
                lineString.GetSegment(2).Distance(new Point2(1, 5)));
        }

        [Fact]
        public void distance_squared() {
            var lineString = new LineString2(_points);

            _points.Select(lineString.DistanceSquared).Should().OnlyContain(x => x == 0);
            lineString.DistanceSquared(new Point2(1, 3)).Should().Be(0);
            lineString.DistanceSquared(new Point2(1, 5)).Should().Be(
                lineString.GetSegment(2).DistanceSquared(new Point2(1, 5)));
        }

        [Fact]
        public void points_constructor() {
            var lineString = new LineString2(_points);

            lineString.Should().HaveCount(_points.Length);
            lineString.SegmentCount.Should().Be(_points.Length - 1);
            lineString.Should().Equal(_points);
        }

        [Fact]
        public void expected_size_constructor() {
            var lineString = new LineString2(10);

            lineString.Should().HaveCount(0);
            lineString.SegmentCount.Should().Be(0);
        }

        [Fact]
        public void default_constructor() {
            var lineString = new LineString2();

            lineString.Should().HaveCount(0);
            lineString.SegmentCount.Should().Be(0);
        }

    }
}