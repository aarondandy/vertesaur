using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Vertesaur
{
    /// <summary>
    /// An equatorial spheroid defined by the semi-major axis and the flattening value.
    /// </summary>
    public struct SpheroidEquatorialF :
        ISpheroid<double>,
        IEquatable<ISpheroid<double>>,
        IEquatable<SpheroidEquatorialF>
    {

        /// <inheritdoc/>
        [Pure]
        public static bool operator ==(SpheroidEquatorialF a, SpheroidEquatorialF b) {
            return a.Equals(b);
        }

        /// <inheritdoc/>
        [Pure]
        public static bool operator !=(SpheroidEquatorialF a, SpheroidEquatorialF b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// The semi-major axis.
        /// </summary>
        public readonly double A;

        /// <summary>
        /// The flattening value.
        /// </summary>
        public readonly double F;

        /// <summary>
        /// Creates a new equatorial spheroid defined by the given semi-major axis and flattening value.
        /// </summary>
        /// <param name="a">Semi-major axis.</param>
        /// <param name="f">Flattening value.</param>
        public SpheroidEquatorialF(double a, double f) {
            A = a;
            F = f;
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.A { get { return A; } }

        /// <inheritdoc/>
        public double B { [Pure] get { return A * (1.0 - F); } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        double ISpheroid<double>.F { get { return F; } }

        /// <inheritdoc/>
        public double InvF {
            [Pure] get { return 1.0 / F; }
        }

        /// <inheritdoc/>
        public double E {
            [Pure] get { return F * Math.Sqrt((2.0 / F) - 1); }
        }

        /// <inheritdoc/>
        public double ESquared {
            [Pure] get { return F * F * ((2 / F) - 1); }
        }

        /// <inheritdoc/>
        public double ESecond {
            [Pure] get {
                return Math.Sqrt((2 / F) - 1)
                    / ((1 / F) - 1);
            }
        }

        /// <inheritdoc/>
        public double ESecondSquared {
            [Pure] get {
                var x = (2 / F) - 1;
                return x / ((1 / (F * F)) - x);
            }
        }

        /// <inheritdoc/>
        [Pure] public bool Equals(ISpheroid<double> other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return !ReferenceEquals(null, other)
                && A == other.A
                && (other.F == F || other.B == B);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            if (obj is SpheroidEquatorialF)
                return Equals((SpheroidEquatorialF)obj);
            return Equals(obj as ISpheroid<double>);
        }

        /// <inheritdoc/>
        [Pure] public override int GetHashCode() {
            return A.GetHashCode();
        }

        /// <inheritdoc/>
        [Pure] public bool Equals(SpheroidEquatorialF other) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return other.A == A && other.F == F;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }
    }
}
