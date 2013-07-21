// ===============================================================================
//
// Copyright (c) 2011 Aaron Dandy 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// ===============================================================================

using System;
using System.Diagnostics.Contracts;

namespace Vertesaur.SegmentOperation
{

    /// <summary>
    /// An operation that will find the geometric result of intersecting two segments.
    /// </summary>
    public static class SegmentIntersectionOperation
    {

        /// <summary>
        /// Classification for the various types of segment intersection.
        /// </summary>
        /// <remarks>
        /// In the event of an intersection result being a segment rather than a point,
        /// multiple flags may be used. Ex:
        /// <c>isecType = SegmentIntersectionType.Head | SegmentIntersectionType.Range</c>.
        /// </remarks>
        [Flags]
        public enum SegmentIntersectionType
        {
            /// <summary>
            /// There is no intersection.
            /// </summary>
            None = 0,
            /// <summary>
            /// The intersection occurs at the head (second point) of a segment.
            /// </summary>
            Head = 1,
            /// <summary>
            /// The intersection occurs at the butt (first point) of a segment.
            /// </summary>
            Butt = 2,
            /// <summary>
            /// The intersection occurs at some place between the ends of the segment.
            /// </summary>
            Range = 4
        }

        [Pure]
        private static SegmentIntersectionType ClassifyIntersectionType(double v) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (v == 0)
                return SegmentIntersectionType.Butt;
            if (v == 1)
                return SegmentIntersectionType.Head;
            if (v > 0 && v < 1)
                return SegmentIntersectionType.Range;
            return SegmentIntersectionType.None;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// A segment intersection result.
        /// </summary>
        public interface IResult
        {
            /// <summary>
            /// The geometry of the result.
            /// </summary>
            IPlanarGeometry Geometry { get; }
        }

        /// <summary>
        /// A point intersection result.
        /// </summary>
        public struct PointResult : IResult
        {
            /// <summary>
            /// Constructs a new point intersection result.
            /// </summary>
            /// <param name="p">The intersection location.</param>
            /// <param name="s">The ratio along the first segment.</param>
            /// <param name="t">The ratio along the second segment.</param>
            public PointResult(Point2 p, double s, double t) {
                P = p;
                S = s;
                T = t;
            }

            /// <summary>
            /// The intersection location.
            /// </summary>
            public readonly Point2 P;
            /// <summary>
            /// The ratio along the first segment.
            /// </summary>
            public readonly double S;
            /// <summary>
            /// The ratio along the second segment.
            /// </summary>
            public readonly double T;

            IPlanarGeometry IResult.Geometry { [Pure] get { return P; } }

            /// <summary>
            /// Returns the segment location ratios as a vector object.
            /// </summary>
            public Vector2 Ratios { [Pure] get { return new Vector2(S, T); } }

            /// <summary>
            /// Returns the intersection classification with respect to the first segment.
            /// </summary>
            public SegmentIntersectionType TypeA { [Pure] get { return ClassifyIntersectionType(S); } }

            /// <summary>
            /// Returns the intersection classification with respect to the second segment.
            /// </summary>
            public SegmentIntersectionType TypeB { [Pure] get { return ClassifyIntersectionType(T); } }

        }

        /// <summary>
        /// A segment intersection result.
        /// </summary>
        /// <remarks>
        /// Occurs when two parallel lines intersect.
        /// </remarks>
        public sealed class SegmentResult : IResult
        {
            /// <summary>
            /// Constructs a new segment intersection result.
            /// </summary>
            /// <param name="a">The first point intersection defining an end of the segment intersection.</param>
            /// <param name="b">The second point intersection defining an end of the segment intersection.</param>
            public SegmentResult(PointResult a, PointResult b) {
                A = a;
                B = b;
            }

            /// <summary>
            /// The first point intersection defining an end of the segment intersection.
            /// </summary>
            public readonly PointResult A;
            /// <summary>
            /// The second point intersection defining an end of the segment intersection.
            /// </summary>
            public readonly PointResult B;

            IPlanarGeometry IResult.Geometry { [Pure] get { return S; } }

            /// <summary>
            /// The segment resulting from the intersection.
            /// </summary>
            public Segment2 S {
                [Pure]
                get {
                    return new Segment2(A.P, B.P);
                }
            }
        }

        /// <summary>
        /// A result that signifies no intersection.
        /// </summary>
        public struct NullResult : IResult
        {
            IPlanarGeometry IResult.Geometry { get { return null; } }
        }

        // ReSharper disable RedundantDefaultFieldInitializer
        private static readonly NullResult DefaultNoIntersection = new NullResult();
        // ReSharper restore RedundantDefaultFieldInitializer

        /// <summary>
        /// Calculates the intersection between two segments.
        /// </summary>
        /// <param name="a">The first segment.</param>
        /// <param name="b">The second segment.</param>
        /// <returns>The geometry resulting from the intersection.</returns>
        public static IPlanarGeometry Intersection(Segment2 a, Segment2 b) {
            return null == a || null == b
                ? null
                : IntersectionDetails(a, b).Geometry;
        }

        /// <summary>
        /// Calculates the intersection between two segments defined by four points.
        /// </summary>
        /// <param name="a">The first point of the first segment.</param>
        /// <param name="b">The second point of the first segment.</param>
        /// <param name="c">The first point of the second segment.</param>
        /// <param name="d">The second point of the second segment.</param>
        /// <returns>The geometry resulting from the intersection.</returns>
        public static IPlanarGeometry Intersection(Point2 a, Point2 b, Point2 c, Point2 d) {
            return IntersectionDetails(a, b, c, d).Geometry;
        }

        /// <summary>
        /// Calculates the intersection between two segments.
        /// </summary>
        /// <param name="a">The first segment.</param>
        /// <param name="b">The second segment.</param>
        /// <returns>The detailed result of the intersection.</returns>
        public static IResult IntersectionDetails(Segment2 a, Segment2 b) {
            Contract.Ensures(Contract.Result<IResult>() != null);
            if (null == a || null == b)
                return DefaultNoIntersection;
            return IntersectionDetails(a.A, a.B, b.A, b.B);
        }

        /// <summary>
        /// Calculates the intersection between two segments defined by four points.
        /// </summary>
        /// <param name="a">The first point of the first segment.</param>
        /// <param name="b">The second point of the first segment.</param>
        /// <param name="c">The first point of the second segment.</param>
        /// <param name="d">The second point of the second segment.</param>
        /// <returns>The detailed result of the intersection.</returns>
        public static IResult IntersectionDetails(Point2 a, Point2 b, Point2 c, Point2 d) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            Contract.Ensures(Contract.Result<IResult>() != null);

            var d0 = b - a;
            var d1 = d - c;
            var e = c - a;

            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross == 0.0) {
                // parallel
                return (e.X*d0.Y) == (e.Y*d0.X)
                    ? IntersectionDetailsParallel(d0, d1, e, a, b, c, d)
                    : DefaultNoIntersection;
            }

            // not parallel
            var s = ((e.X * d1.Y) - (e.Y * d1.X)) / cross;
            if (s < 0 || s > 1.0)
                return DefaultNoIntersection; // not intersecting on this segment

            var t = ((e.X * d0.Y) - (e.Y * d0.X)) / cross;
            if (t < 0 || t > 1.0)
                return DefaultNoIntersection; // not intersecting on other segment

            Point2 p;
            if (s == 0.0)
                p = a;
            else if (s == 1.0)
                p = b;
            else if (t == 0.0)
                p = c;
            else if (t == 1.0)
                p = d;
            else
                p = a + d0.GetScaled(s); // it must intersect at a point, so find where
            return new PointResult(p, s, t);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        private static IResult IntersectionDetailsParallel(Vector2 d0, Vector2 d1, Vector2 e, Point2 a, Point2 b, Point2 c, Point2 d) {
            Contract.Ensures(Contract.Result<IResult>() != null);

            var squareMagnitude0 = d0.GetMagnitudeSquared();
            var d0DotD1 = d0.Dot(d1);
            var s1 = d0.Dot(e) / squareMagnitude0;
            var s2 = s1 + (d0DotD1 / squareMagnitude0);
            double sMin, sMax;
            if (s1 <= s2) {
                sMin = s1;
                sMax = s2;
            }
            else {
                sMin = s2;
                sMax = s1;
            }

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (sMax < 0 || sMin > 1.0)
                return DefaultNoIntersection; // no intersection
            if (sMax == 0.0)
                return new PointResult(a, 0.0, a == c ? 0.0 : 1.0); // the start point
            if (sMin == 1.0)
                return new PointResult(b, 1.0, b == c ? 0.0 : 1.0); // the end point
            // ReSharper restore CompareOfFloatsByEqualityOperator

            PointResult resultA;
            PointResult resultB;
            double squareMagnitude1;
            if (sMin <= 0.0 && sMax >= 1.0) {
                squareMagnitude1 = d1.GetMagnitudeSquared();
                var t1 = d1.Dot(e.GetNegative()) / squareMagnitude1;
                resultA = new PointResult(a, 0.0, t1);
                resultB = new PointResult(b, 1.0, t1 + (d0DotD1 / squareMagnitude1));
            }
            else if (sMin >= 0.0 && sMax <= 1.0) {
                // reuse s1 and s2 from above
                resultA = new PointResult(c, s1, 0.0);
                resultB = new PointResult(d, s2, 1.0);
            }
            else {
                squareMagnitude1 = d1.GetMagnitudeSquared();
                var p1 = (0.0 < sMin) ? a + (d0.GetScaled(sMin)) : a;
                var p2 = a + (d0.GetScaled(sMax < 1.0 ? sMax : 1.0));
                var pd = p2 - p1;
                s1 = d0.Dot(p1 - a) / squareMagnitude0;
                s2 = s1 + (d0.Dot(pd) / squareMagnitude0);
                var t1 = d1.Dot(p1 - c) / squareMagnitude1;
                resultA = new PointResult(p1, s1, t1);
                resultB = new PointResult(p2, s2, t1 + (d1.Dot(pd) / squareMagnitude1));
            }

            return new SegmentResult(resultA, resultB);
        }

    }
}
