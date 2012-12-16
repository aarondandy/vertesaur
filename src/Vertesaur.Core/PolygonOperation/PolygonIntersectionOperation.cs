﻿// ===============================================================================
//
// Copyright (c) 2011,2012 Aaron Dandy 
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
using Vertesaur.Contracts;
using Vertesaur.Utility;
using CrossingType = Vertesaur.PolygonOperation.PolygonCrossing.CrossingType;

namespace Vertesaur.PolygonOperation {

	/// <summary>
	/// An operation that will find the geometric result of intersecting two polygons.
	/// </summary>
	public class PolygonIntersectionOperation
	{

		private sealed class IntersectionResults
		{
			public Polygon2 Polygon;
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

			public PolygonCrossingsData() {
				RingCrossingsA = new Dictionary<int, List<PolygonCrossing>>();
				RingCrossingsB = new Dictionary<int, List<PolygonCrossing>>();
			}

			public void VisitEntrance(PolygonCrossing entrance) {
				if(Entrances.Remove(entrance))
					VisitedCrossings.Add(entrance);
			}

			public void VisitExit(PolygonCrossing exit) {
				if(Exits.Remove(exit))
					VisitedCrossings.Add(exit);
			}

			public PolygonCrossing FindNextStartableEntrance() {
				Contract.Assume(null != Entrances);
				Contract.Assume(null != EntranceHops);
				Contract.Assume(null != ExitHops);
				return Entrances.FirstOrDefault(entrance =>
					!EntranceHops.ContainsKey(entrance)
					&& !ExitHops.ContainsValue(entrance)
				);
			}

			public IEnumerable<Ring2> FindUntouchedRings(Polygon2 poly, Func<PolygonCrossing, int> getRingIndex) {
				Contract.Requires(poly != null);
				Contract.Requires(getRingIndex != null);
				Contract.Ensures(Contract.Result<IEnumerable<Ring2>>() != null);

				if(null == VisitedCrossings || 0 == VisitedCrossings.Count)
					return poly;
				var includedRingIndices = new HashSet<int>(VisitedCrossings.Select(getRingIndex));
				return poly.Count == includedRingIndices.Count
					? Enumerable.Empty<Ring2>()
					: poly.Where((ring, i) => !includedRingIndices.Contains(i));
			}

			[ContractInvariantMethod]
			private void CodeContractInvariant() {
				//Contract.Invariant(RingCrossingsA != null);
				//Contract.Invariant(Contract.ForAll(RingCrossingsA, x => x.Value != null && x.Key >= 0));
				//Contract.Invariant(RingCrossingsB != null);
				//Contract.Invariant(Contract.ForAll(RingCrossingsB, x => x.Value != null && x.Key >= 0));
			}
		}

		[Pure] private static PolygonBoundaryLocation GetLocationA(PolygonCrossing crossing) {
			Contract.Requires(crossing != null);
			return crossing.LocationA;
		}

		[Pure] private static PolygonBoundaryLocation GetLocationB(PolygonCrossing crossing) {
			Contract.Requires(crossing != null);
			return crossing.LocationB;
		}

		[Pure] private static int GetRingIndexA(PolygonCrossing crossing) {
			Contract.Requires(crossing != null);
			return crossing.LocationA.RingIndex;
		}

		[Pure] private static int GetRingIndexB(PolygonCrossing crossing) {
			Contract.Requires(crossing != null);
			return crossing.LocationB.RingIndex;
		}

		private readonly PolygonBinaryOperationOptions _options;

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
		internal PolygonIntersectionOperation(PolygonBinaryOperationOptions options) {
			_options = PolygonBinaryOperationOptions.CloneOrDefault(options);
		}

		/// <summary>
		/// Calculates the intersection between two polygons.
		/// </summary>
		/// <param name="a">A polygon.</param>
		/// <param name="b">A polygon.</param>
		/// <returns>The intersection result, which may be a geometry collection containing points, segments, and polygons.</returns>
		public IPlanarGeometry Intersect(Polygon2 a, Polygon2 b) {
			if (null == a || a.Count == 0 || null == b || b.Count == 0)
				return null;
			if (ReferenceEquals(a, b))
				return a.Clone();

			if (_options.InvertLeftHandSide)
				a = PolygonInverseOperation.Invert(a);
			if (_options.InvertRightHandSide)
				b = PolygonInverseOperation.Invert(b);

			// find all the crossings
			Contract.Assume(null != a);
			Contract.Assume(null != b);
			var fillWinding = DetermineFillWinding(a.Concat(b));
			
			var crossingsData = FindPointCrossingsCore(a,b);
			// traverse the rings
			var results = BuildIntersectionResults(crossingsData, fillWinding, a, b);
			if (null == results.Polygon)
				return null;

			// TODO: this stuff is not so great
			results.Polygon = (results.Polygon.Count == 0 ? null : results.Polygon);
			if (null != results.Polygon) {
				if(fillWinding != PointWinding.Unknown)
					results.Polygon.ForceFillWinding(fillWinding);
			}

			if (_options.InvertResult)
				results.Polygon = PolygonInverseOperation.Invert(results.Polygon);

			return results.Polygon;
		}

		private static PointWinding DetermineFillWinding(IEnumerable<Ring2> rings) {
			Contract.Requires(null != rings);

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

		private IntersectionResults BuildFinalResults(
			PolygonCrossingsData crossingsData,
			Polygon2 a,
			Polygon2 b,
			Polygon2 intersectedPolygon
		) {
			Contract.Requires(crossingsData != null);
			Contract.Requires(a != null);
			Contract.Requires(b != null);
			Contract.Requires(intersectedPolygon != null);
			Contract.Ensures(Contract.Result<IntersectionResults>() != null);
			Contract.Ensures(intersectedPolygon.Count >= Contract.OldValue(intersectedPolygon).Count);

			if (intersectedPolygon.Count == 0) {
				var untouchedA = new Polygon2(crossingsData.FindUntouchedRings(a, GetRingIndexA));
				var untouchedB = new Polygon2(crossingsData.FindUntouchedRings(b, GetRingIndexB));
				intersectedPolygon.AddRange(QualifyRings(untouchedA, untouchedB, true));
				intersectedPolygon.AddRange(QualifyRings(untouchedB, untouchedA, false));

			}
			else {
				var intersectedResultTree = new RingBoundaryTree(intersectedPolygon);

				intersectedPolygon.AddRange(
					FilterQualifiedRingsToBoundaryTree(
						QualifyRings(
							crossingsData.FindUntouchedRings(a, GetRingIndexA),
							b, true
						),
						intersectedResultTree
					)
				);

				intersectedPolygon.AddRange(
					FilterQualifiedRingsToBoundaryTree(
						QualifyRings(
							crossingsData.FindUntouchedRings(b, GetRingIndexB),
							a, false
						),
						intersectedResultTree
					)
				);
			}
			return new IntersectionResults{
				Polygon = intersectedPolygon
			};
		}
		
		private static List<Ring2> FilterQualifiedRingsToBoundaryTree(List<Ring2> rings, RingBoundaryTree boundaryTree) {
			Contract.Requires(rings != null);
			Contract.Requires(boundaryTree != null);
			Contract.Ensures(Contract.Result<List<Ring2>>() != null);

			var results = new List<Ring2>();
			for (int ringIndex = 0; ringIndex < rings.Count; ringIndex++) {
				var ring = rings[ringIndex];
				if (
					ring.Hole.HasValue && ring.Hole.Value
						? boundaryTree.NonIntersectingContains(ring)
						: !boundaryTree.NonIntersectingContains(ring)
					) {
					results.Add(ring);
				}
			}
			return results;
		}

		private static List<Ring2> QualifyRings(
			IEnumerable<Ring2> untouchedRings,
			Polygon2 polygon,
			bool qualifyEqual
		) {
			Contract.Requires(untouchedRings != null);
			Contract.Requires(polygon != null);
			Contract.Ensures(Contract.Result<List<Ring2>>() != null);
			Contract.Ensures(Contract.ForAll(Contract.Result<List<Ring2>>(), x => null != x));

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
			Contract.Assume(Contract.ForAll(result, x => null != x));
			return result;
		}

		private IntersectionResults BuildIntersectionResults(
			PolygonCrossingsData crossingsData,
			PointWinding fillWinding,
			Polygon2 a,
			Polygon2 b
		) {
			Contract.Requires(null != crossingsData);
			Contract.Requires(null != a);
			Contract.Requires(null != b);
			Contract.Ensures(Contract.Result<IntersectionResults>() != null);

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

			if (fillWinding != PointWinding.Unknown)
				rings.ForceFillWinding(fillWinding);

			return BuildFinalResults(crossingsData, a, b, rings);
		}

		private PolygonCrossing TraverseASide(
			PolygonCrossing startExit,
			List<Point2> buildingRing,
			PolygonCrossingsData crossingsData,
			Polygon2 a
		) {
			Contract.Requires(startExit != null);
			Contract.Requires(buildingRing != null);
			Contract.Requires(crossingsData != null);
			Contract.Requires(a != null);
			Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

			PolygonCrossing exit = startExit;
			PolygonCrossing entrance;
			do {
				Contract.Assume(null != crossingsData.RingCrossingsA[exit.LocationA.RingIndex]);
				Contract.Assume(null != crossingsData.Entrances);
				entrance = TraverseASideRing(
					buildingRing,
					exit,
					crossingsData.RingCrossingsA[exit.LocationA.RingIndex],
					crossingsData.Entrances,
					a,
					crossingsData
				);
				if (null == entrance)
					return null;

				exit = TraverseASideHops(entrance, crossingsData);
			} while (null != exit);
			return entrance;
		}

		private PolygonCrossing TraverseBSide(
			PolygonCrossing startEntrance,
			List<Point2> buildingRing,
			PolygonCrossingsData crossingsData,
			Polygon2 b
		) {
			Contract.Requires(startEntrance != null);
			Contract.Requires(buildingRing != null);
			Contract.Requires(crossingsData !=  null);
			Contract.Requires(b != null);
			Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

			PolygonCrossing entrance = startEntrance;
			PolygonCrossing exit;
			do {
				Contract.Assume(null != crossingsData.RingCrossingsB[entrance.LocationB.RingIndex]);
				Contract.Assume(null != crossingsData.Exits);
				exit = TraverseBSideRing(
					buildingRing,
					entrance,
					crossingsData.RingCrossingsB[entrance.LocationB.RingIndex],
					crossingsData.Exits,
					b,
					crossingsData
				);
				if (null == exit)
					return null;

				entrance = TraverseBSideHops(exit, crossingsData);
			} while (null != entrance);
			return exit;
		}

		private static PolygonCrossing TraverseASideHops(PolygonCrossing start, PolygonCrossingsData crossingsData) {
			Contract.Requires(start != null);
			Contract.Requires(crossingsData != null);

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

		private static PolygonCrossing TraverseBSideHops(PolygonCrossing start, PolygonCrossingsData crossingsData) {
			Contract.Requires(start != null);
			Contract.Requires(crossingsData != null);

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

		private static IEnumerable<PolygonCrossing> TraverseCrossings(
			PolygonCrossing fromCrossing,
			List<PolygonCrossing> ringCrossings,
			IComparer<PolygonCrossing> crossingComparer
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

		private PolygonCrossing TraverseASideRing(
			List<Point2> buildingRing,
			PolygonCrossing fromCrossing,
			List<PolygonCrossing> ringCrossings,
			HashSet<PolygonCrossing> entrances,
			Polygon2 a,
			PolygonCrossingsData crossingData
		) {
			Contract.Requires(buildingRing != null);
			Contract.Requires(fromCrossing != null);
			Contract.Requires(ringCrossings != null);
			Contract.Requires(entrances != null);
			Contract.Requires(a != null);
			Contract.Requires(crossingData != null);
			Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

			var fromSegmentLocationA = fromCrossing.LocationA;
			if (0 == buildingRing.Count || buildingRing[buildingRing.Count-1] != fromCrossing.Point)
				buildingRing.Add(fromCrossing.Point);
			foreach (var crossing in TraverseCrossings(fromCrossing, ringCrossings, PolygonCrossing.LocationAComparer.Default)) {
				// hops should have already been taken care of
				if (crossing.Point == fromCrossing.Point && crossingData.EntranceHops.ContainsKey(crossing))
					continue; // no, lets go somewhere useful with this

				var locationA = crossing.LocationA;
				Contract.Assume(locationA.RingIndex < a.Count);
				var ringA = a[locationA.RingIndex];
				Contract.Assume(ringA != null);

				AddPointsBetweenForward(buildingRing, ringA, fromSegmentLocationA, locationA);
				fromSegmentLocationA = locationA; // for later...

				if (entrances.Contains(crossing))
					return crossing; // if we found it, stop

				if (buildingRing.Count == 0 || buildingRing[buildingRing.Count - 1] != crossing.Point)
					buildingRing.Add(crossing.Point); // if it is something else, lets add it
			}
			return null;
		}

		private PolygonCrossing TraverseBSideRing(
			List<Point2> buildingRing,
			PolygonCrossing fromCrossing,
			List<PolygonCrossing> ringCrossings,
			HashSet<PolygonCrossing> exits,
			Polygon2 b,
			PolygonCrossingsData crossingData
		) {
			Contract.Requires(buildingRing != null);
			Contract.Requires(fromCrossing != null);
			Contract.Requires(ringCrossings != null);
			Contract.Requires(exits != null);
			Contract.Requires(b != null);
			Contract.Requires(crossingData != null);
			Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

			var fromSegmentLocationB = fromCrossing.LocationB;
			if (0 == buildingRing.Count || buildingRing[buildingRing.Count - 1] != fromCrossing.Point)
				buildingRing.Add(fromCrossing.Point);
			foreach (var crossing in TraverseCrossings(fromCrossing, ringCrossings, PolygonCrossing.LocationBComparer.Default)) {
				// hops should have already been taken care of
				if (crossing.Point == fromCrossing.Point && crossingData.ExitHops.ContainsKey(crossing))
					continue; // no, lets go somewhere useful with this

				var locationB = crossing.LocationB;
				Contract.Assume(locationB.RingIndex < b.Count);
				var ringB = b[locationB.RingIndex];
				Contract.Assume(null != ringB);

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
			List<Point2> results,
			Ring2 ring,
			PolygonBoundaryLocation from,
			PolygonBoundaryLocation to
		) {
			Contract.Requires(results != null);
			Contract.Requires(ring != null);
			Contract.Requires(from != null);
			Contract.Requires(to != null);
			Contract.Ensures(results.Count >= Contract.OldValue(results).Count);

			if (from.SegmentIndex == to.SegmentIndex && from.SegmentRatio <= to.SegmentRatio)
				return; // no points will result from this

			var segmentCount = ring.SegmentCount;
			var currentSegmentIndex = IterationUtils.AdvanceLoopingIndex(from.SegmentIndex, segmentCount);
			do {
				if (currentSegmentIndex == to.SegmentIndex) {
					if (0 < to.SegmentRatio) {
						Contract.Assume(currentSegmentIndex < ring.Count);
						results.Add(ring[currentSegmentIndex]);
					}
					return;
				}
				Contract.Assume(currentSegmentIndex < ring.Count);
				results.Add(ring[currentSegmentIndex]);
				currentSegmentIndex = IterationUtils.AdvanceLoopingIndex(currentSegmentIndex, segmentCount);
			} while (true);
		}

		private static Dictionary<int, List<PolygonCrossing>> SortedRingLookUp(
			List<PolygonCrossing> crossings,
			Comparison<PolygonCrossing> crossingComparison,
			Func<PolygonCrossing, int> ringIndexSelector,
			int maxSize
		) {
			Contract.Requires(null != crossings);
			Contract.Requires(null != crossingComparison);
			Contract.Requires(null != ringIndexSelector);
			Contract.Ensures(Contract.Result<Dictionary<int, List<PolygonCrossing>>>() != null);

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
		private PolygonCrossingsData BuildCrossingsData(List<PolygonCrossing> crossings,Polygon2 a,Polygon2 b) {
			Contract.Requires(crossings != null);
			Contract.Requires(a != null);
			Contract.Requires(b != null);
			Contract.Ensures(Contract.Result<PolygonCrossingsData>() != null);

			// TODO: maybe 0 should be an invalid value?
			if (crossings.Count == 0)
				return new PolygonCrossingsData{
					EntranceHops = new Dictionary<PolygonCrossing, PolygonCrossing>(),
					ExitHops = new Dictionary<PolygonCrossing, PolygonCrossing>(),
					Entrances = new HashSet<PolygonCrossing>(),
					Exits = new HashSet<PolygonCrossing>(),
					AllCrossings = crossings
				};

			// TODO: test this with one crossing... somehow (is that even possible?)

			var ringCrossingsA = SortedRingLookUp(crossings, PolygonCrossing.LocationAComparer.Default.Compare, GetRingIndexA, a.Count);
			var ringCrossingsB = SortedRingLookUp(crossings, PolygonCrossing.LocationBComparer.Default.Compare, GetRingIndexB, b.Count);

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
					Contract.Assume(ringIndexA < a.Count);
					ringA = a[ringIndexA];
					crossingsOnRingA = ringCrossingsA[ringIndexA];
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
					Contract.Assume(ringIndexB < b.Count);
					ringB = b[ringIndexB];
					crossingsOnRingB = ringCrossingsB[ringIndexB];
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
			foreach (var ringCrossings in ringCrossingsA) {
				Contract.Assume(ringCrossings.Key >= 0 && ringCrossings.Key < a.Count);
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

			var entranceHops = new Dictionary<PolygonCrossing, PolygonCrossing>();
			var exitHops = new Dictionary<PolygonCrossing, PolygonCrossing>();

			// TODO: merge these two groups of loops together?
			foreach(var entrance in entrances) {
				var entranceLocationB = entrance.LocationB;
				foreach(var exit in exits) {
					if(exit.LocationB == entranceLocationB) {
						entranceHops.Add(entrance, exit);
						break;
					}
				}
			}
			foreach(var exit in exits) {
				var exitLocationA = exit.LocationA;
				foreach(var entrance in entrances) {
					if(entrance.LocationA == exitLocationA) {
						exitHops.Add(exit, entrance);
						break;
					}
				}
			}

			var result = new PolygonCrossingsData {
				AllCrossings = crossings,
				EntranceHops = entranceHops,
				ExitHops = exitHops,
				Entrances = entrances,
				Exits = exits,
				RingCrossingsA = ringCrossingsA,
				RingCrossingsB = ringCrossingsB
			};

			return result;
		}

		private static PolygonCrossing FindNextCrossingNotEqual(
			PolygonCrossing currentCrossing,
			List<PolygonCrossing> crossingsOnRing,
			IComparer<PolygonCrossing> crossingComparer
		) {
			Contract.Requires(currentCrossing != null);
			Contract.Requires(crossingsOnRing != null);
			Contract.Requires(crossingComparer != null);

			var currentPoint = currentCrossing.Point;
			var currentCrossingIndex = crossingsOnRing.BinarySearch(currentCrossing, crossingComparer);
			if (currentCrossingIndex < 0)
				return null;
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
			PolygonCrossing currentCrossing,
			List<PolygonCrossing> crossingsOnRing,
			Ring2 ring,
			Func<PolygonCrossing, PolygonBoundaryLocation> getLocation,
			IComparer<PolygonCrossing> crossingComparer
		) {
			Contract.Requires(currentCrossing != null);
			Contract.Requires(crossingsOnRing != null);
			Contract.Requires(ring != null);
			Contract.Requires(getLocation != null);
			Contract.Requires(crossingComparer != null);

			var currentLocation = getLocation(currentCrossing);
			var segmentCount = ring.SegmentCount;
			int nextSegmentIndex = IterationUtils.AdvanceLoopingIndex(currentLocation.SegmentIndex, segmentCount);
			Contract.Assume(nextSegmentIndex < ring.Count);
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

		private static PolygonCrossing FindPreviousCrossingNotEqual(
			PolygonCrossing currentCrossing,
			List<PolygonCrossing> crossingsOnRing,
			IComparer<PolygonCrossing> crossingComparer
		) {
			Contract.Requires(currentCrossing != null);
			Contract.Requires(crossingsOnRing != null);
			Contract.Requires(crossingComparer != null);

			var currentPoint = currentCrossing.Point;
			int currentCrossingIndex = crossingsOnRing.BinarySearch(currentCrossing, crossingComparer);
			if (currentCrossingIndex < 0)
				return null;
			int crossingsCount = crossingsOnRing.Count;
			// find the first crossing before this one that has a different point location
			Contract.Assume(currentCrossingIndex < crossingsCount);
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
			PolygonCrossing currentCrossing,
			List<PolygonCrossing> crossingsOnRing,
			Ring2 ring,
			Func<PolygonCrossing, PolygonBoundaryLocation> getLocation,
			IComparer<PolygonCrossing> crossingComparer
		) {
			Contract.Requires(currentCrossing != null);
			Contract.Requires(crossingsOnRing != null);
			Contract.Requires(ring != null);
			Contract.Requires(getLocation != null);
			Contract.Requires(crossingComparer != null);

			var currentLocation = getLocation(currentCrossing);
			var segmentCount = ring.SegmentCount;
			Contract.Assume(currentLocation.SegmentIndex < segmentCount);
			var previousSegmentIndex = IterationUtils.RetreatLoopingIndex(currentLocation.SegmentIndex, segmentCount);
			var previousCrossing = FindPreviousCrossingNotEqual(currentCrossing, crossingsOnRing, crossingComparer);
			if(null == previousCrossing) {
				Contract.Assume(previousSegmentIndex < ring.Count);
				Contract.Assume(currentLocation.SegmentIndex < ring.Count);
				return ring[
					currentLocation.SegmentRatio == 0
					? previousSegmentIndex
					: currentLocation.SegmentIndex
				];
			}
			var previousCrossingLocation = getLocation(previousCrossing);
			if(currentLocation.SegmentRatio == 0) {
				Contract.Assume(previousSegmentIndex < ring.Count);
				return previousSegmentIndex == previousCrossingLocation.SegmentIndex
					? previousCrossing.Point
					: ring[previousSegmentIndex];
			}
			Contract.Assume(currentLocation.SegmentIndex < ring.Count);
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
		public List<PolygonCrossing> FindPointCrossings(Polygon2 a, Polygon2 b) {
			Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
			if(null == a || null == b) return new List<PolygonCrossing>(0);
			return FindPointCrossingsCore(a, b).AllCrossings ?? new List<PolygonCrossing>(0);
		}

		private PolygonCrossingsData FindPointCrossingsCore(Polygon2 a, Polygon2 b) {
			Contract.Requires(a != null);
			Contract.Requires(b != null);
			Contract.Ensures(Contract.Result<PolygonCrossingsData>() != null);

			var crossingGenerator = new PolygonPointCrossingGenerator(a, b);
			var allCrossings = crossingGenerator.GenerateCrossings();
			return BuildCrossingsData(allCrossings, a, b);
		}

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(_options != null);
		}

	}

}
