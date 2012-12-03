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
using System.Diagnostics.Contracts;
using System.Text;
using JetBrains.Annotations;
using Vertesaur.Contracts;

namespace Vertesaur {

	/// <summary>
	/// A multi-point object in 2D space. A collection of points.
	/// </summary>
	public class MultiPoint2 :
		Collection<Point2>,
		IPlanarGeometry,
		IEquatable<MultiPoint2>,
		IRelatableIntersects<Point2>,
		IHasDistance<Point2, double>,
		IHasMbr<Mbr, double>,
		IHasCentroid<Point2>,
		ICloneable
	{

		/// <summary>
		/// Constructs a new empty multi-point.
		/// </summary>
		public MultiPoint2() { }
		/// <summary>
		/// Constructs a new empty multi-point expecting the given number of points.
		/// </summary>
		/// <param name="expectedCapacity">The expected number of points.</param>
		public MultiPoint2(int expectedCapacity)
			: this(new List<Point2>(expectedCapacity)) { }
		/// <summary>
		/// Constructs a new multi-point containing the given points.
		/// </summary>
		/// <param name="points">The points the multi-point will be composed of.</param>
		public MultiPoint2([CanBeNull] IEnumerable<Point2> points)
			: this(null == points ? null : new List<Point2>(points)) { }
		/// <summary>
		/// This private constructor is used to initialize the collection with a new list.
		/// All constructors must eventually call this constructor.
		/// </summary>
		/// <param name="points">The list that will store the points. This list MUST be owned by this class.</param>
		/// <remarks>
		/// All public access to the points must be through the Collection wrapper around the points list.
		/// </remarks>
		private MultiPoint2([CanBeNull] List<Point2> points)
			: base(points ?? new List<Point2>()) {
		}

		/// <summary>
		/// Calculates the distance between this multi-point and the point, <paramref name="p"/>
		/// </summary>
		/// <param name="p">The point to calculate distance to.</param>
		/// <returns>The distance.</returns>
		public double Distance(Point2 p) {
			return Math.Sqrt(DistanceSquared(p));
		}

		/// <summary>
		/// Calculates the squared distance between this multi-point and the point, <paramref name="p"/>
		/// </summary>
		/// <param name="p">The point to calculate squared distance to.</param>
		/// <returns>The squared distance.</returns>
		public double DistanceSquared(Point2 p) {
			if (Count <= 0)
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
		/// Determines if a point intersects this multi-point.
		/// </summary>
		/// <param name="p">A point to test intersection with.</param>
		/// <returns>True when a point intersects this multi-point.</returns>
		public bool Intersects(Point2 p) {
			for (var i = 0; i < Count; i++) {
				if (this[i].Equals(p)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Calculates a minimum bounding rectangle for this multi-point.
		/// </summary>
		/// <returns>A minimum bounding rectangle.</returns>
		public Mbr GetMbr() {
			return Mbr.Create(this);
		}

		/// <summary>
		/// Calculates the centroid.
		/// </summary>
		/// <returns>A centroid.</returns>
		public Point2 GetCentroid() {
			if (0 == Count)
				return Point2.Invalid;
			if (1 == Count)
				return this[0];

			var xSum = this[0].X;
			var ySum = this[0].Y;
			for (var i = 1; i < Count; i++) {
				xSum += this[i].X;
				ySum += this[i].Y;
			}
			double c = Count;
			return new Point2(xSum / c, ySum / c);
		}

		/// <summary>
		/// Creates an identical multi-point.
		/// </summary>
		/// <returns>A multi-point.</returns>
		/// <remarks>Functions as a deep clone.</remarks>
		[NotNull] public MultiPoint2 Clone() {
			Contract.Ensures(Contract.Result<MultiPoint2>() != null);
			Contract.EndContractBlock();
			return new MultiPoint2(new List<Point2>(this));
		}

		object ICloneable.Clone() {
			return Clone();
		}

		/// <inheritdoc/>
		[ContractAnnotation("null=>false")]
		public bool Equals([CanBeNull] MultiPoint2 other) {
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
		[ContractAnnotation("null=>false")]
		public override bool Equals([CanBeNull] object obj) {
			return Equals(obj as MultiPoint2);
		}

		/// <inheritdoc/>
		public override int GetHashCode() {
			return GetMbr().GetHashCode() ^ 732903982;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString() {
			var sb = new StringBuilder(String.Concat("MultiPoint, ", Count, "points"));

			if (Count < 4) {
				for (var i = 0; i < Count; i++) {
					sb.Append(' ');
					sb.Append(this[i]);
				}
			}
			return sb.ToString();
		}

	}

}
