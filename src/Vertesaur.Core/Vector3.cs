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
	/// <summary>
	/// A vector in 2D space.
	/// </summary>
	public struct Vector3 :
		IVector3<double>,
		IEquatable<Vector3>,
		IEquatable<ICoordinateTriple<double>>,
		IComparable<Vector3> {

		// ReSharper disable CompareOfFloatsByEqualityOperator

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">A vector.</param>
		/// <param name="b">A vector.</param>
		/// <returns>True if both vectors have the same component values.</returns>
		public static bool operator ==(Vector3 a, Vector3 b) {
			return a.Equals(b);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">A vector.</param>
		/// <param name="b">A vector.</param>
		/// <returns>True if both vectors do not have the same component values.</returns>
		public static bool operator !=(Vector3 a, Vector3 b) {
			return !a.Equals(b);
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Vector3 operator +(Vector3 leftHandSide, Vector3 rightHandSide) {
			return leftHandSide.Add(rightHandSide);
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Vector3 operator -(Vector3 leftHandSide, Vector3 rightHandSide) {
			return leftHandSide.Difference(rightHandSide);
		}

		/// <summary>
		/// A vector with all components set to zero.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Vector3 Zero = new Vector3(0, 0, 0);
		/// <summary>
		/// A vector with all components set to zero.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Vector3 Invalid = new Vector3(Double.NaN,Double.NaN,Double.NaN);
		/// <summary>
		/// A vector with a magnitude of one and oriented in the direction of the positive X axis.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Vector3 XUnit = new Vector3(1, 0, 0);
		/// <summary>
		/// A vector with a magnitude of one and oriented in the direction of the positive Y axis.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Vector3 YUnit = new Vector3(0, 0, 1);
		/// <summary>
		/// A vector with a magnitude of one and oriented in the direction of the positive Y axis.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Vector3 ZUnit = new Vector3(0, 0, 1);

		/// <summary>
		/// The x-coordinate of this vector.
		/// </summary>
		public readonly double X;
		/// <summary>
		/// The y-coordinate of this vector.
		/// </summary>
		public readonly double Y;
		/// <summary>
		/// The z-coordinate of this vector.
		/// </summary>
		public readonly double Z;

		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <param name="z">The z-coordinate.</param>
		public Vector3(double x, double y, double z) {
			X = x;
			Y = y;
			Z = z;
		}
		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="v">The coordinate tuple to copy values from.</param>
		public Vector3([NotNull] ICoordinateTriple<double> v) {
			if(v == null) throw new ArgumentNullException("v");
			Contract.EndContractBlock();
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinateTriple<double>.X { get { return X; } }
		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinateTriple<double>.Y { get { return Y; } }
		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinateTriple<double>.Z { get { return Z; } }

		/// <inheritdoc/>
		[Pure] public int CompareTo(Vector3 other) {
			var c = X.CompareTo(other.X);
			return 0 != c
				? c
				: (0 == (c = Y.CompareTo(other.Y))
					? Z.CompareTo(other.Z)
					: c
				)
			;
		}

		/// <inheritdoc/>
		[Pure] public bool Equals(Vector3 other) {
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		/// <inheritdoc/>
		[Pure, ContractAnnotation("null=>false")]
		public bool Equals([CanBeNull] ICoordinateTriple<double> other) {
			return !ReferenceEquals(null, other)
				&& X == other.X && Y == other.Y && Z == other.Z;
		}

		/// <inheritdoc/>
		[Pure, ContractAnnotation("null=>false")]
		public override bool Equals([CanBeNull] object obj) {
			return null != obj && (
				(obj is Vector3 && Equals((Vector3)obj))
				||
				Equals(obj as ICoordinateTriple<double>)
			);
		}

		/// <inheritdoc/>
		[Pure] public override int GetHashCode() {
			return X.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString() {
			return String.Concat(X, ' ', Y, ' ', Z);
		}

		/// <summary>
		/// Calculates the magnitude of this vector.
		/// </summary>
		/// <returns>The magnitude.</returns>
		[Pure] public double GetMagnitude() {
			return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
		}

		/// <summary>
		/// Calculates the squared magnitude of this vector.
		/// </summary>
		/// <returns>The squared magnitude.</returns>
		[Pure] public double GetMagnitudeSquared() {
			return (X * X) + (Y * Y) + (Z * Z);
		}

		/// <summary>
		/// Calculates a vector resulting from adding the given vector to this vector.
		/// </summary>
		/// <param name="rightHandSide">The vector to add.</param>
		/// <returns>A result of adding this vector with the given vector.</returns>
		[Pure] public Vector3 Add(Vector3 rightHandSide) {
			return new Vector3(
				X + rightHandSide.X,
				Y + rightHandSide.Y,
				Z + rightHandSide.Z
			);
		}

		/// <summary>
		/// Calculates a new point which is offset from the given point by this vector.
		/// </summary>
		/// <param name="rightHandSide">The point to calculate the offset from.</param>
		/// <returns>The point offset by this vector from the given point.</returns>
		[Pure] public Point3 Add(Point3 rightHandSide) {
			return new Point3(
				X + rightHandSide.X,
				Y + rightHandSide.Y,
				Z + rightHandSide.Z
			);
		}

		/// <summary>
		/// Calculates a vector resulting from subtracting the given vector to this vector.
		/// </summary>
		/// <param name="rightHandSide">The vector to subtract.</param>
		/// <returns>A result of subtracting the given vector from this vector.</returns>
		[Pure] public Vector3 Difference(Vector3 rightHandSide) {
			return new Vector3(
				X - rightHandSide.X,
				Y - rightHandSide.Y,
				Z - rightHandSide.Z
			);
		}

		/// <summary>
		/// Calculates the resulting point from subtracting the values of the given point from this vector.
		/// </summary>
		/// <param name="rightHandSide">The point to subtract from the vector values.</param>
		/// <returns>A point that represents the difference.</returns>
		[Pure] public Point3 Difference(Point3 rightHandSide) {
			return new Point3(
				X - rightHandSide.X,
				Y - rightHandSide.Y,
				Z - rightHandSide.Z
			);
		}

		/// <summary>
		/// Calculates a vector with the same direction but a magnitude of one.
		/// </summary>
		/// <returns>A unit length vector.</returns>
		[Pure] public Vector3 GetNormalized() {
			var m = GetMagnitude();
			return 0 == m ? Zero : new Vector3(X / m, Y / m, Z / m);
		}

		/// <summary>
		/// Calculates the dot product between this vector and another vector.
		/// </summary>
		/// <param name="rightHandSide">Another vector to use for the calculation of the dot product.</param>
		/// <returns>The dot product.</returns>
		[Pure] public double Dot(Vector3 rightHandSide) {
			return (X * rightHandSide.X) + (Y * rightHandSide.Y) + (Z * rightHandSide.Z);
		}

		/// <summary>
		/// Calculates the cross product of this vector and another.
		/// </summary>
		/// <param name="rightHandSide">The other vector to calculate the cross product for.</param>
		/// <returns>The cross product.</returns>
		[Pure] public Vector3 Cross(Vector3 rightHandSide) {
			return new Vector3(
				(Y * rightHandSide.Z) - (Z * rightHandSide.Y),
				(Z * rightHandSide.X) - (X * rightHandSide.Z),
				(X * rightHandSide.Y) - (Y * rightHandSide.X)
			);
		}

		/// <summary>
		/// Calculates a vector oriented in the opposite direction.
		/// </summary>
		/// <returns>A vector with the same component values but different signs.</returns>
		[Pure] public Vector3 GetNegative() {
			return new Vector3(-X, -Y, -Z);
		}

		/// <summary>
		/// Creates a new vector which is scaled from this vector.
		/// </summary>
		/// <param name="factor">The scaling factor.</param>
		/// <returns>A scaled vector.</returns>
		[Pure] public Vector3 GetScaled(double factor) {
			return new Vector3(X * factor, Y * factor, Z * factor);
		}

		// ReSharper restore CompareOfFloatsByEqualityOperator

	}
}
