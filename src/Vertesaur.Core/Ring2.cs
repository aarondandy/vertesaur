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
using System.Linq;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using Vertesaur.Contracts;

namespace Vertesaur {
	/// <summary>
	/// A collection of points representing a closed boundary edge of a polygon.
	/// </summary>
	/// <remarks>
	/// <list type="bullet">
	/// <item><term>Point Winding and the <see cref="Vertesaur.Ring2.Hole"/> Property</term>
	/// <description>
	/// It is good practice to set the <see cref="Vertesaur.Ring2.Hole"/> property
	/// to <c>true</c> or <c>false</c> for interoperability purposes.
	/// Winding must be taken into account when operating on these rings and should
	/// either be explicitly set before use of the ring or the <see cref="Vertesaur.Ring2.Hole"/> property
	/// should be set and used.
	/// <see cref="Vertesaur.Ring2.ForceFillWinding"/> can be used to enforce a specific winding
	/// for your needs after setting the <see cref="Vertesaur.Ring2.Hole"/> property of the ring.
	/// </description>
	/// </item>
	/// <item><term>Redundant End Points</term>
	/// <description>
	/// This ring class does not require the storage of matching start and end points and
	/// some algorithms may remove or disregard the end point or may need to compensate for
	/// a missing redundant end point.
	/// It is preferred that a ring does not have a redundant start and end point.
	/// This means that a triangle may be defined by 3 unique points instead of 4 points
	/// where the start and end are equivalent.
	/// This also means that a triangle may be defined by 4 points where the start and end
	/// are equivalent.
	/// The final segment of the ring will stretch from the last point to the first point.
	/// Even when the first and last point are equal there will be a final segment of magnitude 0.
	/// When matching start and end points are required for interoperability, the redundant end
	/// point becomes the responsibility of that functionality.
	/// You may use <see cref="Vertesaur.Ring2.HasRedundantEndPoint"/> to determine if the ring
	/// contains a redundant end point. When possible please, avoid the use of redundant endpoints.
	/// </description>
	/// </item>
	/// </list>
	/// </remarks>
	/// <seealso cref="Vertesaur.Point2"/>
	/// <seealso cref="Vertesaur.Segment2"/>
	/// <seealso cref="Vertesaur.PointWinding"/>
	public class Ring2 :
		Collection<Point2>,
		IPlanarGeometry,
		IHasMagnitude<double>,
		IHasArea<double>,
		IEquatable<Ring2>,
		IRelatableIntersects<Point2>,
		IHasDistance<Point2, double>,
		IHasCentroid<Point2>,
		ISpatiallyEquatable<Ring2>,
		ICloneable
	{


		private class AllPointsCacheData
		{
			public double AreaSumValue;
			public Mbr CalculatedMbr;
		}

		/// <summary>
		/// This is a lazy evaluation for calculated data that should be cached.
		/// </summary>
#if WINDOWS_PHONE
		private AllPointsCacheData _allPointsCacheData;
#warning We removed the use of Lazy which may cause some threading issues.
#else
		private Lazy<AllPointsCacheData> _allPointsCacheData;
#endif
		/// <summary>
		/// Flags the ring as being a fill ring or a hole ring, <c>null</c> means it has not yet been defined.
		/// </summary>
		private bool? _hole;

		/// <summary>
		/// The backing list that stores the points. Do not mutate directly.
		/// </summary>
		/// <remarks>
		/// Only expose this field through the Collection wrapper.
		/// </remarks>
		[NotNull] private readonly List<Point2> _pointList;

		/// <summary>
		/// Constructs a ring with no points.
		/// </summary>
		public Ring2() : this(null, null) { }
		/// <summary>
		/// Constructs a ring with no points while expecting the given number of points at a later time.
		/// </summary>
		/// <param name="expectedCapacity">The expected capacity of the ring.</param>
		public Ring2(int expectedCapacity) : this(null, new List<Point2>(expectedCapacity)) { }
		/// <summary>
		/// Constructs a ring that will be explicitly set as a hole or fill.
		/// </summary>
		/// <param name="hole"><c>true</c> to flag as a hole, <c>false</c> to flag as a fill.</param>
		public Ring2(bool hole) : this(hole, null) { }
		/// <summary>
		/// Constructs a ring with no points while expecting the given number of points at a later time with the hole/fill property explicitly set.
		/// </summary>
		/// <param name="hole"><c>true</c> to flag as a hole, <c>false</c> to flag as a fill.</param>
		/// <param name="expectedCapacity">The expected capacity of the ring.</param>
		public Ring2(bool hole, int expectedCapacity) : this(hole, new List<Point2>(expectedCapacity)) { }
		/// <summary>
		/// Constructs a ring with the given points.
		/// </summary>
		/// <param name="points">The ordered set of points that define the ring.</param>
		public Ring2([CanBeNull] IEnumerable<Point2> points) : this(null, null == points ? null : new List<Point2>(points)) { }
		/// <summary>
		/// Constructs a ring with the given points and with the hole property explicitly set.
		/// </summary>
		/// <param name="points">The ordered set of points that define the ring.</param>
		/// <param name="hole"><c>true</c> to flag as a hole, <c>false</c> to flag as a fill.</param>
		public Ring2([CanBeNull] IEnumerable<Point2> points, bool hole) : this(hole, null == points ? null : new List<Point2>(points)) { }

		/// <summary>
		/// Construct a new ring which is cloned from the given <paramref name="source"/> ring.
		/// </summary>
		/// <param name="source">The source ring to clone.</param>
		public Ring2(Ring2 source) : this(
			null == source ? default(bool?) : source._hole,
			null == source ? null : new List<Point2>(source)
		) { }

		/// <summary>
		/// This private constructor is used to initialize the collection with a new list.
		/// All constructors must eventually call this constructor.
		/// </summary>
		/// <param name="hole">The value for the hole flag.</param>
		/// <param name="points">The list that will store the points. This list MUST be owned by this class.</param>
		/// <remarks>
		/// All public access to the points must be through the Collection wrapper around the points list.
		/// </remarks>
		private Ring2(bool? hole, [CanBeNull] List<Point2> points)
			: base(points = (points ?? new List<Point2>())) {

			_hole = hole;
			_pointList = points;
			ResetAllPointsCacheData();
			Contract.Assume(SegmentCount <= Count);
		}

		private void ResetAllCache() {
			ResetAllPointsCacheData();
		}

		private void ResetAllPointsCacheData() {
#if WINDOWS_PHONE
			_allPointsCacheData = null;
#else
			if (null == _allPointsCacheData || _allPointsCacheData.IsValueCreated)
				_allPointsCacheData = new Lazy<AllPointsCacheData>(
					CalculateAllPointsCacheData,
					LazyThreadSafetyMode.ExecutionAndPublication);
#endif
			Contract.Assume(SegmentCount <= Count);
		}

		private AllPointsCacheData GetAllPointsCacheData() {
#if WINDOWS_PHONE
			return _allPointsCacheData ?? (_allPointsCacheData = CalculateAllPointsCacheData());
#else
			return _allPointsCacheData.Value;
#endif
		}

		[NotNull]
		private AllPointsCacheData CalculateAllPointsCacheData() {
			Contract.Ensures(Contract.Result<AllPointsCacheData>() != null);
			Contract.EndContractBlock();
			return new AllPointsCacheData{
				CalculatedMbr = Mbr.Create(_pointList),
				AreaSumValue = CalculateAreaSumValue()
			};
		}

		/// <inheritdoc/>
		protected override void ClearItems() {
			base.ClearItems();
			ResetAllCache();
		}

		/// <inheritdoc/>
		protected override void InsertItem(int index, Point2 item) {
			base.InsertItem(index, item);
			ResetAllCache();
		}

		/// <inheritdoc/>
		protected override void RemoveItem(int index) {
			base.RemoveItem(index);
			ResetAllCache();
		}

		/// <inheritdoc/>
		protected override void SetItem(int index, Point2 item) {
			base.SetItem(index, item);
			ResetAllCache();
		}

		/// <summary>
		/// Adds a collection of points to the ring.
		/// </summary>
		/// <param name="points">Points to be added.</param>
		public void AddRange([NotNull, InstantHandle] IEnumerable<Point2> points) {
			Contract.Requires(null != points);
			Contract.EndContractBlock();

			_pointList.AddRange(points);
			ResetAllCache();
		}

		/// <summary>
		/// Used to mark a ring as being a hole or a fill.
		/// </summary>
		/// <remarks>
		/// <list type="table">
		/// <item><term><c>true</c></term><description>The ring is a hole.</description></item>
		/// <item><term><c>false</c></term><description>The ring is a fill.</description></item>
		/// <item><term><c>null</c></term><description>It is unknown or undeclared if the ring is a hole or fill.</description></item>
		/// </list>
		/// </remarks>
		public bool? Hole {
			get {
				return _hole;
			}
			set {
				_hole = value;
				Contract.Assume(SegmentCount <= Count);
			}
		}

		/// <summary>
		/// Determines if the first point and the last point in this ring are equal.
		/// </summary>
		public bool HasRedundantEndPoint {
			get {
				var lastIndex = _pointList.Count - 1;
				return (lastIndex >= 2 && _pointList[0].Equals(_pointList[lastIndex]));
			}
		}

		/// <summary>
		/// Determines which side of the directed boundary is the fill side.
		/// </summary>
		/// <remarks>
		/// This property is biased towards counter-clockwise winding in the event it cannot determine winding.
		/// </remarks>
		public RelativeDirectionType FillSide {
			get {
				// fillSide	=	holeA	^	Winding
				// ----------------------------------
				//	Left	|	fill	|	is CCW
				//	Right	|	fill	|	is CW
				//	Right	|	hole	|	is CCW
				//	Left	|	hole	|	is CW
				return (DetermineWinding() == PointWinding.Clockwise) ^ (_hole.HasValue && _hole.Value)
					? RelativeDirectionType.Right
					: RelativeDirectionType.Left;
			}
		}

		/// <summary>
		/// Determines the number of segments within this ring.
		/// </summary>
		/// <remarks>
		/// There can never be two segments.
		/// 
		/// Be aware that the number of segments may be one more than expected if
		/// a redundant endpoint is used. When possible please, avoid the use of redundant endpoints.
		/// </remarks>
		public int SegmentCount {
			get {
				var c = _pointList.Count;
				return (
					c >= 3
					? c
					: (0 == c ? 0 : c - 1)
				);
			}
		}

		/// <inheritdoc/>
		[ContractAnnotation("null=>false")]
		public override bool Equals([CanBeNull] object obj) {
			return Equals(obj as Ring2);
		}

		/// <inheritdoc/>
		public override int GetHashCode() {
			int code = unchecked(Count^929283730);
			for (int i = 0; i < 3 && i < Count; i++)
				code ^= this[i].GetHashCode();
			return code;
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		[ContractAnnotation("null=>false")]
		public bool Equals([CanBeNull] Ring2 other) {
			if (ReferenceEquals(null, other))
				return false; 
			if (ReferenceEquals(this, other))
				return true;
			if (Count != other.Count || !_hole.Equals(other._hole))
				return false;
			for (var i = 0; i < _pointList.Count; i++) {
				Contract.Assume(i < _pointList.Count);
				Contract.Assume(i < other.Count);
				if (!_pointList[i].Equals(other[i]))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Determines if two rings are spatially equal.
		/// </summary>
		/// <param name="other">Another ring to compare.</param>
		/// <returns>true when the given ring is spatially equal to this ring.</returns>
		[ContractAnnotation("null=>false")]
		public bool SpatiallyEqual(Ring2 other) {
			if (ReferenceEquals(null, other))
				return false;

			if (Equals(other))
				return true;

			if (GetMbr() != other.GetMbr())
				return false;

			if (DetermineWinding() != other.DetermineWinding())
				return false;

			if (!Hole.HasValue && other.Hole.HasValue || Hole.HasValue && !other.Hole.HasValue) {
				if (DetermineWinding() != other.DetermineWinding())
					return false;
			}
			else {
				if (FillSide != other.FillSide)
					return false;
			}

			// TODO: MUST OPTIMIZE THIS GARBAGE
			if (!AllPointsOnBoundary(this, other))
				return false;

			if (!AllPointsOnBoundary(other, this))
				return false;

			return true;
		}

		private static bool AllPointsOnBoundary([NotNull] Ring2 points, [NotNull] Ring2 boundary) {
			Contract.Requires(points != null);
			Contract.Requires(boundary != null);
			Contract.EndContractBlock();

			int segmentCount = boundary.SegmentCount;
			if (segmentCount == 0) {
				if (boundary.Count == 0)
					return points.Count == 0;
				if (boundary.Count == 1)
					return points.All(p => p.Equals(boundary[0]));
			}
			if (segmentCount == 1 || segmentCount == 2) {
				var s = boundary.GetSegment(0);
				return points.All(s.Intersects);
			}

			int indexHint = 0;
			foreach(var p in points) {
				int i = indexHint;
				var ok = false;
				do {
					Contract.Assume(segmentCount == boundary.SegmentCount);
					Contract.Assume(i >= 0 && i < boundary.SegmentCount);
					if(boundary.GetSegment(i).Intersects(p)) {
						indexHint = i;
						ok = true;
						break;
					}
					i = Utility.IterationUtils.AdvanceLoopingIndex(i, segmentCount);
				} while (i != indexHint);
				if (!ok)
					return false;
			}
			return true;
		}

		/// <inheritdoc/>
		public override string ToString() {
			var sb = new StringBuilder("Ring, " + _pointList.Count + "points");
			
			if (_hole.HasValue) {
				sb.Append(", ");
				sb.Append(_hole.Value ? "hole" : "fill");
			}

			const int maxDisplay = 4;
			for (var i = 0; i < Math.Min(_pointList.Count, maxDisplay); i++) {
				sb.Append(' ');
				sb.Append(_pointList[i]);
			}

			if (_pointList.Count > maxDisplay)
				sb.Append("...");
			
			return sb.ToString();
		}

		/// <summary>
		/// Creates a segment at the specified index within this ring.
		/// </summary>
		/// <param name="i">The segment number to create.</param>
		/// <returns>A segment.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Parameter <paramref name="i"/> must be positive and less than <see cref="Vertesaur.Ring2.SegmentCount"/> for this ring.</exception>
		/// <remarks>
		/// Be aware that the number of segments may be one more than expected if
		/// a redundant endpoint is used. When possible please, avoid the use of redundant endpoints.
		/// </remarks>
		[NotNull]
		public Segment2 GetSegment(int i) {
			if (i < 0 || i >= SegmentCount) throw new ArgumentOutOfRangeException("i", "Parameter i must be positive and less than SegmentCount for this ring.");
			Contract.Ensures(Contract.Result<Segment2>() != null);
			Contract.EndContractBlock();
			return new Segment2(
				_pointList[i],
				_pointList[((i + 1) >= _pointList.Count) ? 0 : (i + 1)]
			);
		}

		/// <summary>
		/// Creates a new ring which is the inverse of this ring.
		/// </summary>
		/// <returns>The inverse of this ring.</returns>
		/// <remarks>
		/// The inverse function will create a hole from a fill or a fill from a hole.
		/// If the hold flag is not set the point winding order will be reversed.
		/// </remarks>
		[NotNull]
		public Ring2 GetInverse() {
			Contract.Ensures(Contract.Result<Ring2>() != null);
			Contract.EndContractBlock();
			if (Hole.HasValue)
				return new Ring2(this.Reverse(), !Hole.Value);
			return new Ring2(this.Reverse());
		}

		/// <summary>
		/// Determines the perimeter of the ring.
		/// </summary>
		/// <returns>The perimeter of the ring.</returns>
		public double GetMagnitude() {
			var lastIndex = _pointList.Count - 1;
			if (lastIndex > 1) {
				var sum = _pointList[lastIndex].Distance(_pointList[0]);
				for (int i = 0, nextIndex = 1; i < lastIndex; i = nextIndex++) {
					sum += _pointList[i].Distance(_pointList[nextIndex]);
				}
				return sum;
			}
			return 1 == lastIndex ? _pointList[0].Distance(_pointList[1]) : 0.0;
		}

		/// <inheritdoc/>
		double IHasMagnitude<double>.GetMagnitudeSquared() {
			var m = GetMagnitude();
			return m * m;
		}

		private double CalculateAreaSumValue() {
			if (SegmentCount < 3)
				return 0;

			var sum = 0.0;
			var lastIndex = _pointList.Count - 1;
			for (int i = lastIndex, nextIndex = 0; nextIndex <= lastIndex; i = nextIndex++) {
				var a = _pointList[i];
				var b = _pointList[nextIndex];
				sum += (a.X * b.Y) - (b.X * a.Y);
			}
			return sum;
		}

		private double GetAreaSumValue() {
			return GetAllPointsCacheData().AreaSumValue;
		}

		/// <summary>
		/// Calculates the area of the interior of the ring.
		/// </summary>
		/// <returns>The area of the interior of the ring.</returns>
		/// <remarks>
		/// The area of a hole and a fill made of the same points will both have the same magnitude but a different sign
		/// <list type="table">
		/// <item><term>Fill</term><description>Will return the area of the interior of the ring boundary.</description></item>
		/// <item><term>Hole</term><description>Will return the negative area of the interior of the ring boundary (as Double.Infinity would not be of much use).</description></item>
		/// <item><term>Undefined</term><description>CounterClockwise point ordering will be treated as a fill, Clockwise ordering will be treated as a hole, and Unknown ordering will have an area of 0.</description></item>
		/// </list>
		/// </remarks>
		public double GetArea() {
			var sum = GetAreaSumValue() / 2.0;
			return _hole.HasValue && (_hole.Value ^ (sum < 0))
				? -sum
				: sum;
		}

		/// <summary>
		/// Determines the centroid of the ring.
		/// </summary>
		/// <returns>The centroid of the ring.</returns>
		public Point2 GetCentroid() {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			var lastIndex = _pointList.Count - 1;
			if (lastIndex > 1) {
				var sumValue = 0.0;
				var xSum = 0.0;
				var ySum = 0.0;
				for (int i = lastIndex, nextIndex = 0; nextIndex <= lastIndex; i = nextIndex++) {
					var a = _pointList[i];
					var b = _pointList[nextIndex];
					var f = (a.X * b.Y) - (b.X * a.Y);
					xSum += (a.X + b.X) * f;
					ySum += (a.Y + b.Y) * f;
					sumValue += f;
				}
				if (0 != sumValue) {
					sumValue *= 3.0;
					return new Point2(xSum / sumValue, ySum / sumValue);
				}
				return _pointList[0];
			}
			return (
				0 == lastIndex
				? _pointList[0]
				: (
					1 == lastIndex
					? new Point2(
						(_pointList[0].X + _pointList[1].X) / 2.0,
						(_pointList[0].Y + _pointList[1].Y) / 2.0
					)
					: Point2.Invalid
				)
			);
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// Determines the point winding of the ring.
		/// </summary>
		/// <remarks><list type="table">
		/// <item><term>Clockwise</term><description>The points in the ring are specified in clockwise order.</description></item>
		/// <item><term>CounterClockwise</term><description>The points in the ring are specified in counter clockwise order.</description></item>
		/// <item><term>Unknown</term><description>The order of the points in the ring can not be determined.</description></item>
		/// </list></remarks>
		/// <returns>The point winding of the ring.</returns>
		public PointWinding DetermineWinding() {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			var areaSum = GetAreaSumValue();
			return (
				(0 == areaSum || Double.IsNaN(areaSum) || Double.IsInfinity(areaSum))
				? PointWinding.Unknown
				: (
					0 > areaSum
					? PointWinding.Clockwise
					: PointWinding.CounterClockwise
				)
			);
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// Forces the fill winding of the points within the ring.
		/// </summary>
		/// <param name="desiredWinding">The desired winding order to specify a fill region.</param>
		/// <remarks>
		/// If the current <see cref="Vertesaur.Ring2.Hole"/> property of this ring is <c>null</c>
		/// then the Hole property will be set if possible to the value corresponding to the given
		/// fill point winding.
		/// If the current <see cref="Vertesaur.Ring2.Hole"/> property of this ring is <c>true</c>
		/// or <c>false</c> then the points will be ordered if possible in a way to match the given
		/// fill point winding and hole property.
		/// <list type="bullet">
		/// <item><term>Example 1</term><description>
		/// Hole is initially <c>null</c> and points are in CounterClockwise order.
		/// If <paramref name="desiredWinding"/> is Clockwise then Hole will then be set to <c>true</c>
		/// and the points will not be changed.
		/// </description></item>
		/// <item><term>Example 2</term><description>
		/// Hole is <c>false</c> and points are in Clockwise order. If <paramref name="desiredWinding"/>
		/// is CounterClockwise then the order of the points in the ring will be reversed.
		/// </description></item>
		/// </list>
		/// </remarks>
		public void ForceFillWinding(PointWinding desiredWinding) {
			if (PointWinding.Unknown == desiredWinding)
				throw new ArgumentException("DesiredWinding may not be Unknown.", "desiredWinding");
			Contract.EndContractBlock();
			
			var currentWinding = DetermineWinding();
			if (PointWinding.Unknown == currentWinding)
				return;

			if (_hole.HasValue) {
				if (
					(currentWinding != desiredWinding && !_hole.Value)
					||
					(currentWinding == desiredWinding && _hole.Value)
				) {
					_pointList.Reverse();
				}
			}
			else {
				_hole = (currentWinding != desiredWinding);
			}
			Contract.Assume(SegmentCount <= Count);
		}

		

		/// <summary>
		/// Determines if the point intersects the filled region or boundary of this ring.
		/// </summary>
		/// <param name="p">The point to test.</param>
		/// <returns><c>true</c> if the point intersects the filled region or boundary of this ring.</returns>
		/// <remarks>
		/// If this ring is a fill the filled region lies within the boundary but when the ring is a hole the filled region lies outside the ring boundary.
		/// </remarks>
		public bool Intersects(Point2 p) {
			return Intersects(p, true);
		}

		/// <summary>
		/// Determines the distance from this ring to the given point <paramref name="p"/>.
		/// </summary>
		/// <param name="p">The point to determine the distance to.</param>
		/// <returns>The distance from this ring to the given point <paramref name="p"/>.</returns>
		/// <remarks>
		/// The distance will be 0 when the ring is a fill and the point lies within the boundary, when the ring is a hole and the point lies outside of the boundary, or when the point lies on the boundary.
		/// </remarks>
		public double Distance(Point2 p) {
			return Math.Sqrt(DistanceSquared(p));
		}

		/// <summary>
		/// Determines the squared distance from this ring to the given point <paramref name="p"/>.
		/// </summary>
		/// <param name="p">The point to determine the squared distance to.</param>
		/// <returns>The squared distance from this ring to the given point <paramref name="p"/>.</returns>
		/// <remarks>
		/// The squared distance will be 0 when the ring is a fill and the point lies within the boundary, when the ring is a hole and the point lies outside of the boundary, or when the point lies on the boundary.
		/// </remarks>
		public double DistanceSquared(Point2 p) {
			var lastIndex = _pointList.Count - 1;
			if (lastIndex > 1) {
				if (Intersects(p, false)) {
					return 0;
				}
				var d = Segment2.DistanceSquared(_pointList[lastIndex], _pointList[0], p);
				for (int i = 0, nextIndex = 1; i < lastIndex; i = nextIndex++) {
					d = Math.Min(d, Segment2.DistanceSquared(_pointList[i], _pointList[nextIndex], p));
				}
				return d;
			}
			return 0 == lastIndex
				? _pointList[0].DistanceSquared(p)
				: 1 == lastIndex
					? Segment2.DistanceSquared(_pointList[0], _pointList[1], p)
					: Double.NaN
			;
		}

		/// <summary>
		/// Calculates a MBR encapsulating all points within this ring.
		/// </summary>
		/// <returns>A MBR encapsulating all points within this ring.</returns>
		public Mbr GetMbr() {
			return GetAllPointsCacheData().CalculatedMbr;
		}

		/// <summary>
		/// Creates an identical ring.
		/// </summary>
		/// <returns>A ring.</returns>
		/// <remarks>Functions as a deep clone.</remarks>
		[NotNull] public Ring2 Clone() {
			Contract.Ensures(Contract.Result<Ring2>() != null);
			Contract.EndContractBlock();
			var points = new List<Point2>(_pointList.Count);
			points.AddRange(_pointList);
			Contract.Assume(SegmentCount <= Count);
			return new Ring2(_hole, points);
		}

		object ICloneable.Clone() {
			return Clone();
		}

		internal int IntersectionPositiveXRayCount(Point2 p) {
			var lastIndex = _pointList.Count - 1;
			var intersectionCount = 0;
			for (int i = lastIndex, nextIndex = 0; nextIndex <= lastIndex; i = nextIndex++) {
				var a = _pointList[i];
				var b = _pointList[nextIndex];
				if (
					(
						p.Y < b.Y
						&&
						a.Y <= p.Y
						&&
						(
							((p.Y - a.Y) * (b.X - a.X))
							>
							((p.X - a.X) * (b.Y - a.Y))
						)
					)
					||
					(
						p.Y < a.Y
						&&
						b.Y <= p.Y
						&&
						(
							((p.Y - a.Y) * (b.X - a.X))
							<
							((p.X - a.X) * (b.Y - a.Y))
						)
					)
				) {
					intersectionCount++;
				}
			}
			return intersectionCount;
		}

		internal bool Intersects(Point2 p, bool guaranteeEdge) {
			var lastIndex = _pointList.Count - 1;
			if (2 <= lastIndex) {
				var intersectionCount = IntersectionPositiveXRayCount(p);
				if (intersectionCount > 0) {
					return (intersectionCount % 2) == ((_hole.HasValue && _hole.Value) ? 0 : 1);
				}
				if ((_hole.HasValue && _hole.Value)) {
					return true;
				}
				if (guaranteeEdge) {
					for (int i = lastIndex, nextIndex = 0; nextIndex <= lastIndex; i = nextIndex++) {
						if (Segment2.Intersects(_pointList[i], _pointList[nextIndex], p)) {
							return true;
						}
					}
				}
				return false;
			}
			return (
				0 == lastIndex
				? _pointList[0].Equals(p)
				: 1 == lastIndex && Segment2.Intersects(_pointList[0], _pointList[1], p)
			);
		}

		/// <summary>
		/// Determines if this ring is within the given ring.
		/// </summary>
		/// <param name="testContainer">The ring to test with.</param>
		/// <returns>true if this ring is within the other ring.</returns>
		[ContractAnnotation("null=>false")]
		internal bool NonIntersectingWithin([CanBeNull] Ring2 testContainer) {
			if (ReferenceEquals(null, testContainer))
				return false;

			if (testContainer.Hole.HasValue && testContainer.Hole.Value) {
				if (!GetMbr().Intersects(testContainer.GetMbr())) {
					return true;
				}
			}
			else {
				if (!GetMbr().Intersects(testContainer.GetMbr())) {
					return false;
				}
			}
			// need to test points
			bool mayHaveIt = false;
			foreach (var p in this) {
				int res = testContainer.AdvancedIntersectionTest(p);
				if (res > 0) {
					return true;
				}
				if (res < 0) {
					return false;
				}
				mayHaveIt = true;
			}
			return mayHaveIt;
		}

		/// <summary>
		/// Advanced intersection testing.
		/// </summary>
		/// <param name="p"></param>
		/// <returns>0 for boundary, 1 for within, -1 for outside</returns>
		internal int AdvancedIntersectionTest(Point2 p) {
			if (Count >= 2 && GetMbr().Intersects(p)) {
				Contract.Assume(Count >= 2);
				var b = this[Count - 1];
				for (var nextIndex = 0; nextIndex < Count; nextIndex++) {
					var a = b;
					b = this[nextIndex];
					if (Segment2.Intersects(a, b, p)) {
						return 0;
					}
				}
			}
			return Intersects(p, false) ? 1 : -1;

		}

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(SegmentCount <= Count);
			Contract.Invariant(SegmentCount >= 0);
			Contract.Invariant(Count >= 0);
			Contract.Invariant(_pointList != null);
		}
		
	}
}
