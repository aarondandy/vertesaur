using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur
{

    /// <summary>
    /// A straight line of infinite length defined by a point and a direction.
    /// </summary>
    /// <remarks>
    /// There are many ways to define a line but this class defines one by
    /// specifying a single point that lies on that line and one direction vector
    /// from that point which describes the lines orientation. This is a good
    /// compromise to reduce the amount of arithmetic required to work with the
    /// object. Define a line by specifying a single point on the line and one of
    /// the two directions in which it flies off into the vast infiniteness of
    /// space.
    /// </remarks>
    /// <seealso cref="Vertesaur.Segment2"/>
    public sealed class Line2 :
        ILine2<double>,
        IEquatable<Line2>,
        IHasMbr<Mbr, double>,
        IHasDistance<Point2, double>,
        IRelatableIntersects<Point2>,
        IRelatableIntersects<Segment2>,
        IRelatableIntersects<Ray2>,
        IRelatableIntersects<Line2>,
        IHasIntersectionOperation<Segment2, IPlanarGeometry>,
        IHasIntersectionOperation<Line2, IPlanarGeometry>,
        IHasIntersectionOperation<Ray2, IPlanarGeometry>,
        IHasIntersectionOperation<Point2, IPlanarGeometry>,
        ICloneable
    {

        /// <summary>
        /// Constructs a line from slope intercept parameters.
        /// </summary>
        /// <param name="m">Slope.</param>
        /// <param name="b">Y-intercept.</param>
        /// <returns>A line.</returns>
        /// <remarks>
        /// <code>y = (m*x) + b</code>
        /// </remarks>
        public static Line2 SlopeIntercept(double m, double b) {
            Contract.Ensures(Contract.Result<Line2>() != null);
            return new Line2(new Point2(0.0, b), new Vector2(1.0, m));
        }

        /// <summary>
        /// Constructs a line from general form.
        /// </summary>
        /// <param name="a">The x-coefficient.</param>
        /// <param name="b">The y-coefficient.</param>
        /// <param name="c">A constant.</param>
        /// <returns>A line.</returns>
        /// <remarks>
        /// <code>(A*x) + (B*y) + C = 0</code>
        /// </remarks>
        public static Line2 General(double a, double b, double c) {
            Contract.Ensures(Contract.Result<Line2>() != null);
            return Standard(a, b, -c);
        }

        /// <summary>
        /// Constructs a line from standard form.
        /// </summary>
        /// <param name="a">The x-coefficient.</param>
        /// <param name="b">The y-coefficient.</param>
        /// <param name="c">A constant.</param>
        /// <returns>A line.</returns>
        /// <remarks>
        /// <code>(A*x) + (B*y) = C</code>
        /// </remarks>
        public static Line2 Standard(double a, double b, double c) {
            Contract.Ensures(Contract.Result<Line2>() != null);

            Point2 p;
            Vector2 v;
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (a == 0.0) {
                p = b == 0.0
                    ? Point2.Zero
                    : new Point2(0.0, c / b);
                v = Vector2.XUnit;
            }
            else {
                p = new Point2(c / a, 0.0);
                v = b == 0.0
                    ? Vector2.YUnit
                    : new Vector2(-p.X, (c / b) - p.Y);
            }
            return new Line2(p, v);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// A point which lies on the line.
        /// </summary>
        /// <remarks>
        /// This is a point that intersects the line. While a line intersects an
        /// infinite number of points it is just nit practical to define more than
        /// two. This point represents any arbitrary point really while the second
        /// point that is on this line, which defines its direction and position is
        /// derived from the direction property.
        /// </remarks>
        public readonly Point2 P;

        /// <summary>
        /// The direction of the line from P.
        /// </summary>
        /// <remarks>
        /// While the defined direction goes in only one direction from the defined
        /// point the line actually goes in both the defined direction as well as the
        /// negative of the defined direction from the defined point. If it went in
        /// only one direction it would be a ray but it is a lot easier and more
        /// light-weight to define the line this way.
        /// </remarks>
        public readonly Vector2 Direction;

        /// <summary>
        /// Constructs a line with infinite length passing though <paramref name="p"/> with direction <paramref name="d"/>.
        /// </summary>
        /// <param name="p">A point on the line.</param>
        /// <param name="d">The direction of the line.</param>
        public Line2(Point2 p, Vector2 d) {
            P = p;
            Direction = d;
        }

        /// <summary>
        /// Constructs a line with infinite length passing though <paramref name="p"/> with direction <paramref name="d"/>.
        /// </summary>
        /// <param name="p">A point on the line.</param>
        /// <param name="d">The direction of the line.</param>
        public Line2(IPoint2<double> p, IVector2<double> d)
            : this(new Point2(p), new Vector2(d)) {
            Contract.Requires(p != null);
            Contract.Requires(d != null);
        }

        /// <summary>
        /// Constructs a line with infinite length passing though both points <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">A point.</param>
        /// <param name="b">A point.</param>
        public Line2(Point2 a, Point2 b)
            : this(a, b - a) { }

        /// <summary>
        /// Constructs a line with infinite length passing though both points <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">A point.</param>
        /// <param name="b">A point.</param>
        public Line2(IPoint2<double> a, IPoint2<double> b)
            : this(new Point2(a), new Point2(b)) {
            Contract.Requires(a != null);
            Contract.Requires(b != null);
        }

        /// <summary>
        /// Constructs a line with infinite length identical to the given <paramref name="line"/> which is defined by the same points and direction..
        /// </summary>
        /// <param name="line">A line.</param>
        public Line2(Line2 line) {
            if (null == line) throw new ArgumentNullException("line");
            Contract.EndContractBlock();
            P = line.P;
            Direction = line.Direction;
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IPoint2<double> ILine2<double>.P { get { return P; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IVector2<double> ILine2<double>.Direction { get { return Direction; } }

        /// <summary>
        /// Determines if the line is valid.
        /// </summary>
        public bool IsValid {
            get{
                return P.IsValid
                    && Direction.IsValid
                    && Direction != Vector2.Zero;
            }
        }

        /// <inheritdoc/>
        public bool Equals(Line2 other) {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return P.Equals(other.P)
                && Direction.Equals(other.Direction);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            return Equals(obj as Line2);
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return Direction.GetHashCode() ^ P.Y.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString() {
            return String.Concat('(', P, ") (", Direction, ')');
        }

        /// <summary>
        /// Calculates the length of this line.
        /// </summary>
        /// <returns>The length.</returns>
        [Pure]
        public double GetMagnitude() {
            return Vector2.Zero == Direction ? 0.0 : Double.PositiveInfinity;
        }

        /// <summary>
        /// Calculates the squared length of this line.
        /// </summary>
        /// <returns>The length.</returns>
        [Pure]
        public double GetMagnitudeSquared() {
            return GetMagnitude();
        }

        /// <summary>
        /// Creates a copy of this line.
        /// </summary>
        /// <returns>
        /// A new identical line.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        /// <remarks>Functions as a deep clone and a shallow clone.</remarks>
        public Line2 Clone() {
            Contract.Ensures(Contract.Result<Line2>() != null);
            return new Line2(this);
        }

        /// <inheritdoc/>
        object ICloneable.Clone() {
            return Clone();
        }

        /// <summary>
        /// Calculates a minimum bounding rectangle for this line.
        /// </summary>
        /// <returns>A minimum bounding rectangle.</returns>
        [Pure]
        public Mbr GetMbr() {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return (
                (Direction.X == 0.0)
                ? (
                    (Direction.Y == 0.0)
                    ? new Mbr(P)
                    : new Mbr(P.X, Double.NegativeInfinity, P.X, Double.PositiveInfinity)
                )
                : (
                    (Direction.Y == 0.0)
                    ? new Mbr(Double.NegativeInfinity, P.Y, Double.PositiveInfinity, P.Y)
                    : new Mbr(Double.NegativeInfinity, Double.NegativeInfinity, Double.PositiveInfinity, Double.PositiveInfinity)
                )
            );
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Calculates the distance between this line and the given <paramref name="point"/>
        /// </summary>
        /// <param name="point">The point to calculate distance to.</param>
        /// <returns>The distance.</returns>
        [Pure]
        public double Distance(Point2 point) {
            var v0 = point - P;
            var m = Direction.GetMagnitudeSquared();
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (m != 0.0) {
                var aDot = Direction.Dot(v0);
                return Math.Sqrt(v0.GetMagnitudeSquared() - ((aDot * aDot) / m));
            }
            return v0.GetMagnitude();
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Calculates the squared distance between this line and the given <paramref name="point"/>
        /// </summary>
        /// <param name="point">The point to calculate squared distance to.</param>
        /// <returns>The squared distance.</returns>
        [Pure]
        public double DistanceSquared(Point2 point) {
            var v0 = point - P;
            var m = Direction.GetMagnitudeSquared();
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (m != 0.0) {
                var aDot = Direction.Dot(v0);
                return v0.GetMagnitudeSquared() - ((aDot * aDot) / m);
            }
            return v0.GetMagnitudeSquared();
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Determines if a point intersects this line.
        /// </summary>
        /// <param name="p">A point.</param>
        /// <returns>True when intersecting.</returns>
        [Pure]
        public bool Intersects(Point2 p) {
            var v0 = p - P;
            var m = Direction.GetMagnitudeSquared();
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (m != 0.0) {
                var aDot = Direction.Dot(v0);
                // NOTE: preserve the /m to be consistent with DistanceSquared
                return v0.GetMagnitudeSquared() - ((aDot * aDot) / m) == 0.0;
            }
            return v0.X == 0.0 && v0.Y == 0.0;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Determines if this line intersect another <paramref name="segment"/>.
        /// </summary>
        /// <param name="segment">A segment.</param>
        /// <returns><c>true</c> when another object intersects this object.</returns>
        public bool Intersects(Segment2 segment) {
            return !ReferenceEquals(null, segment) && segment.Intersects(this);
        }

        /// <summary>
        /// Determines if this line intersect another <paramref name="ray"/>.
        /// </summary>
        /// <param name="ray">A ray.</param>
        /// <returns><c>true</c> when another object intersects this object.</returns>
        public bool Intersects(Ray2 ray) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (ReferenceEquals(null, ray))
                return false;

            var d0 = Direction;
            var d1 = ray.Direction;
            var e = ray.P - P;
            var tNumerator = (e.X * d0.Y) - (e.Y * d0.X);
            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross == 0.0) {
                // parallel
                return tNumerator == 0.0;
            }

            // not parallel
            var t = tNumerator / cross;
            return t >= 0.0; // it intersects at a point if on the positive side of the ray

            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Determines if this line intersect another line.
        /// </summary>
        /// <param name="other">A line.</param>
        /// <returns><c>true</c> when another object intersects this object.</returns>
        public bool Intersects(Line2 other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other) || P.Equals(other.P) && Direction.Equals(other.Direction))
                return true;

            Point2 a, c;
            Vector2 d0, d1;
            // order the lines
            var compareResult = P.CompareTo(other.P);
            if (0 < ((compareResult == 0) ? Direction.CompareTo(other.Direction) : compareResult)) {
                a = other.P;
                c = P;
                d0 = other.Direction;
                d1 = Direction;
            }
            else {
                a = P;
                c = other.P;
                d0 = Direction;
                d1 = other.Direction;
            }

            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross == 0.0)
            {
                // parallel
                var e = c - a;
                var tNumerator = (e.X * d0.Y) - (e.Y * d0.X);
                return tNumerator == 0.0;
            }

            return true; // not parallel
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc/>
        public IPlanarGeometry Intersection(Segment2 other) {
            return ReferenceEquals(null, other)
                ? null
                : other.Intersection(this);
        }

        /// <inheritdoc/>
        public IPlanarGeometry Intersection(Line2 other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (ReferenceEquals(null, other))
                return null;
            if (ReferenceEquals(this, other) || P.Equals(other.P) && Direction.Equals(other.Direction))
                return Clone();

            Point2 a, c;
            Vector2 d0, d1;
            // order the lines
            var compareResult = P.CompareTo(other.P);
            if (0 < ((compareResult == 0) ? Direction.CompareTo(other.Direction) : compareResult)) {
                a = other.P;
                c = P;
                d0 = other.Direction;
                d1 = Direction;
            }
            else {
                a = P;
                c = other.P;
                d0 = Direction;
                d1 = other.Direction;
            }
            var e = c - a;
            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross != 0.0)
                return a + d0.GetScaled(((e.X * d1.Y) - (e.Y * d1.X)) / cross); // not parallel

            // parallel
            var tNumerator = (e.X * d0.Y) - (e.Y * d0.X);
            return tNumerator == 0.0
                ? new Line2(a, d0) // construct a new line from the a/d0 values to be consistent
                : null;

            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc/>
        public IPlanarGeometry Intersection(Ray2 ray) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (ray == null)
                return null;

            Point2 a, c;
            Vector2 d0, d1;
            a = P;
            c = ray.P;
            d0 = Direction;
            d1 = ray.Direction;
            var e = c - a;
            var tNumerator = (e.X * d0.Y) - (e.Y * d0.X);
            var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
            if (cross == 0.0) {
                // parallel
                return tNumerator == 0.0
                    ? ray // NOTE: this relies on Ray2 being immutable
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
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc/>
        public IPlanarGeometry Intersection(Point2 other) {
            return Intersects(other) ? (IPlanarGeometry)other : null;
        }
    }

}
