﻿using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
        /// Creates a zero vector.
        /// </summary>
        /// <returns>A zero constant value.</returns>
        public static Vector2<TValue> CreateZero() {
            var v = BasicOperations<TValue>.Default.ZeroValue;
            return new Vector2<TValue>(v, v);
        }

        /// <summary>
        /// Convert a generically typed vector to a double typed vector.
        /// </summary>
        /// <param name="value">The vector to cast and convert.</param>
        /// <returns>The resulting double vector representation of the casted generic vector.</returns>
        public static explicit operator Vector2(Vector2<TValue> value) {
            if(BasicOperations<TValue>.Default.ToDouble == null)
                throw new InvalidOperationException();
            return new Vector2(
                BasicOperations<TValue>.Default.ToDouble(value.X),
                BasicOperations<TValue>.Default.ToDouble(value.Y));
        }

        /// <summary>
        /// Convert a double typed vector to a generically typed vector.
        /// </summary>
        /// <param name="value">The vector to cast and convert.</param>
        /// <returns>The resulting generic vector representation of the casted double vector.</returns>
        public static explicit operator Vector2<TValue>(Vector2 value) {
            return new Vector2<TValue>(value);
        }

        /// <summary>
        /// The x-coordinate of this vector.
        /// </summary>
        public readonly TValue X;
        /// <summary>
        /// The y-coordinate of this vector.
        /// </summary>
        public readonly TValue Y;

        /// <summary>
        /// Creates a 2D vector.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public Vector2(TValue x, TValue y) {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a 2D vector.
        /// </summary>
        /// <param name="v">The coordinate tuple to copy values from.</param>
        public Vector2(ICoordinatePair<TValue> v) {
            if (null == v) throw new ArgumentNullException("v");
            Contract.EndContractBlock();
            X = v.X;
            Y = v.Y;
        }

        /// <summary>
        /// Constructs a new vector from a double typed vector, converting the coordinate values.
        /// </summary>
        /// <param name="v">The vector to convert and clone from.</param>
        /// <exception cref="System.ArgumentException">Coordinate type conversion fails.</exception>
        public Vector2(Vector2 v) {
            if (BasicOperations<TValue>.Default.FromDouble == null)
                throw new InvalidOperationException();
            X = BasicOperations<TValue>.Default.FromDouble(v.X);
            Y = BasicOperations<TValue>.Default.FromDouble(v.Y);
        }

        /// <summary>
        /// The x-coordinate of this vector.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        TValue ICoordinatePair<TValue>.X { get { return X; } }

        /// <summary>
        /// The y-coordinate of this vector.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        TValue ICoordinatePair<TValue>.Y { get { return Y; } }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return X.GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals(Vector2<TValue> other) {
            return X.Equals(other.X) && Y.Equals(other.Y);
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
            if (VectorOperations<TValue>.Default.Magnitude2D == null)
                throw new InvalidOperationException();
            return VectorOperations<TValue>.Default.Magnitude2D(X, Y);
        }

        /// <inheritdoc/>
        public TValue GetMagnitudeSquared() {
            if (VectorOperations<TValue>.Default.SquaredMagnitude2D == null)
                throw new InvalidOperationException();
            return VectorOperations<TValue>.Default.SquaredMagnitude2D(X, Y);
        }

        /// <summary>
        /// Calculates a vector resulting from adding the given vector to this vector.
        /// </summary>
        /// <param name="rightHandSide">The vector to add.</param>
        /// <returns>A result of adding this vector with the given vector.</returns>
        public Vector2<TValue> Add(Vector2<TValue> rightHandSide) {
            if(BasicOperations<TValue>.Default.Add == null)
                throw new InvalidOperationException();
            return new Vector2<TValue>(
                BasicOperations<TValue>.Default.Add(X, rightHandSide.X),
                BasicOperations<TValue>.Default.Add(Y, rightHandSide.Y));
        }

        /// <summary>
        /// Calculates a vector resulting from adding the given vector to this vector.
        /// </summary>
        /// <param name="rightHandSide">The vector to add.</param>
        /// <returns>A result of adding this vector with the given vector.</returns>
        public Point2<TValue> Add(Point2<TValue> rightHandSide) {
            if(BasicOperations<TValue>.Default.Add == null)
                throw new InvalidOperationException();
            return new Point2<TValue>(
                BasicOperations<TValue>.Default.Add(X, rightHandSide.X),
                BasicOperations<TValue>.Default.Add(Y, rightHandSide.Y));
        }

        /// <summary>
        /// Calculates a vector resulting from subtracting the given vector to this vector.
        /// </summary>
        /// <param name="rightHandSide">The vector to subtract.</param>
        /// <returns>A result of subtracting the given vector from this vector.</returns>
        public Vector2<TValue> Difference(Vector2<TValue> rightHandSide) {
            if(BasicOperations<TValue>.Default.Subtract == null)
                throw new InvalidOperationException();
            return new Vector2<TValue>(
                BasicOperations<TValue>.Default.Subtract(X, rightHandSide.X),
                BasicOperations<TValue>.Default.Subtract(Y, rightHandSide.Y));
        }

        /// <summary>
        /// Calculates a vector resulting from subtracting the given vector to this vector.
        /// </summary>
        /// <param name="rightHandSide">The vector to subtract.</param>
        /// <returns>A result of subtracting the given vector from this vector.</returns>
        public Point2<TValue> Difference(Point2<TValue> rightHandSide) {
            if(BasicOperations<TValue>.Default.Subtract == null)
                throw new InvalidOperationException();
            return new Point2<TValue>(
                BasicOperations<TValue>.Default.Subtract(X, rightHandSide.X),
                BasicOperations<TValue>.Default.Subtract(Y, rightHandSide.Y));
        }

        /// <summary>
        /// Calculates the dot product between this vector and another vector.
        /// </summary>
        /// <param name="rightHandSide">Another vector to use for the calculation of the dot product.</param>
        /// <returns>The dot product.</returns>
        public TValue Dot(Vector2<TValue> rightHandSide) {
            if (VectorOperations<TValue>.Default.DotProduct2D == null)
                throw new InvalidOperationException();
            return VectorOperations<TValue>.Default.DotProduct2D(X, Y, rightHandSide.X, rightHandSide.Y);
        }

        /// <summary>
        /// Calculates a vector oriented in the opposite direction.
        /// </summary>
        /// <returns>A vector with the same component values but different signs.</returns>
        public Vector2<TValue> GetNegative() {
            if(BasicOperations<TValue>.Default.Negate == null)
                throw new InvalidOperationException();
            return new Vector2<TValue>(
                BasicOperations<TValue>.Default.Negate(X),
                BasicOperations<TValue>.Default.Negate(Y));
        }

        /// <summary>
        /// Creates a new vector which is scaled from this vector.
        /// </summary>
        /// <param name="factor">The scaling factor.</param>
        /// <returns>A scaled vector.</returns>
        public Vector2<TValue> GetScaled(TValue factor) {
            if (null == factor) throw new ArgumentNullException("factor");
            Contract.EndContractBlock();

            if(BasicOperations<TValue>.Default.Multiply == null)
                throw new InvalidOperationException();

            return new Vector2<TValue>(
                BasicOperations<TValue>.Default.Multiply(X, factor),
                BasicOperations<TValue>.Default.Multiply(Y, factor));
        }

        /// <summary>
        /// Calculates the dot product of this vector and a vector perpendicular to the other vector.
        /// </summary>
        /// <param name="rightHandSide">A vector.</param>
        /// <returns>The z-coordinate of the cross product.</returns>
        /// <remarks>Also calculates the z-coordinate of the cross product of this vector and another vector.</remarks>
        public TValue PerpendicularDot(Vector2<TValue> rightHandSide) {
            if (VectorOperations<TValue>.Default.PerpendicularDotProduct2D == null)
                throw new InvalidOperationException();
            return VectorOperations<TValue>.Default.PerpendicularDotProduct2D(
                X, Y, rightHandSide.X, rightHandSide.Y);
        }

        /// <summary>
        /// Gets a clock-wise perpendicular vector with the same magnitude as this vector.
        /// </summary>
        /// <returns>A vector.</returns>
        public Vector2<TValue> GetPerpendicularClockwise() {
            if (BasicOperations<TValue>.Default.Negate == null)
                throw new InvalidOperationException();
            return new Vector2<TValue>(Y, BasicOperations<TValue>.Default.Negate(X));
        }

        /// <summary>
        /// Gets a counter clock-wise perpendicular vector with the same magnitude as this vector.
        /// </summary>
        /// <returns>A vector.</returns>
        public Vector2<TValue> GetPerpendicularCounterClockwise() {
            if (BasicOperations<TValue>.Default.Negate == null)
                throw new InvalidOperationException();
            return new Vector2<TValue>(BasicOperations<TValue>.Default.Negate(Y), X);
        }

        /// <summary>
        /// Calculates a vector with the same direction but a magnitude of one.
        /// </summary>
        /// <returns>A unit length vector.</returns>
        public Vector2<TValue> GetNormalized() {
            var m = GetMagnitude();
            if (BasicOperations<TValue>.Default.ZeroValue.Equals(m))
                return CreateZero();

            if (BasicOperations<TValue>.Default.Divide == null)
                throw new InvalidOperationException();

            return new Vector2<TValue>(
                BasicOperations<TValue>.Default.Divide(X, m),
                BasicOperations<TValue>.Default.Divide(Y, m));
        }

    }
}
