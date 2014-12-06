using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Vertesaur
{
    /// <summary>
    /// A perfect sphere defined by a radius.
    /// </summary>
    public struct Sphere :
        ISpheroid<double>,
        IEquatable<ISpheroid<double>>,
        IEquatable<Sphere>
    {

        /// <inheritdoc/>
        public static bool operator ==(Sphere a, Sphere b) {
            return a.Equals(b);
        }

        /// <inheritdoc/>
        public static bool operator !=(Sphere a, Sphere b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// The radius of the sphere.
        /// </summary>
        public readonly double Radius;

        /// <summary>
        /// Creates a new sphere defined by the given <paramref name="radius"/>.
        /// </summary>
        /// <param name="radius">The radius of the sphere.</param>
        public Sphere(double radius) {
            Radius = radius;
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.A { get { return Radius; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.B { get { return Radius; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.F { get { return 0.0; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.InvF { get { return Double.PositiveInfinity; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.E { get { return 0.0; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.ESquared { get { return 0.0; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.ESecond { get { return 0.0; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.ESecondSquared { get { return 0.0; } }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            if (obj is Sphere)
                return Equals((Sphere)obj);
            return Equals(obj as ISpheroid<double>);
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return Radius.GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals(ISpheroid<double> other) {
            return (
                !ReferenceEquals(null, other)
                && Radius == other.A
                && (Radius == other.B || other.F == 0)
            );
        }

        /// <inheritdoc/>
        public bool Equals(Sphere other) {
            return Radius == other.Radius;
        }

        /// <inheritdoc/>
        public override string ToString() {
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
            var result = String.Format("r:" + Radius.ToString(CultureInfo.InvariantCulture));
            Contract.Assume(!String.IsNullOrEmpty(result));
            return result;
        }

    }
}
