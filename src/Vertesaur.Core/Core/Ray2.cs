using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur
{

    /// <summary>
    /// A straight ray of infinite length emanating from a point.
    /// </summary>
    public sealed class Ray2 :
        IRay2<double>,
        IEquatable<Ray2>,
        IHasMbr<Mbr, double>,
        IHasDistance<Point2, double>,
        IRelatableIntersects<Point2>,
        IRelatableIntersects<Segment2>,
        IRelatableIntersects<Ray2>,
        IRelatableIntersects<Line2>,
        ISpatiallyEquatable<Ray2>,
        IHasIntersectionOperation<Segment2, IPlanarGeometry>,
        IHasIntersectionOperation<Ray2, IPlanarGeometry>,
        IHasIntersectionOperation<Line2, IPlanarGeometry>,
        IHasIntersectionOperation<Point2, IPlanarGeometry>,
        ICloneable
    {

        /// <summary>
        /// The point of origin for the ray.
        /// </summary>
        public readonly Point2 P;

        /// <summary>
        /// The direction of the ray from P.
        /// </summary>
        public readonly Vector2 Direction;

        /// <summary>
        /// Constructs a ray with infinite length originating from <paramref name="p"/> with direction <paramref name="d"/>.
        /// </summary>
        /// <param name="p">Point of origin.</param>
        /// <param name="d">The direction of the ray.</param>
        public Ray2(Point2 p, Vector2 d) {
            P = p;
            Direction = d;
        }
        /// <summary>
        /// Constructs a ray with infinite length originating from <paramref name="p"/> with direction <paramref name="v"/>.
        /// </summary>
        /// <param name="p">Point of origin.</param>
        /// <param name="v">The direction of the ray.</param>
        public Ray2(IPoint2<double> p, IVector2<double> v)
            : this(new Point2(p), new Vector2(v)) {
            Contract.Requires(p != null);
            Contract.Requires(v != null);
        }
        /// <summary>
        /// Constructs a ray with infinite length originating from <paramref name="a"/> and passing through <paramref name="b"/>.
        /// </summary>
        /// <param name="a">Point of origin.</param>
        /// <param name="b">A point on the ray.</param>
        public Ray2(Point2 a, Point2 b)
            : this(a, b - a) { }
        /// <summary>
        /// Constructs a ray with infinite length originating from <paramref name="a"/> and passing through <paramref name="b"/>.
        /// </summary>
        /// <param name="a">Point of origin.</param>
        /// <param name="b">A point on the ray.</param>
        public Ray2(IPoint2<double> a, IPoint2<double> b)
            : this(new Point2(a), new Point2(b)) {
            Contract.Requires(a != null);
            Contract.Requires(b != null);
        }
        /// <summary>
        /// Constructs a ray identical to the given <paramref name="ray"/>.
        /// </summary>
        /// <param name="ray">A ray.</param>
        public Ray2(Ray2 ray) {
            if (null == ray) throw new ArgumentNullException("ray");
            Contract.EndContractBlock();
            P = ray.P;
            Direction = ray.Direction;
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IPoint2<double> IRay2<double>.P { get { return P; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IVector2<double> IRay2<double>.Direction { get { return Direction; } }

        /// <summary>
        /// Determines if the ray is valid.
        /// </summary>
        public bool IsValid {
            get {
                return P.IsValid
                    && Direction.IsValid
                    && !Vector2.Zero.Equals(Direction);
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Ray2 other) {
            return !ReferenceEquals(null, other)
                && P.Equals(other.P)
                && Direction.Equals(other.Direction);
        }

        /// <summary>
        /// Determines if this ray is geometrically equal to another. 
        /// </summary>
        /// <param name="other">Another ray</param>
        /// <returns><c>true</c> if the rays are spatially equal.</returns>
        public bool SpatiallyEqual(Ray2 other) {
            return !ReferenceEquals(null, other)
                && P.Equals(other.P)
                && (
                    Direction.Equals(other.Direction)
                    || Direction.GetNormalized().Equals(other.Direction.GetNormalized())
                );
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            return Equals(obj as Ray2);
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return P.GetHashCode() ^ -123456;
        }

        /// <inheritdoc/>
        public override string ToString() {
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
            var result = String.Concat('(', P, ") (", Direction, ')');
            Contract.Assume(!String.IsNullOrEmpty(result));
            return result;
        }

        /// <summary>
        /// Calculates the length of this ray.
        /// </summary>
        /// <returns>The length.</returns>
        public double GetMagnitude() {
            Contract.Ensures(
                Contract.Result<double>() == 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Contract.Result<double>() == Double.PositiveInfinity);
            return Vector2.Zero.Equals(Direction) ? 0 : Double.PositiveInfinity;
        }

        /// <summary>
        /// Calculates the squared length of this ray.
        /// </summary>
        /// <returns>The length.</returns>
        public double GetMagnitudeSquared() {
            Contract.Ensures(
                Contract.Result<double>() == 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Contract.Result<double>() == Double.PositiveInfinity);
            return GetMagnitude();
        }

        /// <summary>
        /// Creates a copy of this ray.
        /// </summary>
        /// <returns>
        /// A new identical ray.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        /// <remarks>Functions as a deep clone.</remarks>
        public Ray2 Clone() {
            Contract.Ensures(Contract.Result<Ray2>() != null);
            return new Ray2(this);
        }

        object ICloneable.Clone() {
            return Clone();
        }

        /// <summary>
        /// Calculates a minimum bounding rectangle for this ray.
        /// </summary>
        /// <returns>A minimum bounding rectangle.</returns>
        public Mbr GetMbr() {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return (
                (Direction.X == 0.0)
                ? (
                    (Direction.Y == 0.0)
                    ? new Mbr(P)
                    : (
                        (Direction.Y > 0.0)
                        ? new Mbr(P.X, P.Y, P.X, Double.PositiveInfinity)
                        : new Mbr(P.X, Double.NegativeInfinity, P.X, P.Y)
                    )
                )
                : (
                    (Direction.Y == 0.0)
                    ? (
                        (Direction.X > 0.0)
                        ? new Mbr(P.X, P.Y, Double.PositiveInfinity, P.Y)
                        : new Mbr(Double.NegativeInfinity, P.Y, P.X, P.Y)
                    )
                    : (
                        (Direction.X > 0.0)
                        ? (
                            (Direction.Y > 0.0)
                            ? new Mbr(P.X, P.Y, Double.PositiveInfinity, Double.PositiveInfinity)
                            : new Mbr(P.X, Double.NegativeInfinity, Double.PositiveInfinity, P.Y)
                        )
                        : (
                            (Direction.Y > 0.0)
                            ? new Mbr(Double.NegativeInfinity, P.Y, P.X, Double.PositiveInfinity)
                            : new Mbr(Double.NegativeInfinity, Double.NegativeInfinity, P.X, P.Y)
                        )
                    )
                )
            );
        }

        /// <summary>
        /// Calculates the distance between this ray and the given <paramref name="point"/>
        /// </summary>
        /// <param name="point">The point to calculate distance to.</param>
        /// <returns>The distance.</returns>
        public double Distance(Point2 point) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            var v0 = point - P;
            var aDot = Direction.Dot(v0);
            return (
                (aDot <= 0)
                ? v0.GetMagnitude()
                : Math.Sqrt((v0.GetMagnitudeSquared() - ((aDot * aDot) / Direction.GetMagnitudeSquared())))
            );
        }

        /// <summary>
        /// Calculates the squared distance between this ray and <paramref name="point"/>
        /// </summary>
        /// <param name="point">The point to calculate squared distance to.</param>
        /// <returns>The squared distance.</returns>
        public double DistanceSquared(Point2 point) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            var v0 = point - P;
            var aDot = Direction.Dot(v0);
            return (
                (aDot <= 0)
                ? v0.GetMagnitudeSquared()
                : (v0.GetMagnitudeSquared() - ((aDot * aDot) / Direction.GetMagnitudeSquared()))
            );
        }

        /// <summary>
        /// Determines if a <paramref name="point"/> intersects this ray.
        /// </summary>
        /// <param name="point">A point.</param>
        /// <returns>True when intersecting.</returns>
        public bool Intersects(Point2 point) {
            var v0 = point - P;
            var aDot = Direction.Dot(v0);
            return (
                (aDot <= 0.0)
                ? v0.X == 0.0 && v0.Y == 0.0
                : v0.GetMagnitudeSquared() - ((aDot * aDot) / Direction.GetMagnitudeSquared()) == 0.0
            );
        }


        /// <inheritdoc/>
        public IPlanarGeometry Intersection(Point2 other)
        {
            if (Intersects(other))
                return other;
            return null;
        }

        /// <summary>
        /// Determines if this ray intersect another <paramref name="line"/>.
        /// </summary>
        /// <param name="line">A line.</param>
        /// <returns><c>true</c> when another object intersects this object.</returns>
        public bool Intersects(Line2 line) {
            if (line == null)
                return false;

            var lineB = line.P + line.Direction;
            if (line.P.CompareTo(lineB) > 0)
            {
                line = new Line2(lineB, line.P);
            }

            Point2 a, c;
            Vector2 d0, d1;
            a = line.P;
            c = P;
            d0 = line.Direction;
            d1 = Direction;

            var e = c - a;
            var tNumerator = (e.X * d0.Y) - (e.Y * d0.X);
            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross == 0.0)
            {
                // parallel
                return tNumerator == 0.0;
            }

            // not parallel
            return (!((tNumerator / cross) < 0.0));
        }

        /// <summary>
        /// Calculates the intersection geometry between this ray and a line.
        /// </summary>
        /// <param name="line">The line to find the intersection with.</param>
        /// <returns>The intersection geometry or <c>null</c> for no intersection.</returns>
        public IPlanarGeometry Intersection(Line2 line)
        {
            if (line == null)
                return null;

            var lineB = line.P + line.Direction;
            if (line.P.CompareTo(lineB) > 0)
            {
                line = new Line2(lineB, line.P);
            }

            Point2 a, c;
            Vector2 d0, d1;
            a = line.P;
            c = P;
            d0 = line.Direction;
            d1 = Direction;

            var e = c - a;
            var tNumerator = (e.X * d0.Y) - (e.Y * d0.X);
            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross == 0.0)
            {
                // parallel
                return tNumerator == 0.0
                    ? this // NOTE: this relies on Ray2 being immutable
                    : null;
            }

            // not parallel
            var t = tNumerator / cross;
            if (t < 0.0)
                return null; // not intersecting on other ray
            if (t == 0.0)
                return c; // clips the butt of the ray

            // it must intersect at a point, so find where
            var s = ((e.X * d1.Y) - (e.Y * d1.X)) / cross;
            if (s == 0.0)
                return a;

            return a + d0.GetScaled(s);
        }

        /// <summary>
        /// Determines if this ray intersect another <paramref name="segment"/>.
        /// </summary>
        /// <param name="segment">A segment.</param>
        /// <returns><c>true</c> when another object intersects this object.</returns>
        public bool Intersects(Segment2 segment)
        {
            return Intersection(segment) != null; // TODO: optimize
        }

        /// <summary>
        /// Calculates the intersection geometry between this ray and a segment.
        /// </summary>
        /// <param name="segment">The segment to find the intersection with.</param>
        /// <returns>The intersection geometry or <c>null</c> for no intersection.</returns>
        public IPlanarGeometry Intersection(Segment2 segment) {
            if (segment == null)
                return null;

            var a = segment.A;
            var b = segment.B;
            Point2.Order(ref a, ref b);

            return IntersectionSegment(a, b);
        }

        /// <summary>
        /// Performs segment intersection against sorted endpoints of a segment.
        /// </summary>
        /// <param name="a">The lesser end point.</param>
        /// <param name="b">The greater end point.</param>
        /// <returns>The intersection result.</returns>
        private IPlanarGeometry IntersectionSegment(Point2 a, Point2 b)
        {
            var d0 = b - a;
            var d1 = Direction;
            var e = P - a;
            var tNumerator = (e.X * d0.Y) - (e.Y * d0.X);
            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross == 0.0)
            {
                // parallel
                return tNumerator == 0.0
                    ? this.IntersectionSegmentParallel(d0, d1, e, a, b)
                    : null;
            }

            // not parallel

            var t = tNumerator / cross;
            if (t < 0.0)
                return null; // not intersecting on the ray

            var s = ((e.X * d1.Y) - (e.Y * d1.X)) / cross;
            if (s < 0.0 || s > 1.0)
                return null; // not intersecting on this segment

            // it must intersect at a point, so find where
            if (0.0 == s)
                return a;
            if (0.0 == t)
                return P;
            return a + d0.GetScaled(s);
        }

        /// <summary>
        /// This method is extracted from IntersectionSegment(Point2,Point2) as it is a rare case.
        /// </summary>
        private IPlanarGeometry IntersectionSegmentParallel(Vector2 d0, Vector2 d1, Vector2 e, Point2 segmentA, Point2 segmentB)
        {
            var magnitudeSquared0 = d0.GetMagnitudeSquared();
            var sa = d0.Dot(e) / magnitudeSquared0;
            var sb = (d0.Dot(d1) / magnitudeSquared0) + sa;
            var sd = sb - sa;

            if (sd < 0)
            {
                if (sa < 0)
                {
                    return null; // start point is before the segment
                }
                if (sa >= 1.0)
                {
                    return new Segment2(segmentA, segmentB); // start point is past the segment but pointing back at it
                }
                if (sa == 0)
                {
                    return segmentA; // start point is on the start of the segment
                }
                return new Segment2(segmentA, P); // start point is somewhere inside the segment
            }

            if (sd > 0)
            {
                if (sa > 1.0)
                {
                    return null; // start point is past the segment and pointing away
                }
                if (sa <= 0)
                {
                    return new Segment2(segmentA, segmentB); // start point is before the segment and going through it
                }
                if (sa == 1.0)
                {
                    return segmentB; // start point is on the segment end and pointing away
                }
                return new Segment2(P, segmentB); // start point is in the segment and going through it to the end
            }

            return null;
        }

        /// <summary>
        /// Determines if this ray intersect another <paramref name="ray"/>.
        /// </summary>
        /// <param name="ray">A ray.</param>
        /// <returns><c>true</c> when another object intersects this object.</returns>
        public bool Intersects(Ray2 ray)
        {
            if (ray == null)
                return false;
            if (ray == this || P.Equals(ray.P) && Direction.Equals(ray.Direction))
                return true; // NOTE: requires ray to be immutable

            Point2 a, c;
            Vector2 d0, d1;
            // next order the rays
            var compareResult = P.CompareTo(ray.P);
            if (0 < ((compareResult == 0) ? Direction.CompareTo(ray.Direction) : compareResult))
            {
                a = ray.P;
                c = P;
                d0 = ray.Direction;
                d1 = Direction;
            }
            else
            {
                a = P;
                c = ray.P;
                d0 = Direction;
                d1 = ray.Direction;
            }

            var e = c - a;
            var tNumerator = (e.X * d0.Y) - (e.Y * d0.X);
            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross == 0.0)
            {
                // parallel
                return tNumerator == 0.0 && IntersectsRayParallel(d0, d1, e, a, c);
            }

            // not parallel

            var t = tNumerator / cross;
            if (t < 0.0)
                return false; // not intersecting on other ray

            var s = ((e.X * d1.Y) - (e.Y * d1.X)) / cross;
            if (s < 0.0)
                return false; // not intersecting on this ray

            return true;
        }
        
        private bool IntersectsRayParallel(Vector2 d0, Vector2 d1, Vector2 e, Point2 a, Point2 c)
        {
            var magnitudeSquared0 = d0.GetMagnitudeSquared();
            var sa = d0.Dot(e) / magnitudeSquared0;
            var sb = sa + (d0.Dot(d1) / magnitudeSquared0);
            var sd = sb - sa;

            if (sd < 0.0)
            {
                return sa >= 0.0;
            }

            return sd > 0.0;
        }

        /// <summary>
        /// Calculates the intersection geometry between this ray and another.
        /// </summary>
        /// <param name="ray">The ray to find the intersection with.</param>
        /// <returns>The intersection geometry or <c>null</c> for no intersection.</returns>
        public IPlanarGeometry Intersection(Ray2 ray) {
            if (ray == null)
                return null;
            if (ray == this || P.Equals(ray.P) && Direction.Equals(ray.Direction))
                return ray; // NOTE: requires ray to be immutable

            Point2 a, c;
            Vector2 d0, d1;
            // next order the segments
            var compareResult = P.CompareTo(ray.P);
            if (0 < ((compareResult == 0) ? Direction.CompareTo(ray.Direction) : compareResult)) {
                a = ray.P;
                c = P;
                d0 = ray.Direction;
                d1 = Direction;
            }
            else {
                a = P;
                c = ray.P;
                d0 = Direction;
                d1 = ray.Direction;
            }

            var e = c - a;
            var tNumerator = (e.X * d0.Y) - (e.Y * d0.X);
            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross == 0.0) {
                // parallel
                return tNumerator == 0.0
                    ? IntersectionRayParallel(d0, d1, e, a, c)
                    : null; // no intersection
            }

            // not parallel

            var t = tNumerator / cross;
            if (t < 0.0)
                return null; // not intersecting on other ray

            var s = ((e.X * d1.Y) - (e.Y * d1.X)) / cross;
            if (s < 0.0)
                return null; // not intersecting on this ray

            if (s == 0.0)
                return a;
            if (t == 0.0)
                return c;
            return a + d0.GetScaled(s); // it must intersect at a point, so find where
        }

        private IPlanarGeometry IntersectionRayParallel(Vector2 d0, Vector2 d1, Vector2 e, Point2 a, Point2 c) {
            var magnitudeSquared0 = d0.GetMagnitudeSquared();
            var sa = d0.Dot(e) / magnitudeSquared0;
            var sb = sa + (d0.Dot(d1) / magnitudeSquared0);
            var sd = sb - sa;

            if (sd < 0.0) {
                // going against the grain
                if (sa > 0.0)
                    return new Segment2(a, c);
                if (sa == 0.0)
                    return a;
                return null; // they don’t touch
            }
            if (sd > 0.0) {
                // going with the grain
                return sa > 0.0
                    ? new Ray2(c, d1)
                    : new Ray2(a, d0);
            }
            return null;
        }

        /// <summary>
        /// Creates a new ray starting from the same point of origin but going in the opposite direction.
        /// </summary>
        /// <returns>A new reversed ray.</returns>
        public Ray2 GetReverse() {
            Contract.Ensures(Contract.Result<Ray2>() != null);
            return new Ray2(P, Direction.GetNegative());
        }
    }
}
