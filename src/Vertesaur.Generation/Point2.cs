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
using Vertesaur.Contracts;

namespace Vertesaur.Generation
{
	/// <summary>
	/// A point in 2D space.
	/// </summary>
	/// <typeparam name="TValue">The coordinate value data type.</typeparam>
	/// <remarks>
	/// The type used as the generic argument for a coordinate value should be immutable.
	/// </remarks>
	public struct Point2<TValue> :
		IPoint2<TValue>,
		IEquatable<Point2<TValue>>
	{
		/// <summary>
		/// The x-coordinate of this point.
		/// </summary>
		public readonly TValue X;
		/// <summary>
		/// The y-coordinate of this point.
		/// </summary>
		public readonly TValue Y;

		/// <summary>
		/// Creates a point with the given <paramref name="x"/> and <paramref name="y"/> coordinates.
		/// </summary>
		/// <param name="x">A coordinate.</param>
		/// <param name="y">A coordinate.</param>
		public Point2(TValue x, TValue y) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if (x == null) throw new ArgumentNullException("x");
			if (y == null) throw new ArgumentNullException("y");
			Contract.EndContractBlock();
			X = x;
			Y = y;
			Contract.Assume(null != X);
			Contract.Assume(null != Y);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}
		/// <summary>
		/// Creates a point with the same coordinates as the given point.
		/// </summary>
		/// <param name="p">A coordinate pair.</param>
		public Point2(ICoordinatePair<TValue> p) {
			if (null == p) throw new ArgumentNullException("p");
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if (p.X == null || p.Y == null) throw new ArgumentException("Null coordinate values are not allowed.", "v");
			Contract.EndContractBlock();
			X = p.X;
			Y = p.Y;
			Contract.Assume(null != X);
			Contract.Assume(null != Y);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		TValue ICoordinatePair<TValue>.X {
			get { return X; }
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		TValue ICoordinatePair<TValue>.Y {
			get { return Y; }
		}

		/// <inheritdoc/>
		public override int GetHashCode() {
			return X.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString() {
			return String.Concat(X, ' ', Y);
		}

		/// <inheritdoc/>
		public bool Equals(Point2<TValue> other) {
			return X.Equals(other.X) && Y.Equals(other.Y);
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) {
			return obj is Point2<TValue> && Equals((Point2<TValue>)obj);
		}
	}
}
