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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Vertesaur.Contracts;
using Vertesaur.Generation.GenericOperations;

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
		IEquatable<Point2<TValue>>,
		ISpatiallyRelatable<Point2<TValue>>,
		IHasDistance<Point2<TValue>,TValue>
	{

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">A point.</param>
		/// <param name="b">A point.</param>
		/// <returns>True if both points have the same component values.</returns>
		public static bool operator ==(Point2<TValue> a, Point2<TValue> b) {
			return a.Equals(b);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">A point.</param>
		/// <param name="b">A point.</param>
		/// <returns>True if both points do not have the same component values.</returns>
		public static bool operator !=(Point2<TValue> a, Point2<TValue> b) {
			return !a.Equals(b);
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A point.</param>
		/// <returns>The result.</returns>
		public static Point2<TValue> operator +(Vector2<TValue> leftHandSide, Point2<TValue> rightHandSide) {
			return leftHandSide.Add(rightHandSide);
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="leftHandSide">A point.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Point2<TValue> operator +(Point2<TValue> leftHandSide, Vector2<TValue> rightHandSide) {
			return leftHandSide.Add(rightHandSide);
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A point.</param>
		/// <returns>The result.</returns>
		public static Point2<TValue> operator -(Vector2<TValue> leftHandSide, Point2<TValue> rightHandSide) {
			return leftHandSide.Difference(rightHandSide);
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="leftHandSide">A point.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Point2<TValue> operator -(Point2<TValue> leftHandSide, Vector2<TValue> rightHandSide) {
			return leftHandSide.Difference(rightHandSide);
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="leftHandSide">A point.</param>
		/// <param name="rightHandSide">A point.</param>
		/// <returns>The vector difference between two points.</returns>
		public static Vector2<TValue> operator -(Point2<TValue> leftHandSide, Point2<TValue> rightHandSide) {
			return leftHandSide.Difference(rightHandSide);
		}

		/// <summary>
		/// Convert a generically typed point to a double typed point.
		/// </summary>
		/// <param name="value">The point to cast and convert.</param>
		/// <returns>The resulting double point representation of the casted generic point.</returns>
		public static explicit operator Point2(Point2<TValue> value) {
			var x = BasicOperations<TValue>.Default.ToDouble(value.X);
			var y = BasicOperations<TValue>.Default.ToDouble(value.Y);
			return new Point2(x, y);
		}

		/// <summary>
		/// Convert a double typed point to a generically typed point.
		/// </summary>
		/// <param name="value">The point to cast and convert.</param>
		/// <returns>The resulting generic point representation of the casted double point.</returns>
		public static explicit operator Point2<TValue>(Point2 value) {
			return new Point2<TValue>(value);
		}

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
			if (p.X == null || p.Y == null) throw new ArgumentException("Null coordinate values are not allowed.", "p");
			Contract.EndContractBlock();
			X = p.X;
			Y = p.Y;
			Contract.Assume(null != X);
			Contract.Assume(null != Y);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		/// <summary>
		/// Constructs a new point from a double typed point, converting the coordinate values.
		/// </summary>
		/// <param name="p">The point to convert and clone from.</param>
		/// <exception cref="System.ArgumentException">Coordinate type conversion fails.</exception>
		public Point2(Point2 p) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			X = BasicOperations<TValue>.Default.FromDouble(p.X);
			Y = BasicOperations<TValue>.Default.FromDouble(p.Y);
			if(null == X || null == Y) throw new ArgumentException("Converted to a null coordinate.","p");
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

		/// <summary>
		/// Calculates the distance between this and the given <paramref name="point"/>.
		/// </summary>
		/// <param name="point">The point to calculate distance to.</param>
		/// <returns>The distance.</returns>
		public TValue Distance(Point2<TValue> point) {
			return VectorOperations<TValue>.Default.Distance2D(X, Y, point.X, point.Y);
		}

		/// <summary>
		/// Calculates the squared distance between this and the given <paramref name="point"/>.
		/// </summary>
		/// <param name="point">The point to calculate squared distance to.</param>
		/// <returns>The squared distance.</returns>
		public TValue DistanceSquared(Point2<TValue> point) {
			return VectorOperations<TValue>.Default.DistanceSquared2D(X, Y, point.X, point.Y);
		}

		/// <summary>
		/// Adds the given <paramref name="delta"/> to this point.
		/// </summary>
		/// <param name="delta">An offset vector.</param>
		/// <returns>An offset point.</returns>
		public Point2<TValue> Add(Vector2<TValue> delta) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			var x = BasicOperations<TValue>.Default.Add(X, delta.X);
			var y = BasicOperations<TValue>.Default.Add(Y, delta.Y);
			Contract.Assume(null != x);
			Contract.Assume(null != y);
			return new Point2<TValue>(x, y);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		/// <summary>
		/// Finds the vector difference between this point and another.
		/// </summary>
		/// <param name="b">The other point.</param>
		/// <returns>The vector difference.</returns>
		public Vector2<TValue> Difference(Point2<TValue> b) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			var x = BasicOperations<TValue>.Default.Subtract(X, b.X);
			var y = BasicOperations<TValue>.Default.Subtract(Y, b.Y);
			Contract.Assume(null != x);
			Contract.Assume(null != y);
			return new Vector2<TValue>(x, y);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		/// <summary>
		/// Creates a point offset from this point by a given vector.
		/// </summary>
		/// <param name="b">The vector.</param>
		/// <returns>The offset point.</returns>
		public Point2<TValue> Difference(Vector2<TValue> b) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			var x = BasicOperations<TValue>.Default.Subtract(X, b.X);
			var y = BasicOperations<TValue>.Default.Subtract(Y, b.Y);
			Contract.Assume(null != x);
			Contract.Assume(null != y);
			return new Point2<TValue>(x, y);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}


		bool ISpatiallyEquatable<Point2<TValue>>.SpatiallyEqual(Point2<TValue> other) {
			return Equals(other);
		}

		bool IRelatableDisjoint<Point2<TValue>>.Disjoint(Point2<TValue> other) {
			return !Equals(other);
		}

		bool IRelatableIntersects<Point2<TValue>>.Intersects(Point2<TValue> other) {
			return Equals(other);
		}

		bool IRelatableTouches<Point2<TValue>>.Touches(Point2<TValue> other) {
			return false;
		}

		bool IRelatableCrosses<Point2<TValue>>.Crosses(Point2<TValue> other) {
			return false;
		}

		bool IRelatableWithin<Point2<TValue>>.Within(Point2<TValue> other) {
			return Equals(other);
		}

		bool IRelatableContains<Point2<TValue>>.Contains(Point2<TValue> other) {
			return Equals(other);
		}

		bool IRelatableOverlaps<Point2<TValue>>.Overlaps(Point2<TValue> other) {
			return false;
		}
	}
}
