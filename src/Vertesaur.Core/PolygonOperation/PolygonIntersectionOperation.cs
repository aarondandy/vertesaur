using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
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
            // TODO: public MultiLineString2 LineStrings;
            // TODO: public MultiPoint2 Points;
        }

        private sealed class PolygonRingCrossingLookup
        {

            public static PolygonRingCrossingLookup Build(Dictionary<int, List<PolygonCrossing>> input, Comparison<PolygonCrossing> comparison) {
                Contract.Requires(input != null);
                Contract.Requires(comparison != null);
                Contract.Ensures(Contract.Result<PolygonRingCrossingLookup>() != null);
                var core = new Dictionary<int, PolygonCrossing[]>(input.Count);
                foreach (var set in input) {
                    var crossings = set.Value.ToArray();
                    Array.Sort(crossings, comparison);
                    core.Add(set.Key, crossings);
                }
                return new PolygonRingCrossingLookup(core);
            }

            public PolygonRingCrossingLookup(int initialCapacity)
                : this(new Dictionary<int, PolygonCrossing[]>(initialCapacity)) {}

            private PolygonRingCrossingLookup(Dictionary<int, PolygonCrossing[]> ringCrossings) {
                Contract.Requires(ringCrossings != null);
                RingCrossings = ringCrossings;
                _cache = new KeyValuePair<int, PolygonCrossing[]>(-1,null);
            }

            private KeyValuePair<int, PolygonCrossing[]> _cache;
            public readonly Dictionary<int, PolygonCrossing[]> RingCrossings;

            [ContractInvariantMethod]
            private void CodeContractInvariants() {
                Contract.Invariant(RingCrossings != null);
            }

            public PolygonCrossing[] Get(int ringIndex) {
                Contract.Requires(ringIndex >= 0);
                Contract.Ensures(Contract.Result<PolygonCrossing[]>() != null);
                Contract.Ensures(Contract.ForAll(Contract.Result<PolygonCrossing[]>(), x => x != null));
                if (_cache.Key == ringIndex)
                    return _cache.Value;

                PolygonCrossing[] crossings;
                if (!RingCrossings.TryGetValue(ringIndex, out crossings)) {
                    crossings = new PolygonCrossing[0];
                }
                _cache = new KeyValuePair<int, PolygonCrossing[]>(ringIndex, crossings);
                Contract.Assume(crossings != null);
                return crossings;
            }

        }

        private sealed class PolygonCrossingsAlgorithmKernel
        {

            public readonly Polygon2 A;
            public readonly Polygon2 B;
            public readonly PolygonRingCrossingLookup RingCrossingsA;
            public readonly PolygonRingCrossingLookup RingCrossingsB;
            public readonly HashSet<PolygonCrossing> Entrances;
            public readonly HashSet<PolygonCrossing> Exits;
            public readonly Dictionary<PolygonCrossing, PolygonCrossing> EntranceHops;
            public readonly Dictionary<PolygonCrossing, PolygonCrossing> ExitHops;
            public readonly List<PolygonCrossing> VisitedCrossings;
            public readonly HashSet<int> VisitedCrossingsRingIndicesA;
            public readonly HashSet<int> VisitedCrossingsRingIndicesB;

            [ContractVerification(false)] // TODO: remvoe when CC bugs are fixed
            public PolygonCrossingsAlgorithmKernel(Polygon2 a, Polygon2 b, List<PolygonCrossing> crossings) {
                Contract.Requires(a != null);
                Contract.Requires(b != null);
                Contract.Requires(crossings != null);
                Contract.Requires(Contract.ForAll(crossings, x => x != null));
                A = a;
                B = b;

                var ringCrossingsABuilder = new Dictionary<int, List<PolygonCrossing>>(a.Count);
                var ringCrossingsBBuilder = new Dictionary<int, List<PolygonCrossing>>(b.Count);
                foreach (var crossing in crossings) {
                    List<PolygonCrossing> list;
                    if (!ringCrossingsABuilder.TryGetValue(crossing.LocationA.RingIndex, out list)) {
                        list = new List<PolygonCrossing>();
                        ringCrossingsABuilder.Add(crossing.LocationA.RingIndex, list);
                    }
                    Contract.Assume(list != null);
                    list.Add(crossing);
                    Contract.Assume(crossing.LocationB != null);
                    if (!ringCrossingsBBuilder.TryGetValue(crossing.LocationB.RingIndex, out list)) {
                        Contract.Assume(crossing.LocationB != null);
                        list = new List<PolygonCrossing>();
                        ringCrossingsBBuilder.Add(crossing.LocationB.RingIndex, list);
                    }
                    Contract.Assume(list != null);
                    list.Add(crossing);
                }
                RingCrossingsA = PolygonRingCrossingLookup.Build(ringCrossingsABuilder, PolygonCrossing.LocationAComparer.CompareNonNull);
                RingCrossingsB = PolygonRingCrossingLookup.Build(ringCrossingsBBuilder, PolygonCrossing.LocationBComparer.CompareNonNull);

                Entrances = new HashSet<PolygonCrossing>();
                Exits = new HashSet<PolygonCrossing>();
                EntranceHops = new Dictionary<PolygonCrossing, PolygonCrossing>();
                ExitHops = new Dictionary<PolygonCrossing, PolygonCrossing>();
                VisitedCrossings = new List<PolygonCrossing>();
                VisitedCrossingsRingIndicesA = new HashSet<int>();
                VisitedCrossingsRingIndicesB = new HashSet<int>();
            }

            [ContractInvariantMethod]
            private void CodeContractInvariants() {
                Contract.Invariant(A != null);
                Contract.Invariant(B != null);
                Contract.Invariant(RingCrossingsA != null);
                Contract.Invariant(Contract.ForAll(RingCrossingsA.RingCrossings, x => x.Value != null && x.Key >= 0 && x.Key < A.Count));
                Contract.Invariant(RingCrossingsB != null);
                Contract.Invariant(Contract.ForAll(RingCrossingsB.RingCrossings, x => x.Value != null && x.Key >= 0 && x.Key < B.Count));
                Contract.Invariant(Entrances != null);
                Contract.Invariant(Contract.ForAll(Entrances, x => x != null));
                Contract.Invariant(Exits != null);
                Contract.Invariant(Contract.ForAll(Exits, x => x != null));
                Contract.Invariant(EntranceHops != null);
                Contract.Invariant(Contract.ForAll(EntranceHops.Values, x => x != null));
                Contract.Invariant(ExitHops != null);
                Contract.Invariant(Contract.ForAll(ExitHops.Values, x => x != null));
                Contract.Invariant(VisitedCrossings != null);
                Contract.Invariant(VisitedCrossingsRingIndicesA != null);
                Contract.Invariant(VisitedCrossingsRingIndicesA.Count <= A.Count);
                Contract.Invariant(Contract.ForAll(VisitedCrossingsRingIndicesA, x => x >= 0 && x < A.Count));
                Contract.Invariant(VisitedCrossingsRingIndicesB != null);
                Contract.Invariant(Contract.ForAll(VisitedCrossingsRingIndicesB, x => x >= 0 && x < B.Count));
                Contract.Invariant(VisitedCrossingsRingIndicesB.Count <= B.Count);
            }

            private void ApplyCrossingVisit(PolygonCrossing crossing) {
                Contract.Requires(crossing != null);
                VisitedCrossings.Add(crossing);
                VisitedCrossingsRingIndicesA.Add(crossing.LocationA.RingIndex);
                VisitedCrossingsRingIndicesB.Add(crossing.LocationB.RingIndex);
            }

            private void VisitEntrance(PolygonCrossing entrance) {
                Contract.Requires(entrance != null);
                if (Entrances.Remove(entrance))
                    ApplyCrossingVisit(entrance);
            }

            private void VisitExit(PolygonCrossing exit) {
                Contract.Requires(exit != null);
                if (Exits.Remove(exit))
                    ApplyCrossingVisit(exit);
            }

            private PolygonCrossing FindNextStartableEntrance() {
                foreach (var entrance in Entrances) {
                    if (!EntranceHops.ContainsKey(entrance) && !ExitHops.ContainsValue(entrance))
                        return entrance;
                }
                return null;
            }

            private Polygon2 FindUntouchedRingsA() {
                Contract.Ensures(Contract.Result<IEnumerable<Ring2>>() != null);
                if (VisitedCrossingsRingIndicesA.Count == 0)
                    return A;

                var missingCount = A.Count - VisitedCrossingsRingIndicesA.Count;
                var results = new Polygon2(missingCount);
                if (missingCount == 0)
                    return results;

                for (int i = 0; i < A.Count; i++) {
                    if(!VisitedCrossingsRingIndicesA.Contains(i))
                        results.Add(A[i]);
                }
                return results;
            }

            private Polygon2 FindUntouchedRingsB() {
                Contract.Ensures(Contract.Result<IEnumerable<Ring2>>() != null);
                if (VisitedCrossingsRingIndicesB.Count == 0)
                    return B;

                var missingCount = B.Count - VisitedCrossingsRingIndicesB.Count;
                var results = new Polygon2(missingCount);
                if (missingCount == 0)
                    return results;

                for (int i = 0; i < B.Count; i++) {
                    if (!VisitedCrossingsRingIndicesB.Contains(i))
                        results.Add(B[i]);
                }
                return results;
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
                    Contract.Assume(current != null);
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
                PolygonCrossing[] ringCrossings
            ) {
                Contract.Requires(buildingRing != null);
                Contract.Requires(fromCrossing != null);
                Contract.Requires(ringCrossings != null);
                Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

                var fromSegmentLocationA = fromCrossing.LocationA;
                if (0 == buildingRing.Count || buildingRing[buildingRing.Count - 1] != fromCrossing.Point)
                    buildingRing.Add(fromCrossing.Point);

                var fromCrossingIndex = Array.BinarySearch(ringCrossings, fromCrossing, PolygonCrossing.LocationAComparer.Default);
                Contract.Assume(fromCrossingIndex >= 0);
                foreach (var crossing in TraverseCrossings(fromCrossing, ringCrossings, fromCrossingIndex)) {
                    // hops should have already been taken care of
                    if (crossing.Point == fromCrossing.Point && EntranceHops.ContainsKey(crossing))
                        continue; // no, lets go somewhere useful with this

                    var locationA = crossing.LocationA;
                    Contract.Assume(locationA.RingIndex < A.Count);
                    Contract.Assume(A[locationA.RingIndex] != null);
                    AddPointsBetweenForward(buildingRing, A[locationA.RingIndex], fromSegmentLocationA, locationA);
                    fromSegmentLocationA = locationA; // for later...

                    if (Entrances.Contains(crossing))
                        return crossing; // if we found it, stop

                    if (buildingRing.Count == 0 || buildingRing[buildingRing.Count - 1] != crossing.Point)
                        buildingRing.Add(crossing.Point); // if it is something else, lets add it
                }
                return null;
            }

            private PolygonCrossing TraverseASide(
                PolygonCrossing exit,
                List<Point2> buildingRing
            ) {
                Contract.Requires(exit != null);
                Contract.Requires(buildingRing != null);
                Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);
                PolygonCrossing entrance;
                do {
                    entrance = TraverseASideRing(buildingRing, exit, RingCrossingsA.Get(exit.LocationA.RingIndex));
                    if (entrance == null)
                        return null;
                } while ((exit = TraverseASideHops(entrance)) != null);
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
                    Contract.Assume(current != null);
                    VisitExit(current);
                    if (!EntranceHops.TryGetValue(next, out current))
                        break;
                }
                return result;
            }

            private PolygonCrossing TraverseBSideRing(
                List<Point2> buildingRing,
                PolygonCrossing fromCrossing,
                PolygonCrossing[] ringCrossings
            ) {
                Contract.Requires(buildingRing != null);
                Contract.Requires(fromCrossing != null);
                Contract.Requires(ringCrossings != null);
                Contract.Requires(Contract.ForAll(ringCrossings, x => x != null));
                Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);

                var fromSegmentLocationB = fromCrossing.LocationB;
                if (0 == buildingRing.Count || buildingRing[buildingRing.Count - 1] != fromCrossing.Point)
                    buildingRing.Add(fromCrossing.Point);

                var fromCrossingIndex = Array.BinarySearch(ringCrossings, fromCrossing, PolygonCrossing.LocationBComparer.Default);
                Contract.Assume(fromCrossingIndex >= 0);
                foreach (var crossing in TraverseCrossings(fromCrossing, ringCrossings, fromCrossingIndex)) {
                    // hops should have already been taken care of
                    if (crossing.Point == fromCrossing.Point && ExitHops.ContainsKey(crossing))
                        continue; // no, lets go somewhere useful with this

                    var locationB = crossing.LocationB;
                    Contract.Assume(locationB.RingIndex < B.Count);
                    Contract.Assume(B[locationB.RingIndex] != null);
                    AddPointsBetweenForward(buildingRing, B[locationB.RingIndex], fromSegmentLocationB, locationB);
                    fromSegmentLocationB = locationB; // for later...

                    if (Exits.Contains(crossing))
                        return crossing; // if we found it, stop

                    if (buildingRing.Count == 0 || buildingRing[buildingRing.Count - 1] != crossing.Point)
                        buildingRing.Add(crossing.Point); // if it is something else, lets add it
                }
                return null;
            }

            private PolygonCrossing TraverseBSide(
                PolygonCrossing entrance,
                List<Point2> buildingRing
            ) {
                Contract.Requires(entrance != null);
                Contract.Requires(buildingRing != null);
                Contract.Ensures(buildingRing.Count >= Contract.OldValue(buildingRing).Count);
                PolygonCrossing exit;
                do {
                    exit = TraverseBSideRing(buildingRing, entrance, RingCrossingsB.Get(entrance.LocationB.RingIndex));
                    if (exit == null)
                        return null;
                } while ((entrance = TraverseBSideHops(exit)) != null);
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
                while ((startEntrance = FindNextStartableEntrance()) != null) {
                    var buildingRing = new List<Point2>();
                    var entrance = startEntrance;
                    do {
                        var exit = TraverseBSide(entrance, buildingRing);
                        if (exit == null) {
                            VisitEntrance(startEntrance); // may need to do this to be safe
                            break; // unmatched entrance
                        }
                        VisitExit(exit);
                        entrance = TraverseASide(exit, buildingRing);
                        if (entrance == null) {
                            break; // unmatched exit
                        }
                        VisitEntrance(entrance);
                    } while (entrance != startEntrance);
                    if (buildingRing.Count >= 3)
                        rings.Add(new Ring2(buildingRing)); // here is a new ring
                }

                if (fillWinding != PointWinding.Unknown)
                    rings.ForceFillWinding(fillWinding);

                return BuildFinalResults(rings);
            }

            private static IEnumerable<Ring2> FilterQualifiedRingsToBoundaryTree(List<Ring2> rings, RingBoundaryTree boundaryTree) {
                Contract.Requires(rings != null);
                Contract.Requires(boundaryTree != null);
                Contract.Ensures(Contract.Result<IEnumerable<Ring2>>() != null);
                Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<Ring2>>(), x => x != null));
                return rings.Where(ring => ring.Hole.GetValueOrDefault() == boundaryTree.NonIntersectingContains(ring));
            }

            private IntersectionResults BuildFinalResults(Polygon2 intersectedPolygon) {
                Contract.Requires(intersectedPolygon != null);
                Contract.Ensures(Contract.Result<IntersectionResults>() != null);
                Contract.Ensures(intersectedPolygon.Count >= Contract.OldValue(intersectedPolygon).Count);

                if (intersectedPolygon.Count == 0) {
                    var untouchedA = FindUntouchedRingsA();
                    var untouchedB = FindUntouchedRingsB();
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

        [Pure] private static PolygonBoundaryLocation GetLocationA(PolygonCrossing crossing) {
            Contract.Requires(crossing != null);
            Contract.Ensures(Contract.Result<PolygonBoundaryLocation>() != null);
            return crossing.LocationA;
        }

        [Pure] private static PolygonBoundaryLocation GetLocationB(PolygonCrossing crossing) {
            Contract.Requires(crossing != null);
            Contract.Ensures(Contract.Result<PolygonBoundaryLocation>() != null);
            return crossing.LocationB;
        }

        [Pure] private static int GetRingIndexA(PolygonCrossing crossing) {
            Contract.Requires(crossing != null);
            Contract.Ensures(Contract.Result<int>() >= 0);
            return crossing.LocationA.RingIndex;
        }

        [Pure] private static int GetRingIndexB(PolygonCrossing crossing) {
            Contract.Requires(crossing != null);
            Contract.Ensures(Contract.Result<int>() >= 0);
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
            var fillWinding = DetermineFillWinding(a.Concat(b));

            var kernel = CreateIntersectionKernel(a, b);
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
            Polygon2 untouchedRings,
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
                Contract.Assume(ring != null);
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
            PolygonCrossing[] ringCrossings,
            int fromCrossingIndex
        ) {
            Contract.Requires(fromCrossing != null);
            Contract.Requires(ringCrossings != null);
            //Contract.Requires(crossingComparer != null);
            Contract.Requires(fromCrossingIndex >= 0);
            Contract.Ensures(Contract.Result<IEnumerable<PolygonCrossing>>() != null);

            //var fromCrossingIndex = ringCrossings.BinarySearch(fromCrossing, crossingComparer);
            var ringCrossingsCount = ringCrossings.Length;

            Stack<PolygonCrossing> priorResults = null;
            var backTrackIndex = fromCrossingIndex;
            do {
                var priorIndex = IterationUtils.RetreatLoopingIndex(backTrackIndex, ringCrossingsCount);
                var priorCrossing = ringCrossings[priorIndex];
                if (priorCrossing.Point != fromCrossing.Point)
                    break;

                backTrackIndex = priorIndex;
                (priorResults ?? (priorResults = new Stack<PolygonCrossing>()))
                    .Push(priorCrossing);
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
                if (to.SegmentIndex == currentSegmentIndex) {
                    if (to.SegmentRatio > 0) {
                        results.Add(ring[currentSegmentIndex]);
                    }
                    return;
                }
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
        private PolygonCrossingsAlgorithmKernel CreateIntersectionKernelFromCrossings(List<PolygonCrossing> crossings, Polygon2 a, Polygon2 b) {
            Contract.Requires(crossings != null);
            Contract.Requires(a != null);
            Contract.Requires(b != null);
            Contract.Ensures(Contract.Result<PolygonCrossingsAlgorithmKernel>() != null);

            var kernel = new PolygonCrossingsAlgorithmKernel(a, b, crossings);

            if (crossings.Count == 0)
                return kernel;

            foreach (var currentCrossing in crossings) {
                var ringIndexA = currentCrossing.LocationA.RingIndex;
                Contract.Assume(ringIndexA < a.Count);
                var ringA = a[ringIndexA];
                Contract.Assume(ringA != null);
                var crossingsOnRingA = kernel.RingCrossingsA.Get(ringIndexA);

                var priorPointA = FindPreviousRingPoint(currentCrossing, crossingsOnRingA, ringA, GetLocationA, PolygonCrossing.LocationAComparer.Default);
                var nextPointA = FindNextRingPoint(currentCrossing, crossingsOnRingA, ringA, GetLocationA, PolygonCrossing.LocationAComparer.Default);

                var ringIndexB = currentCrossing.LocationB.RingIndex;
                Contract.Assume(ringIndexB < b.Count);
                var ringB = b[ringIndexB];
                Contract.Assume(ringB != null);
                var crossingsOnRingB = kernel.RingCrossingsB.Get(ringIndexB);

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

            foreach (var ringCrossings in kernel.RingCrossingsA.RingCrossings) {
                Contract.Assume(ringCrossings.Key >= 0 && ringCrossings.Key < a.Count);
                if (a[ringCrossings.Key].FillSide == RelativeDirectionType.Right) {
                    foreach (var crossing in ringCrossings.Value) {
                        Contract.Assume(crossing != null);
                        var crossLegType = crossing.CrossType & CrossingType.Parallel;
                        if (crossLegType == CrossingType.CrossToRight || crossLegType == CrossingType.DivergeRight)
                            kernel.Entrances.Add(crossing);
                        else if (crossLegType == CrossingType.CrossToLeft || crossLegType == CrossingType.ConvergeRight)
                            kernel.Exits.Add(crossing);
                    }
                }
                else {
                    foreach (var crossing in ringCrossings.Value) {
                        Contract.Assume(crossing != null);
                        var crossLegType = crossing.CrossType & CrossingType.Parallel;
                        if (crossLegType == CrossingType.CrossToLeft || crossLegType == CrossingType.DivergeLeft)
                            kernel.Entrances.Add(crossing);
                        else if (crossLegType == CrossingType.CrossToRight || crossLegType == CrossingType.ConvergeLeft)
                            kernel.Exits.Add(crossing);
                    }
                }
            }

            var sortedEntrances = kernel.Entrances.ToArray();
            Array.Sort(sortedEntrances, PolygonCrossing.LocationAComparer.CompareNonNull);
            Contract.Assume(kernel.Exits != null);
            var sortedExits = kernel.Exits.ToArray();
            Array.Sort(sortedExits, PolygonCrossing.LocationBComparer.CompareNonNull);

            for (int i = 0; i < sortedExits.Length; i++) {
                var exit = sortedExits[i];
                var locationIndex = Array.BinarySearch(sortedEntrances, exit, PolygonCrossing.LocationAComparer.Default);
                if(locationIndex >= 0)
                    kernel.ExitHops.Add(exit, sortedEntrances[locationIndex]);
            }
            for (int i = 0; i < sortedEntrances.Length; i++) {
                var entrance = sortedEntrances[i];
                var locationIndex = Array.BinarySearch(sortedExits, entrance, PolygonCrossing.LocationBComparer.Default);
                if(locationIndex >= 0)
                    kernel.EntranceHops.Add(entrance, sortedExits[locationIndex]);
            }

            return kernel;
        }

        private static PolygonCrossing FindNextCrossingNotEqual(
            PolygonCrossing currentCrossing,
            PolygonCrossing[] crossingsOnRing,
            IComparer<PolygonCrossing> crossingComparer
        ) {
            Contract.Requires(currentCrossing != null);
            Contract.Requires(crossingsOnRing != null);
            Contract.Requires(Contract.ForAll(crossingsOnRing, x => x != null));
            Contract.Requires(crossingComparer != null);

            var currentPoint = currentCrossing.Point;
            var currentCrossingIndex = Array.BinarySearch(crossingsOnRing, currentCrossing, crossingComparer);
            if (currentCrossingIndex < 0)
                return null;
            var crossingsCount = crossingsOnRing.Length;
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
            PolygonCrossing[] crossingsOnRing,
            Ring2 ring,
            Func<PolygonCrossing, PolygonBoundaryLocation> getLocation,
            IComparer<PolygonCrossing> crossingComparer
        ) {
            Contract.Requires(currentCrossing != null);
            Contract.Requires(crossingsOnRing != null);
            Contract.Requires(Contract.ForAll(crossingsOnRing, x => x != null));
            Contract.Requires(ring != null);
            Contract.Requires(getLocation != null);
            Contract.Requires(crossingComparer != null);

            var currentLocation = getLocation(currentCrossing);
            var segmentCount = ring.SegmentCount;
            int nextSegmentIndex = IterationUtils.AdvanceLoopingIndex(currentLocation.SegmentIndex, segmentCount);
            var nextCrossing = FindNextCrossingNotEqual(currentCrossing, crossingsOnRing, crossingComparer);
            // NOTE: this method assumes a segment ratio less than 1, verify we can trust this
            if (null == nextCrossing) {
                return ring[nextSegmentIndex];
            }
            var nextCrossingLocation = getLocation(nextCrossing);
            Contract.Assume(nextCrossingLocation != null);
            return currentLocation.SegmentIndex == nextCrossingLocation.SegmentIndex
                && currentLocation.SegmentRatio < nextCrossingLocation.SegmentRatio
                ? nextCrossing.Point
                : ring[nextSegmentIndex];
        }

        private static PolygonCrossing FindPreviousCrossingNotEqual(
            PolygonCrossing currentCrossing,
            PolygonCrossing[] crossingsOnRing,
            IComparer<PolygonCrossing> crossingComparer
        ) {
            Contract.Requires(currentCrossing != null);
            Contract.Requires(crossingsOnRing != null);
            Contract.Requires(Contract.ForAll(crossingsOnRing, x => x != null));
            Contract.Requires(crossingComparer != null);

            var currentPoint = currentCrossing.Point;
            int currentCrossingIndex = Array.BinarySearch(crossingsOnRing, currentCrossing, crossingComparer);
            if (currentCrossingIndex < 0)
                return null;
            int crossingsCount = crossingsOnRing.Length;
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
            PolygonCrossing currentCrossing,
            PolygonCrossing[] crossingsOnRing,
            Ring2 ring,
            Func<PolygonCrossing, PolygonBoundaryLocation> getLocation,
            IComparer<PolygonCrossing> crossingComparer
        ) {
            Contract.Requires(currentCrossing != null);
            Contract.Requires(crossingsOnRing != null);
            Contract.Requires(Contract.ForAll(crossingsOnRing, x => x != null));
            Contract.Requires(ring != null);
            Contract.Requires(getLocation != null);
            Contract.Requires(crossingComparer != null);

            var currentLocation = getLocation(currentCrossing);
            var segmentCount = ring.SegmentCount;
            Contract.Assume(currentLocation.SegmentIndex < segmentCount);
            var previousSegmentIndex = IterationUtils.RetreatLoopingIndex(currentLocation.SegmentIndex, segmentCount);
            var previousCrossing = FindPreviousCrossingNotEqual(currentCrossing, crossingsOnRing, crossingComparer);
            if (null == previousCrossing) {
                return ring[
                    currentLocation.SegmentRatio == 0
                    ? previousSegmentIndex
                    : currentLocation.SegmentIndex
                ];
            }
            var previousCrossingLocation = getLocation(previousCrossing);
            Contract.Assume(previousCrossingLocation != null);
            if (currentLocation.SegmentRatio == 0) {
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
        public List<PolygonCrossing> FindPointCrossings(Polygon2 a, Polygon2 b) {
            Contract.Ensures(Contract.Result<List<PolygonCrossing>>() != null);
            if (null == a || null == b)
                return new List<PolygonCrossing>(0);
            var crossingGenerator = new PolygonPointCrossingGenerator(a, b);
            var allCrossings = crossingGenerator.GenerateCrossings();
            return allCrossings;
        }

        private PolygonCrossingsAlgorithmKernel CreateIntersectionKernel(Polygon2 a, Polygon2 b) {
            Contract.Requires(a != null);
            Contract.Requires(b != null);
            Contract.Ensures(Contract.Result<PolygonCrossingsAlgorithmKernel>() != null);
            var allCrossings = FindPointCrossings(a,b);
            return CreateIntersectionKernelFromCrossings(allCrossings, a, b);
        }

    }

}
