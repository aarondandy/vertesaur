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
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur.PolygonOperation
{

    /// <summary>
    /// A single point of intersection between two polygons.
    /// </summary>
    public sealed class PolygonCrossing
    {
        /// <summary>
        /// Classifications of various types of crossings that can occur between the edges and points of a polygon.
        /// </summary>
        [Flags]
        public enum CrossingType : byte
        {
            /// <summary>
            /// No crossing data.
            /// </summary>
            None = 0,

            /// <summary>
            /// The next vector is to the right.
            /// </summary>
            NextRight = 1,
            /// <summary>
            /// The next vector is to the left.
            /// </summary>
            NextLeft = 2,
            /// <summary>
            /// The next vector is parallel.
            /// </summary>
            NextParallel = NextRight | NextLeft,

            /// <summary>
            /// The previous vector is to the right.
            /// </summary>
            PriorRight = 4,
            /// <summary>
            /// The previous vector is to the left.
            /// </summary>
            PriorLeft = 8,
            /// <summary>
            /// The previous vector is parallel.
            /// </summary>
            PriorParallel = PriorRight | PriorLeft,

            /// <summary>
            /// The crossing is going with the other direction.
            /// </summary>
            With = 16,
            /// <summary>
            /// The crossing is going against the other direction.
            /// </summary>
            Against = 32,
            /// <summary>
            /// The crossing is perpendicular to the other direction.
            /// </summary>
            Perpendicular = With | Against,

            /// <summary>
            /// The crossing, crosses to the right.
            /// </summary>
            CrossToRight = NextRight | PriorLeft,
            /// <summary>
            /// The crossing, crosses to the left.
            /// </summary>
            CrossToLeft = NextLeft | PriorRight,
            /// <summary>
            /// The crossing is a kiss from the right.
            /// </summary>
            KissRight = NextRight | PriorRight,
            /// <summary>
            /// The crossing is a kiss from the left.
            /// </summary>
            KissLeft = NextLeft | PriorLeft,
            /// <summary>
            /// The crossing is a divergence to the right.
            /// </summary>
            DivergeRight = NextRight | PriorParallel,
            /// <summary>
            /// The crossing is a divergence to the left.
            /// </summary>
            DivergeLeft = NextLeft | PriorParallel,
            /// <summary>
            /// The crossing is a convergence to the right.
            /// </summary>
            ConvergeRight = NextParallel | PriorRight,
            /// <summary>
            /// The crossing is a convergence to the left.
            /// </summary>
            ConvergeLeft = NextParallel | PriorLeft,
            /// <summary>
            /// The crossing is parallel to the related geometry.
            /// </summary>
            Parallel = NextParallel | PriorParallel

        }

        /// <summary>
        /// Determines the crossing type given the vectors the define the crossing.
        /// </summary>
        /// <param name="vectorANext">The vector on path A leaving the point of intersection.</param>
        /// <param name="vectorAPrior">The negated vector on path A entering the point of intersection.</param>
        /// <param name="vectorBNext">The vector on path B leaving the point of intersection.</param>
        /// <param name="vectorBPrior">The negated vector on path B entering the point of intersection.</param>
        /// <returns>A crossing type classification.</returns>
        [Pure]
        public static CrossingType DetermineCrossingType(Vector2 vectorANext, Vector2 vectorAPrior, Vector2 vectorBNext, Vector2 vectorBPrior) {
            var crossANextBNext = vectorANext.PerpendicularDot(ref vectorBNext);
            var crossANextBPrior = vectorANext.PerpendicularDot(ref vectorBPrior);
            var crossAPriorBNext = vectorAPrior.PerpendicularDot(ref vectorBNext);
            var crossAPriorBPrior = vectorAPrior.PerpendicularDot(ref vectorBPrior);
            //var dotANextBNext = vectorANext.Dot(vectorBNext);
            //var dotANextBPrior = vectorANext.Dot(vectorBPrior);
            //var dotAPriorBNext = vectorAPrior.Dot(vectorBNext);
            //var dotAPriorBPrior = vectorAPrior.Dot(vectorBPrior);

            // get parallel out of the way, so we don’t have to worry about it
            // ReSharper disable CompareOfFloatsByEqualityOperator
            var parallelANextBNext = vectorANext.Equals(vectorBNext) || (crossANextBNext == 0 && vectorANext.Dot(vectorBNext) > 0);
            var parallelAPriorBNext = vectorAPrior.Equals(vectorBNext) || (crossAPriorBNext == 0 && vectorAPrior.Dot(vectorBNext) > 0);
            var parallelANextBPrior = vectorANext.Equals(vectorBPrior) || (crossANextBPrior == 0 && vectorANext.Dot(vectorBPrior) > 0);
            var parallelAPriorBPrior = vectorAPrior.Equals(vectorBPrior) || (crossAPriorBPrior == 0 && vectorAPrior.Dot(vectorBPrior) > 0);
            // ReSharper restore CompareOfFloatsByEqualityOperator

            // parallel cases
            if (parallelANextBNext && parallelAPriorBPrior)
                return CrossingType.Parallel | CrossingType.With;
            if (parallelANextBPrior && parallelAPriorBNext)
                return CrossingType.Parallel | CrossingType.Against;

            CrossingType sideBNext;
            CrossingType sideBPrior;
            var crossANextAPrior = vectorANext.PerpendicularDot(vectorAPrior);
            if (crossANextAPrior < 0) {
                // A kinks to the right
                sideBNext = (crossANextBNext <= 0 && crossAPriorBNext >= 0) ? CrossingType.NextRight : CrossingType.NextLeft;
                sideBPrior = (crossANextBPrior <= 0 && crossAPriorBPrior >= 0) ? CrossingType.PriorRight : CrossingType.PriorLeft;
            }
            else if (crossANextAPrior > 0) {
                // A kinks to the left
                sideBNext = (crossANextBNext >= 0 && crossAPriorBNext <= 0) ? CrossingType.NextLeft : CrossingType.NextRight;
                sideBPrior = (crossANextBPrior >= 0 && crossAPriorBPrior <= 0) ? CrossingType.PriorLeft : CrossingType.PriorRight;
            }
            else {
                // A is straight or folds in on itself
                sideBNext = crossANextBNext < 0 ? CrossingType.NextRight : CrossingType.NextLeft;
                sideBPrior = crossANextBPrior < 0 ? CrossingType.PriorRight : CrossingType.PriorLeft;
                if (vectorANext == vectorAPrior || vectorANext.Dot(vectorAPrior) > 0)
                    throw new NotImplementedException("What if they are the same vectors?");
            }

            // diverge cases
            if (parallelAPriorBPrior) {
                return CrossingType.PriorParallel | CrossingType.With | sideBNext;
            }
            if (parallelANextBPrior) {
                return CrossingType.PriorParallel | CrossingType.Against | sideBNext;
            }

            // converge cases
            if (parallelANextBNext) {
                return CrossingType.NextParallel | CrossingType.With | sideBPrior;
            }
            if (parallelAPriorBNext) {
                return CrossingType.NextParallel | CrossingType.Against | sideBPrior;
            }

            // kiss or cross
            return sideBNext | sideBPrior; // TODO: with, against, perpendicular
        }

        internal sealed class LocationAComparer : IComparer<PolygonCrossing>
        {

            public static readonly LocationAComparer Default = new LocationAComparer();

            [Pure]
            public int Compare(PolygonCrossing x, PolygonCrossing y) {
                if (ReferenceEquals(x, y))
                    return 0;
                int compareResult;
                return (compareResult = x.LocationA.CompareTo(y.LocationA)) == 0
                    ? x.LocationB.CompareTo(y.LocationB)
                    : compareResult;
            }

            [Pure]
            public static int CompareNonNull(PolygonCrossing x, PolygonCrossing y) {
                Contract.Requires(x != null);
                Contract.Requires(y != null);
                if (ReferenceEquals(x, y))
                    return 0;
                int compareResult;
                return (compareResult = x.LocationA.CompareTo(y.LocationA)) == 0
                    ? x.LocationB.CompareTo(y.LocationB)
                    : compareResult;
            }

        }

        internal sealed class LocationBComparer : IComparer<PolygonCrossing>
        {

            public static readonly LocationBComparer Default = new LocationBComparer();

            [Pure]
            public int Compare(PolygonCrossing x, PolygonCrossing y) {
                if (ReferenceEquals(x, y))
                    return 0;
                int compareResult;
                return (compareResult = x.LocationB.CompareTo(y.LocationB)) == 0
                    ? x.LocationA.CompareTo(y.LocationA)
                    : compareResult;
            }

            [Pure]
            public static int CompareNonNull(PolygonCrossing x, PolygonCrossing y) {
                Contract.Requires(x != null);
                Contract.Requires(y != null);
                if (ReferenceEquals(x, y))
                    return 0;
                int compareResult;
                return (compareResult = x.LocationB.CompareTo(y.LocationB)) == 0
                    ? x.LocationA.CompareTo(y.LocationA)
                    : compareResult;
            }
        }

        /// <summary>
        /// The calculated location of the intersection.
        /// </summary>
        public Point2 Point { get; private set; }
        /// <summary>
        /// The crossing location on polygon A.
        /// </summary>
        public PolygonBoundaryLocation LocationA { get; private set; }
        /// <summary>
        /// The crossing location on polygon A.
        /// </summary>
        public PolygonBoundaryLocation LocationB { get; private set; }
        /// <summary>
        /// The crossing type.
        /// </summary>
        public CrossingType CrossType { get; set; }

        /// <summary>
        /// Creates a new polygon crossing defined by a point on the respective location on each polygon boundary.
        /// </summary>
        /// <param name="p">The calculated point of intersection.</param>
        /// <param name="locationA">The location on the first polygon boundary.</param>
        /// <param name="locationB">The location on the second polygon boundary.</param>
        public PolygonCrossing(Point2 p, PolygonBoundaryLocation locationA, PolygonBoundaryLocation locationB) {
            if (null == locationA) throw new ArgumentNullException("locationA");
            if (null == locationB) throw new ArgumentNullException("locationB");
            Contract.EndContractBlock();
            Point = p;
            LocationA = locationA;
            LocationB = locationB;
        }

        /// <inheritdoc/>
        public override string ToString() {
            return string.Format("P:{0} A:{1} B:{2}", Point, LocationA, LocationB);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants() {
            Contract.Invariant(LocationA != null);
            Contract.Invariant(LocationB != null);
        }

    }
}
