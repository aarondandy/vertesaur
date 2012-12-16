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
	/// An operation that will find the geometric difference of one polygon from another.
	/// </summary>
	public class PolygonDifferenceOperation
	{

		private static readonly PolygonBinaryOperationOptions DefaultInverseRightIntersectionOptions;
		private static readonly PolygonIntersectionOperation DefaultInverseRightIntersectionOperation;

		static PolygonDifferenceOperation() {
			DefaultInverseRightIntersectionOptions = new PolygonBinaryOperationOptions {
				InvertLeftHandSide = false,
				InvertRightHandSide = true,
				InvertResult = false
			};
			DefaultInverseRightIntersectionOperation = new PolygonIntersectionOperation(DefaultInverseRightIntersectionOptions);
		}

		private readonly PolygonIntersectionOperation _rightInverseIntersectionOperation;

		/// <summary>
		/// Creates a default polygon difference operation.
		/// </summary>
		public PolygonDifferenceOperation() : this(null) { }

		internal PolygonDifferenceOperation(PolygonIntersectionOperation inverseRightOperation) {
			_rightInverseIntersectionOperation = inverseRightOperation ?? DefaultInverseRightIntersectionOperation;
		}

		/// <summary>
		/// Calculates the resulting difference of polygon <paramref name="b"/> subtracted from polygon <paramref name="a"/>.
		/// </summary>
		/// <param name="a">The polygon to be subtracted from.</param>
		/// <param name="b">The polygon used to subtract from a.</param>
		/// <returns>The difference resulting from subtracting <paramref name="b"/> from <paramref name="a"/>.</returns>
		public IPlanarGeometry Difference(Polygon2 a, Polygon2 b) {
			var result = _rightInverseIntersectionOperation.Intersect(a, b);
			return result;
		}

	}
}
