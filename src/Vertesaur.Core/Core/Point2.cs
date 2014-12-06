using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur
{

    /// <summary>
    /// A point in 2D space.
    /// </summary>
    public struct Point2 :
        IPoint2<double>,
        IEquatable<Point2>,
        IComparable<Point2>,
        IEquatable<ICoordinatePair<double>>,
        ISpatiallyRelatable<Point2>,
        IHasDistance<Point2, double>,
        IHasMbr<Mbr, double>
    {

        /// <summary>
        /// Implements the equality operator.
        /// </summary>
        /// <param name="a">A point from the left argument.</param>
        /// <param name="b">A point from the right argument.</param>
        /// <returns>True if both points have the same component values.</returns>
        public static bool operator ==(Point2 a, Point2 b) {
            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A point from the left argument.</param>
        /// <param name="b">A point from the right argument.</param>
        /// <returns>True if both points do not have the same component values.</returns>
        public static bool operator !=(Point2 a, Point2 b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="leftHandSide">A vector from the left argument.</param>
        /// <param name="rightHandSide">A point from the right argument.</param>
        /// <returns>The result.</returns>
        public static Point2 operator +(Vector2 leftHandSide, Point2 rightHandSide) {
            return leftHandSide.Add(rightHandSide);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="leftHandSide">A point from the left argument.</param>
        /// <param name="rightHandSide">A vector from the right argument.</param>
        /// <returns>The result.</returns>
        public static Point2 operator +(Point2 leftHandSide, Vector2 rightHandSide) {
            return leftHandSide.Add(rightHandSide);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="leftHandSide">A vector from the left argument.</param>
        /// <param name="rightHandSide">A point from the right argument.</param>
        /// <returns>The result.</returns>
        public static Point2 operator -(Vector2 leftHandSide, Point2 rightHandSide) {
            return leftHandSide.Difference(rightHandSide);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="leftHandSide">A point from the left argument.</param>
        /// <param name="rightHandSide">A vector from the right argument.</param>
        /// <returns>The result.</returns>
        public static Point2 operator -(Point2 leftHandSide, Vector2 rightHandSide) {
            return leftHandSide.Difference(rightHandSide);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="leftHandSide">A point from the left argument.</param>
        /// <param name="rightHandSide">A point from the right argument.</param>
        /// <returns>The vector difference between two points.</returns>
        public static Vector2 operator -(Point2 leftHandSide, Point2 rightHandSide) {
            return leftHandSide.Difference(rightHandSide);
        }

        /// <summary>
        /// Multiplies the point by a scalar.
        /// </summary>
        /// <param name="tuple">The point to multiply.</param>
        /// <param name="factor">The scalar value to multiply by.</param>
        /// <returns>The resulting scaled point.</returns>
        public static Point2 operator *(Point2 tuple, double factor) {
            return new Point2(tuple.X * factor, tuple.Y * factor);
        }

        /// <summary>
        /// Multiplies the point by a scalar.
        /// </summary>
        /// <param name="tuple">The point to multiply.</param>
        /// <param name="factor">The scalar value to multiply by.</param>
        /// <returns>The resulting scaled point.</returns>
        public static Point2 operator *(double factor, Point2 tuple) {
            return new Point2(tuple.X * factor, tuple.Y * factor);
        }

        /// <inheritdoc/>
        public static implicit operator Vector2(Point2 value) {
            return new Vector2(value);
        }

        /// <summary>
        /// A point with all components set to zero.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly Point2 Zero = new Point2(0, 0);
        /// <summary>
        /// An invalid point.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly Point2 Invalid = new Point2(Double.NaN, Double.NaN);

        internal static void Order(ref Point2 a, ref Point2 b) {
            if (a.CompareTo(b) > 0) {
                var t = a;
                a = b;
                b = t;
            }
        }

        internal static void SegmentOrder(ref Point2 a, ref Point2 b, ref Point2 c, ref Point2 d) {
            // first order the points in the segments
            Order(ref a, ref b);
            Order(ref c, ref d);
            // next order the segments
            var compareResult = a.CompareTo(c);
            if (compareResult == 0)
                compareResult = b.CompareTo(d);
            if (0 < compareResult) {
                var t = a;
                a = c;
                c = t;
                t = b;
                b = d;
                d = t;
            }
        }

        /// <summary>
        /// The x-coordinate of this point.
        /// </summary>
        public readonly double X;
        /// <summary>
        /// The y-coordinate of this point.
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// Creates a point with the given <paramref name="x"/> and <paramref name="y"/> coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        public Point2(double x, double y) {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Creates a point with the same coordinates as the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">A coordinate pair.</param>
        public Point2(ICoordinatePair<double> point) {
            if (null == point) throw new ArgumentNullException("point");
            Contract.EndContractBlock();
            X = point.X;
            Y = point.Y;
        }

        /// <summary>
        /// Clones a point from a vector.
        /// </summary>
        /// <param name="vector">The vector to clone as a point.</param>
        public Point2(Vector2 vector)
            : this(vector.X, vector.Y) { }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ICoordinatePair<double>.X { get { return X; } }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ICoordinatePair<double>.Y { get { return Y; } }

        /// <inheritdoc/>
        [Pure]
        public bool Equals(Point2 other) {
            return X == other.X && Y == other.Y;
        }

        /// <inheritdoc/>
        [Pure]
        public bool Equals(ICoordinatePair<double> other) {
            return !ReferenceEquals(null, other)
                && X == other.X && Y == other.Y;
        }

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) {
            return null != obj && (
                (obj is Point2 && Equals((Point2)obj))
                ||
                Equals(obj as ICoordinatePair<double>)
            );
        }

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() {
            return X.GetHashCode();
        }

        /// <inheritdoc/>
        [Pure]
        public override string ToString() {
            return String.Concat(X, ' ', Y);
        }

        /// <inheritdoc/>
        [Pure]
        public int CompareTo(Point2 other) {
            var c = X.CompareTo(other.X);
            return 0 == c ? Y.CompareTo(other.Y) : c;
        }

        /// <summary>
        /// Calculates the distance between this and the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point to calculate distance to.</param>
        /// <returns>The distance.</returns>
        [Pure]
        public double Distance(Point2 point) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Contract.Result<double>().Equals(Double.NaN));
            var dx = point.X - X;
            var dy = point.Y - Y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        /// <summary>
        /// Calculates the squared distance between this and the given <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point to calculate squared distance to.</param>
        /// <returns>The squared distance.</returns>
        [Pure]
        public double DistanceSquared(Point2 point) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Contract.Result<double>().Equals(Double.NaN));
            var dx = point.X - X;
            var dy = point.Y - Y;
            return (dx * dx) + (dy * dy);
        }

        /// <summary>
        /// Adds the given <paramref name="delta"/> to this point.
        /// </summary>
        /// <param name="delta">An offset vector.</param>
        /// <returns>An offset point.</returns>
        [Pure]
        public Point2 Add(Vector2 delta) {
            return new Point2(X + delta.X, Y + delta.Y);
        }

        /// <summary>
        /// Finds the vector difference between this point and another.
        /// </summary>
        /// <param name="b">The other point.</param>
        /// <returns>The vector difference.</returns>
        [Pure]
        public Vector2 Difference(Point2 b) {
            return new Vector2(X - b.X, Y - b.Y);
        }

        /// <summary>
        /// Creates a point offset from this point by a given vector.
        /// </summary>
        /// <param name="b">The vector.</param>
        /// <returns>The offset point.</returns>
        [Pure]
        public Point2 Difference(Vector2 b) {
            return new Point2(X - b.X, Y - b.Y);
        }

        /// <inheritdoc/>
        [Pure]
        public Mbr GetMbr() {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return new Mbr(this);
        }

        /// <summary>
        /// Determines if the point is valid.
        /// </summary>
        public bool IsValid {
            [Pure]
            get { return !Double.IsNaN(X) && !Double.IsNaN(Y); }
        }

        /// <inheritdoc/>
        bool IRelatableIntersects<Point2>.Intersects(Point2 other) {
            return Equals(other);
        }

        /// <inheritdoc/>
        bool IRelatableDisjoint<Point2>.Disjoint(Point2 other) {
            return !Equals(other);
        }

        /// <inheritdoc/>
        bool IRelatableTouches<Point2>.Touches(Point2 other) {
            return false;
        }

        /// <inheritdoc/>
        bool IRelatableCrosses<Point2>.Crosses(Point2 other) {
            return false;
        }

        /// <inheritdoc/>
        bool IRelatableWithin<Point2>.Within(Point2 other) {
            return Equals(other);
        }

        /// <inheritdoc/>
        bool IRelatableContains<Point2>.Contains(Point2 other) {
            return Equals(other);
        }

        /// <inheritdoc/>
        bool IRelatableOverlaps<Point2>.Overlaps(Point2 other) {
            return false;
        }

        /// <inheritdoc/>
        bool ISpatiallyEquatable<Point2>.SpatiallyEqual(Point2 other) {
            return Equals(other);
        }
    }
}
