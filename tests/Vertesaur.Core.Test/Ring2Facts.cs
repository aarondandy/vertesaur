using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class Ring2Facts
    {

        [Fact]
        public void constructor_default() {
            var ring = new Ring2();

            Assert.Empty(ring);
            Assert.Null(ring.Hole);
        }

        [Fact]
        public void constructor_hole() {
            var fill = new Ring2(false);
            var hole = new Ring2(true);

            Assert.Empty(fill);
            Assert.Empty(hole);
            fill.Hole.Should().Be(false);
            hole.Hole.Should().Be(true);
        }

        [Fact]
        public void constructor_expected_size() {
            var ring = new Ring2(12);

            Assert.Empty(ring);
            Assert.Null(ring.Hole);
        }

        [Fact]
        public void constructor_points() {
            var ring = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) });
            Assert.Equal(4, ring.Count);
            Assert.Equal(new Point2(0, 0), ring[0]);
            Assert.Equal(new Point2(1, 0), ring[1]);
            Assert.Equal(new Point2(0, 1), ring[2]);
            Assert.Equal(new Point2(0, 0), ring[3]);
        }

        [Fact]
        public void segments() {
            var a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) });
            var b = new Ring2();
            var c = new Ring2(new[] { new Point2(0, 0) });
            var d = new Ring2(new[] { new Point2(0, 0), new Point2(1, 1) });
            var e = new Ring2(new[] { new Point2(0, 0), new Point2(1, 1), new Point2(1, 0) });

            Assert.Equal(4, a.SegmentCount);
            Assert.Equal(new Segment2(new Point2(0, 0), new Point2(1, 0)), a.GetSegment(0));
            Assert.Equal(new Segment2(new Point2(1, 0), new Point2(0, 1)), a.GetSegment(1));
            Assert.Equal(new Segment2(new Point2(0, 1), new Point2(0, 0)), a.GetSegment(2));
            Assert.Equal(new Segment2(new Point2(0, 0), new Point2(0, 0)), a.GetSegment(3));
            Assert.Equal(0, b.SegmentCount);
            Assert.Equal(0, c.SegmentCount);
            Assert.Equal(1, d.SegmentCount);
            Assert.Equal(3, e.SegmentCount);
        }

        [Fact]
        public void redundant_endpoint() {
            var a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 1), new Point2(1, 0) });
            var b = new Ring2(new[] { new Point2(0, 0), new Point2(1, 1), new Point2(1, 0), new Point2(0, 0) });

            Assert.False(a.HasRedundantEndPoint);
            Assert.True(b.HasRedundantEndPoint);
        }

        [Fact]
        public void magnitude_or_perimeter() {
            var a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) });

            Assert.Equal(2 + System.Math.Sqrt(2), a.GetMagnitude());
        }

        [Fact]
        public void area () {
            var a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) });
            var b = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) }, true);

            Assert.Equal(0.5, a.GetArea());
            Assert.Equal(-0.5, b.GetArea());
        }

        [Fact]
        public void centroid() {
            var a = new Ring2(new[] {
				new Point2(0,0), 
				new Point2(1,0), 
				new Point2(1,.5), 
				new Point2(.5,.5), 
				new Point2(.5,.75), 
				new Point2(0,.75)});

            Assert.Equal(new Point2(9 / 20.0, 13 / 40.0), a.GetCentroid());
        }

        [Fact]
        public void winding_detection() {
            var a = new Ring2(new[] {
				new Point2(0,0), 
				new Point2(1,0), 
				new Point2(1,.5), 
				new Point2(.5,.5), 
				new Point2(.5,.75), 
				new Point2(0,.75)});
            var b = new Ring2(new[] { new Point2(0, 1), new Point2(1, 0), new Point2(0, 0) });
            var c = new Ring2(new[] { new Point2(0, 0) });
            var d = new Ring2(true);
            var e = new Ring2(false);
            var f = new Ring2();

            Assert.Equal(PointWinding.CounterClockwise, a.DetermineWinding());
            Assert.Equal(PointWinding.Clockwise, b.DetermineWinding());
            Assert.Equal(PointWinding.Unknown, c.DetermineWinding());
            Assert.Equal(PointWinding.Unknown, d.DetermineWinding());
            Assert.Equal(PointWinding.Unknown, e.DetermineWinding());
            Assert.Equal(PointWinding.Unknown, f.DetermineWinding());
        }

    }
}

