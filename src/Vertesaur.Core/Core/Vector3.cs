using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur
{
    /// <summary>
    /// A vector in 3D space.
    /// </summary>
    public struct Vector3 :
        IVector3<double>,
        IEquatable<Vector3>,
        IEquatable<ICoordinateTriple<double>>,
        IComparable<Vector3>
    {

        // ReSharper disable CompareOfFloatsByEqualityOperator

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A vector from the left argument.</param>
        /// <param name="b">A vector from the right argument.</param>
        /// <returns>True if both vectors have the same component values.</returns>
        public static bool operator ==(Vector3 a, Vector3 b) {
            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A vector from the left argument.</param>
        /// <param name="b">A vector from the right argument.</param>
        /// <returns>True if both vectors do not have the same component values.</returns>
        public static bool operator !=(Vector3 a, Vector3 b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="leftHandSide">A vector from the left argument.</param>
        /// <param name="rightHandSide">A vector from the right argument.</param>
        /// <returns>The result.</returns>
        public static Vector3 operator +(Vector3 leftHandSide, Vector3 rightHandSide) {
            return leftHandSide.Add(rightHandSide);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="leftHandSide">A vector from the left argument.</param>
        /// <param name="rightHandSide">A vector from the right argument.</param>
        /// <returns>The result.</returns>
        public static Vector3 operator -(Vector3 leftHandSide, Vector3 rightHandSide) {
            return leftHandSide.Difference(rightHandSide);
        }

        /// <summary>
        /// Multiplies a row vector by a right matrix.
        /// </summary>
        /// <param name="left">The row vector from the left argument.</param>
        /// <param name="right">The matrix from the right argument.</param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 left, Matrix3 right) {
            Contract.Requires(null != right);
            return left.MultiplyAsRow(right);
        }

        /// <summary>
        /// Multiplies a left matrix by a column vector.
        /// </summary>
        /// <param name="left">The matrix from the left argument.</param>
        /// <param name="right">The column vector from the right argument.</param>
        /// <returns>A transformed vector.</returns>
        public static Vector3 operator *(Matrix3 left, Vector3 right) {
            Contract.Requires(null != left);
            return right.MultiplyAsColumn(left);
        }

        /// <summary>
        /// Multiplies a row vector by a right matrix.
        /// </summary>
        /// <param name="left">The row vector from the left argument.</param>
        /// <param name="right">The matrix from the right argument.</param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 left, Matrix4 right) {
            Contract.Requires(null != right);
            return left.MultiplyAsRow(right);
        }

        /// <summary>
        /// Multiplies a left matrix by a column vector.
        /// </summary>
        /// <param name="left">The matrix from the left argument.</param>
        /// <param name="right">The column vector from the right argument.</param>
        /// <returns>A transformed vector.</returns>
        public static Vector3 operator *(Matrix4 left, Vector3 right) {
            Contract.Requires(null != left);
            return right.MultiplyAsColumn(left);
        }

        /// <summary>
        /// Multiplies the vector by a scalar.
        /// </summary>
        /// <param name="tuple">The vector to multiply.</param>
        /// <param name="factor">The scalar value to multiply by.</param>
        /// <returns>The resulting scaled vector.</returns>
        public static Vector3 operator *(Vector3 tuple, double factor) {
            return tuple.GetScaled(factor);
        }

        /// <summary>
        /// Multiplies the vector by a scalar.
        /// </summary>
        /// <param name="tuple">The vector to multiply.</param>
        /// <param name="factor">The scalar value to multiply by.</param>
        /// <returns>The resulting scaled vector.</returns>
        public static Vector3 operator *(double factor, Vector3 tuple) {
            return tuple.GetScaled(factor);
        }

        /// <inheritdoc/>
        public static implicit operator Point3(Vector3 value) {
            return new Point3(value);
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
        public static readonly Vector3 Invalid = new Vector3(Double.NaN, Double.NaN, Double.NaN);
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
        public Vector3(ICoordinateTriple<double> v) {
            if (v == null) throw new ArgumentNullException("v");
            Contract.EndContractBlock();
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Clones this vector from a point.
        /// </summary>
        /// <param name="p">The point to clone.</param>
        public Vector3(Point3 p)
            : this(p.X, p.Y, p.Z) { }

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
        [Pure] public bool Equals(ICoordinateTriple<double> other) {
            return !ReferenceEquals(null, other)
                && X == other.X && Y == other.Y && Z == other.Z;
        }

        /// <inheritdoc/>
        [Pure] public override bool Equals(object obj) {
            return null != obj && (
                (obj is Vector3 && Equals((Vector3)obj))
                || Equals(obj as ICoordinateTriple<double>)
            );
        }

        /// <inheritdoc/>
        [Pure] public override int GetHashCode() {
            return X.GetHashCode();
        }

        /// <inheritdoc/>
        [Pure] public override string ToString() {
            return String.Concat(X, ' ', Y, ' ', Z);
        }

        /// <summary>
        /// Calculates the magnitude of this vector.
        /// </summary>
        /// <returns>The magnitude.</returns>
        [Pure] public double GetMagnitude() {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        /// <summary>
        /// Calculates the squared magnitude of this vector.
        /// </summary>
        /// <returns>The squared magnitude.</returns>
        [Pure] public double GetMagnitudeSquared() {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            return (X * X) + (Y * Y) + (Z * Z);
        }

        /// <summary>
        /// Calculates a vector resulting from adding the given vector to this vector.
        /// </summary>
        /// <param name="rightHandSide">The vector to add.</param>
        /// <returns>A result of adding this vector with the given vector.</returns>
        [Pure]
        public Vector3 Add(Vector3 rightHandSide) {
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
        [Pure]
        public Point3 Add(Point3 rightHandSide) {
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
        [Pure]
        public Vector3 Difference(Vector3 rightHandSide) {
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
        [Pure]
        public Point3 Difference(Point3 rightHandSide) {
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

        /// <summary>
        /// Creates a new vector resulting from dividing the elements by a given <paramref name="denominator"/>.
        /// </summary>
        /// <param name="denominator">The value to divide all elements by.</param>
        /// <returns>A new vector with elements that are the result of division.</returns>
        [Pure]
        public Vector3 GetDivided(double denominator) {
            return new Vector3(X / denominator, Y / denominator, Z / denominator);     
        }

        /// <summary>
        /// Determines if the vector is valid.
        /// </summary>
        [Pure] public bool IsValid {
            get { return !Double.IsNaN(X) && !Double.IsNaN(Y) && !Double.IsNaN(Z); }
        }

        /// <summary>
        /// Calculates the result of multiplying this vector as a row by a matrix.
        /// </summary>
        /// <param name="rightMatrix">The matrix to multiply by.</param>
        /// <returns>The result of multiplying this vector by the given matrix.</returns>
        [Pure]
        public Vector3 MultiplyAsRow(Matrix3 rightMatrix) {
            if (null == rightMatrix) throw new ArgumentNullException("rightMatrix");
            Contract.EndContractBlock();
            return new Vector3(
                (X * rightMatrix.E00)
                + (Y * rightMatrix.E10)
                + (Z * rightMatrix.E20)
                ,
                (X * rightMatrix.E01)
                + (Y * rightMatrix.E11)
                + (Z * rightMatrix.E21)
                ,
                (X * rightMatrix.E02)
                + (Y * rightMatrix.E12)
                + (Z * rightMatrix.E22)
            );
        }

        /// <summary>
        /// Calculates the result of multiply a matrix by this vector as a column.
        /// </summary>
        /// <param name="leftMatrix">The matrix to multiply by.</param>
        /// <returns>The result of multiplying the given matrix by this vector.</returns>
        [Pure]
        public Vector3 MultiplyAsColumn(Matrix3 leftMatrix) {
            if (null == leftMatrix) throw new ArgumentNullException("leftMatrix");
            Contract.EndContractBlock();
            return new Vector3(
                (leftMatrix.E00 * X)
                + (leftMatrix.E01 * Y)
                + (leftMatrix.E02 * Z)
                ,
                (leftMatrix.E10 * X)
                + (leftMatrix.E11 * Y)
                + (leftMatrix.E12 * Z)
                ,
                (leftMatrix.E20 * X)
                + (leftMatrix.E21 * Y)
                + (leftMatrix.E22 * Z)
            );
        }

        /// <summary>
        /// Calculates the result of multiplying this vector as a row by a matrix.
        /// </summary>
        /// <param name="rightMatrix">The matrix to multiply by.</param>
        /// <returns>The result of multiplying this vector by the given matrix.</returns>
        [Pure]
        public Vector3 MultiplyAsRow(Matrix4 rightMatrix) {
            if (null == rightMatrix) throw new ArgumentNullException("rightMatrix");
            Contract.EndContractBlock();
            return new Vector3(
                (X * rightMatrix.E00)
                + (Y * rightMatrix.E10)
                + (Z * rightMatrix.E20)
                + rightMatrix.E30
                ,
                (X * rightMatrix.E01)
                + (Y * rightMatrix.E11)
                + (Z * rightMatrix.E21)
                + rightMatrix.E31
                ,
                (X * rightMatrix.E02)
                + (Y * rightMatrix.E12)
                + (Z * rightMatrix.E22)
                + rightMatrix.E32
            );
        }

        /// <summary>
        /// Calculates the result of multiply a matrix by this vector as a column.
        /// </summary>
        /// <param name="leftMatrix">The matrix to multiply by.</param>
        /// <returns>The result of multiplying the given matrix by this Vector3.</returns>
        [Pure]
        public Vector3 MultiplyAsColumn(Matrix4 leftMatrix) {
            if (null == leftMatrix) throw new ArgumentNullException("leftMatrix");
            Contract.EndContractBlock();
            return new Vector3(
                (leftMatrix.E00 * X)
                + (leftMatrix.E01 * Y)
                + (leftMatrix.E02 * Z)
                + leftMatrix.E03
                ,
                (leftMatrix.E10 * X)
                + (leftMatrix.E11 * Y)
                + (leftMatrix.E12 * Z)
                + leftMatrix.E13
                ,
                (leftMatrix.E20 * X)
                + (leftMatrix.E21 * Y)
                + (leftMatrix.E22 * Z)
                + leftMatrix.E23
            );
        }

    }
}
