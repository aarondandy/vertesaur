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
	public struct Vector2 :
		IVector2<double>,
		IEquatable<Vector2>,
		IComparable<Vector2>
	{

// ReSharper disable CompareOfFloatsByEqualityOperator

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">A vector.</param>
		/// <param name="b">A vector.</param>
		/// <returns>True if both vectors have the same component values.</returns>
		public static bool operator ==(Vector2 a, Vector2 b) {
			return a.Equals(b);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">A vector.</param>
		/// <param name="b">A vector.</param>
		/// <returns>True if both vectors do not have the same component values.</returns>
		public static bool operator !=(Vector2 a, Vector2 b) {
			return !a.Equals(b);
		}

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Vector2 operator +(Vector2 leftHandSide, Vector2 rightHandSide) {
			return leftHandSide.Add(rightHandSide);
		}

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The result.</returns>
		public static Vector2 operator -(Vector2 leftHandSide, Vector2 rightHandSide) {
			return leftHandSide.Difference(rightHandSide);
		}

		/// <summary>
		/// Implements the operator * as the dot operator.
		/// </summary>
		/// <param name="leftHandSide">A vector.</param>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The dot product.</returns>
		public static double operator *(Vector2 leftHandSide, Vector2 rightHandSide) {
			return leftHandSide.Dot(rightHandSide);
		}

		/// <summary>
		/// A vector with all components set to zero.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Vector2 Zero = new Vector2(0, 0);
		/// <summary>
		/// A vector with all components set to zero.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Vector2 Invalid = new Vector2(Double.NaN, Double.NaN);
		/// <summary>
		/// A vector with a magnitude of one and oriented in the direction of the positive X axis.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Vector2 XUnit = new Vector2(1, 0);
		/// <summary>
		/// A vector with a magnitude of one and oriented in the direction of the positive Y axis.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static readonly Vector2 YUnit = new Vector2(0, 1);

		/// <summary>
		/// The x-coordinate of this vector.
		/// </summary>
		public readonly double X;
		/// <summary>
		/// The y-coordinate of this vector.
		/// </summary>
		public readonly double Y;

		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		public Vector2(double x, double y) {
			X = x;
			Y = y;
		}
		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="v">The coordinate tuple to copy values from.</param>
		public Vector2([NotNull] ICoordinatePair<double> v) {
			if(null == v) throw new ArgumentNullException("v");
			Contract.EndContractBlock();
			X = v.X;
			Y = v.Y;
		}

		/// <summary>
		/// The x-coordinate of this point.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinatePair<double>.X { get { return X; } }
		/// <summary>
		/// The y-coordinate of this point.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double ICoordinatePair<double>.Y { get { return Y; } }

		/// <inheritdoc/>
		[Obsolete("This does not belong here.")]
		[Pure] public int CompareTo(Vector2 other) {
			var c = X.CompareTo(other.X);
			return 0 == c ? Y.CompareTo(other.Y) : c;
		}

		/// <inheritdoc/>
		[Pure] public bool Equals(Vector2 other) {
			return X == other.X && Y == other.Y;
		}

		/// <inheritdoc/>
		[Pure, ContractAnnotation("null=>false")]
		public bool Equals([CanBeNull] ICoordinatePair<double> other) {
			return !ReferenceEquals(null, other) && X == other.X && Y == other.Y;
		}

		/// <inheritdoc/>
		[Pure, ContractAnnotation("null=>false")]
		public override bool Equals([CanBeNull] object obj) {
			return !ReferenceEquals(null, obj) && (
				(obj is Vector2 && Equals((Vector2)obj))
				||
				Equals(obj as ICoordinatePair<double>)
			);
		}

		/// <inheritdoc/>
		[Pure] public override int GetHashCode() {
			return X.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString() {
			return String.Concat(X, ' ', Y);
		}

		/// <summary>
		/// Calculates the magnitude of this vector.
		/// </summary>
		/// <returns>The magnitude.</returns>
		[Pure] public double GetMagnitude() {
			return Math.Sqrt((X * X) + (Y * Y));
		}

		/// <summary>
		/// Calculates the squared magnitude of this vector.
		/// </summary>
		/// <returns>The squared magnitude.</returns>
		[Pure] public double GetMagnitudeSquared() {
			return (X * X) + (Y * Y);
		}

		/// <summary>
		/// Calculates a vector resulting from adding the given vector to this vector.
		/// </summary>
		/// <param name="rightHandSide">The vector to add.</param>
		/// <returns>A result of adding this vector with the given vector.</returns>
		[Pure] public Vector2 Add(Vector2 rightHandSide) {
			return new Vector2(
				X + rightHandSide.X,
				Y + rightHandSide.Y
			);
		}

		/// <summary>
		/// Calculates the point offset by this vector and a given point.
		/// </summary>
		/// <param name="rightHandSide">The point to add this vector to.</param>
		/// <returns>A point offset by this vector from the given point.</returns>
		[Pure] public Point2 Add(Point2 rightHandSide) {
			return new Point2(
				X + rightHandSide.X,
				Y + rightHandSide.Y
			);
		}

		/// <summary>
		/// Calculates a vector resulting from subtracting the given vector to this vector.
		/// </summary>
		/// <param name="rightHandSide">The vector to subtract.</param>
		/// <returns>A result of subtracting the given vector from this vector.</returns>
		[Pure] public Vector2 Difference(Vector2 rightHandSide) {
			return new Vector2(
				X - rightHandSide.X,
				Y - rightHandSide.Y
			);
		}

		/// <summary>
		/// Calculates the difference resulting from subtractiong the given point from this vector.
		/// </summary>
		/// <param name="rightHandSide">The point to subtract.</param>
		/// <returns>A result of subtracting the given point from this vector.</returns>
		[Pure] public Point2 Difference(Point2 rightHandSide) {
			return new Point2(
				X - rightHandSide.X,
				Y - rightHandSide.Y
			);
		}

		/// <summary>
		/// Calculates a vector with the same direction but a magnitude of one.
		/// </summary>
		/// <returns>A unit length vector.</returns>
		[Pure] public Vector2 GetNormalized() {
			var m = GetMagnitude();
			return 0 == m ? Zero : new Vector2(X / m, Y / m);
		}

		/// <summary>
		/// Calculates the dot product between this vector and another vector.
		/// </summary>
		/// <param name="rightHandSide">Another vector to use for the calculation of the dot product.</param>
		/// <returns>The dot product.</returns>
		[Pure] public double Dot(Vector2 rightHandSide) {
			return (X * rightHandSide.X) + (Y * rightHandSide.Y);
		}

		/// <summary>
		/// Calculates a vector oriented in the opposite direction.
		/// </summary>
		/// <returns>A vector with the same component values but different signs.</returns>
		[Pure] public Vector2 GetNegative() {
			return new Vector2(-X, -Y);
		}

		/// <summary>
		/// Creates a new vector which is scaled from this vector.
		/// </summary>
		/// <param name="factor">The scaling factor.</param>
		/// <returns>A scaled vector.</returns>
		[Pure] public Vector2 GetScaled(double factor) {
			return new Vector2(X * factor, Y * factor);
		}

		/// <summary>
		/// Calculates the dot product of this vector and a vector perpendicular to the other vector.
		/// </summary>
		/// <param name="rightHandSide">A vector.</param>
		/// <returns>The z-coordinate of the cross product.</returns>
		/// <remarks>Also calculates the z-coordinate of the cross product of this vector and another vector.</remarks>
		[Pure] public double PerpendicularDot(Vector2 rightHandSide) {
			return (X * rightHandSide.Y) - (Y * rightHandSide.X);
		}

		/// <summary>
		/// Gets a clock-wise perpendicular vector with the same magnitude as this vector.
		/// </summary>
		/// <returns>A vector.</returns>
		[Pure] public Vector2 GetPerpendicularClockwise() {
			return new Vector2(Y, -X);
		}

		/// <summary>
		/// Gets a counter clock-wise perpendicular vector with the same magnitude as this vector.
		/// </summary>
		/// <returns>A vector.</returns>
		[Pure] public Vector2 GetPerpendicularCounterClockwise() {
			return new Vector2(-Y, X);
		}

		/// <summary>
		/// Determines if the vector is valid.
		/// </summary>
		[Pure] public bool IsValid{
			get { return !Double.IsNaN(X) && !Double.IsNaN(Y); }
		}

// ReSharper restore CompareOfFloatsByEqualityOperator

	}
}
