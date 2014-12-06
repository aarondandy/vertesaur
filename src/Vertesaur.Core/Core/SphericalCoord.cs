using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur
{

    /// <summary>
    /// A spherical coordinate in 3D space.
    /// </summary>
    public struct SphericalCoord :
        ISphericalCoordinate<double>,
        IEquatable<SphericalCoord>,
        IEquatable<ISphericalCoordinate<double>>
    {

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A spherical coordinate from the left argument.</param>
        /// <param name="b">A spherical coordinate from the right argument.</param>
        /// <returns>True if both coordinates have the same component values.</returns>
        public static bool operator ==(SphericalCoord a, SphericalCoord b) {
            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A spherical coordinate from the left argument.</param>
        /// <param name="b">A spherical coordinate from the right argument.</param>
        /// <returns>True if both coordinates do not have the same component values.</returns>
        public static bool operator !=(SphericalCoord a, SphericalCoord b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// A vector with all components set to zero.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly SphericalCoord Invalid = new SphericalCoord(Double.NaN, Double.NaN, Double.NaN);

        /// <summary>
        /// The rho component.
        /// </summary>
        public readonly double Rho;
        /// <summary>
        /// The theta component.
        /// </summary>
        public readonly double Theta;
        /// <summary>
        /// The phi component.
        /// </summary>
        public readonly double Phi;

        /// <summary>
        /// Constructs a new spherical coordinate from the given component values.
        /// </summary>
        /// <param name="rho">Tho value.</param>
        /// <param name="theta">Theta value.</param>
        /// <param name="phi">Phi value.</param>
        public SphericalCoord(double rho, double theta, double phi) {
            Rho = rho;
            Theta = theta;
            Phi = phi;
        }

        /// <summary>
        /// Constructs a new spherical coordinate equivalent to the given vector.
        /// </summary>
        /// <param name="vector">The vector to construct a spherical coordinate for.</param>
        public SphericalCoord(Vector3 vector) {
            Rho = vector.GetMagnitude();
            if (0 == Rho) {
                Theta = 0;
                Phi = 0;
            }
            else {
                Theta = Math.Atan2(vector.Y, vector.X);
                Phi = Math.Acos(vector.Z / Rho);
            }
        }

        /// <summary>
        /// Constructs a new spherical coordinate equivalent to the given point.
        /// </summary>
        /// <param name="point">The point to construct a spherical coordinate for.</param>
        public SphericalCoord(Point3 point) {
            Rho = Point3.Zero.Distance(point);
            if (0 == Rho) {
                Theta = 0;
                Phi = 0;
            }
            else {
                Theta = Math.Atan2(point.Y, point.X);
                Phi = Math.Acos(point.Z / Rho);
            }
        }

        /// <inheritdoc/>
        double ISphericalCoordinate<double>.Rho { get { return Rho; } }
        /// <inheritdoc/>
        double ISphericalCoordinate<double>.Theta { get { return Theta; } }
        /// <inheritdoc/>
        double ISphericalCoordinate<double>.Phi { get { return Phi; } }

        /// <inheritdoc/>
        public bool Equals(SphericalCoord other) {
            return Rho == other.Rho
                && Theta == other.Theta
                && Phi == other.Phi;
        }

        /// <inheritdoc/>
        public bool Equals(ISphericalCoordinate<double> other) {
            return !ReferenceEquals(null, other)
                && Rho == other.Rho
                && Theta == other.Theta
                && Phi == other.Phi;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            return obj is SphericalCoord
                ? Equals((SphericalCoord)obj)
                : Equals(obj as ISphericalCoordinate<double>);
        }

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() {
            return Rho.GetHashCode() ^ Phi.GetHashCode();
        }

        /// <summary>
        /// Creates a spherical coordinate with the same rotations but with a length of one.
        /// </summary>
        /// <returns>A spherical coordinate that is 1 unit from the origin.</returns>
        [Pure]
        public SphericalCoord GetNormalized() {
            return new SphericalCoord(1, Theta, Phi);
        }

        /// <summary>
        /// Creates a vector equivalent to this coordinate.
        /// </summary>
        /// <returns>An equivalent vector.</returns>
        [Pure]
        public Vector3 ToVector() {
            var rhoSinPhi = Rho * Math.Sin(Phi);
            return new Vector3(
                rhoSinPhi * Math.Cos(Theta),
                rhoSinPhi * Math.Sin(Theta),
                Rho * Math.Cos(Phi)
            );
        }

        /// <summary>
        /// Creates a point equivalent to this coordinate.
        /// </summary>
        /// <returns>An equivalent point.</returns>
        [Pure]
        public Point3 ToPoint() {
            var rhoSinPhi = Rho * Math.Sin(Phi);
            return new Point3(
                rhoSinPhi * Math.Cos(Theta),
                rhoSinPhi * Math.Sin(Theta),
                Rho * Math.Cos(Phi)
            );
        }

        /// <inheritdoc/>
        [Pure]
        public double GetMagnitude() {
            Contract.Ensures(!(Contract.Result<double>() < 0));
            return Math.Abs(Rho);
        }

        /// <inheritdoc/>
        [Pure]
        public double GetMagnitudeSquared() {
            Contract.Ensures(Contract.Result<double>() >= 0 || Double.IsNaN(Contract.Result<double>()));
            return Rho * Rho;
        }


    }
}
