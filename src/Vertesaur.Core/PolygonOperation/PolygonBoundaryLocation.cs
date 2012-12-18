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
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur.PolygonOperation {
	/// <summary>
	/// A location on a polygon boundary.
	/// </summary>
	public sealed class PolygonBoundaryLocation :
		IEquatable<PolygonBoundaryLocation>,
		IComparable<PolygonBoundaryLocation>
	{

		[Pure]
		private static int Compare(PolygonBoundaryLocation a, PolygonBoundaryLocation b) {
			if(ReferenceEquals(a,null)) {
				if(ReferenceEquals(b, null)) {
					return 0;
				}
				return -1;
			}
			return a.CompareTo(b);
		}

		// TODO: can it be made so that null is an invalid parameter for a or b? May help with performance?

		/// <inheritdoc/>
		public static bool operator ==(PolygonBoundaryLocation a, PolygonBoundaryLocation b) {
			return ReferenceEquals(null,a) ? ReferenceEquals(null,b) : a.Equals(b);
		}
		/// <inheritdoc/>
		public static bool operator !=(PolygonBoundaryLocation a, PolygonBoundaryLocation b) {
			return !(ReferenceEquals(null, a) ? ReferenceEquals(null, b) : a.Equals(b));
		}
		/// <inheritdoc/>
		public static bool operator >(PolygonBoundaryLocation a, PolygonBoundaryLocation b) {
			return Compare(a,b) > 0;
		}
		/// <inheritdoc/>
		public static bool operator >=(PolygonBoundaryLocation a, PolygonBoundaryLocation b) {
			return Compare(a, b) >= 0;
		}
		/// <inheritdoc/>
		public static bool operator <(PolygonBoundaryLocation a, PolygonBoundaryLocation b) {
			return Compare(a, b) < 0;
		}
		/// <inheritdoc/>
		public static bool operator <=(PolygonBoundaryLocation a, PolygonBoundaryLocation b) {
			return Compare(a, b) <= 0;
		}

		/// <summary>
		/// The ring index that the location is on.
		/// </summary>
		public int RingIndex { get; private set; }

		/// <summary>
		/// The segment index that the location is on.
		/// </summary>
		public int SegmentIndex { get; private set; }

		/// <summary>
		/// The approximate ratio along the segment that the location refers to.
		/// This property is not recommended to be used for calculations but is
		/// to be used for sorting.
		/// </summary>
		/// <remarks>
		/// A segment ratio of exactly 0 indicates that the location is the start point of the segment.
		/// A segment ratio of exactly 1.0 indicates that the location is the end point of the segment.
		/// If an intersection is created between the two points that has a ratio of 0 or 1,
		/// you may need to adjust it as these values have special meaning. Offset the value using
		/// an epsilon delta.
		/// Modification of this value may be OK as the location may need to be recalculated
		/// to account for the insertion of newly calculated intersection points.
		/// </remarks>
		public readonly double SegmentRatio;

		/// <summary>
		/// Constructs a new ring location.
		/// </summary>
		/// <param name="ringIndex">The index of the specified ring.</param>
		/// <param name="segmentIndex">The index of the specified segment.</param>
		/// <param name="segmentRatio">The approximate ratio of the location on the segment</param>
		public PolygonBoundaryLocation(int ringIndex, int segmentIndex, double segmentRatio)
		{
			Contract.Requires(ringIndex >= 0);
			Contract.Requires(segmentIndex >= 0);
			Contract.EndContractBlock();

			SegmentIndex = segmentIndex;
			SegmentRatio = segmentRatio;
			RingIndex = ringIndex;
		}

		/// <inheritdoc/>
		[Pure]
		public override int GetHashCode() {
			return RingIndex ^ -SegmentIndex ^ SegmentRatio.GetHashCode();
		}

		/// <inheritdoc/>
		[Pure]
		public override bool Equals(object obj) {
			return Equals(obj as PolygonBoundaryLocation);
		}

		/// <inheritdoc/>
		[Pure]
		public bool Equals(PolygonBoundaryLocation other) {
			return !ReferenceEquals(null, other)
				&& SegmentIndex == other.SegmentIndex
// ReSharper disable CompareOfFloatsByEqualityOperator
				&& SegmentRatio == other.SegmentRatio
// ReSharper restore CompareOfFloatsByEqualityOperator
				&& RingIndex == other.RingIndex;
		}

		/// <inheritdoc/>
		[Pure]
		public override string ToString() {
			return String.Format("{0}:{1}:{2}", RingIndex, SegmentIndex, SegmentRatio);
		}

		/// <inheritdoc/>
		[Pure]
		public int CompareTo(PolygonBoundaryLocation other) {
			if (ReferenceEquals(null, other))
				return 1;
			var compareResult = RingIndex.CompareTo(other.RingIndex);
			if (0 != compareResult)
				return compareResult;
			compareResult = SegmentIndex.CompareTo(other.SegmentIndex);
			return 0 != compareResult
				? compareResult
				: SegmentRatio.CompareTo(other.SegmentRatio);
		}

		[ContractInvariantMethod]
		[Conditional("CONTRACTS_FULL")]
		private void CodeContractInvariant() {
			Contract.Invariant(RingIndex >= 0);
			Contract.Invariant(SegmentIndex >= 0);
		}

	}
}
