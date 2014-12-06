using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public static class LinearIntersectionConsistencyFacts
    {
        private static readonly Point2 A = new Point2(0, 0);
        private static readonly Point2 B = new Point2(3, 3);
        private static readonly Point2 C = new Point2(1, 0);
        private static readonly Point2 D = new Point2(Math.Sqrt(3), 3);

        [Fact]
        public static void line_line_intersection_result()
        {
            var a = new Line2(A, B);
            var b = new Line2(C, D);

            var forward = a.Intersection(b);
            var reverse = b.Intersection(a);

            forward.Should().NotBeNull();
            forward.Should().Be(reverse);
        }

        [Fact]
        public static void ray_ray_intersection_result()
        {
            var a = new Ray2(A, B);
            var b = new Ray2(C, D);

            var forward = a.Intersection(b);
            var reverse = b.Intersection(a);

            forward.Should().NotBeNull();
            forward.Should().Be(reverse);
        }

        [Fact]
        public static void segment_segment_intersection_result()
        {
            var a = new Segment2(A, B);
            var b = new Segment2(C, D);

            var forward = a.Intersection(b);
            var reverse = b.Intersection(a);

            forward.Should().NotBeNull();
            forward.Should().Be(reverse);
        }

        [Fact]
        public static void line_ray_intersection_result()
        {
            var a1 = new Line2(A, B);
            var a2 = new Line2(B, A);
            var b = new Ray2(C, D);

            var forward1 = a1.Intersection(b);
            var forward2 = a2.Intersection(b);
            var reverse1 = b.Intersection(a1);
            var reverse2 = b.Intersection(a2);

            forward1.Should().NotBeNull();
            forward1.Should().Be(forward2);
            forward1.Should().Be(reverse1);
            forward1.Should().Be(reverse2);
        }

        [Fact]
        public static void line_segment_intersection_result()
        {
            var a1 = new Line2(A, B);
            var a2 = new Line2(B, A);
            var b1 = new Segment2(C, D);
            var b2 = new Segment2(D, C);

            var forward1 = a1.Intersection(b1);
            var forward2 = a2.Intersection(b2);
            var reverse1 = b1.Intersection(a1);
            var reverse2 = b2.Intersection(a2);

            forward1.Should().NotBeNull();
            forward1.Should().Be(forward2);
            forward1.Should().Be(reverse1);
            forward1.Should().Be(reverse2);
        }

        [Fact]
        public static void segment_ray_intersection_result()
        {
            var a1 = new Segment2(A, B);
            var a2 = new Segment2(B, A);
            var b = new Ray2(C, D);

            var forward1 = a1.Intersection(b);
            var forward2 = a2.Intersection(b);
            var reverse1 = b.Intersection(a1);
            var reverse2 = b.Intersection(a2);

            forward1.Should().NotBeNull();
            forward1.Should().Be(forward2);
            forward1.Should().Be(reverse1);
            forward1.Should().Be(reverse2);
        }

    }
}
