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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Vertesaur.Contracts;

namespace Vertesaur {

	/// <summary>
	/// A polygon geometry having multiple rings which define multiple fill areas and holes.
	/// </summary>
	/// <remarks>
	/// Multiple fill rings and multiple hole rings are allowed in a polygon.
	/// </remarks>
	/// <seealso cref="Vertesaur.Ring2"/>
	/// <seealso cref="Vertesaur.Segment2"/>
	/// <seealso cref="Vertesaur.PointWinding"/>
	public class Polygon2 :
		Collection<Ring2>,
		IPlanarGeometry,
		IHasMagnitude<double>,
		IHasArea<double>,
		IEquatable<Polygon2>,
		IRelatableIntersects<Point2>,
		IHasIntersectionOperation<Point2,IPlanarGeometry>,
		IRelatableIntersects<MultiPoint2>,
		IHasIntersectionOperation<MultiPoint2,IPlanarGeometry>,
		IHasDistance<Point2, double>,
		IHasCentroid<Point2>,
		ISpatiallyEquatable<Polygon2>,
		ICloneable
	{

		private static List<Ring2> BuildListForSingleRing(Ring2 ring) {
			if(null == ring) throw new ArgumentNullException("ring");
			Contract.Requires(ring != null);
			Contract.Ensures(Contract.Result<List<Ring2>>() != null);
			return new List<Ring2>(1){ring};
		}

		/// <summary>
		/// Constructs a new polygon geometry containing rings.
		/// </summary>
		public Polygon2()
			: this((List<Ring2>)null) { }
		/// <summary>
		/// Constructs a new polygon geometry with no rings but prepared to store the specified number of rings.
		/// </summary>
		/// <param name="expectedCapacity">The expected number of rings the polygon will contain.</param>
		public Polygon2(int expectedCapacity)
			: this(new List<Ring2>(expectedCapacity)) { }
		/// <summary>
		/// Constructs a new polygon geometry composed of the given rings.
		/// </summary>
		/// <param name="rings">The rings the polygon will be composed of.</param>
		/// <remarks>The <paramref name="rings"/> are referenced instead of copied.</remarks>
		public Polygon2(IEnumerable<Ring2> rings)
			: this(null == rings ? null : new List<Ring2>(rings)) { }
		/// <summary>
		/// Constructs a new polygon geometry composed of the given ring.
		/// </summary>
		/// <param name="ring">The ring the polygon will be composed of.</param>
		/// <remarks>The <paramref name="ring"/> is referenced instead of copied.</remarks>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="ring"/> is null.</exception>
		public Polygon2(Ring2 ring)
			: this(BuildListForSingleRing(ring))
		{
			Contract.Requires(ring != null);
		}

		/// <summary>
		/// Constructs a new polygon geometry composed of the given points which form a ring.
		/// </summary>
		/// <param name="points">The points that will form the ring.</param>
		public Polygon2(IEnumerable<Point2> points)
			: this(new Ring2(points)) { }

		/// <summary>
		/// Constructs a new polygon geometry composed of the given points which form a ring.
		/// </summary>
		/// <param name="points">The points that will form the ring.</param>
		/// <param name="hole">True to indicate the polygon represents a hole region.</param>
		public Polygon2(IEnumerable<Point2> points, bool hole)
			: this(new Ring2(points, hole)) { }

		/// <summary>
		/// This private constructor is used to initialize the collection with a new list.
		/// All constructors must eventually call this constructor.
		/// </summary>
		/// <param name="rings">The list that will store the rings. This list MUST be owned by this class.</param>
		/// <remarks>
		/// All public access to the rings must be through the Collection wrapper around the rings list.
		/// </remarks>
		private Polygon2(List<Ring2> rings)
			: base(rings ?? new List<Ring2>()) { }

		/// <summary>
		/// Adds multiple rings to this polygon.
		/// </summary>
		/// <param name="rings">The rings to add.</param>
		/// <exception cref="System.ArgumentException">Thrown if any of the <paramref name="rings"/> is null.</exception>
		public void AddRange(IEnumerable<Ring2> rings) {
			if(null == rings) throw new ArgumentNullException("rings");
			Contract.Requires(Contract.ForAll(rings, x => x != null));

			foreach (var ring in rings) {
				if(null == ring)
					throw new ArgumentException("Null rings are not valid.", "rings");
				Add(ring);
			}
		}

		/// <inheritdoc/>
		protected override void SetItem(int index, Ring2 item) {
			if(null == item) throw new ArgumentNullException("item", "Null rings are not valid.");
			Contract.EndContractBlock();
			base.SetItem(index, item);
		}

		/// <inheritdoc/>
		protected override void InsertItem(int index, Ring2 item) {
			if (null == item) throw new ArgumentNullException("item", "Null rings are not valid.");
			Contract.EndContractBlock();
			base.InsertItem(index, item);
		}

		/// <summary>
		/// Forces the fill winding of the points within all contained rings to be uniform.
		/// </summary>
		/// <param name="desiredWinding">The desired winding order that defines a fill ring.</param>
		/// <seealso cref="Vertesaur.Ring2.ForceFillWinding"/>
		public void ForceFillWinding(PointWinding desiredWinding) {
			if (PointWinding.Unknown == desiredWinding) throw new ArgumentException("desiredWinding may not be Unknown", "desiredWinding");
			Contract.EndContractBlock();
			foreach (var ring in this) {
				ring.ForceFillWinding(desiredWinding);
			}
		}

		/// <summary>
		/// Calculates the centroid.
		/// </summary>
		/// <returns>A centroid.</returns>
		public Point2 GetCentroid() {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (Count > 0) {
				if (Count == 1)
					return this[0].GetCentroid();
				
				var aSum = 0.0;
				var xSum = 0.0;
				var ySum = 0.0;
				foreach (var ring in this) {
					var a = ring.GetArea();
					var p = ring.GetCentroid();
					xSum += p.X * a;
					ySum += p.Y * a;
					aSum += a;
				}
				if (0 != aSum)
					return new Point2(xSum / aSum, ySum / aSum);
			}
			return Point2.Invalid;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// Calculates a minimum bounding rectangle for this polygon.
		/// </summary>
		/// <returns>A minimum bounding rectangle.</returns>
		public Mbr GetMbr() {
			if (Count <= 0)
				return null;

			var mbr = this[0].GetMbr();
			for (var i = 1; i < Count; i++) {
				mbr = mbr.Encompass(this[i].GetMbr());
			}
			return mbr;
		}

		/// <summary>
		/// Determines if another point intersects this polygon.
		/// </summary>
		/// <param name="p">A point to test intersection with.</param>
		/// <returns>True when a point intersects this object.</returns>
		public bool Intersects(Point2 p) {
			var crossCount = 0;
			var hasUnconstrainedHoles = false;
			var fillRings = new List<Ring2>();
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach
			foreach (var ring in this) {
				if (ring.Hole.HasValue && ring.Hole.Value)
					continue;

				fillRings.Add(ring);
			}
			foreach (var r in this) {
				if (r.Count == 0)
					continue;

				var intersectionCount = r.IntersectionPositiveXRayCount(p);
				crossCount += intersectionCount;
				var isHole = r.Hole.HasValue && r.Hole.Value;
				if (!isHole || hasUnconstrainedHoles)
					continue;
				
				var contained = false;
				Contract.Assume(0 < r.Count);
				var hp = r[0];
				for (var i = 0; i < fillRings.Count; i++) {
					if (fillRings[i].Intersects(hp)) {
						contained = true;
						break;
					}
				}

				if (!contained)
					hasUnconstrainedHoles = true;
			}
			return (hasUnconstrainedHoles ? 0 : 1) == (crossCount % 2);
// ReSharper restore ForCanBeConvertedToForeach
// ReSharper restore LoopCanBeConvertedToQuery
		}

		/// <summary>
		/// Calculates the distance between this polygon and a point, <paramref name="p"/>. 
		/// </summary>
		/// <param name="p">The point to calculate distance to.</param>
		/// <returns>The distance between this polygon and <paramref name="p"/>.</returns>
		public double Distance(Point2 p) {
			return Math.Sqrt(DistanceSquared(p));
		}

		/// <summary>
		/// Calculates the squared distance between this polygon and a point, <paramref name="p"/>. 
		/// </summary>
		/// <param name="p">The point to calculate squared distance to.</param>
		/// <returns>The squared distance between this polygon and <paramref name="p"/>.</returns>
		public double DistanceSquared(Point2 p) {
			if (Count == 0)
				return Double.NaN;

			var minDist = this[0].DistanceSquared(p);
			for (var i = 1; i < Count; i++) {
				var localDist = this[i].DistanceSquared(p);
				if (localDist < minDist)
					minDist = localDist;
			}
			return minDist;
		}

		/// <summary>
		/// Determines the perimeter of this polygon.
		/// </summary>
		/// <returns>The perimeter of this polygon.</returns>
		public double GetMagnitude() {
			var sum = 0.0;
			// ReSharper disable LoopCanBeConvertedToQuery
			// ReSharper disable ForCanBeConvertedToForeach
			for (var i = 0; i < Count; i++) {
				sum += this[i].GetMagnitude();
			}
			return sum;
			// ReSharper restore ForCanBeConvertedToForeach
			// ReSharper restore LoopCanBeConvertedToQuery
		}

		/// <inheritdoc/>
		double IHasMagnitude<double>.GetMagnitudeSquared() {
			var m = GetMagnitude();
			return m * m;
		}

		/// <summary>
		/// Calculates the area of this polygon.
		/// </summary>
		/// <returns>The area.</returns>
		public double GetArea() {
			var sum = 0.0;
			// ReSharper disable LoopCanBeConvertedToQuery
			// ReSharper disable ForCanBeConvertedToForeach
			for (var i = 0; i < Count; i++) {
				sum += this[i].GetArea();
			}
			return sum;
			// ReSharper restore ForCanBeConvertedToForeach
			// ReSharper restore LoopCanBeConvertedToQuery
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Polygon2 other) {
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

		/// <summary>
		/// Determines if this polygon occupies the same area as the <paramref name="other"/> polygon.
		/// </summary>
		/// <param name="other">The polygon to compare.</param>
		/// <returns>True when the polygons are spatially equal.</returns>
		public bool SpatiallyEqual(Polygon2 other) {
			if (ReferenceEquals(null, other))
				return false;
			if (Equals(other))
				return true;
			if (Count != other.Count)
				return false;
			if (GetMbr() != other.GetMbr())
				return false;

			var ringsToCheck = new LinkedList<Ring2>(other);
			foreach(var ringA in this) {
				if (ringsToCheck.Count == 0)
					return false; // no rings left to check

				var selected = ringsToCheck.FirstOrDefault(r => r.SpatiallyEqual(ringA));
				if (null == selected)
					return false; // no match for this ring in A

				ringsToCheck.Remove(selected);
			}

			return true;
		}

		/// <summary>
		/// Creates an identical polygon.
		/// </summary>
		/// <returns>A polygon.</returns>
		/// <remarks>Functions as a deep clone.</remarks>
		public Polygon2 Clone() {
			Contract.Ensures(Contract.Result<Polygon2>() != null);
			var p = new Polygon2(Count);
			foreach (var r in this) {
				p.Add(r.Clone());
			}
			return p;
		}
		
		object ICloneable.Clone() {
			return Clone();
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) {
			return Equals(obj as Polygon2);
		}

		/// <inheritdoc/>
		public override int GetHashCode() {
			return GetMbr().GetHashCode() ^ -1111034994;
		}

		/// <inheritdoc/>
		public override string ToString() {
			return String.Concat("Polygon, ", Count, " Rings");
		}

		/// <inheritdoc/>
		public IPlanarGeometry Intersection(Point2 other) {
			return Intersects(other) ? (IPlanarGeometry)other : null;
		}

		/// <inheritdoc/>
		public bool Intersects(MultiPoint2 other) {
			if (Count == 0 || ReferenceEquals(null, other) || other.Count == 0)
				return false;
			return other.Any(Intersects);
		}

		/// <inheritdoc/>
		public IPlanarGeometry Intersection(MultiPoint2 other) {
			if (Count == 0 || ReferenceEquals(null, other) || other.Count == 0)
				return null;
			return MultiPoint2.FixToProperPlanerGeometryResult(new MultiPoint2(other.Where(Intersects)));
		}

		[ContractInvariantMethod]
		[Conditional("CONTRACTS_FULL")]
		private void CodeContractInvariant() {
			Contract.Invariant(Contract.ForAll(this, x => x != null));
			Contract.Invariant(Contract.ForAll(0, Count, i => this[i] != null));
		}
	}
}
