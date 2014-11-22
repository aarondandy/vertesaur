using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Vertesaur
{
    /// <summary>
    /// A 2D axis aligned minimum bounding rectangle, also known as an envelope.
    /// </summary>
    public class Mbr :
        IEquatable<Mbr>,
        IEquatable<IMbr<double>>,
        IMbr<double>,
        ISpatiallyRelatable<Mbr>, ISpatiallyRelatable<Point2>,
        IHasDistance<Mbr, double>, IHasDistance<Point2, double>,
        IHasArea<double>,
        IHasCentroid<Point2>
    {
        // TODO: should this be a value type or reference type (with cloning)?

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A MBR.</param>
        /// <param name="b">A MBR.</param>
        /// <returns>True if both MBRs have the same X and Y ranges.</returns>
        public static bool operator ==(Mbr a, Mbr b) {
            return ReferenceEquals(null, a) ? ReferenceEquals(null, b) : a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A MBR.</param>
        /// <param name="b">A MBR.</param>
        /// <returns>True if both MBRs do not have the same X and Y ranges.</returns>
        public static bool operator !=(Mbr a, Mbr b) {
            return ReferenceEquals(null, a) ? !ReferenceEquals(null, b) : !a.Equals(b);
        }

        /// <summary>
        /// Creates an MBR encompassing all of the given points.
        /// </summary>
        /// <param name="points">The points to encompass.</param>
        /// <returns>The MBR encompassing the given points.</returns>
        public static Mbr Create(Point2[] points) {
            if (null == points) throw new ArgumentNullException("points");
            Contract.EndContractBlock();

            if (points.Length == 0)
                return null;
            if (points.Length == 1)
                return new Mbr(points[0]);
            if (points.Length == 2)
                return new Mbr(points[0], points[1]);

            double xMax, yMax;
            var p = points[0];
            var xMin = xMax = p.X;
            var yMin = yMax = p.Y;

            for (int i = 1; i < points.Length; i++) {
                p = points[i];

                if (p.X < xMin)
                    xMin = p.X;
                else if (p.X > xMax)
                    xMax = p.X;

                if (p.Y < yMin)
                    yMin = p.Y;
                else if (p.Y > yMax)
                    yMax = p.Y;
            }
            return new Mbr(xMin, yMin, xMax, yMax);
        }

        /// <summary>
        /// Creates an MBR encompassing all of the given points.
        /// </summary>
        /// <param name="points">The points to encompass.</param>
        /// <returns>The MBR encompassing the given points.</returns>
        public static Mbr Create(List<Point2> points) {
            if (null == points) throw new ArgumentNullException("points");
            Contract.Ensures(points.Count > 0 ? Contract.Result<Mbr>() != null : Contract.Result<Mbr>() == null);

            if (points.Count == 0)
                return null;
            if (points.Count == 1)
                return new Mbr(points[0]);
            if (points.Count == 2)
                return new Mbr(points[0], points[1]);

            double xMax, yMax;
            var p = points[0];
            var xMin = xMax = p.X;
            var yMin = yMax = p.Y;

            for (int i = 1; i < points.Count; i++) {
                p = points[i];

                if (p.X < xMin)
                    xMin = p.X;
                else if (p.X > xMax)
                    xMax = p.X;

                if (p.Y < yMin)
                    yMin = p.Y;
                else if (p.Y > yMax)
                    yMax = p.Y;
            }
            return new Mbr(xMin, yMin, xMax, yMax);
        }

        /// <summary>
        /// Creates an MBR encompassing all of the given points.
        /// </summary>
        /// <param name="points">The points to encompass.</param>
        /// <returns>The MBR encompassing the given points.</returns>
        public static Mbr Create(IEnumerable<Point2> points) {
            if (null == points) throw new ArgumentNullException("points");
            Contract.EndContractBlock();

            var enumerator = points.GetEnumerator();
            if (!enumerator.MoveNext())
                return null;
            double xMax, yMax;
            var p = enumerator.Current;
            var xMin = xMax = p.X;
            var yMin = yMax = p.Y;

            while (enumerator.MoveNext()) {
                p = enumerator.Current;

                Contract.Assume(xMin <= xMax);
                Contract.Assume(yMin <= yMax);

                if (p.X < xMin)
                    xMin = p.X;
                else if (p.X > xMax)
                    xMax = p.X;

                if (p.Y < yMin)
                    yMin = p.Y;
                else if (p.Y > yMax)
                    yMax = p.Y;
            }

            return new Mbr(xMin, yMin, xMax, yMax);
        }

        /// <summary>
        /// The x-axis range.
        /// </summary>
        public Range X { get; private set; }

        /// <summary>
        /// The y-axis range.
        /// </summary>
        public Range Y { get; private set; }

        /// <summary>
        /// Constructs an MBR encapsulating the coordinate <paramref name="x"/>,<paramref name="y"/>.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public Mbr(double x, double y) {
            X = new Range(x);
            Y = new Range(y);
        }
        /// <summary>
        /// Constructs an MBR encapsulating the coordinates <paramref name="xa"/>,<paramref name="ya"/> and <paramref name="xb"/>,<paramref name="yb"/>.
        /// </summary>
        /// <param name="xa">An x-coordinate.</param>
        /// <param name="ya">A y-coordinate.</param>
        /// <param name="xb">An x-coordinate.</param>
        /// <param name="yb">A y-coordinate.</param>
        public Mbr(double xa, double ya, double xb, double yb) {
            X = new Range(xa, xb);
            Y = new Range(ya, yb);
        }
        /// <summary>
        /// Constructs an MBR encapsulating the given <paramref name="x"/>-range and <paramref name="y"/>-range.
        /// </summary>
        /// <param name="x">A x-axis range.</param>
        /// <param name="y">A y-axis range.</param>
        public Mbr(Range x, Range y) {
            if (null == x) throw new ArgumentNullException("x");
            if (null == y) throw new ArgumentNullException("y");
            Contract.EndContractBlock();
            X = x;
            Y = y;
        }
        /// <summary>
        /// Constructs an MBR encapsulating the given <paramref name="x"/>-range and <paramref name="y"/>-range.
        /// </summary>
        /// <param name="x">A x-axis range.</param>
        /// <param name="y">A y-axis range.</param>
        public Mbr(IRange<double> x, IRange<double> y) {
            if (null == x) throw new ArgumentNullException("x");
            if (null == y) throw new ArgumentNullException("y");
            Contract.EndContractBlock();
            X = new Range(x);
            Y = new Range(y);
        }
        /// <summary>
        /// Constructs an MBR encapsulating the coordinate pair <paramref name="p"/>.
        /// </summary>
        /// <param name="p">A point.</param>
        public Mbr(Point2 p)
            : this(p.X, p.Y) { }

        /// <summary>
        /// Constructs an MBR encapsulating the coordinate pair <paramref name="p"/>.
        /// </summary>
        /// <param name="p">A point.</param>
        public Mbr(ICoordinatePair<double> p) {
            if (null == p) throw new ArgumentNullException("p");
            Contract.EndContractBlock();
            X = new Range(p.X);
            Y = new Range(p.Y);
        }
        /// <summary>
        /// Constructs an MBR encapsulating the coordinate pairs <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">A point.</param>
        /// <param name="b">A point.</param>
        public Mbr(Point2 a, Point2 b)
            : this(a.X, a.Y, b.X, b.Y) { }

        /// <summary>
        /// Constructs an MBR encapsulating the coordinate pairs <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">A point.</param>
        /// <param name="b">A point.</param>
        public Mbr(ICoordinatePair<double> a, ICoordinatePair<double> b) {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");
            Contract.EndContractBlock();
            X = new Range(a.X, b.X);
            Y = new Range(a.Y, b.Y);
        }
        /// <summary>
        /// Constructs a new MBR with the same bounds as the given MBR.
        /// </summary>
        /// <param name="mbr">The MBR to copy the bounds from.</param>
        public Mbr(IMbr<double> mbr) {
            if (null == mbr) throw new ArgumentNullException("mbr");
            Contract.EndContractBlock();
            X = new Range(mbr.XMin, mbr.XMax);
            Y = new Range(mbr.YMin, mbr.YMax);
        }

        /// <summary>
        /// The minimum encompassed value on the x-axis. 
        /// </summary>
        public double XMin {
            get { return X.Low; }
        }

        /// <summary>
        /// The maximum encompassed value on the x-axis. 
        /// </summary>
        public double XMax {
            get { return X.High; }
        }

        /// <summary>
        /// The minimum encompassed value on the y-axis. 
        /// </summary>
        public double YMin {
            get { return Y.Low; }
        }

        /// <summary>
        /// The maximum encompassed value on the y-axis. 
        /// </summary>
        public double YMax {
            get { return Y.High; }
        }

        /// <summary>
        /// The minimum coordinate of this MBR.
        /// </summary>
        public Point2 Min {
            get { return new Point2(XMin, YMin); }
        }

        /// <summary>
        /// The maximum coordinate of this MBR.
        /// </summary>
        public Point2 Max {
            get { return new Point2(XMax, YMax); }
        }

        /// <summary>
        /// The width of the MBR.
        /// </summary>
        public double Width {
            get { return X.GetMagnitude(); }
        }

        /// <summary>
        /// The height of the MBR.
        /// </summary>
        public double Height {
            get { return Y.GetMagnitude(); }
        }

        /// <inheritdoc/>
        public bool Equals(Mbr other) {
            return null != other && X.Equals(other.X) && Y.Equals(other.Y);
        }

        /// <inheritdoc/>
        public bool Equals(IMbr<double> other) {
            return !ReferenceEquals(null, other)
                // ReSharper disable CompareOfFloatsByEqualityOperator
                && XMin == other.XMin
                && XMax == other.XMax
                && YMin == other.YMin
                && YMax == other.YMax
                // ReSharper restore CompareOfFloatsByEqualityOperator
            ;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            var mbr = obj as Mbr;
            return null != mbr
                ? Equals(mbr)
                : Equals(obj as IMbr<double>);
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString() {
            return String.Concat("x:{", X, "} y:{", Y, '}');
        }

        /// <summary>
        /// Creates a new MBR encompassing this MBR as well as the given point.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        /// <returns>A new MBR encompassing this MBR and the given point.</returns>
        public Mbr Encompass(double x, double y) {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return new Mbr(X.Encompass(x), Y.Encompass(y));
        }

        /// <summary>
        /// Creates a new MBR encompassing this MBR as well as the given point.
        /// </summary>
        /// <param name="p">The point to encompass.</param>
        /// <returns>A new MBR encompassing this MBR and the given point.</returns>
        public Mbr Encompass(Point2 p) {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return Encompass(p.X, p.Y);
        }

        /// <summary>
        /// Creates a new MBR encompassing this MBR as well as the given axis ranges.
        /// </summary>
        /// <param name="x">A x-axis range to encompass.</param>
        /// <param name="y">A y-axis range to encompass.</param>
        /// <returns>A new MBR encompassing this MBR and the given axis ranges.</returns>
        public Mbr Encompass(Range x, Range y) {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return new Mbr(X.Encompass(x), Y.Encompass(y));
            /*
            // if range becomes a reference type...
            return null == x
                ? (null == y ? this : new Mbr(X, Y.Encompass(y)))
                : (null == y ? new Mbr(X.Encompass(x), Y) : new Mbr(X.Encompass(x), Y.Encompass(y)));
            */
        }

        /// <summary>
        /// Creates a new MBR encompassing this MBR as well as another.
        /// </summary>
        /// <param name="mbr">The other MBR to encompass.</param>
        /// <returns>A new MBR encompassing this MBR and another.</returns>
        public Mbr Encompass(Mbr mbr) {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return null == mbr ? this : Encompass(mbr.X, mbr.Y);
        }

        /// <summary>
        /// Determines if this MBR intersects another <paramref name="mbr"/>.
        /// </summary>
        /// <param name="mbr">A MBR to test intersection with.</param>
        /// <returns>When an MBR intersects this MBR, <c>true</c>.</returns>
        public bool Intersects(Mbr mbr) {
            if (null == mbr) return false;
            return X.Intersects(mbr.X) && Y.Intersects(mbr.Y);
        }

        /// <summary>
        /// Determines if this MBR intersects another MBR defined by two points.
        /// </summary>
        /// <param name="a">A point defining the boundary of another MBR.</param>
        /// <param name="b">A point defining the boundary of another MBR.</param>
        /// <returns>When an MBR intersects this MBR, <c>true</c>.</returns>
        public bool Intersects(Point2 a, Point2 b) {
            return X.Intersects(a.X, b.X) && Y.Intersects(a.Y, b.Y);
        }

        /// <inheritdoc/>
        public bool Touches(Mbr other) {
            if (null == other)
                return false;
            return (X.Touches(other.X) && Y.Intersects(other.Y))
                || (Y.Touches(other.Y) && X.Intersects(other.X));
        }

        /// <inheritdoc/>
        public bool Crosses(Mbr other) {
            if (null == other)
                return false;

            if (Width == 0) {
                if (Height == 0)
                    return false; // points don't work
                if (other.Width == 0 || other.Height == 0)
                    return false; // only one can be 1D

                return X.Intersects(other.X)
                    && Y.Overlaps(other.Y);
            }
            if (Height == 0){
                if (Width == 0)
                    return false; // points don't work
                if (other.Width == 0 || other.Height == 0)
                    return false; // only one can be 1D

                return Y.Intersects(other.Y)
                    && X.Overlaps(other.X);
            }
            if (other.Width == 0) {
                if (other.Height == 0)
                    return false; // points don't work
                if (Width == 0 || Height == 0)
                    return false; // only one can be 1D

                return X.Intersects(other.X)
                    && Y.Overlaps(other.Y);
            }
            if (other.Height == 0) {
                if (other.Width == 0)
                    return false; // points don't work
                if (Width == 0 || Height == 0)
                    return false; // only one can be 1D

                return Y.Intersects(other.Y)
                    && X.Overlaps(other.X);
            }
            return false;
        }

        /// <inheritdoc/>
        public bool Within(Mbr mbr) {
            return null != mbr && X.Within(mbr.X) && Y.Within(mbr.Y);
        }

        /// <inheritdoc/>
        public bool Contains(Mbr mbr) {
            return null != mbr && X.Contains(mbr.X) && Y.Contains(mbr.Y);
        }

        /// <inheritdoc/>
        public bool Overlaps(Mbr other) {
            if (null == other)
                return false;

            // ReSharper disable CompareOfFloatsByEqualityOperator
            var yMagnitude = Y.GetMagnitude();
            if (0 == X.GetMagnitude()) {
                if (0 == yMagnitude)
                    return false;
                if (other.X.GetMagnitude() != 0 || other.Y.GetMagnitude() == 0)
                    return false;
            }
            else {
                if (0 == yMagnitude) {
                    if (other.X.GetMagnitude() == 0 || other.Y.GetMagnitude() != 0)
                        return false;
                }
                else {
                    if (other.X.GetMagnitude() == 0 || other.Y.GetMagnitude() == 0)
                        return false;
                }
            }
            return !Equals(other) && Intersects(other);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc/>
        public bool Disjoint(Mbr other) {
            if (null == other)
                return true;
            return X.Disjoint(other.X) || Y.Disjoint(other.Y);
        }

        /// <inheritdoc/>
        bool ISpatiallyEquatable<Mbr>.SpatiallyEqual(Mbr other) {
            return null != other && Equals(other);
        }

        /// <summary>
        /// Determines if a point intersects this MBR.
        /// </summary>
        /// <param name="p">A point to test intersection with.</param>
        /// <returns>True when a point intersects this MBR.</returns>
        public bool Intersects(Point2 p) {
            return X.Intersects(p.X) && Y.Intersects(p.Y);
        }

        /// <inheritdoc/>
        public bool Touches(Point2 p) {
            return (X.Touches(p.X) && Y.Intersects(p.Y))
                || (Y.Touches(p.Y) && X.Intersects(p.X))
            ;
        }

        /// <inheritdoc/>
        bool IRelatableCrosses<Point2>.Crosses(Point2 p) {
            return false;
        }

        /// <inheritdoc/>
        public bool Within(Point2 p) {
            return X.Within(p.X) && Y.Within(p.Y);
        }

        /// <inheritdoc/>
        public bool Contains(Point2 p) {
            return X.Contains(p.X) && Y.Contains(p.Y);
        }

        /// <inheritdoc/>
        bool IRelatableOverlaps<Point2>.Overlaps(Point2 p) {
            return false;
        }

        /// <inheritdoc/>
        public bool Disjoint(Point2 p) {
            return X.Disjoint(p.X) || Y.Disjoint(p.Y);
        }

        /// <inheritdoc/>
        public bool SpatiallyEqual(Point2 p) {
            return X.SpatiallyEqual(p.X) && Y.SpatiallyEqual(p.Y);
        }

        /// <summary>
        /// Calculates the distance between this MBR and another <paramref name="mbr"/>.
        /// </summary>
        /// <param name="mbr">The MBR to calculate distance to.</param>
        /// <returns>The distance.</returns>
        public double Distance(Mbr mbr) {
            if (null == mbr) throw new ArgumentNullException("mbr");
            Contract.EndContractBlock();
            return Math.Sqrt(DistanceSquared(mbr));
        }

        /// <summary>
        /// Calculates the squared distance between this MBR and another <paramref name="mbr"/>.
        /// </summary>
        /// <param name="mbr">The MBR to calculate squared distance to.</param>
        /// <returns>The squared distance.</returns>
        public double DistanceSquared(Mbr mbr) {
            if (null == mbr) throw new ArgumentNullException("mbr");
            Contract.EndContractBlock();
            return X.DistanceSquared(mbr.X) + X.DistanceSquared(mbr.Y);
        }

        /// <summary>
        /// Calculates the distance between this MBR and a point <paramref name="p"/>.
        /// </summary>
        /// <param name="p">The point to calculate distance to.</param>
        /// <returns>The distance.</returns>
        public double Distance(Point2 p) {
            return Math.Sqrt(DistanceSquared(p));
        }

        /// <summary>
        /// Calculates the squared distance between this MBR and a point <paramref name="p"/>.
        /// </summary>
        /// <param name="p">The point to calculate squared distance to.</param>
        /// <returns>The squared distance.</returns>
        public double DistanceSquared(Point2 p) {
            return X.DistanceSquared(p.X) + Y.DistanceSquared(p.Y);
        }

        /// <summary>
        /// Calculates the area of the bounding rectangle.
        /// </summary>
        /// <returns>The calculated area.</returns>
        public double GetArea() {
            return Width * Height;
        }

        /// <summary>
        /// Calculates the centroid.
        /// </summary>
        /// <returns>A centroid.</returns>
        public Point2 GetCentroid() {
            return new Point2(X.Mid, Y.Mid);
        }

        /// <summary>
        /// Creates a new minimum bounding rectangle scaled by the given ratio.
        /// </summary>
        /// <param name="factor">The ratio to scale the MBR by.</param>
        /// <returns>A new scaled MBR.</returns>
        public Mbr GetScaled(double factor) {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return GetScaled(factor, factor);
        }

        /// <summary>
        /// Creates a new minimum bounding rectangle scaled by the given ratios.
        /// </summary>
        /// <param name="xFactor">The x-coordinate scaling ratio.</param>
        /// <param name="yFactor">THe y-coordinate scaling ratio.</param>
        /// <returns>A new scaled MBR.</returns>
        public Mbr GetScaled(double xFactor, double yFactor) {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return new Mbr(X.GetScaled(xFactor), Y.GetScaled(yFactor));
        }

        /// <summary>
        /// Creates a new minimum bounding rectangle scaled by the given ratios.
        /// </summary>
        /// <param name="factors">The vector containing the x-coordinate and y-coordinate scaling ratios.</param>
        /// <returns>A new scaled MBR.</returns>
        public Mbr GetScaled(Vector2 factors) {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return new Mbr(X.GetScaled(factors.X), Y.GetScaled(factors.Y));
        }
    }
}
