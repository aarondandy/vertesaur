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
using JetBrains.Annotations;
using Vertesaur.Contracts;

namespace Vertesaur.Generation
{
	/// <summary>
	/// A vector in 2D space.
	/// </summary>
	/// <typeparam name="TValue">The coordinate value data type.</typeparam>
	/// <remarks>
	/// The type used as the generic argument for a coordinate value should be immutable.
	/// </remarks>
	public struct Vector2<TValue> :
		IVector2<TValue>,
		IEquatable<Vector2<TValue>>
	{

		/// <summary>
		/// The x-coordinate of this vector.
		/// </summary>
		[NotNull] public readonly TValue X;
		/// <summary>
		/// The y-coordinate of this vector.
		/// </summary>
		[NotNull] public readonly TValue Y;

		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		public Vector2(TValue x, TValue y) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if(x == null) throw new ArgumentNullException("x");
			if(y == null) throw new ArgumentNullException("y");
			Contract.EndContractBlock();
			X = x;
			Y = y;
			Contract.Assume(null != X);
			Contract.Assume(null != Y);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="v">The coordinate tuple to copy values from.</param>
		public Vector2(ICoordinatePair<TValue> v) {
			if(null == v) throw new ArgumentNullException("v");
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if (v.X == null || v.Y  == null) throw new ArgumentException("Null coordinate values are not allowed.","v");
			Contract.EndContractBlock();
			X = v.X;
			Y = v.Y;
			Contract.Assume(null != X);
			Contract.Assume(null != Y);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		/// <summary>
		/// The x-coordinate of this point.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		TValue ICoordinatePair<TValue>.X {
			get { return X; }
		}

		/// <summary>
		/// The y-coordinate of this point.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		TValue ICoordinatePair<TValue>.Y {
			get { return Y; }
		}

		/// <inheritdoc/>
		public override int GetHashCode() {
			return X.GetHashCode();
		}

		/// <inheritdoc/>
		public bool Equals(Vector2<TValue> other) {
			return X.Equals(other.X) && Y.Equals(other.Y);
			//return Operations<TValue>.Default.Equals(X, other.X) && Operations<TValue>.Default.Equals(Y, other.Y);
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) {
			return obj is Vector2<TValue> && Equals((Vector2<TValue>)obj);
		}

		/// <inheritdoc/>
		public override string ToString() {
			return String.Concat(X, ' ', Y);
		}

		/// <inheritdoc/>
		public TValue GetMagnitude() {
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public TValue GetMagnitudeSquared() {
			throw new NotImplementedException();
		}

	}
}
