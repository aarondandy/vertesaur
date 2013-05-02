// ===============================================================================
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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Vertesaur.Contracts;
using Vertesaur.Utility;
using CrossingType = Vertesaur.PolygonOperation.PolygonCrossing.CrossingType;

namespace Vertesaur.PolygonOperation
{

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

        private sealed class PolygonRingCrossingLookup
        {
            private KeyValuePair<int, List<PolygonCrossing>> _cache;
            public readonly Dictionary<int, List<PolygonCrossing>> RingCrossings;

            public PolygonRingCrossingLookup(int initialCapacity)
                : this(new Dictionary<int, List<PolygonCrossing>>(initialCapacity)) {}

            public PolygonRingCrossingLookup(Dictionary<int, List<PolygonCrossing>> ringCrossings) {
                Contract.Requires(ringCrossings != null);
                RingCrossings = ringCrossings;
                _cache = new KeyValuePair<int, List<PolygonCrossing>>(-1,null);
            }

            [ContractInvariantMethod]
            private void CodeContractInvariants() {
                Contract.Invariant(RingCrossings != null);
            }

            public List<PolygonCrossing> Get(int ringIndex) {
                Contract.Requires(ringIndex >= 0);
                if (_cache.Key == ringIndex)
                    return _cache.Value;

                List<PolygonCrossing> crossings;
                RingCrossings.TryGetValue(ringIndex, out crossings);
                _cache = new KeyValuePair<int, List<PolygonCrossing>>(ringIndex, crossings);
                return crossings;
            }

            public void Add(int ringIndex, PolygonCrossing crossing) {
                Contract.Requires(ringIndex >= 0);
                Contract.Requires(crossing != null);
                if (_cache.Key == ringIndex) {
                    _cache.Value.Add(crossing);
                    return;
                }

                List<PolygonCrossing> crossings;
                if(!RingCrossings.TryGetValue(ringIndex, out crossings)){
                    crossings = new List<PolygonCrossing>();
                    RingCrossings.Add(ringIndex, crossings);
                }
                Contract.Assume(crossings != null);
                crossings.Add(crossing);
                _cache = new KeyValuePair<int, List<PolygonCrossing>>(ringIndex, crossings);
            }

            public void SortLists(Comparison<PolygonCrossing> comparison) {
                foreach (var list in RingCrossings.Values) {
                    list.Sort(comparison);
                }
            }

        }

        private sealed class PolygonCrossingsAlgorithmKernel
        {

            public readonly Polygon2 A;
            public readonly Polygon2 B;
            public readonly List<PolygonCrossing> AllCrossings;
            public readonly PolygonRingCrossingLookup RingCrossingsA;
            public readonly PolygonRingCrossingLookup RingCrossingsB;
            public readonly HashSet<PolygonCrossing> Entrances;
            public readonly HashSet<PolygonCrossing> Exits;
            public readonly Dictionary<PolygonCrossing, PolygonCrossing> EntranceHops;
            public readonly Dictionary<PolygonCrossing, PolygonCrossing> ExitHops;
            public readonly List<PolygonCrossing> VisitedCrossings;

            public PolygonCrossingsAlgorithmKernel(Polygon2 a, Polygon2 b, List<PolygonCrossing> crossings) {
                Contract.Requires(a != null);
                Contract.Requires(b != null);
                Contract.Requires(crossings != null);
                A = a;
                B = b;
                AllCrossings = crossings;

                RingCrossingsA = new PolygonRingCrossingLookup(a.Count);
                RingCrossingsB = new PolygonRingCrossingLookup(b.Count);
                foreach (var crossing in crossings) {
                    RingCrossingsA.Add(crossing.LocationA.RingIndex, crossing);
                    RingCrossingsB.Add(crossing.LocationB.RingIndex, crossing);
                }
                RingCrossingsA.SortLists(PolygonCrossing.LocationAComparer.Default.Compare);
                RingCrossingsB.SortLists(PolygonCrossing.LocationBComparer.Default.Compare);

                Entrances = new HashSet<PolygonCrossing>();
                Exits = new HashSet<PolygonCrossing>();
                EntranceHops = new Dictionary<PolygonCrossing, PolygonCrossing>();
                ExitHops = new Dictionary<PolygonCrossing, PolygonCrossing>();
                VisitedCrossings = new List<PolygonCrossing>();
            }

            //RingCrossingsA = SortedRingLookUp(crossings, PolygonCrossing.LocationAComparer.Default.Compare, GetRingIndexA, A.Count);
            //RingCrossingsB = SortedRingLookUp(crossings, PolygonCrossing.LocationBComparer.Default.Compare, GetRingIndexB, B.Count);

            /*private static Dictionary<int, List<PolygonCrossing>> SortedRingLookUp(
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
                    Contract.Assume(ringCrossings != null);
                    ringCrossings.Add(crossing);
                }

                foreach (var ringCrossings in result.Values)
                    ringCrossings.Sort(crossingComparison);

                return result;
            }*/

            [ContractInvariantMethod]
            private void CodeContractInvariants() {
                Contract.Invariant(A != null);
                Contract.Invariant(B != null);
                Contract.Invariant(AllCrossings != null);
                Contract.Invariant(RingCrossingsA != null);
                Contract.Invariant(Contract.ForAll(RingCrossingsA.RingCrossings, x => x.Value != null && x.Key >= 0));
                Contract.Invariant(RingCrossingsB != null);
                Contract.Invariant(Contract.ForAll(RingCrossingsB.RingCrossings, x => x.Value != null && x.Key >= 0));
                Contract.Invariant(Entrances != null);
                Contract.Invariant(Exits != null);
                Contract.Invariant(EntranceHops != null);
                Contract.Invariant(ExitHops != null);
                Contract.Invariant(VisitedCrossings != null);
            }

            private void VisitEntrance(PolygonCrossing entrance) {
                if (Entrances.Remove(entrance))
                    VisitedCrossings.Add(entrance);
            }

            private void VisitExit(PolygonCrossing exit) {
                if (Exits.Remove(exit))
                    VisitedCrossings.Add(exit);
            }

            private PolygonCrossing FindNextStartableEntrance() {
                return Entrances.FirstOrDefault(entrance =>
                    !EntranceHops.ContainsKey(entrance)
                    && !ExitHops.ContainsValue(entrance)
                );
            }

            private IEnumerable<Ring2> FindUntouchedRingsA() {
                Contract.Ensures(Contract.Result<IEnumerable<Ring2>>() != null);
                if (null == VisitedCrossings || 0 == VisitedCrossings.Count)
                    return A;
                var includedRingIndices = new HashSet<int>(VisitedCrossings.Select(x => x.LocationA.RingIndex));
                return A.Count == includedRingIndices.Count
                    ? Enumerable.Empty<Ring2>()
                    : A.Where((ring, i) => !includedRingIndices.Contains(i));
            }

            private IEnumerable<Ring2> FindUntouchedRingsB() {
                Contract.Ensures(Contract.Result<IEnumerable<Ring2>>() != null);
                if (null == VisitedCrossings || 0 == VisitedCrossings.Count)
                    return B;
                var includedRingIndices = new HashSet<int>(VisitedCrossings.Select(x => x.LocationB.RingIndex));
                return B.Count == includedRingIndices.Count
                    ? Enumerable.Empty<Ring2>()
                    : B.Where((ring, i) => !includedRingIndices.Contains(i));
            }

            private PolygonCrossing TraverseASideHops(PolygonCrossing start) {
                Contract.Requires(start != null);
                PolygonCrossing current = start;
                PolygonCrossing result = null;
                PolygonCrossing next;
                while (
                    Entrances.Contains(current)
                    && EntranceHops.TryGetValue(current, out next)
                    && Exits.Contains(next)
                    && next.LocationA != start.LocationA
                ) {
                    result = next;
                    VisitEntrance(current);
                    VisitExit(next);
                    if (!ExitHops.TryGetValue(next, out current))
                        break;
                }
                return result;
            }

            private PolygonCrossing TraverseASideRing(
                List<Point2> buildingRing,
                PolygonCrossing fromCrossing,
                List<PolygonCrossing> ringCrossings
            ) {
                Contract.Requires(buildingRing != null);
                Contract.Requires(fromCrossing != null);
                Contract.Requires(ringCrossings != null);
                Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

                var fromSegmentLocationA = fromCrossing.LocationA;
                if (0 == buildingRing.Count || buildingRing[buildingRing.Count - 1] != fromCrossing.Point)
                    buildingRing.Add(fromCrossing.Point);
                foreach (var crossing in TraverseCrossings(fromCrossing, ringCrossings, PolygonCrossing.LocationAComparer.Default)) {
                    // hops should have already been taken care of
                    if (crossing.Point == fromCrossing.Point && EntranceHops.ContainsKey(crossing))
                        continue; // no, lets go somewhere useful with this

                    var locationA = crossing.LocationA;
                    Contract.Assume(locationA.RingIndex < A.Count);
                    var ringA = A[locationA.RingIndex];
                    Contract.Assume(ringA != null);

                    AddPointsBetweenForward(buildingRing, ringA, fromSegmentLocationA, locationA);
                    fromSegmentLocationA = locationA; // for later...

                    if (Entrances.Contains(crossing))
                        return crossing; // if we found it, stop

                    if (buildingRing.Count == 0 || buildingRing[buildingRing.Count - 1] != crossing.Point)
                        buildingRing.Add(crossing.Point); // if it is something else, lets add it
                }
                return null;
            }

            private PolygonCrossing TraverseASide(
                PolygonCrossing startExit,
                List<Point2> buildingRing
            ) {
                Contract.Requires(startExit != null);
                Contract.Requires(buildingRing != null);
                Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

                PolygonCrossing exit = startExit;
                PolygonCrossing entrance;
                do {
                    var ringCrossings = RingCrossingsA.Get(exit.LocationA.RingIndex);
                    Contract.Assume(null != ringCrossings);
                    entrance = TraverseASideRing(buildingRing, exit, ringCrossings);
                    if (null == entrance)
                        return null;

                    exit = TraverseASideHops(entrance);
                } while (null != exit);
                return entrance;
            }

            private PolygonCrossing TraverseBSideHops(PolygonCrossing start) {
                Contract.Requires(start != null);
                PolygonCrossing current = start;
                PolygonCrossing result = null;
                PolygonCrossing next;
                while (
                    Exits.Contains(current)
                    && ExitHops.TryGetValue(current, out next)
                    && Entrances.Contains(next)
                    && next.LocationB != start.LocationB
                ) {
                    result = next;
                    VisitEntrance(next);
                    VisitExit(current);
                    if (!EntranceHops.TryGetValue(next, out current))
                        break;
                }
                return result;
            }

            private PolygonCrossing TraverseBSideRing(
                List<Point2> buildingRing,
                PolygonCrossing fromCrossing,
                List<PolygonCrossing> ringCrossings
            ) {
                Contract.Requires(buildingRing != null);
                Contract.Requires(fromCrossing != null);
                Contract.Requires(ringCrossings != null);
                Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

                var fromSegmentLocationB = fromCrossing.LocationB;
                if (0 == buildingRing.Count || buildingRing[buildingRing.Count - 1] != fromCrossing.Point)
                    buildingRing.Add(fromCrossing.Point);
                foreach (var crossing in TraverseCrossings(fromCrossing, ringCrossings, PolygonCrossing.LocationBComparer.Default)) {
                    // hops should have already been taken care of
                    if (crossing.Point == fromCrossing.Point && ExitHops.ContainsKey(crossing))
                        continue; // no, lets go somewhere useful with this

                    var locationB = crossing.LocationB;
                    Contract.Assume(locationB.RingIndex < B.Count);
                    var ringB = B[locationB.RingIndex];
                    Contract.Assume(null != ringB);

                    AddPointsBetweenForward(buildingRing, ringB, fromSegmentLocationB, locationB);
                    fromSegmentLocationB = locationB; // for later...

                    if (Exits.Contains(crossing))
                        return crossing; // if we found it, stop

                    if (buildingRing.Count == 0 || buildingRing[buildingRing.Count - 1] != crossing.Point)
                        buildingRing.Add(crossing.Point); // if it is something else, lets add it
                }
                return null;
            }

            private PolygonCrossing TraverseBSide(
                PolygonCrossing startEntrance,
                List<Point2> buildingRing
            ) {
                Contract.Requires(startEntrance != null);
                Contract.Requires(buildingRing != null);
                Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

                PolygonCrossing entrance = startEntrance;
                PolygonCrossing exit;
                do {
                    var ringCrossings = RingCrossingsB.Get(entrance.LocationB.RingIndex);
                    Contract.Assume(null != ringCrossings);
                    exit = TraverseBSideRing(buildingRing, entrance, ringCrossings);
                    if (null == exit)
                        return null;

                    entrance = TraverseBSideHops(exit);
                } while (null != entrance);
                return exit;
            }

            public IntersectionResults Turtle(PointWinding fillWinding) {
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

                PolygonCrossing startEntrance;
                while (null != (startEntrance = FindNextStartableEntrance())) {
                    var buildingRing = new List<Point2>();
                    var entrance = startEntrance;
                    do {
                        var exit = TraverseBSide(entrance, buildingRing);
                        if (null == exit) {
                            VisitEntrance(startEntrance); // may need to do this to be safe
                            break; // unmatched entrance
                        }
                        VisitExit(exit);
                        entrance = TraverseASide(exit, buildingRing);
                        if (null == entrance) {
                            break; // unmatched exit
                        }
                        VisitEntrance(entrance);
                    } while (entrance != startEntrance);
                    if (buildingRing.Count > 2)
                        rings.Add(new Ring2(buildingRing)); // here is a new ring
                }

                if (fillWinding != PointWinding.Unknown)
                    rings.ForceFillWinding(fillWinding);

                return BuildFinalResults(rings);
            }

            private static List<Ring2> FilterQualifiedRingsToBoundaryTree(List<Ring2> rings, RingBoundaryTree boundaryTree) {
                Contract.Requires(rings != null);
                Contract.Requires(boundaryTree != null);
                Contract.Ensures(Contract.Result<List<Ring2>>() != null);
                return rings
                    .Where(ring =>
                        ring.Hole.GetValueOrDefault()
                        ? boundaryTree.NonIntersectingContains(ring)
                        : !boundaryTree.NonIntersectingContains(ring))
                    .ToList();
            }

            private IntersectionResults BuildFinalResults(Polygon2 intersectedPolygon) {
                Contract.Requires(intersectedPolygon != null);
                Contract.Ensures(Contract.Result<IntersectionResults>() != null);
                Contract.Ensures(intersectedPolygon.Count >= Contract.OldValue(intersectedPolygon).Count);

                if (intersectedPolygon.Count == 0) {
                    var untouchedA = new Polygon2(FindUntouchedRingsA());
                    var untouchedB = new Polygon2(FindUntouchedRingsB());
                    intersectedPolygon.AddRange(QualifyRings(untouchedA, untouchedB, true));
                    intersectedPolygon.AddRange(QualifyRings(untouchedB, untouchedA, false));
                }
                else {
                    var intersectedResultTree = new RingBoundaryTree(intersectedPolygon);
                    intersectedPolygon.AddRange(
                        FilterQualifiedRingsToBoundaryTree(
                            QualifyRings(FindUntouchedRingsA(), B, true),
                            intersectedResultTree
                        )
                    );
                    intersectedPolygon.AddRange(
                        FilterQualifiedRingsToBoundaryTree(
                            QualifyRings(FindUntouchedRingsB(), A, false),
                            intersectedResultTree
                        )
                    );
                }
                return new IntersectionResults {
                    Polygon = intersectedPolygon
                };
            }

        }

        [Pure]
        private static PolygonBoundaryLocation GetLocationA(PolygonCrossing crossing) {
            Contract.Requires(crossing != null);
            return crossing.LocationA;
        }

        [Pure]
        private static PolygonBoundaryLocation GetLocationB(PolygonCrossing crossing) {
            Contract.Requires(crossing != null);
            return crossing.LocationB;
        }

        [Pure]
        private static int GetRingIndexA(PolygonCrossing crossing) {
            Contract.Requires(crossing != null);
            return crossing.LocationA.RingIndex;
        }

        [Pure]
        private static int GetRingIndexB(PolygonCrossing crossing) {
            Contract.Requires(crossing != null);
            return crossing.LocationB.RingIndex;
        }

        /// <summary>
        /// Options for this intersection operation.
        /// </summary>
        protected PolygonBinaryOperationOptions Options { get; private set; }

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
            Options = PolygonBinaryOperationOptions.CloneOrDefault(options);
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(Options != null);
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

            if (Options.InvertLeftHandSide)
                a = PolygonInverseOperation.Invert(a);
            if (Options.InvertRightHandSide)
                b = PolygonInverseOperation.Invert(b);

            // find all the crossings
            Contract.Assume(a != null);
            Contract.Assume(b != null);
            var fillWinding = DetermineFillWinding(a.Concat(b));

            var kernel = FindPointCrossingsCore(a, b);
            // traverse the rings
            var results = kernel.Turtle(fillWinding);
            if (null == results.Polygon)
                return null;

            // TODO: this stuff is not so great
            results.Polygon = (results.Polygon.Count == 0 ? null : results.Polygon);
            if (null != results.Polygon) {
                if (fillWinding != PointWinding.Unknown)
                    results.Polygon.ForceFillWinding(fillWinding);
            }

            if (Options.InvertResult)
                results.Polygon = PolygonInverseOperation.Invert(results.Polygon);

            return results.Polygon;
        }

        /// <summary>
        /// Returns the first known point winding associated with ring which has an explicit hole value, failing that the first known ring winding encountered.
        /// </summary>
        /// <param name="rings">The rings to search.</param>
        /// <returns>The discovered point winding or <see cref="Vertesaur.PointWinding.Unknown"/>.</returns>
        private static PointWinding DetermineFillWinding(IEnumerable<Ring2> rings) {
            Contract.Requires(null != rings);
            var hint = PointWinding.Unknown;
            foreach (var ring in rings) {
                var ringWinding = ring.DetermineWinding();
                if (ringWinding == PointWinding.Unknown)
                    continue;
                if (ring.Hole.HasValue) {
                    if (ring.Hole.Value) {
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
            foreach (var ring in untouchedRings) {
                // TODO: speed this up through some kind of magic, like RingBoundaryTree or something
                // TODO: var eq = new SpatialEqualityComparerThing(ringA); var stuff = otherRings.Where(r = > eq.SpatiallyEqual(r));
                if (polygon.Any(r => ring.SpatiallyEqual(r))) {
                    if (qualifyEqual)
                        result.Add(ring.Clone());
                }
                else if (ringTree.NonIntersectingContains(ring))
                    result.Add(ring.Clone());
            }
            Contract.Assume(Contract.ForAll(result, x => null != x));
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

        /// <summary>
        /// Builds required crossing data. 
        /// </summary>
        /// <param name="crossings">The crossings to calculate. (Note: collection is modified)</param>
        /// <param name="a">The left hand polygon.</param>
        /// <param name="b">The right hand polygon.</param>
        private PolygonCrossingsAlgorithmKernel BuildCrossingsData(List<PolygonCrossing> crossings, Polygon2 a, Polygon2 b) {
            Contract.Requires(crossings != null);
            Contract.Requires(a != null);
            Contract.Requires(b != null);
            Contract.Ensures(Contract.Result<PolygonCrossingsAlgorithmKernel>() != null);

            var kernel = new PolygonCrossingsAlgorithmKernel(a, b, crossings);

            if (kernel.AllCrossings.Count == 0)
                return kernel;

            foreach (var currentCrossing in crossings) {
                var ringIndexA = currentCrossing.LocationA.RingIndex;
                Contract.Assume(ringIndexA < a.Count);
                var ringA = a[ringIndexA];
                Contract.Assume(ringA != null);
                var crossingsOnRingA = kernel.RingCrossingsA.Get(ringIndexA);
                Contract.Assume(crossingsOnRingA != null);

                var priorPointA = FindPreviousRingPoint(currentCrossing, crossingsOnRingA, ringA, GetLocationA, PolygonCrossing.LocationAComparer.Default);
                var nextPointA = FindNextRingPoint(currentCrossing, crossingsOnRingA, ringA, GetLocationA, PolygonCrossing.LocationAComparer.Default);

                var ringIndexB = currentCrossing.LocationB.RingIndex;
                Contract.Assume(ringIndexB < b.Count);
                var ringB = b[ringIndexB];
                Contract.Assume(ringB != null);
                var crossingsOnRingB = kernel.RingCrossingsB.Get(ringIndexB);
                Contract.Assume(crossingsOnRingB != null);

                var priorPointB = FindPreviousRingPoint(currentCrossing, crossingsOnRingB, ringB, GetLocationB, PolygonCrossing.LocationBComparer.Default);
                var nextPointB = FindNextRingPoint(currentCrossing, crossingsOnRingB, ringB, GetLocationB, PolygonCrossing.LocationBComparer.Default);

                // based on the vectors, need to classify the crossing type
                currentCrossing.CrossType = PolygonCrossing.DetermineCrossingType(
                    (nextPointA - currentCrossing.Point).GetNormalized(),
                    (priorPointA - currentCrossing.Point).GetNormalized(),
                    (nextPointB - currentCrossing.Point).GetNormalized(),
                    (priorPointB - currentCrossing.Point).GetNormalized()
                );
            }

            var entrances = kernel.Entrances;
            var exits = kernel.Exits;
            foreach (var ringCrossings in kernel.RingCrossingsA.RingCrossings) {
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

            var entranceHops = kernel.EntranceHops;
            var exitHops = kernel.ExitHops;

            // TODO: merge these two groups of loops together?
            foreach (var entrance in entrances) {
                var entranceLocationB = entrance.LocationB;
                foreach (var exit in exits) {
                    if (entranceLocationB.Equals(exit.LocationB)) {
                        entranceHops.Add(entrance, exit);
                        break;
                    }
                }
            }
            foreach (var exit in exits) {
                var exitLocationA = exit.LocationA;
                foreach (var entrance in entrances) {
                    if (exitLocationA.Equals(entrance.LocationA)) {
                        exitHops.Add(exit, entrance);
                        break;
                    }
                }
            }

            return kernel;
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
            if (null == previousCrossing) {
                Contract.Assume(previousSegmentIndex < ring.Count);
                Contract.Assume(currentLocation.SegmentIndex < ring.Count);
                return ring[
                    currentLocation.SegmentRatio == 0
                    ? previousSegmentIndex
                    : currentLocation.SegmentIndex
                ];
            }
            var previousCrossingLocation = getLocation(previousCrossing);
            if (currentLocation.SegmentRatio == 0) {
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
            if (null == a || null == b) return new List<PolygonCrossing>(0);
            return FindPointCrossingsCore(a, b).AllCrossings;
        }

        private PolygonCrossingsAlgorithmKernel FindPointCrossingsCore(Polygon2 a, Polygon2 b) {
            Contract.Requires(a != null);
            Contract.Requires(b != null);
            Contract.Ensures(Contract.Result<PolygonCrossingsAlgorithmKernel>() != null);
            var crossingGenerator = new PolygonPointCrossingGenerator(a, b);
            var allCrossings = crossingGenerator.GenerateCrossings();
            return BuildCrossingsData(allCrossings, a, b);
        }

    }

}
