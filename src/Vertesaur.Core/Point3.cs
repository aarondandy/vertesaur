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
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace Vertesaur {

// ReSharper disable CompareOfFloatsByEqualityOperator

	/// <summary>
	/// A point in 3D space.
	/// </summary>
	public struct Point3 :
		IPoint3<double>,
		IEquatable<Point3>,
		IComparable<Point3>,
		IEquatable<ICoordinateTriple<double>>,
		ISpatiallyRelatable<Point3>,
		IHasDistance<Point3, double>
	{
	/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">A point.</param>
		/// <param name="b">A point.</param>
		/// <returns>True if both points have the same component values.</returns>
		public static bool operator ==(Point3 a, Point3 b) {
			return a.Equals(b);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">A point.</param>
		/// <param name="b">A point.</param>
		/// <returns>True if both points do not have the same component values.</returns>
		public static bool operator !=(Point3 a, Point3 b) {
			return !a.Equals(b);
		}

		/// <summary>
		/// A point with all components set to zero.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Point3 Zero = new Point3(0, 0, 0);
		/// <summary>
		/// An invalid point.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Point3 Invalid = new Point3(Double.NaN, Double.NaN, Double.NaN);

		/// <summary>
		/// The x-coordinate of this point.
		/// </summary>
		public readonly double X;
		/// <summary>
		/// The y-coordinate of this point.
		/// </summary>
		public readonly double Y;
		/// <summary>
		/// The z-coordinate of this point.
		/// </summary>
		public readonly double Z;

		/// <summary>
		/// Creates a point with the given <paramref name="x"/> and <paramref name="y"/> coordinates.
		/// </summary>
		/// <param name="x">A coordinate.</param>
		/// <param name="y">A coordinate.</param>
		/// <param name="z">A coordinate.</param>
		public Point3(double x, double y, double z) {
			X = x;
			Y = y;
			Z = z;
		}
		/// <summary>
		/// Creates a point with the same coordinates as the given <paramref name="point"/>.
		/// </summary>
		/// <param name="point">A coordinate pair.</param>
		public Point3([NotNull] ICoordinateTriple<double> point) {
			if(null == point) throw new ArgumentNullException("point");
			Contract.EndContractBlock();
			X = point.X;
			Y = point.Y;
			Z = point.Z;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinateTriple<double>.X { get { return X; } }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinateTriple<double>.Y { get { return Y; } }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinateTriple<double>.Z { get { return Z; } }

		/// <inheritdoc/>
		public bool Equals(Point3 other) {
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		/// <inheritdoc/>
		[ContractAnnotation("null=>false")]
		public bool Equals([CanBeNull] ICoordinateTriple<double> other) {
			return !ReferenceEquals(null, other)
				&& X == other.X && Y == other.Y && Z == other.Z;
		}

		/// <inheritdoc/>
		[ContractAnnotation("null=>false")]
		public override bool Equals([CanBeNull] object obj) {
			return null != obj && (
				(obj is Point3 && Equals((Point3)obj))
				||
				Equals(obj as ICoordinateTriple<double>)
			);
		}

		/// <inheritdoc/>
		public override int GetHashCode() {
			return X.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString() {
			return String.Concat(X, ' ', Y, ' ', Z);
		}

		/// <inheritdoc/>
		public int CompareTo(Point3 other) {
			var c = X.CompareTo(other.X);
			return 0 != c
				? c
				: (0 == (c = Y.CompareTo(other.Y))
					? Z.CompareTo(other.Z)
					: c
				)
			;
		}

		/// <summary>
		/// Calculates the distance between this and the given <paramref name="point"/>.
		/// </summary>
		/// <param name="point">The point to calculate distance to.</param>
		/// <returns>The distance.</returns>
		[Pure] public double Distance(Point3 point) {
			var dx = point.X - X;
			var dy = point.Y - Y;
			var dz = point.Z - Z;
			return Math.Sqrt(
				(dx * dx)
				+
				(dy * dy)
				+
				(dz * dz)
			);
		}

		/// <summary>
		/// Calculates the squared distance between this and the given <paramref name="point"/>.
		/// </summary>
		/// <param name="point">The point to calculate squared distance to.</param>
		/// <returns>The squared distance.</returns>
		[Pure] public double DistanceSquared(Point3 point) {
			var dx = point.X - X;
			var dy = point.Y - Y;
			var dz = point.Z - Z;
			return (
				(dx * dx)
				+
				(dy * dy)
				+
				(dz * dz)
			);
		}

		/// <summary>
		/// Adds the given <paramref name="delta"/> to this point.
		/// </summary>
		/// <param name="delta">An offset vector.</param>
		/// <returns>An offset point.</returns>
		public Point3 Add(Vector3 delta) {
			return new Point3(X + delta.X, Y + delta.Y, Z + delta.Z);
		}

		/// <summary>
		/// Finds the vector difference between this point and another.
		/// </summary>
		/// <param name="b">The other point.</param>
		/// <returns>The vector difference.</returns>
		public Vector3 Difference(Point3 b) {
			return new Vector3(X - b.X, Y - b.Y, Z - b.Z);
		}

		/// <inheritdoc/>
		bool IRelatableIntersects<Point3>.Intersects(Point3 other) {
			return Equals(other);
		}

		/// <inheritdoc/>
		bool IRelatableDisjoint<Point3>.Disjoint(Point3 other) {
			return !Equals(other);
		}

		/// <inheritdoc/>
		bool IRelatableTouches<Point3>.Touches(Point3 other) {
			return false;
		}

		/// <inheritdoc/>
		bool IRelatableCrosses<Point3>.Crosses(Point3 other) {
			return false;
		}

		/// <inheritdoc/>
		bool IRelatableWithin<Point3>.Within(Point3 other) {
			return Equals(other);
		}

		/// <inheritdoc/>
		bool IRelatableContains<Point3>.Contains(Point3 other) {
			return Equals(other);
		}

		/// <inheritdoc/>
		bool IRelatableOverlaps<Point3>.Overlaps(Point3 other) {
			return false;
		}

		/// <inheritdoc/>
		bool ISpatiallyEquatable<Point3>.SpatiallyEqual(Point3 other) {
			return Equals(other);
		}
	}

// ReSharper restore CompareOfFloatsByEqualityOperator

}