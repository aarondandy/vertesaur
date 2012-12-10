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
using Vertesaur.Generation.GenericOperations;

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
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">A vector.</param>
		/// <param name="b">A vector.</param>
		/// <returns>True if both vectors have the same component values.</returns>
		public static bool operator ==(Vector2<TValue> a, Vector2<TValue> b) {
			return a.Equals(b);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">A vector.</param>
		/// <param name="b">A vector.</param>
		/// <returns>True if both vectors do not have the same component values.</returns>
		public static bool operator !=(Vector2<TValue> a, Vector2<TValue> b) {
			return !a.Equals(b);
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Vector2<TValue> operator +(Vector2<TValue> leftHandSide, Vector2<TValue> rightHandSide) {
			return leftHandSide.Add(rightHandSide);
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Vector2<TValue> operator -(Vector2<TValue> leftHandSide, Vector2<TValue> rightHandSide) {
			return leftHandSide.Difference(rightHandSide);
		}

		/// <summary>
		/// Implements the operator * as the dot operator.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The dot product.</returns>
		public static TValue operator *(Vector2<TValue> leftHandSide, Vector2<TValue> rightHandSide) {
			return leftHandSide.Dot(rightHandSide);
		}

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
		public TValue GetMagnitude(){
			return VectorOperations<TValue>.Default.Magnitude2D(X, Y);
		}

		/// <inheritdoc/>
		public TValue GetMagnitudeSquared() {
			return VectorOperations<TValue>.Default.SquaredMagnitude2D(X, Y);
		}

		/// <summary>
		/// Calculates a vector resulting from adding the given vector to this vector.
		/// </summary>
		/// <param name="rightHandSide">The vector to add.</param>
		/// <returns>A result of adding this vector with the given vector.</returns>
		public Vector2<TValue> Add(Vector2<TValue> rightHandSide){
			var x = BasicOperations<TValue>.Default.Add(X, rightHandSide.X);
			var y = BasicOperations<TValue>.Default.Add(Y, rightHandSide.Y);
			Contract.Assume(null != x);
			Contract.Assume(null != y);
			return new Vector2<TValue>(x,y);
		}

		/// <summary>
		/// Calculates a vector resulting from subtracting the given vector to this vector.
		/// </summary>
		/// <param name="rightHandSide">The vector to subtract.</param>
		/// <returns>A result of subtracting the given vector from this vector.</returns>
		public Vector2<TValue> Difference(Vector2<TValue> rightHandSide){
			var x = BasicOperations<TValue>.Default.Subtract(X, rightHandSide.X);
			var y = BasicOperations<TValue>.Default.Subtract(Y, rightHandSide.Y);
			Contract.Assume(null != x);
			Contract.Assume(null != y);
			return new Vector2<TValue>(x,y);
		}

		/// <summary>
		/// Calculates the dot product between this vector and another vector.
		/// </summary>
		/// <param name="rightHandSide">Another vector to use for the calculation of the dot product.</param>
		/// <returns>The dot product.</returns>
		public TValue Dot(Vector2<TValue> rightHandSide){
			return VectorOperations<TValue>.Default.DotProduct2D(X, Y, rightHandSide.X, rightHandSide.Y);
		}

	}
}
