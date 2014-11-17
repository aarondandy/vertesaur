using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Vertesaur.Utility;

namespace Vertesaur
{

    // ReSharper disable LoopCanBeConvertedToQuery

    /// <summary>
    /// A collection of line strings.
    /// </summary>
    /// <seealso cref="Vertesaur.LineString2"/>
    public class MultiLineString2 :
        Collection<LineString2>,
        IPlanarGeometry,
        IHasMagnitude<double>,
        IEquatable<MultiLineString2>,
        IRelatableIntersects<Point2>,
        IHasIntersectionOperation<Point2, IPlanarGeometry>,
        IRelatableIntersects<MultiPoint2>,
        IHasIntersectionOperation<MultiPoint2, IPlanarGeometry>,
        IHasDistance<Point2, double>,
        IHasMbr<Mbr, double>,
        IHasCentroid<Point2>,
        ICloneable
    {

        /// <summary>
        /// Constructs a new empty multi-line string.
        /// </summary>
        public MultiLineString2() { }

        /// <summary>
        /// Constructs a new empty multi-line string expecting the given number of line strings.
        /// </summary>
        /// <param name="expectedCapacity">The expected number of line strings.</param>
        public MultiLineString2(int expectedCapacity)
            : this(new List<LineString2>(expectedCapacity)) {
            Contract.Requires(expectedCapacity >= 0);
        }

        private static List<LineString2> ToList(IEnumerable<LineString2> lineStrings) {
            Contract.Ensures(Contract.Result<List<LineString2>>() != null);
            Contract.Ensures(Contract.ForAll(Contract.Result<List<LineString2>>(), x => x != null));

            var result = lineStrings == null
                ? new List<LineString2>()
                : lineStrings.Where(x => x != null).ToList();
            Contract.Assume(Contract.ForAll(result, x => x != null));
            return result;
        }

        /// <summary>
        /// Constructs a new multi-line string containing the given line strings.
        /// </summary>
        /// <param name="lineStrings">The line strings.</param>
        public MultiLineString2(IEnumerable<LineString2> lineStrings)
            : this(ToList(lineStrings)) { }

        /// <summary>
        /// This private constructor is used to initialize the collection with a new list.
        /// All constructors must eventually call this constructor.
        /// </summary>
        /// <param name="lineStrings">The list that will store the line strings. This list MUST be owned by this class.</param>
        /// <remarks>
        /// All public access to the points must be through the Collection wrapper around the points list.
        /// </remarks>
        private MultiLineString2(List<LineString2> lineStrings)
            : base(lineStrings) {
            Contract.Requires(lineStrings != null);
            Contract.Requires(Contract.ForAll(lineStrings, x => x != null));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants() {
            Contract.Invariant(Contract.ForAll(this, x => x != null));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(MultiLineString2 other) {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (Count != other.Count)
                return false;

            for (var i = 0; i < Count; i++) {
                if (!this[i].Equals(other[i])) {
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc/>
        protected override void SetItem(int index, LineString2 item) {
            if (null == item) throw new ArgumentNullException("item", "Null line strings are not allowed.");
            Contract.EndContractBlock();
            base.SetItem(index, item);
        }

        /// <inheritdoc/>
        protected override void InsertItem(int index, LineString2 item) {
            if (null == item) throw new ArgumentNullException("item", "Null line strings are not allowed.");
            Contract.EndContractBlock();
            base.InsertItem(index, item);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            return Equals(obj as MultiLineString2);
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            var hash = -700084810 ^ unchecked(Count * 3);
            var mbr = GetMbr();
            if (mbr != null)
                hash ^= mbr.GetHashCode();
            return hash;
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            var result = "MultiLineString, " + Count + " LineString";
            return 1 != Count ? String.Concat(result, 's') : result;
        }

        /// <summary>
        /// Clones this multi-line string.
        /// </summary>
        /// <returns>A multi-line string.</returns>
        /// <remarks>Functions as a deep clone.</remarks>
        public MultiLineString2 Clone() {
            Contract.Ensures(Contract.Result<MultiLineString2>() != null);
            var lines = new List<LineString2>(Count);
            for (var i = 0; i < Count; i++) {
                Contract.Assume(this[i] != null);
                lines.Add(this[i].Clone());
            }
            Contract.Assume(Contract.ForAll(lines, x => x != null));
            return new MultiLineString2(lines);
        }

        object ICloneable.Clone() {
            return Clone();
        }

        /// <summary>
        /// Calculates a minimum bounding rectangle for this multi-line string.
        /// </summary>
        /// <returns>A minimum bounding rectangle.</returns>
        public Mbr GetMbr() {
            Contract.Ensures(Count != 0 || Contract.Result<Mbr>() == null);
            Mbr mbr = null;
            for (var i = 0; i < Count; i++) {
                Contract.Assume(this[i] != null);
                var thisMbr = this[i].GetMbr();
                mbr = mbr == null
                    ? thisMbr
                    : mbr.Encompass(thisMbr);
            }
            return mbr;
        }

        /// <summary>
        /// Calculates the centroid of all the line strings.
        /// </summary>
        /// <returns>A centroid.</returns>
        public Point2 GetCentroid() {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            var lastIndex = Count - 1;
            if (lastIndex > 0) {
                var mSum = 0.0;
                var xSum = 0.0;
                var ySum = 0.0;
                foreach (var ls in this) {
                    if (0 == ls.Count)
                        continue;

                    var m = ls.GetMagnitude();
                    var p = ls.GetCentroid();
                    xSum += p.X * m;
                    ySum += p.Y * m;
                    mSum += m;
                }

                if (0 != mSum)
                    return new Point2(xSum / mSum, ySum / mSum);
            }
            Contract.Assume(this[0] != null);
            return lastIndex == 0 ? this[0].GetCentroid() : Point2.Invalid;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Calculates the magnitude of this multi-line string which is the sum of the magnitudes for all line strings within.
        /// </summary>
        /// <returns>The magnitude or length.</returns>
        public double GetMagnitude() {
            Contract.Ensures(Contract.Result<double>() >= 0.0 || Contract.Result<double>().Equals(Double.NaN));
            if (0 == Count)
                return Double.NaN;

            Contract.Assume(this[0] != null);
            var sum = this[0].GetMagnitude();
            for (var i = 1; i < Count; i++) {
                Contract.Assume(this[i] != null);
                sum += this[i].GetMagnitude();
            }
            return sum;
        }
        /// <summary>
        /// Calculates the squared magnitude of this multi-line string.
        /// </summary>
        /// <returns>The squared magnitude or squared length.</returns>
        public double GetMagnitudeSquared() {
            Contract.Ensures(Contract.Result<double>() >= 0.0 || Contract.Result<double>().Equals(Double.NaN));
            var m = GetMagnitude();
            return m * m;
        }

        /// <summary>
        /// Calculates the distance between this multi-line string and the point, <paramref name="p"/>.
        /// </summary>
        /// <param name="p">The point to calculate distance to.</param>
        /// <returns>The distance.</returns>
        public double Distance(Point2 p) {
            Contract.Ensures(Contract.Result<double>() >= 0.0 || Contract.Result<double>().Equals(Double.NaN));
            return Math.Sqrt(DistanceSquared(p));
        }

        /// <summary>
        /// Calculates the squared distance between this multi-line string and the point, <paramref name="p"/>
        /// </summary>
        /// <param name="p">The point to calculate squared distance to.</param>
        /// <returns>The squared distance.</returns>
        public double DistanceSquared(Point2 p) {
            Contract.Ensures(Contract.Result<double>() >= 0.0 || Contract.Result<double>().Equals(Double.NaN));
            if (0 == Count)
                return Double.NaN;

            var minDist = Double.NaN;
            for (var i = 0; i < Count; i++) {
                Contract.Assume(this[i] != null);
                var localDist = this[i].DistanceSquared(p);
                if (Double.IsNaN(minDist) || localDist < minDist)
                    minDist = localDist;
            }
            return minDist;
        }

        /// <summary>
        /// Determines if a point intersects this multi-line string.
        /// </summary>
        /// <param name="p">A point to test intersection with.</param>
        /// <returns>True when a point intersects this multi-line string.</returns>
        public bool Intersects(Point2 p) {
            for (var i = 0; i < Count; i++) {
                Contract.Assume(this[i] != null);
                if (this[i].Intersects(p))
                    return true;
            }
            return false;
        }
        /// <inheritdoc/>
        public IPlanarGeometry Intersection(Point2 other) {
            return Intersects(other) ? (IPlanarGeometry)other : null;
        }
        /// <inheritdoc/>
        public bool Intersects(MultiPoint2 other) {
            for (var i = 0; i < Count; i++) {
                Contract.Assume(this[i] != null);
                if (this[i].Intersects(other))
                    return true;
            }
            return false;
        }
        /// <inheritdoc/>
        public IPlanarGeometry Intersection(MultiPoint2 other) {
            if (Count == 0)
                return null;
            return MultiPoint2.FixToProperPlanerGeometryResult(new MultiPoint2(other.Where(Intersects)));
        }
    }

}
