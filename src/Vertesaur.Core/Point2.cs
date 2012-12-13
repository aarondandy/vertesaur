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

namespace Vertesaur {

// ReSharper disable CompareOfFloatsByEqualityOperator

	/// <summary>
	/// A point in 2D space.
	/// </summary>
	public struct Point2 :
		IPoint2<double>,
		IEquatable<Point2>,
		IComparable<Point2>,
		IEquatable<ICoordinatePair<double>>,
		ISpatiallyRelatable<Point2>,
		IHasDistance<Point2, double>,
		IHasMbr<Mbr, double>
	{

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">A point.</param>
		/// <param name="b">A point.</param>
		/// <returns>True if both points have the same component values.</returns>
		public static bool operator ==(Point2 a, Point2 b) {
			return a.Equals(b);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">A point.</param>
		/// <param name="b">A point.</param>
		/// <returns>True if both points do not have the same component values.</returns>
		public static bool operator !=(Point2 a, Point2 b) {
			return !a.Equals(b);
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A point.</param>
		/// <returns>The result.</returns>
		public static Point2 operator +(Vector2 leftHandSide, Point2 rightHandSide) {
			return leftHandSide.Add(rightHandSide);
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="leftHandSide">A point.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Point2 operator +(Point2 leftHandSide, Vector2 rightHandSide) {
			return leftHandSide.Add(rightHandSide);
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A point.</param>
		/// <returns>The result.</returns>
		public static Point2 operator -(Vector2 leftHandSide, Point2 rightHandSide) {
			return leftHandSide.Difference(rightHandSide);
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="leftHandSide">A point.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Point2 operator -(Point2 leftHandSide, Vector2 rightHandSide) {
			return leftHandSide.Difference(rightHandSide);
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="leftHandSide">A point.</param>
		/// <param name="rightHandSide">A point.</param>
		/// <returns>The vector difference between two points.</returns>
		public static Vector2 operator -(Point2 leftHandSide, Point2 rightHandSide) {
			return leftHandSide.Difference(rightHandSide);
		}

		/// <summary>
		/// A point with all components set to zero.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Point2 Zero = new Point2(0, 0);
		/// <summary>
		/// An invalid point.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Point2 Invalid = new Point2(Double.NaN, Double.NaN);

		/// <summary>
		/// The x-coordinate of this point.
		/// </summary>
		public readonly double X;
		/// <summary>
		/// The y-coordinate of this point.
		/// </summary>
		public readonly double Y;

		/// <summary>
		/// Creates a point with the given <paramref name="x"/> and <paramref name="y"/> coordinates.
		/// </summary>
		/// <param name="x">A coordinate.</param>
		/// <param name="y">A coordinate.</param>
		public Point2(double x, double y) {
			X = x;
			Y = y;
		}
		/// <summary>
		/// Creates a point with the same coordinates as the given <paramref name="point"/>.
		/// </summary>
		/// <param name="point">A coordinate pair.</param>
		public Point2(ICoordinatePair<double> point) {
			if(null == point) throw new ArgumentNullException("point");
			Contract.EndContractBlock();
			X = point.X;
			Y = point.Y;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinatePair<double>.X { get { return X; } }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinatePair<double>.Y { get { return Y; } }

		/// <inheritdoc/>
		public bool Equals(Point2 other) {
			return X == other.X && Y == other.Y;
		}

		/// <inheritdoc/>
		public bool Equals(ICoordinatePair<double> other) {
			return !ReferenceEquals(null, other)
				&& X == other.X && Y == other.Y;
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) {
			return null != obj && (
				(obj is Point2 && Equals((Point2)obj))
				||
				Equals(obj as ICoordinatePair<double>)
			);
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
		public int CompareTo(Point2 other) {
			var c = X.CompareTo(other.X);
			return 0 == c ? Y.CompareTo(other.Y) : c;
		}

		/// <summary>
		/// Calculates the distance between this and the given <paramref name="point"/>.
		/// </summary>
		/// <param name="point">The point to calculate distance to.</param>
		/// <returns>The distance.</returns>
		[Pure] public double Distance(Point2 point) {
			var dx = point.X - X;
			var dy = point.Y - Y;
			return Math.Sqrt(
				(dx * dx)
				+
				(dy * dy)
			);
		}

		/// <summary>
		/// Calculates the squared distance between this and the given <paramref name="point"/>.
		/// </summary>
		/// <param name="point">The point to calculate squared distance to.</param>
		/// <returns>The squared distance.</returns>
		[Pure] public double DistanceSquared(Point2 point) {
			var dx = point.X - X;
			var dy = point.Y - Y;
			return (
				(dx * dx)
				+
				(dy * dy)
			);
		}

		/// <summary>
		/// Adds the given <paramref name="delta"/> to this point.
		/// </summary>
		/// <param name="delta">An offset vector.</param>
		/// <returns>An offset point.</returns>
		public Point2 Add(Vector2 delta) {
			return new Point2(X + delta.X, Y + delta.Y);
		}

		/// <summary>
		/// Finds the vector difference between this point and another.
		/// </summary>
		/// <param name="b">The other point.</param>
		/// <returns>The vector difference.</returns>
		public Vector2 Difference(Point2 b) {
			return new Vector2(X - b.X, Y - b.Y);
		}

		/// <summary>
		/// Creates a point offset from this point by a given vector.
		/// </summary>
		/// <param name="b">The vector.</param>
		/// <returns>The offset point.</returns>
		public Point2 Difference(Vector2 b) {
			return new Point2(X - b.X, Y - b.Y);
		}

		/// <inheritdoc/>
		public Mbr GetMbr() {
			Contract.Ensures(Contract.Result<Mbr>() != null);
			return new Mbr(this);
		}

		/// <summary>
		/// Determines if the point is valid.
		/// </summary>
		[Pure]
		public bool IsValid {
			get { return !Double.IsNaN(X) && !Double.IsNaN(Y); }
		}

		/// <inheritdoc/>
		bool IRelatableIntersects<Point2>.Intersects(Point2 other) {
			return Equals(other);
		}

		/// <inheritdoc/>
		bool IRelatableDisjoint<Point2>.Disjoint(Point2 other) {
			return !Equals(other);
		}

		/// <inheritdoc/>
		bool IRelatableTouches<Point2>.Touches(Point2 other) {
			return false;
		}

		/// <inheritdoc/>
		bool IRelatableCrosses<Point2>.Crosses(Point2 other) {
			return false;
		}

		/// <inheritdoc/>
		bool IRelatableWithin<Point2>.Within(Point2 other) {
			return Equals(other);
		}

		/// <inheritdoc/>
		bool IRelatableContains<Point2>.Contains(Point2 other) {
			return Equals(other);
		}

		/// <inheritdoc/>
		bool IRelatableOverlaps<Point2>.Overlaps(Point2 other) {
			return false;
		}

		/// <inheritdoc/>
		bool ISpatiallyEquatable<Point2>.SpatiallyEqual(Point2 other) {
			return Equals(other);
		}
	}

// ReSharper restore CompareOfFloatsByEqualityOperator

}
