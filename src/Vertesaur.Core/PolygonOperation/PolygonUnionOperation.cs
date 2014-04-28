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

using System.Diagnostics.Contracts;

namespace Vertesaur.PolygonOperation
{

    /// <summary>
    /// An operation that will find the geometric union of two polygons.
    /// </summary>
    public class PolygonUnionOperation
    {

        private static readonly PolygonBinaryOperationOptions DefaultInverseIntersectionOptions;
        private static readonly PolygonIntersectionOperation DefaultInverseIntersectionOperation;

        static PolygonUnionOperation() {
            DefaultInverseIntersectionOptions = new PolygonBinaryOperationOptions {
                InvertLeftHandSide = true,
                InvertRightHandSide = true,
                InvertResult = true
            };
            DefaultInverseIntersectionOperation = new PolygonIntersectionOperation(DefaultInverseIntersectionOptions);
        }

        /// <summary>
        /// An inverted intersection operation that inverts both arguments as well as the result.
        /// </summary>
        protected PolygonIntersectionOperation InverseIntersectionOperation { get; private set; }

        /// <summary>
        /// Creates a new default polygon union operation.
        /// </summary>
        public PolygonUnionOperation() : this(null) { }

        internal PolygonUnionOperation(PolygonIntersectionOperation inverseIntersectionOperation) {
            InverseIntersectionOperation = inverseIntersectionOperation ?? DefaultInverseIntersectionOperation;
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants(){
            Contract.Invariant(InverseIntersectionOperation != null);
        }

        /// <summary>
        /// Calculates the resulting union of two polygon geometries.
        /// </summary>
        /// <param name="a">A polygon.</param>
        /// <param name="b">A polygon.</param>
        /// <returns>The union of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public IPlanarGeometry Union(Polygon2 a, Polygon2 b) {
            Contract.Ensures(
                a == null && b == null
                ? Contract.Result<IPlanarGeometry>() == null
                : Contract.Result<IPlanarGeometry>() != null
            );
            if (null == a)
                return b == null ? null : b;
            if (null == b)
                return a;

            var result = InverseIntersectionOperation.Intersect(a, b) as Polygon2;
            return result;
        }

    }
}
