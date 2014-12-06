using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur
{
    /// <summary>
    /// An equatorial polar spheroid defined by the semi-major and semi-minor axes.
    /// </summary>
    public struct SpheroidEquatorialPolar :
        ISpheroid<double>,
        IEquatable<ISpheroid<double>>,
        IEquatable<SpheroidEquatorialPolar>
    {

        /// <inheritdoc/>
        public static bool operator ==(SpheroidEquatorialPolar a, SpheroidEquatorialPolar b) {
            return a.Equals(b);
        }

        /// <inheritdoc/>
        public static bool operator !=(SpheroidEquatorialPolar a, SpheroidEquatorialPolar b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// The semi-major axis.
        /// </summary>
        public readonly double A;
        /// <summary>
        /// The semi-minor axis.
        /// </summary>
        public readonly double B;

        /// <summary>
        /// Creates an equatorial polar spheroid defined by the given semi-major and semi-minor axes.
        /// </summary>
        /// <param name="a">Semi-major axis.</param>
        /// <param name="b">Semi-minor axis</param>
        public SpheroidEquatorialPolar(double a, double b) {
            A = a;
            B = b;
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.A { get { return A; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.B { get { return B; } }

        /// <inheritdoc/>
        public double F {
            [Pure] get { return 1.0 - (B / A); }
        }

        /// <inheritdoc/>
        public double InvF {
            [Pure] get { return A / (A - B); }
        }

        /// <inheritdoc/>
        public double E {
            [Pure] get { return Math.Sqrt(((A * A) - (B * B))) / A; }
        }

        /// <inheritdoc/>
        public double ESquared {
            [Pure] get { return 1.0 - ((B * B) / (A * A)); }
        }

        /// <inheritdoc/>
        public double ESecond {
            [Pure] get { return Math.Sqrt((A * A) - (B * B)) / B; }
        }

        /// <inheritdoc/>
        public double ESecondSquared {
            [Pure] get { return ((A * A) / (B * B)) - 1.0; }
        }

        /// <inheritdoc/>
        [Pure] public bool Equals(ISpheroid<double> other) {
            return !ReferenceEquals(null, other)
                && other.A == A
                && (other.B == B || other.InvF == InvF);
        }

        /// <inheritdoc/>
        [Pure] public override bool Equals(object obj) {
            if (obj is SpheroidEquatorialPolar)
                return Equals((SpheroidEquatorialPolar)obj);
            return Equals(obj as ISpheroid<double>);
        }

        /// <inheritdoc/>
        [Pure] public override int GetHashCode() {
            return A.GetHashCode();
        }

        /// <inheritdoc/>
        [Pure] public bool Equals(SpheroidEquatorialPolar other) {
            return other.A == A && other.B == B;
        }

    }
}
