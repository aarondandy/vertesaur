// ===============================================================================
//
// Copyright (c) 2012 Aaron Dandy 
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

using Vertesaur.Contracts;

namespace Vertesaur.PolygonOperation
{
	/// <summary>
	/// An operation that will find the symmetric difference for two polygons.
	/// Also known as exclusive-or (XOR).
	/// </summary>
	public class PolygonXorOperation
	{

		private static readonly PolygonDifferenceOperation DefaultDifferenceOperation;
		private static readonly PolygonUnionOperation DefaultUnionOperation;

		static PolygonXorOperation() {
			DefaultDifferenceOperation = new PolygonDifferenceOperation();
			DefaultUnionOperation = new PolygonUnionOperation();
		}

		private readonly PolygonDifferenceOperation _differenceOperation;
		private readonly PolygonUnionOperation _unionOperation;

		/// <summary>
		/// Constructs a default symmetric difference operation.
		/// </summary>
		public PolygonXorOperation()
			: this(null, null) { }

		internal PolygonXorOperation(PolygonDifferenceOperation differenceOperation, PolygonUnionOperation unionOperation) {
			_differenceOperation = differenceOperation ?? DefaultDifferenceOperation;
			_unionOperation = unionOperation ?? DefaultUnionOperation;
		}

		/// <summary>
		/// Calculates the symmetric difference between two polygons.
		/// </summary>
		/// <param name="a">A polygon.</param>
		/// <param name="b">A polygon.</param>
		/// <returns>The symetric difference of <paramref name="a"/> and <paramref name="b"/>.</returns>
		public IPlanarGeometry Xor(Polygon2 a, Polygon2 b) {
			if (null == a)
				return b;
			if (null == b)
				return a;

			var removedFromA = _differenceOperation.Difference(a, b) as Polygon2;
			var removedFromB = _differenceOperation.Difference(b, a) as Polygon2;
			var unionedLeftovers = _unionOperation.Union(removedFromA, removedFromB);
			return unionedLeftovers;
		}

	}
}
