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
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;
using Vertesaur.Contracts;
using Vertesaur.Utility;
using CrossingType = Vertesaur.PolygonOperation.PolygonCrossing.CrossingType;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace Vertesaur.PolygonOperation {

	/// <summary>
	/// An operation that will find the geometric result of intersecting two polygons.
	/// </summary>
	public class PolygonIntersectionOperation
	{

		private sealed class IntersectionResults
		{
			[CanBeNull] public Polygon2 Polygon;
			//public MultiLineString2 LineStrings;
			//public MultiPoint2 Points;
		}

		private sealed class PolygonCrossingsData
		{

			public List<PolygonCrossing> AllCrossings;
			public Dictionary<int, List<PolygonCrossing>> RingCrossingsA;
			public Dictionary<int, List<PolygonCrossing>> RingCrossingsB;
			public HashSet<PolygonCrossing> Entrances;
			public HashSet<PolygonCrossing> Exits;
			public Dictionary<PolygonCrossing, PolygonCrossing> EntranceHops;
			public Dictionary<PolygonCrossing, PolygonCrossing> ExitHops;
			public List<PolygonCrossing> VisitedCrossings;

			public void VisitEntrance(PolygonCrossing entrance) {
				if(Entrances.Remove(entrance))
					VisitedCrossings.Add(entrance);
			}

			public void VisitExit(PolygonCrossing exit) {
				if(Exits.Remove(exit))
					VisitedCrossings.Add(exit);
			}

			[CanBeNull]
			public PolygonCrossing FindNextStartableEntrance() {
				Contract.Requires(Entrances != null);
				Contract.Requires(EntranceHops != null);
				Contract.Requires(ExitHops != null);

				return Entrances.FirstOrDefault(entrance =>
					!EntranceHops.ContainsKey(entrance)
					&& !ExitHops.ContainsValue(entrance)
				);
			}

			[NotNull]
			public IEnumerable<Ring2> FindUntouchedRings([NotNull] Polygon2 poly, [NotNull, InstantHandle] Func<PolygonCrossing, int> getRingIndex) {
				Contract.Requires(poly != null);
				Contract.Requires(getRingIndex != null);
				Contract.Ensures(Contract.Result<IEnumerable<Ring2>>() != null);
				Contract.EndContractBlock();

				if(null == VisitedCrossings || 0 == VisitedCrossings.Count)
					return poly;
				var includedRingIndices = new HashSet<int>(VisitedCrossings.Select(getRingIndex));
				return poly.Count == includedRingIndices.Count
					? Enumerable.Empty<Ring2>()
					: poly.Where((ring, i) => !includedRingIndices.Contains(i));
			}
		}

		[Pure] private static PolygonBoundaryLocation GetLocationA([NotNull] PolygonCrossing crossing) {
			Contract.Requires(crossing != null);
			Contract.EndContractBlock();

			return crossing.LocationA;
		}

		[Pure] private static PolygonBoundaryLocation GetLocationB([NotNull] PolygonCrossing crossing) {
			Contract.Requires(crossing != null);
			Contract.EndContractBlock();

			return crossing.LocationB;
		}

		[Pure] private static int GetRingIndexA([NotNull] PolygonCrossing crossing) {
			Contract.Requires(crossing != null);
			Contract.EndContractBlock();

			return crossing.LocationA.RingIndex;
		}

		[Pure] private static int GetRingIndexB([NotNull] PolygonCrossing crossing) {
			Contract.Requires(crossing != null);
			Contract.EndContractBlock();

			return crossing.LocationB.RingIndex;
		}

		[NotNull] private readonly PolygonBinaryOperationOptions _options;

		/// <summary>
		/// Constructs a default polygon intersection operation.
		/// </summary>
		public PolygonIntersectionOperation() : this(null) { }

		/// <summary>
		/// Constructs a new polygon intersection operation using the given options.
		/// </summary>
		/// <param name="options">The operation options to apply.</param>
		/// <remarks>
		/// Using this constructor can help optimize other operators that
		/// rely on intersection operations, such as union or difference.
		/// </remarks>
		internal PolygonIntersectionOperation([CanBeNull] PolygonBinaryOperationOptions options) {
			_options = PolygonBinaryOperationOptions.CloneOrDefault(options);
		}

		/// <summary>
		/// Calculates the intersection between two polygons.
		/// </summary>
		/// <param name="a">A polygon.</param>
		/// <param name="b">A polygon.</param>
		/// <returns>The intersection result, which may be a geometry collection containing points, segments, and polygons.</returns>
		[ContractAnnotation("a:null => null; b:null => null"), CanBeNull]
		public IPlanarGeometry Intersect([CanBeNull] Polygon2 a, [CanBeNull] Polygon2 b) {
			if (ReferenceEquals(null,a) || a.Count == 0 || ReferenceEquals(null,b) || b.Count == 0)
				return null;
			if (ReferenceEquals(a, b))
				return new Polygon2(a);

			// find all the crossings
			var crossingsData = FindPointCrossingsCore(a,b);
			// traverse the rings
			var results = BuildIntersectionResults(crossingsData, a, b);
			if (null == results.Polygon)
				return null;

			// TODO: this stuff is not so great
			results.Polygon = (results.Polygon.Count == 0 ? null : results.Polygon);
			if (null != results.Polygon) {
				var fillWinding = DetermineFillWinding(a.Concat(b));
				if(fillWinding != PointWinding.Unknown)
					results.Polygon.ForceFillWinding(fillWinding);
			}
			return results.Polygon;
		}

		private static PointWinding DetermineFillWinding([NotNull, InstantHandle] IEnumerable<Ring2> rings) {
			Contract.Requires(null != rings);
			Contract.EndContractBlock();

			var hint = PointWinding.Unknown;
			foreach(var ring in rings) {
				var ringWinding = ring.DetermineWinding();
				if(ringWinding == PointWinding.Unknown)
					continue;
				if(ring.Hole.HasValue) {
					if(ring.Hole.Value) {
						return ringWinding == PointWinding.Clockwise
							? PointWinding.CounterClockwise
							: PointWinding.Clockwise;
					}
					return ringWinding;
				}
				hint = ringWinding;
			}
			return hint;
		}

		[NotNull]
		private static IntersectionResults BuildFinalResults(
			[NotNull] PolygonCrossingsData crossingsData,
			[NotNull] Polygon2 a,
			[NotNull] Polygon2 b,
			[NotNull] Polygon2 intersectedPolygon
		) {
			Contract.Requires(crossingsData != null);
			Contract.Requires(a != null);
			Contract.Requires(b != null);
			Contract.Requires(intersectedPolygon != null);
			Contract.Ensures(Contract.Result<IntersectionResults>() != null);
			Contract.Ensures(intersectedPolygon.Count >= Contract.OldValue(intersectedPolygon).Count);
			Contract.EndContractBlock();

			var intersectedResultTree = new RingBoundaryTree(intersectedPolygon);

			intersectedPolygon.AddRange(
				QualifyRings(
					crossingsData.FindUntouchedRings(a, GetRingIndexA),
					b, true)
				.Where(ring => !intersectedResultTree.NonIntersectingContains(ring))
			);

			intersectedPolygon.AddRange(
				QualifyRings(
					crossingsData.FindUntouchedRings(b, GetRingIndexB),
					a, false)
				.Where(ring => !intersectedResultTree.NonIntersectingContains(ring))
			);

			return new IntersectionResults{
				Polygon = intersectedPolygon
			};
		}

		[NotNull]
		private static List<Ring2> QualifyRings(
			[NotNull, InstantHandle] IEnumerable<Ring2> untouchedRings,
			[NotNull] Polygon2 polygon,
			bool qualifyEqual
		) {
			Contract.Requires(untouchedRings != null);
			Contract.Requires(polygon != null);
			Contract.Ensures(Contract.Result<List<Ring2>>() != null);
			Contract.EndContractBlock();

			var result = new List<Ring2>();
			var ringTree = new RingBoundaryTree(polygon);
			foreach(var ring in untouchedRings)
			{
				// TODO: speed this up through some kind of magic, like RingBoundaryTree or something
				// TODO: var eq = new SpatialEqualityComparerThing(ringA); var stuff = otherRings.Where(r = > eq.SpatiallyEqual(r));
				if(polygon.Any(r => ring.SpatiallyEqual(r))) {
					if (qualifyEqual)
						result.Add(ring.Clone());
				}
				else if(ringTree.NonIntersectingContains(ring))
					result.Add(ring.Clone());
			}
			return result;
		}

		[NotNull]
		private static IntersectionResults BuildIntersectionResults(
			[NotNull] PolygonCrossingsData crossingsData,
			[NotNull] Polygon2 a,
			[NotNull] Polygon2 b
		) {
			Contract.Requires(null != crossingsData);
			Contract.Requires(null != a);
			Contract.Requires(null != b);
			Contract.Ensures(Contract.Result<IntersectionResults>() != null);
			Contract.EndContractBlock();


			// now that it is all sorted the turtle starts scooting around, making new rings:
			//
			//           --==  .-----.                     |             ~ard
			//          --==  /       \                    |
			//      A  --==  <_______(`~`)   B             |   D         E
			//   ---*---------//-----//------*-------------*---*---------*---
			//                                             C   |
			//                                                 |
			//
			//         please wait while SPEED TURTLE!!! assembles your new rings

			var rings = new Polygon2();

			if(null != crossingsData.Entrances)
				crossingsData.VisitedCrossings = new List<PolygonCrossing>(crossingsData.AllCrossings.Count);

			PolygonCrossing startEntrance;
			while (null != (startEntrance = crossingsData.FindNextStartableEntrance())) {
				var buildingRing = new List<Point2>();
				var entrance = startEntrance;
				do {
					var exit = TraverseBSide(entrance, buildingRing, crossingsData, b);
					if (null == exit) {
						crossingsData.VisitEntrance(startEntrance); // may need to do this to be safe
						break; // unmatched entrance
					}
					crossingsData.VisitExit(exit);
					entrance = TraverseASide(exit, buildingRing, crossingsData, a);
					if (null == entrance) {
						break; // unmatched exit
					}
					crossingsData.VisitEntrance(entrance);
				} while (entrance != startEntrance);
				if (buildingRing.Count > 2)
					rings.Add(new Ring2(buildingRing)); // here is a new ring
			}

			return BuildFinalResults(crossingsData, a, b, rings);
		}

		[CanBeNull]
		private static PolygonCrossing TraverseASide(
			[NotNull] PolygonCrossing startExit,
			[NotNull] List<Point2> buildingRing,
			[NotNull] PolygonCrossingsData crossingsData,
			[NotNull] Polygon2 p
		) {
			Contract.Requires(startExit != null);
			Contract.Requires(buildingRing != null);
			Contract.Requires(crossingsData != null);
			Contract.Requires(p != null);
			Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);
			Contract.EndContractBlock();

			PolygonCrossing exit = startExit;
			PolygonCrossing entrance;
			do {
				entrance = TraverseASideRing(
					buildingRing,
					exit,
					crossingsData.RingCrossingsA[exit.LocationA.RingIndex],
					crossingsData.Entrances,
					p,
					crossingsData
				);
				if (null == entrance)
					return null;

				exit = TraverseASideHops(entrance, crossingsData);
			} while (null != exit);
			return entrance;
		}

		[CanBeNull]
		private static PolygonCrossing TraverseBSide(
			[NotNull] PolygonCrossing startEntrance,
			[NotNull] List<Point2> buildingRing,
			[NotNull] PolygonCrossingsData crossingsData,
			[NotNull] Polygon2 p
		) {
			Contract.Requires(startEntrance != null);
			Contract.Requires(buildingRing != null);
			Contract.Requires(crossingsData !=  null);
			Contract.Requires(p != null);
			Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);
			Contract.EndContractBlock();

			PolygonCrossing entrance = startEntrance;
			PolygonCrossing exit;
			do {
				exit = TraverseBSideRing(
					buildingRing,
					entrance,
					crossingsData.RingCrossingsB[entrance.LocationB.RingIndex],
					crossingsData.Exits,
					p,
					crossingsData
				);
				if (null == exit)
					return null;

				entrance = TraverseBSideHops(exit, crossingsData);
			} while (null != entrance);
			return exit;
		}

		[CanBeNull]
		private static PolygonCrossing TraverseASideHops([NotNull] PolygonCrossing start, [NotNull] PolygonCrossingsData crossingsData) {
			Contract.Requires(start != null);
			Contract.Requires(crossingsData != null);
			Contract.EndContractBlock();

			PolygonCrossing current = start;
			PolygonCrossing result = null;
			PolygonCrossing next;
			while (
				crossingsData.Entrances.Contains(current)
				&& crossingsData.EntranceHops.TryGetValue(current, out next)
				&& crossingsData.Exits.Contains(next)
				&& next.LocationA != start.LocationA
			) {
				result = next;
				crossingsData.VisitEntrance(current);
				crossingsData.VisitExit(next);
				if (!crossingsData.ExitHops.TryGetValue(next, out current))
					break;
			}
			return result;
		}

		[CanBeNull]
		private static PolygonCrossing TraverseBSideHops([NotNull] PolygonCrossing start, [NotNull] PolygonCrossingsData crossingsData) {
			Contract.Requires(start != null);
			Contract.Requires(crossingsData != null);
			Contract.EndContractBlock();

			PolygonCrossing current = start;
			PolygonCrossing result = null;
			PolygonCrossing next;
			while (
				crossingsData.Exits.Contains(current)
				&& crossingsData.ExitHops.TryGetValue(current, out next)
				&& crossingsData.Entrances.Contains(next)
				&& next.LocationB != start.LocationB
			) {
				result = next;
				crossingsData.VisitEntrance(next);
				crossingsData.VisitExit(current);
				if (!crossingsData.EntranceHops.TryGetValue(next, out current))
					break;
			}
			return result;
		}

		[NotNull]
		private static IEnumerable<PolygonCrossing> TraverseCrossings(
			[NotNull] PolygonCrossing fromCrossing,
			[NotNull] List<PolygonCrossing> ringCrossings,
			[NotNull] IComparer<PolygonCrossing> crossingComparer
		) {
			Contract.Requires(fromCrossing != null);
			Contract.Requires(ringCrossings != null);
			Contract.Requires(crossingComparer != null);
			Contract.Ensures(Contract.Result<IEnumerable<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			var fromCrossingIndex = ringCrossings.BinarySearch(fromCrossing, crossingComparer);
			var ringCrossingsCount = ringCrossings.Count;

			Stack<PolygonCrossing> priorResults = null;
			var backTrackIndex = fromCrossingIndex;
			do {
				var priorIndex = IterationUtils.RetreatLoopingIndex(backTrackIndex, ringCrossingsCount);
				var priorCrossing = ringCrossings[priorIndex];
				if (priorCrossing.Point == fromCrossing.Point) {
					backTrackIndex = priorIndex;
					(priorResults ?? (priorResults = new Stack<PolygonCrossing>()))
						.Push(priorCrossing);
				}
				else {
					break;
				}
			} while (backTrackIndex != fromCrossingIndex);

			if (null != priorResults)
				while (priorResults.Count > 0)
					yield return priorResults.Pop();

			for (
				int i = IterationUtils.AdvanceLoopingIndex(fromCrossingIndex, ringCrossingsCount);
				i != fromCrossingIndex;
				i = IterationUtils.AdvanceLoopingIndex(i, ringCrossingsCount)
			) {
				yield return ringCrossings[i];
			}
		}

		[CanBeNull]
		private static PolygonCrossing TraverseASideRing(
			[NotNull] List<Point2> buildingRing,
			[NotNull] PolygonCrossing fromCrossing,
			[NotNull] List<PolygonCrossing> ringCrossings,
			[NotNull] HashSet<PolygonCrossing> entrances,
			[NotNull] Polygon2 a,
			[NotNull] PolygonCrossingsData crossingData
		) {
			Contract.Requires(buildingRing != null);
			Contract.Requires(fromCrossing != null);
			Contract.Requires(ringCrossings != null);
			Contract.Requires(entrances != null);
			Contract.Requires(a != null);
			Contract.Requires(crossingData != null);
			Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);
			Contract.EndContractBlock();

			var fromSegmentLocationA = fromCrossing.LocationA;
			if (0 == buildingRing.Count || buildingRing[buildingRing.Count-1] != fromCrossing.Point)
				buildingRing.Add(fromCrossing.Point);
			foreach (var crossing in TraverseCrossings(fromCrossing, ringCrossings, PolygonCrossing.LocationAComparer.Default)) {
				// hops should have already been taken care of
				if (crossing.Point == fromCrossing.Point && crossingData.EntranceHops.ContainsKey(crossing))
					continue; // no, lets go somewhere useful with this

				var locationA = crossing.LocationA;
				var ringA = a[locationA.RingIndex];

				AddPointsBetweenForward(buildingRing, ringA, fromSegmentLocationA, locationA);
				fromSegmentLocationA = locationA; // for later...

				if (entrances.Contains(crossing))
					return crossing; // if we found it, stop

				if (buildingRing.Count == 0 || buildingRing[buildingRing.Count - 1] != crossing.Point)
					buildingRing.Add(crossing.Point); // if it is something else, lets add it
			}
			return null;
		}

		[CanBeNull]
		private static PolygonCrossing TraverseBSideRing(
			[NotNull] List<Point2> buildingRing,
			[NotNull] PolygonCrossing fromCrossing,
			[NotNull] List<PolygonCrossing> ringCrossings,
			[NotNull] HashSet<PolygonCrossing> exits,
			[NotNull] Polygon2 b,
			[NotNull] PolygonCrossingsData crossingData
		) {
			Contract.Requires(buildingRing != null);
			Contract.Requires(fromCrossing != null);
			Contract.Requires(ringCrossings != null);
			Contract.Requires(exits != null);
			Contract.Requires(b != null);
			Contract.Requires(crossingData != null);
			Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);
			Contract.EndContractBlock();

			var fromSegmentLocationB = fromCrossing.LocationB;
			if (0 == buildingRing.Count || buildingRing[buildingRing.Count - 1] != fromCrossing.Point)
				buildingRing.Add(fromCrossing.Point);
			foreach (var crossing in TraverseCrossings(fromCrossing, ringCrossings, PolygonCrossing.LocationBComparer.Default)) {
				// hops should have already been taken care of
				if (crossing.Point == fromCrossing.Point && crossingData.ExitHops.ContainsKey(crossing))
					continue; // no, lets go somewhere useful with this

				var locationB = crossing.LocationB;
				var ringB = b[locationB.RingIndex];

				AddPointsBetweenForward(buildingRing, ringB, fromSegmentLocationB, locationB);
				fromSegmentLocationB = locationB; // for later...

				if (exits.Contains(crossing))
					return crossing; // if we found it, stop

				if (buildingRing.Count == 0 || buildingRing[buildingRing.Count - 1] != crossing.Point)
					buildingRing.Add(crossing.Point); // if it is something else, lets add it
			}
			return null;
		}

		private static void AddPointsBetweenForward(
			[NotNull] List<Point2> results,
			[NotNull] Ring2 ring,
			[NotNull] PolygonBoundaryLocation from,
			[NotNull] PolygonBoundaryLocation to
		) {
			Contract.Requires(results != null);
			Contract.Requires(ring != null);
			Contract.Requires(from != null);
			Contract.Requires(to != null);
			Contract.Ensures(results.Count >= Contract.OldValue(results).Count);
			Contract.EndContractBlock();

			if (from.SegmentIndex == to.SegmentIndex && from.SegmentRatio <= to.SegmentRatio)
				return; // no points will result from this

			var segmentCount = ring.SegmentCount;
			var currentSegmentIndex = IterationUtils.AdvanceLoopingIndex(from.SegmentIndex, segmentCount);
			do {
				if (currentSegmentIndex == to.SegmentIndex) {
					if (0 < to.SegmentRatio)
						results.Add(ring[currentSegmentIndex]);
					return;
				}
				results.Add(ring[currentSegmentIndex]);
				currentSegmentIndex = IterationUtils.AdvanceLoopingIndex(currentSegmentIndex, segmentCount);
			} while (true);
		}

		[NotNull]
		private static Dictionary<int, List<PolygonCrossing>> SortedRingLookUp(
			[NotNull] List<PolygonCrossing> crossings,
			[NotNull] Comparison<PolygonCrossing> crossingComparison,
			[NotNull] Func<PolygonCrossing, int> ringIndexSelector,
			int maxSize
		) {
			Contract.Requires(null != crossings);
			Contract.Requires(null != crossingComparison);
			Contract.Requires(null != ringIndexSelector);
			Contract.Ensures(Contract.Result<Dictionary<int, List<PolygonCrossing>>>() != null);
			Contract.EndContractBlock();

			var result = new Dictionary<int, List<PolygonCrossing>>(maxSize);
			foreach (var crossing in crossings) {
				List<PolygonCrossing> ringCrossings;
				if (!result.TryGetValue(ringIndexSelector(crossing), out ringCrossings)) {
					ringCrossings = new List<PolygonCrossing>();
					result.Add(ringIndexSelector(crossing), ringCrossings);
				}
				ringCrossings.Add(crossing);
			}

			foreach (var ringCrossings in result.Values)
				ringCrossings.Sort(crossingComparison);

			return result;
		}

		/// <summary>
		/// Builds required crossing data. 
		/// </summary>
		/// <param name="crossings">The crossings to calculate. (Note: collection is modified)</param>
		/// <param name="a">The left hand polygon.</param>
		/// <param name="b">The right hand polygon.</param>
		[NotNull]
		private static PolygonCrossingsData BuildCrossingsData(
			[NotNull] List<PolygonCrossing> crossings,
			[NotNull] Polygon2 a,
			[NotNull] Polygon2 b
		) {
			Contract.Requires(crossings != null);
			Contract.Requires(a != null);
			Contract.Requires(b != null);
			Contract.Ensures(Contract.Result<PolygonCrossingsData>() != null);
			Contract.EndContractBlock();

			if (crossings.Count == 0)
				return new PolygonCrossingsData();

			// TODO: test this with one crossing... somehow (is that even possible?)
			var result = new PolygonCrossingsData();
			result.AllCrossings = crossings;
			result.RingCrossingsA = SortedRingLookUp(crossings, PolygonCrossing.LocationAComparer.Default.Compare, GetRingIndexA, a.Count);
			result.RingCrossingsB = SortedRingLookUp(crossings, PolygonCrossing.LocationBComparer.Default.Compare, GetRingIndexB, b.Count);

			// these variables are cached through iterations of the loop, and only updated when required as it requires multiple look-ups
			int ringIndexA = -1; // force first cache miss
			int ringIndexB = -1; // force first cache miss
			Ring2 ringA = null;
			Ring2 ringB = null;
			List<PolygonCrossing> crossingsOnRingA = null;
			List<PolygonCrossing> crossingsOnRingB = null;

			foreach(var currentCrossing in crossings){
				if (ringIndexA != currentCrossing.LocationA.RingIndex)
				{
					// need to pull new values
					ringIndexA = currentCrossing.LocationA.RingIndex;
					ringA = a[ringIndexA];
					crossingsOnRingA = result.RingCrossingsA[ringIndexA];
				}

				// ReSharper disable AssignNullToNotNullAttribute
				Contract.Assume(crossingsOnRingA != null);
				Contract.Assume(ringA != null);
				var priorPointA = FindPreviousRingPoint(currentCrossing, crossingsOnRingA, ringA, GetLocationA, PolygonCrossing.LocationAComparer.Default);
				var nextPointA = FindNextRingPoint(currentCrossing, crossingsOnRingA, ringA, GetLocationA, PolygonCrossing.LocationAComparer.Default);
				// ReSharper restore AssignNullToNotNullAttribute

				if (ringIndexB != currentCrossing.LocationB.RingIndex)
				{
					// need to pull new values
					ringIndexB = currentCrossing.LocationB.RingIndex;
					ringB = b[ringIndexB];
					crossingsOnRingB = result.RingCrossingsB[ringIndexB];
				}

				// ReSharper disable AssignNullToNotNullAttribute
				Contract.Assume(crossingsOnRingB != null);
				Contract.Assume(ringB != null);
				var priorPointB = FindPreviousRingPoint(currentCrossing, crossingsOnRingB, ringB, GetLocationB, PolygonCrossing.LocationBComparer.Default);
				var nextPointB = FindNextRingPoint(currentCrossing, crossingsOnRingB, ringB, GetLocationB, PolygonCrossing.LocationBComparer.Default);
				// ReSharper restore AssignNullToNotNullAttribute

				// based on the vectors, need to classify the crossing type
				currentCrossing.CrossType = PolygonCrossing.DetermineCrossingType(
					(nextPointA - currentCrossing.Point).GetNormalized(),
					(priorPointA - currentCrossing.Point).GetNormalized(),
					(nextPointB - currentCrossing.Point).GetNormalized(),
					(priorPointB - currentCrossing.Point).GetNormalized()
				);
			}

			var entrances = new HashSet<PolygonCrossing>();
			var exits = new HashSet<PolygonCrossing>();
			foreach (var ringCrossings in result.RingCrossingsA) {
				if (a[ringCrossings.Key].FillSide == RelativeDirectionType.Right) {
					foreach (var crossing in ringCrossings.Value) {
						var crossLegType = crossing.CrossType & CrossingType.Parallel;
						if (crossLegType == CrossingType.CrossToRight || crossLegType == CrossingType.DivergeRight)
							entrances.Add(crossing);
						else if (crossLegType == CrossingType.CrossToLeft || crossLegType == CrossingType.ConvergeRight)
							exits.Add(crossing);
					}
				}
				else {
					foreach (var crossing in ringCrossings.Value) {
						var crossLegType = crossing.CrossType & CrossingType.Parallel;
						if (crossLegType == CrossingType.CrossToLeft || crossLegType == CrossingType.DivergeLeft)
							entrances.Add(crossing);
						else if (crossLegType == CrossingType.CrossToRight || crossLegType == CrossingType.ConvergeLeft)
							exits.Add(crossing);
					}
				}
			}
			result.Entrances = entrances;
			result.Exits = exits;

			result.EntranceHops = new Dictionary<PolygonCrossing, PolygonCrossing>();
			result.ExitHops = new Dictionary<PolygonCrossing, PolygonCrossing>();

			// TODO: merge these two groups of loops together?
			foreach(var entrance in entrances) {
				var entranceLocationB = entrance.LocationB;
				foreach(var exit in exits) {
					if(exit.LocationB == entranceLocationB) {
						result.EntranceHops.Add(entrance, exit);
						break;
					}
				}
			}
			foreach(var exit in exits) {
				var exitLocationA = exit.LocationA;
				foreach(var entrance in entrances) {
					if(entrance.LocationA == exitLocationA) {
						result.ExitHops.Add(exit, entrance);
						break;
					}
				}
			}

			return result;
		}

		[CanBeNull]
		private static PolygonCrossing FindNextCrossingNotEqual(
			[NotNull] PolygonCrossing currentCrossing,
			[NotNull] List<PolygonCrossing> crossingsOnRing,
			[NotNull] IComparer<PolygonCrossing> crossingComparer
		) {
			Contract.Requires(currentCrossing != null);
			Contract.Requires(crossingsOnRing != null);
			Contract.Requires(crossingComparer != null);
			Contract.EndContractBlock();

			var currentPoint = currentCrossing.Point;
			var currentCrossingIndex = crossingsOnRing.BinarySearch(currentCrossing, crossingComparer);
			var crossingsCount = crossingsOnRing.Count;
			// find the first crossing after this one that has a different point location
			for (
				var i = IterationUtils.AdvanceLoopingIndex(currentCrossingIndex, crossingsCount);
				i != currentCrossingIndex;
				i = IterationUtils.AdvanceLoopingIndex(i, crossingsCount)
			) {
				var crossing = crossingsOnRing[i];
				if (crossing.Point != currentPoint)
					return crossing;
			}
			return null;
		}

		private static Point2 FindNextRingPoint(
			[NotNull] PolygonCrossing currentCrossing,
			[NotNull] List<PolygonCrossing> crossingsOnRing,
			[NotNull] Ring2 ring,
			[NotNull, InstantHandle] Func<PolygonCrossing, PolygonBoundaryLocation> getLocation,
			[NotNull] IComparer<PolygonCrossing> crossingComparer
		) {
			Contract.Requires(currentCrossing != null);
			Contract.Requires(crossingsOnRing != null);
			Contract.Requires(ring != null);
			Contract.Requires(getLocation != null);
			Contract.Requires(crossingComparer != null);
			Contract.EndContractBlock();

			var currentLocation = getLocation(currentCrossing);
			var segmentCount = ring.SegmentCount;
			int nextSegmentIndex = IterationUtils.AdvanceLoopingIndex(currentLocation.SegmentIndex, segmentCount);
			var nextCrossing = FindNextCrossingNotEqual(currentCrossing, crossingsOnRing, crossingComparer);
			// NOTE: this method assumes a segment ratio less than 1, verify we can trust this
			if (null == nextCrossing) {
				return ring[nextSegmentIndex];
			}
			var nextCrossingLocation = getLocation(nextCrossing);
			return currentLocation.SegmentIndex == nextCrossingLocation.SegmentIndex
				&& currentLocation.SegmentRatio < nextCrossingLocation.SegmentRatio
				? nextCrossing.Point
				: ring[nextSegmentIndex];
		}

		[CanBeNull]
		private static PolygonCrossing FindPreviousCrossingNotEqual(
			[NotNull] PolygonCrossing currentCrossing,
			[NotNull] List<PolygonCrossing> crossingsOnRing,
			[NotNull] IComparer<PolygonCrossing> crossingComparer
		) {
			Contract.Requires(currentCrossing != null);
			Contract.Requires(crossingsOnRing != null);
			Contract.Requires(crossingComparer != null);
			Contract.EndContractBlock();

			var currentPoint = currentCrossing.Point;
			int currentCrossingIndex = crossingsOnRing.BinarySearch(currentCrossing,crossingComparer);
			int crossingsCount = crossingsOnRing.Count;
			// find the first crossing before this one that has a different point location
			for (
				int i = IterationUtils.RetreatLoopingIndex(currentCrossingIndex, crossingsCount);
				i != currentCrossingIndex;
				i = IterationUtils.RetreatLoopingIndex(i, crossingsCount)
			) {
				var crossing = crossingsOnRing[i];
				if (crossing.Point != currentPoint)
					return crossing;
			}
			return null;
		}

// ReSharper disable CompareOfFloatsByEqualityOperator
		private static Point2 FindPreviousRingPoint(
			[NotNull] PolygonCrossing currentCrossing,
			[NotNull] List<PolygonCrossing> crossingsOnRing,
			[NotNull] Ring2 ring,
			[NotNull, InstantHandle] Func<PolygonCrossing, PolygonBoundaryLocation> getLocation,
			[NotNull] IComparer<PolygonCrossing> crossingComparer
		) {
			Contract.Requires(currentCrossing != null);
			Contract.Requires(crossingsOnRing != null);
			Contract.Requires(ring != null);
			Contract.Requires(getLocation != null);
			Contract.Requires(crossingComparer != null);
			Contract.EndContractBlock();

			var currentLocation = getLocation(currentCrossing);
			var segmentCount = ring.SegmentCount;
			var previousSegmentIndex = IterationUtils.RetreatLoopingIndex(currentLocation.SegmentIndex, segmentCount);
			var previousCrossing = FindPreviousCrossingNotEqual(currentCrossing, crossingsOnRing, crossingComparer);
			if(null == previousCrossing) {
				return ring[
					currentLocation.SegmentRatio == 0
					? previousSegmentIndex
					: currentLocation.SegmentIndex
				];
			}
			var previousCrossingLocation = getLocation(previousCrossing);
			if(currentLocation.SegmentRatio == 0) {
				return previousSegmentIndex == previousCrossingLocation.SegmentIndex
					? previousCrossing.Point
					: ring[previousSegmentIndex];
			}
			return currentLocation.SegmentIndex == previousCrossingLocation.SegmentIndex
				&& currentLocation.SegmentRatio > previousCrossingLocation.SegmentRatio
				? previousCrossing.Point
				: ring[currentLocation.SegmentIndex];
		}
// ReSharper restore CompareOfFloatsByEqualityOperator

		/// <summary>
		/// Determines the points that would need to be inserted into the resulting
		/// intersection geometry between the two given polygons, at the location where
		/// their boundaries cross.
		/// </summary>
		/// <param name="a">The first polygon to test.</param>
		/// <param name="b">The second polygon to test.</param>
		[NotNull]
		public List<PolygonCrossing> FindPointCrossings(
			[NotNull] Polygon2 a,
			[NotNull] Polygon2 b
		) {
			if(null == a)
				throw new ArgumentNullException("a");
			if(null == b)
				throw new ArgumentNullException("b");
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			Contract.EndContractBlock();

			return FindPointCrossingsCore(a, b).AllCrossings ?? new List<PolygonCrossing>();
		}

		[NotNull]
		private PolygonCrossingsData FindPointCrossingsCore(
			[NotNull] Polygon2 a,
			[NotNull] Polygon2 b
		) {
			Contract.Requires(a != null);
			Contract.Requires(b != null);
			Contract.Ensures(Contract.Result<PolygonCrossingsData>() != null);
			Contract.EndContractBlock();

			var crossingGenerator = new PolygonPointCrossingGenerator(a,b);
			var allCrossings = crossingGenerator.GenerateCrossings();
			return BuildCrossingsData(allCrossings, a, b);
		}

	}

}
